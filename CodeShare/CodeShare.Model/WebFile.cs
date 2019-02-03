using Newtonsoft.Json;
using CodeShare.Services;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using CodeShare.Utilities;

namespace CodeShare.Model
{
    public class WebFile : Entity, IWebFile
    {
        public string Extension { get; set; }
        public string Description { get; set; }

        [NotMapped, JsonIgnore]
        public Uri Url => new Uri($@"{FtpService.RootDirectoryHttp}/{Path}");

        [NotMapped, JsonIgnore]
        public string Path => $"{Uid.ToString()}{Extension}";

        [NotMapped, JsonIgnore]
        public bool Exist => FtpService.Exist(Path);

        public WebFile()
        {
        }

        public WebFile(byte[] fileInBytes, string extension)
        {
            Extension = extension;

            if (!Exist)
            {
                Upload(fileInBytes);
            }
        }

        public bool Upload(byte[] fileInBytes)
        {
            Logger.WriteLine($"Uploading {fileInBytes.Length} bytes at path '{Path}' to FTP web server...");

            if (fileInBytes == null || fileInBytes.Length == 0)
            {
                Logger.WriteLine($"Failed to upload file: {Path}. No bytes were supplied.");
                return false;
            }
            if (!FtpService.Upload(fileInBytes, Path))
            {
                Logger.WriteLine($"Failed to upload file: {Path}. Bad server response.");
                return false;
            }

            Logger.WriteLine($"The bytes was successfully uploaded as '{Path}'!");

            Updated = DateTime.Now;
            return true;
        }

        public async Task<bool> UploadAsync(byte[] fileInBytes)
        {
            Logger.WriteLine($"Uploading {fileInBytes.Length} bytes at path '{Path}' to FTP web server...");

            if (fileInBytes == null || fileInBytes.Length == 0)
            {
                Logger.WriteLine($"Failed to upload file: {Path}. No bytes were supplied.");
                return false;
            }
            if (await FtpService.UploadAsync(fileInBytes, Path) == false)
            {
                Logger.WriteLine($"Failed to upload file: {Path}. Bad server response.");
                return false;
            }

            Logger.WriteLine($"The bytes was successfully uploaded as '{Path}'!");

            Updated = DateTime.Now;
            return true;
        }

        public byte[] Download() => FtpService.Download(Path);

        public async Task<byte[]> DownloadAsync() => await FtpService.DownloadAsync(Path);

        public bool Delete() => FtpService.Delete(Path);

        public async Task<bool> DeleteAsync() => await FtpService.DeleteAsync(Path);

        public override bool Equals(object obj) => obj is WebFile file && file.Uid.Equals(Uid);

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
