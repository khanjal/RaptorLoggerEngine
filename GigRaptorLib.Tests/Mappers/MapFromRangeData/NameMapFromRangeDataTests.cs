﻿using FluentAssertions;
using GigRaptorLib.Entities;
using GigRaptorLib.Enums;
using GigRaptorLib.Mappers;
using GigRaptorLib.Tests.Data.Helpers;
using GigRaptorLib.Utilities.Google;

namespace GigRaptorLib.Tests.Mappers.MapFromRangeData;

public class NameMapFromRangeDataTests : IAsyncLifetime
{
    private static IList<IList<object>>? _values;
    private static List<NameEntity>? _entities;

    public async Task InitializeAsync()
    {
        var configuration = TestConfigurationHelper.GetConfiguration();
        var spreadsheetId = configuration.GetSection("spreadsheet_id").Value;

        var googleSheetHelper = new GoogleSheetHelper();
        var result = await googleSheetHelper.GetSheetData(spreadsheetId!, SheetEnum.NAMES);

        //_values = JsonHelpers.LoadJsonSheetData("Name");
        _values = result;
        _entities = NameMapper.MapFromRangeData(_values!);
    }

    [Fact]
    public void GivenNameSheetData_ThenReturnRangeData()
    {
        var nonEmptyValues = _values!.Where(x => !string.IsNullOrEmpty(x[0].ToString())).ToList();
        _entities.Should().HaveCount(nonEmptyValues.Count - 1);

        foreach (var entity in _entities!)
        {
            entity.Id.Should().NotBe(0);
            entity.Name.Should().NotBeNullOrEmpty();
            entity.Visits.Should().BeGreaterThan(0);
            entity.Pay.Should().NotBeNull();
            entity.Tip.Should().NotBeNull();
            entity.Bonus.Should().NotBeNull();
            entity.Total.Should().NotBeNull();
            entity.Cash.Should().NotBeNull();
            entity.Distance.Should().BeGreaterThanOrEqualTo(0);
        }
    }

    [Fact]
    public void GivenNameSheetDataColumnOrderRandomized_ThenReturnSameRangeData()
    {
        var sheetOrder = new int[] { 0 }.Concat([.. RandomHelpers.GetRandomOrder(1, _values![0].Count - 1)]).ToArray();
        var randomValues = RandomHelpers.RandomizeValues(_values, sheetOrder);

        var randomEntities = NameMapper.MapFromRangeData(randomValues);
        var nonEmptyRandomValues = randomValues!.Where(x => !string.IsNullOrEmpty(x[0].ToString())).ToList();
        randomEntities.Should().HaveCount(nonEmptyRandomValues.Count - 1);

        for (int i = 0; i < randomEntities.Count; i++)
        {
            var entity = _entities![i];
            var randomEntity = randomEntities[i];

            entity.Id.Should().Be(randomEntity.Id);
            entity.Name.Should().BeEquivalentTo(randomEntity.Name);
            entity.Visits.Should().Be(randomEntity.Visits);
            entity.Pay.Should().Be(randomEntity.Pay);
            entity.Tip.Should().Be(randomEntity.Tip);
            entity.Bonus.Should().Be(randomEntity.Bonus);
            entity.Total.Should().Be(randomEntity.Total);
            entity.Cash.Should().Be(randomEntity.Cash);
            entity.Distance.Should().Be(randomEntity.Distance);
        }
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}
