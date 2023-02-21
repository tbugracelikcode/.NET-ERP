using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.DataAccess.Services.Login;

namespace TsiErp.ErpUI.Services
{
    [ServiceRegistration(typeof(IExcelService), DependencyInjectionType.Scoped)]
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
