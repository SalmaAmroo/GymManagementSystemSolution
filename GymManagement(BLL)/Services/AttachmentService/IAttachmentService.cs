using Microsoft.AspNetCore.Http;

namespace GymManagementBLL.Services.AttachmentService
{
    public interface IAttachmentService
    {
        string? Upload(string FolderName, IFormFile file);
        bool Delete(string FolderName, string FileName);

    }
}
