using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenAI.Chat;
using ImageCaptionService.Contracts;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using ImageCaptionService.ImageCaptionServices.Prompts;
using Shared.Models;

namespace ImageCaptionService.ImageCaptionServices.InferCaption
{
    internal class InferCaption : IInferCaption
    {
        private readonly ILogger<InferCaption> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _endpoint;
        private readonly string _key;
        private readonly string _deploymentOrModelName4;
        private readonly string _visionDeploymentModelName;
        private readonly string _deploymentOrModelNameSecondary;
        private readonly float _temperature;
        private readonly float _nucleus;
        private readonly int _maxTokens = 4096; // Max tokens represents token limit for entire completion: Input and output
        private readonly int _maxOutputTokens = 2048;

        public InferCaption(IConfiguration configuration,
                            ILogger<InferCaption> logger)
        {
            _logger = logger;

            _configuration = configuration;

            // Get the Azure OpenAI Service configuration values
            _endpoint = _configuration["ai-endpoint"]
                ?? throw new ArgumentException("ai-endpoint is Missing");

            _visionDeploymentModelName = _configuration["ai-visionmodelname"]
                ?? throw new ArgumentException("ai-visionmodelname is Missing");

            _deploymentOrModelName4 = _configuration["ai-primarymodelname"]
                ?? throw new ArgumentException("ai-primarymodelname is Missing");

            _deploymentOrModelNameSecondary = _configuration["ai-secondarymodelname"]
                ?? throw new ArgumentException("ai-secondarymodelname is Missing");

            _temperature = float.Parse(_configuration["ai-temperature"]
                ?? throw new ArgumentException("ai-temperature"));

            _nucleus = float.Parse(_configuration["ai-nucleus"]
                ?? throw new ArgumentException("ai-nucleus"));

            _key = _configuration["ai-aikey"]
             ?? throw new ArgumentException("ai-aikey");

            _maxTokens = Convert.ToInt32(_configuration["ai-maxtokens"]
                ?? throw new ArgumentException("ai-maxtokens is Missing"));

            _maxOutputTokens = Convert.ToInt32(_configuration["ai-maxoutputtokens"]
                ?? throw new ArgumentException("ai-maxoutputtokens is Missing"));
        }

        // This method is responsible for inferring the caption of an image by sending it to an AI service.
        public async Task<ImageCaptionResult> InferImageCaptionAsync(byte[] imageBytes)
        {
            try
            {
                // Convert the image bytes to a Base64 string.
                var encodedImage = Convert.ToBase64String(imageBytes);

                // Construct the service address for the AI model endpoint.
                var serviceAddress = $"{_endpoint}openai/deployments/{_deploymentOrModelName4}/chat/completions?api-version=2024-08-01-preview";

                // Retrieve the system and user prompt templates.
                var system = PromptTemplates.SystemPromptTemplate;
                var user = PromptTemplates.MainPromptTemplate;

                // Create an HttpClient instance to send the request.
                using (var httpClient = new HttpClient())
                {
                    // Add the API key to the request headers.
                    httpClient.DefaultRequestHeaders.Add("api-key", _key);

                    // Construct the payload for the request.
                    var payload = new
                    {
                        // The messages array contains the conversation history.
                        // The first message is from the system, setting the context for the AI.
                        messages = new object[]
                        {
                            new {
                                role = "system",
                                content = new object[] {
                                    new {
                                        // The type of content is text.
                                        type = "text",
                                        // The actual text content is retrieved from the system prompt template.
                                        text = PromptTemplates.SystemPromptTemplate
                                    }
                                }
                            },
                            // The second message is from the user, containing the image and the user prompt.
                            new {
                                role = "user",
                                content = new object[] {
                                    new {
                                        // The type of content is an image URL.
                                        type = "image_url",
                                        // The image URL is constructed using the base64 encoded image.
                                        image_url = new {
                                            url = $"data:image/jpeg;base64,{encodedImage}"
                                        }
                                    },
                                    new {
                                        // The type of content is text.
                                        type = "text",
                                        // The actual text content is retrieved from the main prompt template.
                                        text = PromptTemplates.MainPromptTemplate
                                    }
                                }
                            }
                        },
                        // Set the temperature for the AI model to control randomness.
                        temperature = 0.1,
                        // Set the nucleus sampling parameter.
                        top_p = 0.8,
                        // Set the maximum number of tokens for the response.
                        max_tokens = 4096,
                        // Disable streaming responses.
                        stream = false
                    };

                    // Send the POST request to the AI service.
                    var response = await httpClient.PostAsync(serviceAddress, new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"));

                    // Check if the response is successful.
                    if (response.IsSuccessStatusCode)
                    {
                        // Deserialize the response content.
                        var responseData = JsonConvert.DeserializeObject<OpenAIResponse>(await response.Content.ReadAsStringAsync());

                        // Check if there are any choices in the response.
                        if (responseData.Choices.Count == 0)
                        {
                            return new ImageCaptionResult(string.Empty, 0.0);
                        }

                        // Extract the content of the first choice.
                        var choices = responseData.Choices[0].Message.Content;

                        // Extract the JSON object from the response content.
                        int startIndex = choices.IndexOf('{');
                        int endIndex = choices.LastIndexOf('}');
                        if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
                        {
                            choices = choices.Substring(startIndex, endIndex - startIndex + 1);
                        }

                        // Parse the JSON object and cleanse it.
                        var jsonObject = JObject.Parse(choices);
                        var cleansedJson = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);

                        // Deserialize the cleansed JSON into an ImageCaptionResult object.
                        ImageCaptionResult inferenceResult = JsonConvert.DeserializeObject<ImageCaptionResult>(cleansedJson);

                        return inferenceResult;

                        //choices = choices.Trim();

                        //jsonResponse = responseData.Trim();

                        //var inferenceResult = JsonConvert.DeserializeObject<ImageCaptionResult>(responseData);

                        //var responseData = JsonConvert.DeserializeObject<OpenAIResponse>(await response.Content.ReadAsStringAsync());
                        //var responseContent = await response.Content.ReadAsStringAsync();
                        //ImageCaptionResult responseData2 = JsonConvert.DeserializeObject<ImageCaptionResult>(responseContent);
                        ////return responseData;

                        //return "Good";
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode}, {response.ReasonPhrase}");
                        return new ImageCaptionResult(string.Empty, 0.0);
                    }

                    // Assuming you want to return the object name as the caption
                    //return responseData.Object;
                    //var responseData = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
                    ////var responseData = JsonConvert.DeserializeObject<OpenAIResponse>(await response.Content.ReadAsStringAsync());

                    ////if (responseData.Choices.Count == 0)
                    ////{
                    ////    return string.Empty;
                    ////}

                    ////var jsonResponse = responseData.Choices[0].Message.Content;

                    ////jsonResponse = jsonResponse.Trim();

                    ////var inferenceResult = JsonConvert.DeserializeObject<ImageCaptionResult>(jsonResponse);


                    ////return jsonResponse;
                    ///
                    //    return null;
                    //}
                    //else
                    //{
                    //    Console.WriteLine($"Error: {response.StatusCode}, {response.ReasonPhrase}");
                    //    return string.Empty;
                    //}
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"Exception throw in InferCaption.InferImageCaptionAsync: {ex.Message}";
                _logger.LogError(errorMessage);
                throw;
            }
        }

        public class OpenAIResponse
        {
            public List<Choice> Choices { get; set; }
            public string FinishReason { get; set; }
        }

        public class Choice
        {
            public Message Message { get; set; }
        }

        public class Message
        {
            public string Content { get; set; }
        }






        //// Instatiate OpenAIClient
        //AzureOpenAIClient client = new(
        //    new Uri(_endpoint),
        //    new AzureKeyCredential(_key));

        //ChatClient chatClient = client.GetChatClient(_deploymentOrModelName4);

        ////imageBase64 = imageBase64.Replace("data:image/jpeg;base64,", base64);
        //imageBase64 = string.Concat("data:image/jpeg;base64,", base64);

        //var messages = new List<ChatMessage>();
        //messages.Add(new SystemChatMessage(PromptTemplates.SystemPromptTemplate));
        //messages.Add(new UserChatMessage(PromptTemplates.MainPromptTemplate.Replace("{{$base64_string}}", imageBase64)));

        //ChatCompletionOptions completionOptions = new ChatCompletionOptions
        //{
        //    Temperature = _temperature,
        //    //MaxTokens = _maxTokens,
        //    TopP = _nucleus,
        //};

        //ChatCompletion chatCompletion = chatClient.CompleteChat(messages, completionOptions);

        //var ContentFilterReason = chatCompletion.FinishReason;

        //return chatCompletion.Content[0].Text;



        //// Set up HttpClient
        //var httpClient = new HttpClient();
        ////httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _key);
        //httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_key}");

        //// Create request with multipart form-data
        //var formData = new MultipartFormDataContent();
        //formData.Add(new ByteArrayContent(imageBytes), "file", "image.png");
        //formData.Add(new StringContent("You are an assistant that identifies objects in images."), "system");
        //formData.Add(new StringContent("Please identify the main object in the provided image."), "user");


        ////https://ai-openai-playground-west.openai.azure.com/openai/deployments/gpt-4-vision/chat/completions?api-version=2024-08-01-preview



        //var serviceCall = $"{_endpoint}openai/deployments/{_visionDeploymentModelName}/chat/completions?api-version=2024-08-01-preview";
        //var response = await httpClient.PostAsync(serviceCall, formData);


        //// Log the status code
        //_logger.LogInformation($"Response Status Code: {response.StatusCode}");

        //// Log the headers
        //foreach (var header in response.Headers)
        //{
        //    _logger.LogInformation($"{header.Key}: {string.Join(", ", header.Value)}");
        //}


        //var responseContent = await response.Content.ReadAsStringAsync();

        //return responseContent;

        //// Send the POST request
        ////var response = await httpClient.PostAsync("https://<your-endpoint>.openai.azure.com/openai/deployments/<your-deployment>/chat/completions?api-version=2023-09-01-preview", formData);
        ////var response = await httpClient.PostAsync($"{_endpoint}openai/deployments/{_visionDeploymentModelName}/chat/completions?api-version=2023-09-01-preview", formData);
    }
}
