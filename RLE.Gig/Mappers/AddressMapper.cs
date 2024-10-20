using RLE.Core.Extensions;
using RLE.Core.Helpers;
using RLE.Core.Models.Google;
using RLE.Gig.Constants;
using RLE.Gig.Entities;
using RLE.Gig.Enums;
using RLE.Gig.Helpers;

namespace RLE.Gig.Mappers;

public static class AddressMapper
{
    public static List<AddressEntity> MapFromRangeData(IList<IList<object>> values)
    {
        var addresses = new List<AddressEntity>();
        var headers = new Dictionary<int, string>();
        values = values!.Where(x => !string.IsNullOrEmpty(x[0].ToString())).ToList();
        var id = 0;

        foreach (List<object> value in values)
        {
            id++;
            if (id == 1)
            {
                headers = HeaderHelper.ParserHeader(value);
                continue;
            }

            if (value.Count < headers.Count)
            {
                value.AddItems(headers.Count - value.Count);
            };

            AddressEntity address = new()
            {
                Id = id,
                Address = HeaderHelper.GetStringValue(HeaderEnum.ADDRESS.GetDescription(), value, headers),
                Visits = HeaderHelper.GetIntValue(HeaderEnum.TRIPS.GetDescription(), value, headers),
                Pay = HeaderHelper.GetDecimalValue(HeaderEnum.PAY.GetDescription(), value, headers),
                Tip = HeaderHelper.GetDecimalValue(HeaderEnum.TIP.GetDescription(), value, headers),
                Bonus = HeaderHelper.GetDecimalValue(HeaderEnum.BONUS.GetDescription(), value, headers),
                Total = HeaderHelper.GetDecimalValue(HeaderEnum.TOTAL.GetDescription(), value, headers),
                Cash = HeaderHelper.GetDecimalValue(HeaderEnum.CASH.GetDescription(), value, headers),
                Distance = HeaderHelper.GetDecimalValue(HeaderEnum.DISTANCE.GetDescription(), value, headers),
            };

            addresses.Add(address);
        }
        return addresses;
    }

    public static SheetModel GetSheet()
    {
        var sheet = SheetsConfig.AddressSheet;

        var tripSheet = TripMapper.GetSheet();

        sheet.Headers = GigSheetHelpers.GetCommonTripGroupSheetHeaders(tripSheet, HeaderEnum.ADDRESS_END);

        return sheet;
    }
}