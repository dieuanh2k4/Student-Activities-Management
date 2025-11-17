using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Minio;
using Minio.DataModel.Args;
using StudentActivities.src.Services.Interfaces;

namespace StudentActivities.src.Services.Implements
{
    public class MinioStorageService : IStorageService
    {
        private readonly IMinioClient _minioClient;
        private readonly IConfiguration _configuration;
        private readonly string _bucketName;

        public MinioStorageService(IMinioClient minioClient, IConfiguration configuration)
        {
            _minioClient = minioClient;
            _configuration = configuration;
            _bucketName = _configuration["Minio:BucketName"] ?? "studentactivities";
        }

        public async Task<string> UploadImageAsync(IFormFile file, string folder = "images")
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File không tồn tại");
            }

            // Đảm bảo bucket tồn tại
            var bucketExists = await _minioClient.BucketExistsAsync(
                new BucketExistsArgs().WithBucket(_bucketName)
            );

            if (!bucketExists)
            {
                await _minioClient.MakeBucketAsync(
                    new MakeBucketArgs().WithBucket(_bucketName)
                );
            }

            // Tạo tên file unique
            var fileExtension = Path.GetExtension(file.FileName);
            var fileName = $"{folder}/{Guid.NewGuid()}{fileExtension}";

            // Upload file
            using var stream = file.OpenReadStream();
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileName)
                .WithStreamData(stream)
                .WithObjectSize(file.Length)
                .WithContentType(file.ContentType);

            await _minioClient.PutObjectAsync(putObjectArgs);

            return fileName;
        }

        public async Task<bool> DeleteImageAsync(string fileName)
        {
            try
            {
                var removeObjectArgs = new RemoveObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(fileName);

                await _minioClient.RemoveObjectAsync(removeObjectArgs);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetImageUrl(string fileName)
        {
            // Dùng PublicEndpoint để client bên ngoài có thể truy cập
            var endpoint = _configuration["Minio:PublicEndpoint"] ?? _configuration["Minio:Endpoint"];
            var useSsl = _configuration.GetValue<bool>("Minio:UseSsl");
            var protocol = useSsl ? "https" : "http";
            
            return $"{protocol}://{endpoint}/{_bucketName}/{fileName}";
        }
    }
}
