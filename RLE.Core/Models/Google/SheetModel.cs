using RLE.Core.Enums;

namespace RLE.Core.Models.Google;

public class SheetModel
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public List<SheetCellModel> Headers { get; set; } = [];
    public ColorEnum TabColor { get; set; }
    public ColorEnum CellColor { get; set; }
    public bool ProtectSheet { get; set; }
    public int FreezeColumnCount { get; set; }
    public int FreezeRowCount { get; set; }
}