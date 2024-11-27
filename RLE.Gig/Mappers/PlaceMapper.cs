using RLE.Core.Extensions;
using RLE.Core.Helpers;
using RLE.Core.Models.Google;
using RLE.Gig.Constants;
using RLE.Gig.Entities;
using RLE.Gig.Enums;
using RLE.Gig.Helpers;

namespace RLE.Gig.Mappers;

public static class PlaceMapper
{
    public static List<PlaceEntity> MapFromRangeData(IList<IList<object>> values)
    {
        var places = new List<PlaceEntity>();
        var headers = new Dictionary<int, string>();
        values = values!.Where(x => !string.IsNullOrEmpty(x[0].ToString())).ToList();
        var id = 0;

        foreach (var value in values)
        {
            id++;
            if (id == 1)
            {
                headers = HeaderHelpers.ParserHeader(value);
                continue;
            }

            if (value[0].ToString() == "")
            {
                continue;
            }

            PlaceEntity place = new()
            {
                Id = id,
                Place = HeaderHelpers.GetStringValue(HeaderEnum.PLACE.GetDescription(), value, headers),
                Trips = HeaderHelpers.GetIntValue(HeaderEnum.TRIPS.GetDescription(), value, headers),
                Pay = HeaderHelpers.GetDecimalValue(HeaderEnum.PAY.GetDescription(), value, headers),
                Tip = HeaderHelpers.GetDecimalValue(HeaderEnum.TIP.GetDescription(), value, headers),
                Bonus = HeaderHelpers.GetDecimalValue(HeaderEnum.BONUS.GetDescription(), value, headers),
                Total = HeaderHelpers.GetDecimalValue(HeaderEnum.TOTAL.GetDescription(), value, headers),
                Cash = HeaderHelpers.GetDecimalValue(HeaderEnum.CASH.GetDescription(), value, headers),
                Distance = HeaderHelpers.GetDecimalValue(HeaderEnum.DISTANCE.GetDescription(), value, headers),
            };

            places.Add(place);
        }

        return places;
    }

    public static SheetModel GetSheet()
    {
        var sheet = SheetsConfig.PlaceSheet;

        var tripSheet = TripMapper.GetSheet();

        sheet.Headers = GigSheetHelpers.GetCommonTripGroupSheetHeaders(tripSheet, HeaderEnum.PLACE);

        // // Days/Visit
        // sheet.Headers.Add(new SheetCellModel{Name = HeaderEnum.DAYS_PER_VISIT.DisplayName(),
        //     Formula = $"=ARRAYFORMULA(IFS(ROW($A:$A)=1,\"{HeaderEnum.DAYS_PER_VISIT.DisplayName()}\",ISBLANK($A:$A), \"\", true, DAYS(K:K,J:J)/B:B))",
        //     Note = $"Average days between visits.{(char)10}{(char)10}Doesn't take into account active time vs all time."});
        // // Since
        // sheet.Headers.Add(new SheetCellModel{Name = HeaderEnum.DAYS_SINCE_VISIT.DisplayName(),
        //     Formula = $"=ARRAYFORMULA(IFS(ROW($A:$A)=1,\"{HeaderEnum.DAYS_SINCE_VISIT.DisplayName()}\",ISBLANK($A:$A), \"\", true, DAYS(TODAY(),K:K)))",
        //     Note = "Days since last visit."});

        return sheet;
    }

}