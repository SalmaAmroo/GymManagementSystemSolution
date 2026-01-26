using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace GymManagementBLL.Services.AttachmentService
{
    public class AttachmentService : IAttachmentService
    {
        #region Steps to implement file upload:
        //1- Check Extension
        //2- Check Size
        //3- Get Located Folder Path
        //4- Make Attachment Name Unique -- Guid
        //5- Get File Path
        //6- Create File Stream To Copy File [Unmanaged]
        //7- Use Stream to Copy File 
        //8- Return File Name To Store in Database
        #endregion

        public AttachmentService(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png" };
        private readonly long _maxFileSize = 5 * 1024 * 1024;
        private readonly IWebHostEnvironment _webHost;
        public string? Upload(string FolderName, IFormFile file)
        {
            try
            {
                if (FolderName is null || file is null || file.Length == 0) return null;
                if (file.Length > _maxFileSize) return null;
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!_allowedExtensions.Contains(extension)) return null;

                var folderPath = Path.Combine(_webHost.WebRootPath, "images", FolderName);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(folderPath, fileName);
                using var stream = new FileStream(filePath, FileMode.Create);

                file.CopyTo(stream);

                return fileName;
            }
            catch (Exception ex)
            { 
                Console.WriteLine($"Failed To Upload File To Folder = {FolderName} : {ex}");
                return null;
            }
        }

        public bool Delete(string FolderName, string FileName)
        {
            try 
            {
                if (string.IsNullOrEmpty(FileName) || string.IsNullOrEmpty(FolderName)) return false;
                var filePath = Path.Combine(_webHost.WebRootPath, "images", FolderName, FileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed To Delete File From Folder = {FolderName} : {ex}");
                return false;
            }
        }

       
    }
}
