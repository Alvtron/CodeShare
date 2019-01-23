using System;
using System.Threading.Tasks;

namespace CodeShare.Model
{
    public interface IWebFile
    {
        string Description { get; set; }
        bool Exist { get; }
        string Extension { get; set; }
        string Path { get; }
        Uri Url { get; }

        bool Delete();
        Task<bool> DeleteAsync();
        byte[] Download();
        Task<byte[]> DownloadAsync();
        bool Equals(object obj);
        int GetHashCode();
        bool Upload(byte[] fileInBytes);
        Task<bool> UploadAsync(byte[] fileInBytes);
    }
}