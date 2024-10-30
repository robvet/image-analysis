//using Azure;
//using Azure.AI.OpenAI;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using OpenAI.Chat;
//using ImageCaptionService.Contracts;
//using System.Net.Http.Headers;
//using ValidateImageCaptionAPI.Models;
//using ImageCaptionService.ImageCaptionServices.Prompts;
//using System.Net;


//using System.IO;
//using System.Net.Http;
//using System.Threading.Tasks;
//using System.Reflection;

//namespace ImageCaptionService.ImageCaptionServices.InferCaption
//{
//    internal class InferCaption : IInferCaption
//    {
//        private readonly ILogger<InferCaption> _logger;
//        //private readonly ITokenManager _tokenManager;
//        private readonly IConfiguration _configuration;
//        private readonly string _endpoint;
//        private readonly string _key;
//        private readonly string _deploymentOrModelName4;
//        private readonly string _visionDeploymentModelName;
//        private readonly string _deploymentOrModelNameSecondary;
//        private readonly float _temperature;
//        private readonly float _nucleus;
//        private int _maxTokens = 4096; // Max tokens represents token limit for entire completion: Input and output

//        private int _maxOutputTokens = 2048;

//        public InferCaption(IConfiguration configuration,
//                                ILogger<InferCaption> logger)
//        {
//            _logger = logger;

//            _configuration = configuration;

//            // Get the Azure OpenAI Service configuration values
//            _endpoint = _configuration["ai-endpoint"]
//                ?? throw new ArgumentException("ai-endpoint is Missing");

//            _visionDeploymentModelName = _configuration["ai-visionmodelname"]
//                ?? throw new ArgumentException("ai-visionmodelname is Missing");

//            _deploymentOrModelName4 = _configuration["ai-primarymodelname"]
//                ?? throw new ArgumentException("ai-primarymodelname is Missing");

//            _deploymentOrModelNameSecondary = _configuration["ai-secondarymodelname"]
//                ?? throw new ArgumentException("ai-secondarymodelname is Missing");

//            _temperature = float.Parse(_configuration["ai-temperature"]
//                ?? throw new ArgumentException("ai-temperature"));

//            _nucleus = float.Parse(_configuration["ai-nucleus"]
//                ?? throw new ArgumentException("ai-nucleus"));

//            _key = _configuration["ai-aikey"]
//             ?? throw new ArgumentException("ai-aikey");

//            _maxTokens = Convert.ToInt32(_configuration["ai-maxtokens"]
//                ?? throw new ArgumentException("ai-maxtokens is Missing"));

//            _maxOutputTokens = Convert.ToInt32(_configuration["ai-maxoutputtokens"]
//                ?? throw new ArgumentException("ai-maxoutputtokens is Missing"));
//        }

//        public async Task<ImageCaptionResult> InferImageCaptionAsync(byte[] imageBytes)
//        {
//            try
//            {

//                // Set up HttpClient
//                var httpClient = new HttpClient();
//                //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _key);
//                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_key}");
//                httpClient.DefaultRequestHeaders.Add("api-key", _key);

//                // Create request with multipart form-data
//                //var formData = new MultipartFormDataContent();
//                //var imageContent = new ByteArrayContent(imageBytes);
//                //imageContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
//                //formData.Add(imageContent, "file", "image.png");

//                //formData.Add(new StringContent("You are an assistant that identifies objects in images."), "system");
//                //formData.Add(new StringContent("Please identify the main object in the provided image."), "user");


//                //// Create request with multipart form-data
//                var formData = new MultipartFormDataContent();

//                var imageContent = new ByteArrayContent(imageBytes);
//                imageContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
//                formData.Add(imageContent, "file", "image.png");

//                formData.Add(new StringContent("You are an assistant that identifies objects in images."), "system");
//                //formData.Add(new ByteArrayContent(imageBytes), "file", "image.png");
//                formData.Add(new StringContent("Please identify the main object in the provided image."), "user");

//                //https://ai-openai-playground-west.openai.azure.com/openai/deployments/gpt-4-vision/chat/completions?api-version=2024-08-01-preview

//                var serviceCall = $"{_endpoint}openai/deployments/{_visionDeploymentModelName}/chat/completions?api-version=2024-08-01-preview";

//                // Log the request details
//                _logger.LogInformation($"Service Call: {serviceCall}");
//                _logger.LogInformation($"Headers: {string.Join(", ", httpClient.DefaultRequestHeaders)}");
//                _logger.LogInformation($"Form Data: {formData}");




//                var response = await httpClient.PostAsync(serviceCall, formData);


//                // Log the status code
//                _logger.LogInformation($"Response Status Code: {response.StatusCode}");

//                // Log the headers
//                foreach (var header in response.Headers)
//                {
//                    _logger.LogInformation($"{header.Key}: {string.Join(", ", header.Value)}");
//                }


//                var responseContent = await response.Content.ReadAsStringAsync();

//                //return responseContent;



//                ////var encodedImage = Convert.ToBase64String(imageBytes);

//                ////using (var client = new HttpClient())
//                ////{
//                ////    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _key);
//                ////    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

//                ////    // Create the form content
//                ////    using (var content = new MultipartFormDataContent())
//                ////    {
//                ////        // Add the image file
//                ////        content.Add(new ByteArrayContent(imageBytes), "image", "image.jpg");


//                ////        content.Add(new StringContent(_visionDeploymentModelName), "model");

//                ////        // Add any text prompt if needed
//                ////        content.Add(new StringContent("Analyze this image"), "prompt");

//                ////        // Send the POST request
//                ////        var response = await client.PostAsync(_endpoint, content);
//                ////        response.EnsureSuccessStatusCode(); // Throws for non success codes

//                ////        var responseString = await response.Content.ReadAsStringAsync();

//                ////        Console.WriteLine(responseString);
//                ////    }
//                //}



//                //    // Instatiate OpenAIClient
//                //    AzureOpenAIClient client = new(
//                //    new Uri(_endpoint),
//                //    new AzureKeyCredential(_key));

//                //ChatClient chatClient = client.GetChatClient(_deploymentOrModelName4);

//                //var messages = new List<ChatMessage>();
//                //messages.Add(new SystemChatMessage(PromptTemplates.SystemPromptTemplate));

//                //messages.Add(new UserChatMessage(encodedImage));

//                ////messages.Add(new UserChatMessage(PromptTemplates.MainPromptTemplate.Replace("{{$base64_string}}", encodedImage)));
//                //messages.Add(new UserChatMessage(PromptTemplates.MainPromptTemplate));

//                //ChatCompletionOptions completionOptions = new ChatCompletionOptions
//                //{

//                //    Temperature = _temperature,
//                //    //MaxTokens = _maxTokens,
//                //    TopP = _nucleus,
//                //};

//                //ChatCompletion chatCompletion = chatClient.CompleteChat(messages, completionOptions);
//                ////var response = chatCompletion.Content[^1].Text;

//                //var probablityInfo = chatCompletion.ContentTokenLogProbabilities;

//                //var ContentFilterReason = chatCompletion.FinishReason;

//                //var response = chatCompletion.Content[0].Text;

//                //if (string.IsNullOrEmpty(response))
//                //{
//                //    response = "No Response";
//                //}




//                //// Set up HttpClient
//                //var httpClient = new HttpClient();
//                ////httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _key);
//                ////httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_key}");

//                //httpClient.DefaultRequestHeaders.Add("api-key", _key);

//                //// Create request with multipart form-data
//                //var formData = new MultipartFormDataContent();
//                //var imageContent = new ByteArrayContent(imageBytes);

//                ////imageContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
//                ////imageContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

//                //formData.Add(new StringContent("You are an assistant that identifies objects in images."), "system");

//                //formData.Add(imageContent, "file", "image.png");


//                //formData.Add(new StringContent("Please identify the main object in the provided image."), "user");


//                ////// Create request with multipart form-data
//                ////var formData = new MultipartFormDataContent();
//                ////formData.Add(new ByteArrayContent(imageBytes), "file", "image.png");
//                ////formData.Add(new StringContent("You are an assistant that identifies objects in images."), "system");
//                ////formData.Add(new StringContent("Please identify the main object in the provided image."), "user");


//                ////https://ai-openai-playground-west.openai.azure.com/openai/deployments/gpt-4-vision/chat/completions?api-version=2024-08-01-preview



//                //var serviceCall = $"{_endpoint}openai/deployments/{_visionDeploymentModelName}/chat/completions?api-version=2024-08-01-preview";
//                //var response = await httpClient.PostAsync(serviceCall, formData);


//                //// Log the status code
//                //_logger.LogInformation($"Response Status Code: {response.StatusCode}");

//                //// Log the headers
//                //foreach (var header in response.Headers)
//                //{
//                //    _logger.LogInformation($"{header.Key}: {string.Join(", ", header.Value)}");
//                //}


//                //var responseContent = await response.Content.ReadAsStringAsync();

//                //return responseContent;

//                //// Send the POST request
//                ////var response = await httpClient.PostAsync("https://<your-endpoint>.openai.azure.com/openai/deployments/<your-deployment>/chat/completions?api-version=2023-09-01-preview", formData);
//                ////var response = await httpClient.PostAsync($"{_endpoint}openai/deployments/{_visionDeploymentModelName}/chat/completions?api-version=2023-09-01-preview", formData);
//            }
//            catch (Exception ex)
//            {
//                var errorMessage = $"Exception throw in InferCaption.InferImageCaptionAsync: {ex.Message}";
//                _logger.LogError(errorMessage);
//                throw;
//            }

//            return new ImageCaptionResult("", 0);




//            //// Instatiate OpenAIClient
//            //AzureOpenAIClient client = new(
//            //    new Uri(_endpoint),
//            //    new AzureKeyCredential(_key));

//            //ChatClient chatClient = client.GetChatClient(_deploymentOrModelName4);

//            ////imageBase64 = imageBase64.Replace("data:image/jpeg;base64,", base64);
//            //imageBase64 = string.Concat("data:image/jpeg;base64,", base64);

//            //var messages = new List<ChatMessage>();
//            //messages.Add(new SystemChatMessage(PromptTemplates.SystemPromptTemplate));
//            //messages.Add(new UserChatMessage(PromptTemplates.MainPromptTemplate.Replace("{{$base64_string}}", imageBase64)));

//            //ChatCompletionOptions completionOptions = new ChatCompletionOptions
//            //{
//            //    Temperature = _temperature,
//            //    //MaxTokens = _maxTokens,
//            //    TopP = _nucleus,
//            //};

//            //ChatCompletion chatCompletion = chatClient.CompleteChat(messages, completionOptions);

//            //var ContentFilterReason = chatCompletion.FinishReason;

//            //return chatCompletion.Content[0].Text;

//        }
//    }
//}
