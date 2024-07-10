using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Syncfusion.XlsIO;
using System.Data;
using System.Dynamic;

namespace TsiErp.ErpUI.Pages.TestPages.ExcelImportDataGrid
{
    public partial class ExcelImportDataGrid
    {
        SfGrid<ExpandoObject> Grid;
        public DataTable table = new DataTable(); 
        private void OnChange(UploadChangeEventArgs args)
        {
            foreach (var file in args.Files)
            {
                #region Expando Object Örneği

                var path = file.FileInfo.Name;
                ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Excel2016;

                var check = ExcelService.ImportGetPath(path);
                FileStream openFileStream = new FileStream(check, FileMode.OpenOrCreate);
                file.Stream.WriteTo(openFileStream);
                FileStream fileStream = new FileStream(check, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                IWorkbook workbook = application.Workbooks.Open(fileStream);
                IWorksheet worksheet = workbook.Worksheets[1];
                table = worksheet.ExportDataTable(worksheet.UsedRange, ExcelExportDataTableOptions.ColumnNames);
                GenerateListFromTable(table);

                #endregion

            }
        }
        string[] Columns;
        public List<ExpandoObject> CustomerList;
        public void GenerateListFromTable(DataTable input)
        {
            var list = new List<ExpandoObject>();
            Columns = input.Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToArray();
            foreach (DataRow row in input.Rows)
            {
                ExpandoObject e = new ExpandoObject();
                foreach (DataColumn col in input.Columns)
                    e.TryAdd(col.ColumnName, row.ItemArray[col.Ordinal]);
                list.Add(e);
            }
            CustomerList = list;
        }


    }
}
