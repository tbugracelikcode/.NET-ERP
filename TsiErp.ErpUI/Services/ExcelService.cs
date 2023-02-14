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
            string path = Path.Combine(_hostEnvironment.WebRootPath, filename);
            return path;
        }
        
    }
}
