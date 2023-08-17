using BlazorInputFile;

namespace TsiErp.ErpUI.Services
{
    public interface IFileUploadService
    {
        Task<string> UploadTechnicalDrawing(IFileListEntry file, string rootPath, string fileName);
        Task<string> UploadTechnicalDrawingPDF(IFileListEntry file, string rootPath, string fileName);
        Task<string> UploadOperationPicture(IFileListEntry file, string rootPath, string fileName);
        Task<string> UploadOperationPicturePDF(IFileListEntry file, string rootPath, string fileName);

        string GetRootPath();

    }
}
