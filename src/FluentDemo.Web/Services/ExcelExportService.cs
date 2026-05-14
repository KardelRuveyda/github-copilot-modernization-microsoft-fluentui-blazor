using ClosedXML.Excel;
using Microsoft.JSInterop;

namespace FluentDemo.Web.Services;

public class ExcelExportService
{
    private readonly IJSRuntime _js;
    public ExcelExportService(IJSRuntime js) => _js = js;

    public async Task ExportAsync<T>(string fileName, string sheetName, IEnumerable<T> rows, params (string Header, Func<T, object?> Selector)[] columns)
    {
        using var wb = new XLWorkbook();
        var ws = wb.AddWorksheet(sheetName);

        // Header
        for (int c = 0; c < columns.Length; c++)
        {
            var cell = ws.Cell(1, c + 1);
            cell.Value = columns[c].Header;
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#0078d4");
            cell.Style.Font.FontColor = XLColor.White;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
        }

        // Rows
        int r = 2;
        foreach (var row in rows)
        {
            for (int c = 0; c < columns.Length; c++)
            {
                var value = columns[c].Selector(row);
                var cell = ws.Cell(r, c + 1);
                switch (value)
                {
                    case null:           cell.Value = string.Empty; break;
                    case DateTime dt:    cell.Value = dt; cell.Style.NumberFormat.Format = "yyyy-mm-dd hh:mm"; break;
                    case DateOnly d:     cell.Value = d.ToDateTime(TimeOnly.MinValue); cell.Style.NumberFormat.Format = "yyyy-mm-dd"; break;
                    case decimal dec:    cell.Value = dec; cell.Style.NumberFormat.Format = "#,##0.00"; break;
                    case double dbl:     cell.Value = dbl; break;
                    case int i:          cell.Value = i; break;
                    case bool b:         cell.Value = b ? "Yes" : "No"; break;
                    default:             cell.Value = value.ToString(); break;
                }
            }
            r++;
        }

        ws.Columns().AdjustToContents();
        ws.SheetView.FreezeRows(1);
        ws.RangeUsed()!.SetAutoFilter();

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        var bytes = ms.ToArray();
        var base64 = Convert.ToBase64String(bytes);
        await _js.InvokeVoidAsync("fluentDemoDownload",
            fileName,
            "data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64," + base64);
    }
}
