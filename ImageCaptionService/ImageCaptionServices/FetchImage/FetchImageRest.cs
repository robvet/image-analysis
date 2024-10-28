//using Azure.Storage.Blobs;
//using ImageCaptionService.Contracts;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;

//namespace ImageCaptionService.ImageCaptionServices
//{
//    internal class FetchImageRest : IFetchImage
//    {
//        private readonly ILogger<FetchImage> _logger;
//        private readonly BlobClient _blobClient;
//        private readonly IConfiguration _configuration;

//        public FetchImageRest(ILogger<FetchImage> logger,
//                          BlobClient blobClient,
//                          IConfiguration configuration)
//        {
//            _logger = logger;
//            _blobClient = blobClient;
//            _configuration = configuration;
//        }

//        public async Task<string> FetchImageAsync(string imageName)
//        {
//            try
//            {
//                //// Get the container name from the configuration
//                var containerName = _configuration["blobstorage_containername"] ?? throw new ArgumentException("blobstorage_containername is missing");


//                var downloadInfo = await _blobClient.DownloadContentAsync();
//                var imageBytes = downloadInfo.Value.Content.ToArray();


//                // Create a BlobContainerClient
//                var blobContainerClient = new BlobContainerClient(_blobClient.Uri, new BlobClientOptions());

//                // Get a reference to the blob (image)
//                var blobClient = blobContainerClient.GetBlobClient(imageName);

//                if (!await blobClient.ExistsAsync())
//                {
//                    _logger.LogError($"Blob '{imageName}' does not exist.");
//                    throw new FileNotFoundException($"Blob '{imageName}' does not exist.");
//                }

//                // Download the blob to a memory stream
//                //MemoryStream memoryStream = new MemoryStream();
//                //await blobClient.DownloadToAsync(memoryStream);
//                //memoryStream.Position = 0;

//                byte[] imageBytes;
//                using (MemoryStream ms = new MemoryStream())
//                {
//                    await blobClient.DownloadToAsync(ms);
//                    imageBytes = ms.ToArray();
//                }

//                // Step 2: Convert image to Base64
//                string imageBase64 = Convert.ToBase64String(imageBytes);

//                return imageBase64;

//                // At this point, you have the image in the memoryStream
//                // You can now send the image to LLM for inference or Azure Computer Vision API
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, ex.Message);
//                throw;
//            }
//        }
//    }
//}
