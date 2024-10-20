﻿using FluentAssertions;
using Google.Apis.Sheets.v4.Data;
using RLE.Gig.Enums;
using RLE.Gig.Mappers;
using RLE.Core.Constants;
using RLE.Core.Models.Google;
using RLE.Gig.Helpers;
using RLE.Core.Helpers;

namespace RLE.Gig.Tests.Helpers;

public class GoogleSheetHelpersTests
{
    public static IEnumerable<object[]> Sheets =>
    [
        [AddressMapper.GetSheet(), GoogleSheetHelper.Generate([SheetEnum.ADDRESSES])],
        [DailyMapper.GetSheet(), GoogleSheetHelper.Generate([SheetEnum.DAILY])],
        [MonthlyMapper.GetSheet(), GoogleSheetHelper.Generate([SheetEnum.MONTHLY])],
        [NameMapper.GetSheet(), GoogleSheetHelper.Generate([SheetEnum.NAMES])],
        [PlaceMapper.GetSheet(), GoogleSheetHelper.Generate([SheetEnum.PLACES])],
        [RegionMapper.GetSheet(), GoogleSheetHelper.Generate([SheetEnum.REGIONS])],
        [ServiceMapper.GetSheet(), GoogleSheetHelper.Generate([SheetEnum.SERVICES])],
        [ShiftMapper.GetSheet(), GoogleSheetHelper.Generate([SheetEnum.SHIFTS])],
        [TripMapper.GetSheet(), GoogleSheetHelper.Generate([SheetEnum.TRIPS])],
        [TypeMapper.GetSheet(), GoogleSheetHelper.Generate([SheetEnum.TYPES])],
        [WeekdayMapper.GetSheet(), GoogleSheetHelper.Generate([SheetEnum.WEEKDAYS])],
        [WeeklyMapper.GetSheet(), GoogleSheetHelper.Generate([SheetEnum.WEEKLY])],
        [YearlyMapper.GetSheet(), GoogleSheetHelper.Generate([SheetEnum.YEARLY])],
    ];

    [Theory]
    [MemberData(nameof(Sheets))]
    public void GivenSheetConfig_ThenReturnSheetRequest(SheetModel config, BatchUpdateSpreadsheetRequest batchRequest)
    {
        var index = 0; // AddSheet should be first request

        batchRequest.Requests[index].AddSheet.Should().NotBeNull();

        var sheetRequest = batchRequest.Requests[index].AddSheet;
        sheetRequest.Properties.Title.Should().Be(config.Name);
        sheetRequest.Properties.TabColor.Should().BeEquivalentTo(SheetHelpers.GetColor(config.TabColor));
        sheetRequest.Properties.GridProperties.FrozenColumnCount.Should().Be(config.FreezeColumnCount);
        sheetRequest.Properties.GridProperties.FrozenRowCount.Should().Be(config.FreezeRowCount);
    }

    [Theory]
    [MemberData(nameof(Sheets))]
    public void GivenSheetHeaders_ThenReturnSheetHeaders(SheetModel config, BatchUpdateSpreadsheetRequest batchRequest)
    {
        var sheetId = batchRequest.Requests.First().AddSheet.Properties.SheetId;

        // Check on if it had to expand the number of rows (headers > 26)
        if (config.Headers.Count > 26)
        {
            var appendDimension = batchRequest.Requests.First(x => x.AppendDimension != null).AppendDimension;
            appendDimension.Dimension.Should().Be("COLUMNS");
            appendDimension.Length.Should().Be(config.Headers.Count - 26);
            appendDimension.SheetId.Should().Be(sheetId);
        }

        var appendCells = batchRequest.Requests.First(x => x.AppendCells != null).AppendCells;
        appendCells.SheetId.Should().Be(sheetId);
        appendCells.Rows.Should().HaveCount(1);
        appendCells.Rows[0].Values.Should().HaveCount(config.Headers.Count);
    }

    [Theory]
    [MemberData(nameof(Sheets))]
    public void GivenSheetColors_ThenReturnSheetBanding(SheetModel config, BatchUpdateSpreadsheetRequest batchRequest)
    {
        var sheetId = batchRequest.Requests.First().AddSheet.Properties.SheetId;

        var bandedRange = batchRequest.Requests.First(x => x.AddBanding != null).AddBanding.BandedRange;
        bandedRange.Range.SheetId.Should().Be(sheetId);
        bandedRange.RowProperties.HeaderColor.Should().BeEquivalentTo(SheetHelpers.GetColor(config.TabColor));
        bandedRange.RowProperties.SecondBandColor.Should().BeEquivalentTo(SheetHelpers.GetColor(config.CellColor));
    }

    [Theory]
    [MemberData(nameof(Sheets))]
    public void GivenSheetProtected_ThenReturnProtectRequest(SheetModel config, BatchUpdateSpreadsheetRequest batchRequest)
    {
        var sheetId = batchRequest.Requests.First().AddSheet.Properties.SheetId;
        var protectRange = batchRequest.Requests.Where(x => x.AddProtectedRange != null).ToList();

        if (!config.ProtectSheet)
        {
            return;
        }

        protectRange.Should().HaveCount(1);
        var sheetProtection = protectRange.First().AddProtectedRange.ProtectedRange;
        sheetProtection.Range.SheetId.Should().Be(sheetId);
        sheetProtection.Description.Should().Be(ProtectionWarnings.SheetWarning);
        sheetProtection.WarningOnly.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(Sheets))]
    public void GivenSheetNotProtected_ThenReturnProtectRequests(SheetModel config, BatchUpdateSpreadsheetRequest batchRequest)
    {
        var sheetId = batchRequest.Requests.First().AddSheet.Properties.SheetId;
        var protectRange = batchRequest.Requests.Where(x => x.AddProtectedRange != null).ToList();

        if (config.ProtectSheet)
        {
            return;
        }

        var columnProtections = config.Headers.Where(x => !string.IsNullOrEmpty(x.Formula)).ToList();

        protectRange.Should().HaveCount(columnProtections.Count + 1); // +1 for header protection

        for (var i = 0; i < protectRange.Count; i++)
        {
            var protectedRange = protectRange[i].AddProtectedRange.ProtectedRange;
            protectedRange.Range.SheetId.Should().Be(sheetId);
            protectedRange.WarningOnly.Should().BeTrue();

            if (i == protectRange.Count - 1) // Header protection (last) 
            {
                protectedRange.Description.Should().Be(ProtectionWarnings.HeaderWarning);
            }
            else
            {
                protectedRange.Description.Should().Be(ProtectionWarnings.ColumnWarning);
            }
        }
    }

    [Theory]
    [MemberData(nameof(Sheets))]
    public void GivenSheetHeaderFormatOrValidation_ThenReturnRepeatCellsRequest(SheetModel config, BatchUpdateSpreadsheetRequest batchRequest)
    {
        var sheetId = batchRequest.Requests.First().AddSheet.Properties.SheetId;
        var repeatCells = batchRequest.Requests.Where(x => x.RepeatCell != null).ToList();
        var repeatHeaders = config.Headers.Where(x => x.Format != null || x.Validation != null).ToList();

        repeatCells.Should().HaveCount(repeatHeaders.Count);
    }
}
