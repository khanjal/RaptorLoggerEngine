namespace GigRaptorLib.Utilities
{
    public static class ArrayFormulaHelper
    {
        public static string ArrayFormulaCountIf(string keyRange, string headerText, string countRange)
        {
            return $"=ARRAYFORMULA(IFS(ROW({keyRange})=1,\"{headerText}\",ISBLANK({keyRange}), \"\",true,COUNTIF({countRange},{keyRange})))";
        }

        public static string ArrayFormulaAmountPer(string keyRange, string headerText)
        {
            //return $"=ARRAYFORMULA(IFS(ROW({keyRange})=1,\"{headerText}\",ISBLANK({keyRange}), \"\", {sheet.GetLocalRange(HeaderEnum.TOTAL)} = 0, 0,true,{sheet.GetLocalRange(HeaderEnum.TOTAL)}/IF({sheet.GetLocalRange(HeaderEnum.TRIPS)}=0,1,{sheet.GetLocalRange(HeaderEnum.TRIPS)})))";
            return "";
        }

        public static string ArrayFormulaSumIf(string keyRange, string headerText, string range, string sumRange)
        {
            return $"=ARRAYFORMULA(IFS(ROW({keyRange})=1,\"{headerText}\",ISBLANK({keyRange}), \"\",true,SUMIF({range},{keyRange},{sumRange})))";
        }

        public static string ArrayFormulaTotal(string keyRange, string headerText, string payRange, string tipRange, string bonusRange)
        {
            return $"=ARRAYFORMULA(IFS(ROW({keyRange})=1,\"{headerText}\",ISBLANK({keyRange}), \"\",true,{payRange}+{tipRange}+{bonusRange}))";
        }

        public static string ArrayFormulaVisit(string keyRange, string headerText, string referenceSheet, string columnStart, string columnEnd, bool first)
        {
            return $"=ARRAYFORMULA(IFS(ROW({keyRange})=1,\"{headerText}\",ISBLANK({keyRange}), \"\", true, IFERROR(VLOOKUP({keyRange},SORT(QUERY({referenceSheet}!{columnStart}:{columnEnd},\"SELECT {columnEnd}, {columnStart}\"),2,{first}),2,0),\"\")))";
        }

        public static string ArrayForumlaUnique(string keyRange, string headerText)
        {
            return "={\"" + headerText + "\";SORT(UNIQUE({" + keyRange + "}))}";
        }

        public static string ArrayForumlaUniqueFilter(string keyRange, string headerText)
        {
            return "={\"" + headerText + "\";UNIQUE(IFERROR(FILTER(" + keyRange + "," + keyRange + "<>\"\")))}";
        }

        public static string ArrayForumlaUniqueFilterSort(string keyRange, string headerText)
        {
            return "={\"" + headerText + "\";SORT(UNIQUE(IFERROR(FILTER(" + keyRange + "," + keyRange + "<>\"\"))))}";
        }
    }
}