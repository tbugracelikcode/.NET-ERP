using BlazorInputFile;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.IO;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;

namespace TsiErp.ErpUI.Services
{
    [ServiceRegistration(typeof(IFileUploadService), DependencyInjectionType.Scoped)]
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileUploadService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public string GetRootPath()
        {
            string rootpath = _webHostEnvironment.WebRootPath;
            return rootpath;
        }

        public async Task<string> UploadTechnicalDrawing(IFileListEntry file, string rootPath, string fileName)
        {
            string result = "";

            try
            {
                rootPath = _webHostEnvironment.WebRootPath + "/" + rootPath;

                if (!Directory.Exists(rootPath))
                {
                    Directory.CreateDirectory(rootPath);
                }

                var path = Path.Combine(_webHostEnvironment.WebRootPath, rootPath, fileName);

                var memoryStream = new MemoryStream();

                await file.Data.CopyToAsync(memoryStream);

                using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    memoryStream.WriteTo(fileStream);


                    result = "Successful";
                }
            }
            catch (Exception)
            {

                var path = Path.Combine(_webHostEnvironment.WebRootPath, rootPath, fileName);
                File.Delete(path);
                result = "Error";
            }

            return result;
        }

        public async Task<string> UploadTechnicalDrawingPDF(IFileListEntry file, string rootPath, string fileName)
        {
            string result = "";

            try
            {
                rootPath = _webHostEnvironment.WebRootPath + "/" + rootPath;

                if (!Directory.Exists(rootPath))
                {
                    Directory.CreateDirectory(rootPath);
                }

                var path = Path.Combine(_webHostEnvironment.WebRootPath, rootPath, fileName);

                var memoryStream = new MemoryStream();

                await file.Data.CopyToAsync(memoryStream);

                using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    memoryStream.WriteTo(fileStream);


                    result = "Successful";
                }
            }
            catch (Exception)
            {

                var path = Path.Combine(_webHostEnvironment.WebRootPath, rootPath, fileName);
                File.Delete(path);
                result = "Error";
            }

            return result;
        }

        public async Task<string> UploadOperationPicture(byte[] file, string rootPath, string fileName)
        {
            string result = "";

            try
            {
                rootPath = _webHostEnvironment.WebRootPath + "/" + rootPath;

                if (!Directory.Exists(rootPath))
                {
                    Directory.CreateDirectory(rootPath);
                }

                var path = Path.Combine(_webHostEnvironment.WebRootPath, rootPath, fileName);

                File.WriteAllBytes(path, file);
            }
            catch (Exception)
            {
                var path = Path.Combine(_webHostEnvironment.WebRootPath, rootPath, fileName);
                File.Delete(path);
                result = "Error";
            }

            return result;
        }

        public async Task<string> UploadOperationPicturePDF(IFileListEntry file, string rootPath, string fileName)
        {
            string result = "";

            try
            {
                rootPath = _webHostEnvironment.WebRootPath + "/" + rootPath;

                if (!Directory.Exists(rootPath))
                {
                    Directory.CreateDirectory(rootPath);
                }

                var path = Path.Combine(_webHostEnvironment.WebRootPath, rootPath, fileName);

                var memoryStream = new MemoryStream();

                await file.Data.CopyToAsync(memoryStream);

                using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    memoryStream.WriteTo(fileStream);


                    result = "Successful";
                }
            }
            catch (Exception)
            {

                var path = Path.Combine(_webHostEnvironment.WebRootPath, rootPath, fileName);
                File.Delete(path);
                result = "Error";
            }

            return result;
        }
    }
}
