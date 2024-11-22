﻿using RLE.Core.Wrappers;

namespace RLE.Core.Services;

public interface IGoogleDriveService
{
    public Task<IList<string>> GetSheetFiles();
}

public class GoogleDriveService : IGoogleDriveService
{
    private DriveServiceWrapper _driveService;

    public GoogleDriveService(string accessToken)
    {
        _driveService = new DriveServiceWrapper(accessToken);
    }

    public GoogleDriveService(Dictionary<string, string> parameters)
    {
        _driveService = new DriveServiceWrapper(parameters);
    }

    public async Task<IList<string>> GetSheetFiles()
    {
        var sheets = await _driveService.GetSheetFiles();

        var sheetList = sheets.Select(s => s.Name.ToString()).ToList();

        return sheetList;
    }
}