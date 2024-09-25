﻿using RaptorLoggerEngine.Enums;
using RaptorLoggerEngine.Models;
using RaptorLoggerEngine.Utilities.Extensions;
using RLE.Core.Enums;

namespace RaptorLoggerEngine.Tests.Data;

public class TestSheetData
{
    public static SheetModel GetModelData()
    {
        var sheet = new SheetModel
        {
            Name = "Test Sheet",
            Headers = []
        };

        sheet.Headers.AddColumn(new SheetCellModel
        {
            Name = HeaderEnum.WEEK.DisplayName(),
            Formula = "Formula",
            Format = FormatEnum.TEXT
        });

        sheet.Headers.AddColumn(new SheetCellModel
        {
            Name = HeaderEnum.DATE.DisplayName(),
            Formula = "None",
            Format = FormatEnum.NUMBER
        });

        return sheet;
    }
}