using GigRaptorLib.Entities;
using GigRaptorLib.Enums;
using GigRaptorLib.Models;
using GigRaptorLib.Utilities;
using GigRaptorLib.Utilities.Extensions;

namespace GigRaptorLib.Mappers
{
    public static class WeeklyMapper
    {
        public static List<WeeklyEntity> MapFromRangeData(IList<IList<object>> values)
        {
            var weeklyList = new List<WeeklyEntity>();
            var headers = new Dictionary<int, string>();
            var id = 0;

            foreach (var value in values)
            {
                id++;
                if (id == 1)
                {
                    headers = HeaderParser.ParserHeader(value);
                    continue;
                }

                if (value[0].ToString() == "")
                {
                    continue;
                }

                WeeklyEntity weekly = new()
                {
                    Id = id,
                    Week = HeaderParser.GetStringValue(HeaderEnum.WEEK.DisplayName(), value, headers),
                    Trips = HeaderParser.GetIntValue(HeaderEnum.TRIPS.DisplayName(), value, headers),
                    Days = HeaderParser.GetIntValue(HeaderEnum.DAYS.DisplayName(), value, headers),
                    Pay = HeaderParser.GetDecimalValue(HeaderEnum.PAY.DisplayName(), value, headers),
                    Tip = HeaderParser.GetDecimalValue(HeaderEnum.TIP.DisplayName(), value, headers),
                    Bonus = HeaderParser.GetDecimalValue(HeaderEnum.BONUS.DisplayName(), value, headers),
                    Total = HeaderParser.GetDecimalValue(HeaderEnum.TOTAL.DisplayName(), value, headers),
                    Cash = HeaderParser.GetDecimalValue(HeaderEnum.CASH.DisplayName(), value, headers),
                    AmountPerTrip = HeaderParser.GetDecimalValue(HeaderEnum.AMOUNT_PER_TRIP.DisplayName(), value, headers),
                    Distance = HeaderParser.GetDecimalValue(HeaderEnum.DISTANCE.DisplayName(), value, headers),
                    AmountPerDistance = HeaderParser.GetDecimalValue(HeaderEnum.AMOUNT_PER_DISTANCE.DisplayName(), value, headers),
                    Time = HeaderParser.GetStringValue(HeaderEnum.TIME_TOTAL.DisplayName(), value, headers),
                    AmountPerTime = HeaderParser.GetDecimalValue(HeaderEnum.AMOUNT_PER_TIME.DisplayName(), value, headers),
                    Average = HeaderParser.GetDecimalValue(HeaderEnum.AVERAGE.DisplayName(), value, headers),
                    AmountPerDay = HeaderParser.GetDecimalValue(HeaderEnum.AMOUNT_PER_DAY.DisplayName(), value, headers),
                    Number = HeaderParser.GetIntValue(HeaderEnum.NUMBER.DisplayName(), value, headers),
                    Year = HeaderParser.GetIntValue(HeaderEnum.YEAR.DisplayName(), value, headers),
                    Begin = HeaderParser.GetDateValue(HeaderEnum.DATE_BEGIN.DisplayName(), value, headers),
                    End = HeaderParser.GetDateValue(HeaderEnum.DATE_END.DisplayName(), value, headers),
                };

                weeklyList.Add(weekly);
            }
            return weeklyList;
        }

        public static SheetModel GetSheet()
        {
            var sheet = new SheetModel
            {
                Name = SheetEnum.WEEKLY.DisplayName(),
                TabColor = ColorEnum.LIGHT_GREEN,
                CellColor = ColorEnum.LIGHT_GRAY,
                FreezeColumnCount = 1,
                FreezeRowCount = 1,
                ProtectSheet = true
            };

            var dailySheet = DailyMapper.GetSheet();

            sheet.Headers = SheetHelper.GetCommonTripGroupSheetHeaders(dailySheet, HeaderEnum.WEEK);
            var sheetKeyRange = sheet.GetLocalRange(HeaderEnum.WEEK);

            // #
            sheet.Headers.AddColumn(new SheetCellModel
            {
                Name = HeaderEnum.NUMBER.DisplayName(),
                Formula = $"=ARRAYFORMULA(IFS(ROW({sheetKeyRange})=1,\"{HeaderEnum.NUMBER.DisplayName()}\",ISBLANK({sheetKeyRange}), \"\",true,IFERROR(INDEX(SPLIT({sheetKeyRange}, \"-\"), 0,1), 0)))"
            });

            // Year
            sheet.Headers.AddColumn(new SheetCellModel
            {
                Name = HeaderEnum.YEAR.DisplayName(),
                Formula = $"=ARRAYFORMULA(IFS(ROW({sheetKeyRange})=1,\"{HeaderEnum.YEAR.DisplayName()}\",ISBLANK({sheetKeyRange}), \"\",true,IFERROR(INDEX(SPLIT({sheetKeyRange}, \"-\"), 0,2), 0)))"
            });

            // Begin
            sheet.Headers.AddColumn(new SheetCellModel
            {
                Name = HeaderEnum.DATE_BEGIN.DisplayName(),
                Formula = $"=ARRAYFORMULA(IFS(ROW({sheetKeyRange})=1,\"{HeaderEnum.DATE_BEGIN.DisplayName()}\",ISBLANK({sheetKeyRange}), \"\",true,DATE({sheet.GetLocalRange(HeaderEnum.YEAR)},1,1)+(({sheet.GetLocalRange(HeaderEnum.NUMBER)}-1)*7)-WEEKDAY(DATE({sheet.GetLocalRange(HeaderEnum.YEAR)},1,1),3)))",
                Format = FormatEnum.DATE
            });

            // End
            sheet.Headers.AddColumn(new SheetCellModel
            {
                Name = HeaderEnum.DATE_END.DisplayName(),
                Formula = $"=ARRAYFORMULA(IFS(ROW({sheetKeyRange})=1,\"{HeaderEnum.DATE_END.DisplayName()}\",ISBLANK({sheetKeyRange}), \"\",true,DATE({sheet.GetLocalRange(HeaderEnum.YEAR)},1,7)+(({sheet.GetLocalRange(HeaderEnum.NUMBER)}-1)*7)-WEEKDAY(DATE({sheet.GetLocalRange(HeaderEnum.YEAR)},1,1),3)))",
                Format = FormatEnum.DATE
            });

            return sheet;
        }
    }
}