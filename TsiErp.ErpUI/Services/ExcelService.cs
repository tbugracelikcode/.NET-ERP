using Syncfusion.XlsIO;
using System.Data;

namespace TsiErp.ErpUI.Services
{
    public class ExcelService : IExcelService
    {
        private IWebHostEnvironment _hostEnvironment;

        public ExcelService(IWebHostEnvironment environment)
        {
            _hostEnvironment = environment;
        }

       

        public string GetPath(string filename)
        {
            string filePath = _hostEnvironment.WebRootPath + @"\" + "ExportedGridFiles";
            string path = Path.Combine(filePath, filename);
            return path;
        }

       

    }
}
