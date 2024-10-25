using Microsoft.AspNetCore.Http;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Interface
{
    public interface IFileRepository
    {
        Task<string> SaveFileAsync(IFormFile file, string directory);
        Task<byte[]> GetFileAsync(string filePath);
        void DeleteFile(string filePath);
        bool FileExists(string filePath);
        string GetFileUrl(string filePath, HttpRequest request);
        Task<UserDto> UpdateProfileAsync(UpdateUserProfileDto updateUserProfileDto);
        bool ValidateFile(IFormFile file, string[] allowedExtensions, long maxSize);
    }
}
