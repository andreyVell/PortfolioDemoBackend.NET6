using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using MimeMapping;
using Services.Helpers;
using Services.Interfaces;
using Services.Models._BaseModels;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Services.Implementations
{
    public class FileProviderService : IFileProviderService
    {
        private readonly IGlobalSettings _globalSettings;
        private readonly Guid _currentUserOwnerId;
        public FileProviderService(
            IGlobalSettings globalSettings,
            ICurrentUserDataService currentUserService)
        {
            _globalSettings = globalSettings;
            _currentUserOwnerId = currentUserService.GetCurrentUserOwnerId();
        }
        public async Task<AttachFileModel?> GetFileDataUrlAsync(string? fileKey)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileKey)) return null;
                var result = new AttachFileModel();
                result.FileName = Path.GetFileName(fileKey);

                byte[] fileBytes = await GetObjectFromS3(fileKey);
                string base64String = Convert.ToBase64String(fileBytes);
                string mimeType = MimeUtility.GetMimeMapping(fileKey);

                result.FileContent = $"data:{mimeType};base64,{base64String}";
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + '\n' + ex.StackTrace);
                return null;
            }
            
        }

        public async Task<string> SaveFileAsync(AttachFileModel file)
        {
            try
            {
                string fullFileKey = string.Empty;
                if (file != null && !string.IsNullOrWhiteSpace(file.FileContent) && !string.IsNullOrWhiteSpace(file.FileName))
                {
                    var base64Data = Regex.Match(file.FileContent, @"data:(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                    var fileData = Convert.FromBase64String(base64Data);
                    var md5hash = MD5.Create();
                    byte[] inputBytes = Encoding.ASCII.GetBytes(file.FileName + Guid.NewGuid());
                    byte[] hashBytes = md5hash.ComputeHash(inputBytes);
                    string hash = Convert.ToHexString(hashBytes);
                    fullFileKey = $"OWNER-{_currentUserOwnerId}-FILE-{hash.Substring(0, 10)}.{file.FileName.Split('.').LastOrDefault()}";
                    await SaveObjectToS3(fullFileKey, fileData);
                }
                return _globalSettings.S3BucketName + '/' + fullFileKey;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message + '\n' + ex.StackTrace);
                return string.Empty;
            }
            
        }

        public async Task<string> CompressAndSaveImageAsync(AttachFileModel file, int newWidth, int newHeight)
        {
            try
            {
                string fullFileKey = string.Empty;
                if (file != null && !string.IsNullOrWhiteSpace(file.FileContent) && !string.IsNullOrWhiteSpace(file.FileName))
                {
                    var base64Data = Regex.Match(file.FileContent, @"data:(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                    var fileData = Convert.FromBase64String(base64Data);
                    var md5hash = MD5.Create();
                    byte[] inputBytes = Encoding.ASCII.GetBytes(file.FileName + Guid.NewGuid());
                    byte[] hashBytes = md5hash.ComputeHash(inputBytes);
                    string hash = Convert.ToHexString(hashBytes);
                    fullFileKey = $"OWNER-{_currentUserOwnerId}-FILE-{hash.Substring(0, 10)}.{file.FileName.Split('.').LastOrDefault()}"; 
                    await SaveObjectToS3(fullFileKey, await ImageFormatter.CompressImageAsync(fileData, newWidth, newHeight), _globalSettings.S3BucketForSmallImagesName);
                }
                return _globalSettings.S3BucketForSmallImagesName + '/' + fullFileKey;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + '\n' + ex.StackTrace);
                return string.Empty;
            }

        }

        public async Task DeleteAsync(string? fileKey)
        {
            await DeleteObjectFromS3(fileKey);
        }

        public async Task DeleteFilesAsync(IEnumerable<string> filesKeys)
        {
            await DeleteObjectsFromS3(filesKeys);
        }





        private async Task SaveObjectToS3(string fileKey, byte[] fileData, string? bucketName = null)
        {
            using (var ms = new MemoryStream(fileData))
            {
                var putRequest = new PutObjectRequest
                {
                    BucketName = bucketName ?? _globalSettings.S3BucketName,
                    Key = fileKey,
                    InputStream = ms
                };

                AmazonS3Config configsS3 = new AmazonS3Config
                {
                    ServiceURL = _globalSettings.S3ServiceUrl,
                };

                AWSCredentials awsCredentials = new BasicAWSCredentials(_globalSettings.S3AccessKey, _globalSettings.S3SecretKey);
                using (AmazonS3Client s3client = new AmazonS3Client(awsCredentials, configsS3))
                {
                    var response = await s3client.PutObjectAsync(putRequest);
                }
            }
        }        

        private async Task DeleteObjectFromS3(string? fileKey)
        {
            if (string.IsNullOrEmpty(fileKey)) return;
            var fileData = fileKey.Split('/');
            AmazonS3Config configsS3 = new AmazonS3Config
            {
                ServiceURL = _globalSettings.S3ServiceUrl,
            };

            AWSCredentials awsCredentials = new BasicAWSCredentials(_globalSettings.S3AccessKey, _globalSettings.S3SecretKey);
            using (AmazonS3Client s3client = new AmazonS3Client(awsCredentials, configsS3))
            {
                var response = await s3client.DeleteObjectAsync(fileData[0], fileData[1]);
            }
        }

        private async Task DeleteObjectsFromS3(IEnumerable<string> filesKeys)
        {
            var fileKeysGroupByBucketName = filesKeys.GroupBy(e => e.Split('/')[0]);
            foreach (var fileKeyGroup in fileKeysGroupByBucketName)
            {
                var fileKeys = fileKeyGroup.Select(e => e.Split('/')[1]);
                AmazonS3Config configsS3 = new AmazonS3Config
                {
                    ServiceURL = _globalSettings.S3ServiceUrl,
                };

                DeleteObjectsRequest multiObjectDeleteRequest = new DeleteObjectsRequest
                {
                    BucketName = fileKeyGroup.Key,
                    Objects = fileKeys.Select(e => new KeyVersion() { Key = e, VersionId = string.Empty }).ToList(),
                };

                AWSCredentials awsCredentials = new BasicAWSCredentials(_globalSettings.S3AccessKey, _globalSettings.S3SecretKey);
                using (AmazonS3Client s3client = new AmazonS3Client(awsCredentials, configsS3))
                {
                    var response = await s3client.DeleteObjectsAsync(multiObjectDeleteRequest);
                }
            }            
        }

        private async Task<byte[]> GetObjectFromS3(string fileKey)
        {
            byte[] resultBytes;
            var fileData = fileKey.Split('/');
            var request = new GetObjectRequest
            {
                BucketName = fileData[0],
                Key = fileData[1],
            };

            // Issue request and remember to dispose of the response
            AmazonS3Config configsS3 = new AmazonS3Config
            {
                ServiceURL = _globalSettings.S3ServiceUrl,
            };

            AWSCredentials awsCredentials = new BasicAWSCredentials(_globalSettings.S3AccessKey, _globalSettings.S3SecretKey);
            using (AmazonS3Client s3client = new AmazonS3Client(awsCredentials, configsS3))
            {
                GetObjectResponse response = await s3client.GetObjectAsync(request);                
                using (var binaryReader = new BinaryReader(response.ResponseStream))
                {                    
                    //2gb max limit 
                    resultBytes = binaryReader.ReadBytes((int)response.ResponseStream.Length);
                }
            }
            return resultBytes;
        }
    }
}
