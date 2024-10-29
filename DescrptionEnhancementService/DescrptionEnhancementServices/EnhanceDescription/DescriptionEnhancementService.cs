using Azure.AI.OpenAI;
using Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenAI.Chat;
using DescrptionEnhancementService.DescrptionEnhancementServices.Prompts;
using DescrptionEnhancementService.DescrptionEnhancementServices.Contracts;

namespace DescrptionEnhancementService.DescrptionEnhancementServices.EnhanceDescription
{
    internal class DescriptionEnhancementService : IDescriptionEnhancementService
    //: IChatProcessor
    {
        private readonly ILogger<DescriptionEnhancementService> _logger;
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

        public DescriptionEnhancementService(IConfiguration configuration,
                                        //ITokenManager tokenManager, 
                                        ILogger<DescriptionEnhancementService> logger)
        {
            _logger = logger;

            _configuration = configuration;

            // Get the Azure OpenAI Service configuration values
            _endpoint = _configuration["desc-endpoint"]
                ?? throw new ArgumentException("desc-endpoint is Missing");

            _deploymentOrModelName4 = _configuration["desc-primarymodelname"]
                ?? throw new ArgumentException("desc-primarymodelname is Missing");

            _deploymentOrModelNameSecondary = _configuration["desc-secondarymodelname"]
                ?? throw new ArgumentException("desc-secondarymodelname is Missing");

            _temperature = float.Parse(_configuration["desc-temperature"]
                ?? throw new ArgumentException("desc-temperature"));

            _nucleus = float.Parse(_configuration["desc-nucleus"]
                ?? throw new ArgumentException("desc-nucleus"));

            _key = _configuration["desc-aikey"]
                ?? throw new ArgumentException("desc-aikey");

            _maxTokens = Convert.ToInt32(_configuration["desc-maxtokens"]
               ?? throw new ArgumentException("desc-maxtokens is Missing"));

            _maxOutputTokens = Convert.ToInt32(_configuration["desc-maxoutputtokens"]
                ?? throw new ArgumentException("desc-maxoutputtokens is Missing"));
        }

        //public async Task<(ChatCompletions response, ChatCompletions followup, int promptTokens, int completionTokens, int suggestionTokens)> ChatCompletionAsync(EZCompletionOptions ezCompletionOptions)
        public async Task<string> ChatCompletionAsync(string vendorDescription)
        {
            try
            {
                // Instatiate OpenAIClient
                AzureOpenAIClient client = new(
                    new Uri(_endpoint),
                    new AzureKeyCredential(_key));

                ChatClient chatClient = client.GetChatClient(_deploymentOrModelName4);

                var messages = new List<ChatMessage>();
                messages.Add(new SystemChatMessage(PromptTemplates.SystemPromptTemplate));
                messages.Add(new UserChatMessage(PromptTemplates.MainPromptTemplate.Replace("{{$prompt}}", "vendorDescription")));

                ChatCompletionOptions completionOptions = new ChatCompletionOptions
                {

                    Temperature = _temperature,
                    //MaxTokens = _maxTokens,
                    TopP = _nucleus,
                };

                ChatCompletion chatCompletion = chatClient.CompleteChat(messages, completionOptions);
                //var response = chatCompletion.Content[^1].Text;

                var probablityInfo = chatCompletion.ContentTokenLogProbabilities;

                var ContentFilterReason = chatCompletion.FinishReason;

                var response = chatCompletion.Content[0].Text;

                if (string.IsNullOrEmpty(response))
                {
                    response = "No Response";
                }

                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Exception throw in DataExtractionService.DataExtractionCompletion: {ex.Message}";
                _logger.LogError(errorMessage);
                throw;
            }
        }
    }
}

// Extract ChatMessage objects from the EZCompletionOptions object
//var chatMessages = chatInteractionContextModel.ChatMessages;

// Build system prompt from templlate and add to beginning of list.
////messages.Add(new SystemChatMessage(
////    PromptTemplates.SystemPromptTemplate.Replace("{{$systemPrompt}}",
////    chatInteractionContextModel.SystemPrompt)));

// tempate causing content filtering???
//messages.Add(new SystemChatMessage(chatInteractionContextModel.SystemPrompt));

// Add system prompt to chat history to beginning of list
//chatMessages.Add(new ChatMessage(Role.System, PromptTemplates.SystemPromptTemplate, 0));

// User prompt is the current text request from the user. It will always be the last message in the chat history
//var userPrompt = chatMessages.LastOrDefault();

//// Remove the last message from the chat history as it will be the main prompt.
//chatMessages.RemoveAt(chatMessages.Count - 1);

// At this point, the chat history is not optimized
// Send the chat history to the token manager to optimize so that we don't exceed the max tokens
//////if (chatMessages.Count > 1)
//////{
//////    chatMessages = await _tokenManager.OptimizeChatHistoryAsync(chatMessages,
//////                                                                userPrompt.Content,
//////                                                                _maxTokens,
//////                                                                _maxOutputTokens);
//////}

//foreach (var message in chatInteractionContextModel.ChatMessages.Take(chatInteractionContextModel.ChatMessages.Count))
//{
//    if (message.Role == Role.Assistant)
//    {
//        messages.Add(new AssistantChatMessage(message.Content));
//    }
//    else
//    {
//        messages.Add(new UserChatMessage(message.Content));
//    }
//}

//// Lastly, add the main prompt last and AFTER the chat history
//// (Rule: System prompt first, chat history second, main prompt last)
//////messages.Add(new UserChatMessage(PromptTemplates.MainPromptTemplate
//////   .Replace("{{$prompt}}", userPrompt.Content)));


//// tempate causing content filtering???
//messages.Add(new UserChatMessage(userPrompt.Content));

// Build the completion options object 

//ChatCompletionOptions chatCompletionOptions = new ChatCompletionOptions
//{

//    ChatMessages = chatMessages,
//    SystemPrompt = systemPrompt,
//    UserPrompt = userPrompt,
//    Temperature = completionOverrides.Temperature,
//    MaxTokens = completionOverrides.MaxOutputTokens,
//    Nucleus = completionOverrides.Nucleus,
//    DeploymentName = _deploymentOrModelName4
//};

//var FilteredAnnotations = chatCompletion.Annotations.  Content[0].Annotations?.Select(a => a.Category).ToList()  // Capture annotations if available

//var Response = chatCompletion.Content[0].Text; // completionResponse.Value.Choices.FirstOrDefault()?.Message?.Content,
//var PromptTokens2 = chatCompletion.Usage.InputTokens; //.Value.Usage.PromptTokens,
//var ResponseTokens = chatCompletion.Usage.OutputTokens; // .Value.Usage.CompletionTokens,
//var ChatHistory1 = chatMessages;
//var ProbablityInfo = chatCompletion.ContentTokenLogProbabilities;
////SuggestionTokens = completionOverrides.Suggestions ? suggestionResponse.Value.Usage.TotalTokens : 0,
////Suggestions = completionOverrides.Suggestions ? suggestionResponse.Value.Choices.FirstOrDefault()?.Message?.Content : string.Empty

//var mymodel = model;

//return null;

//////if (completionOverrides.Suggestions)
//////{
//////    var followupTemplate = PromptTemplates.SugestionsPromptTemplate
//////        .Replace("{{$answer}}", chatCompletion.Content[0].Text); //.Value.Choices.FirstOrDefault()?.Message?.Content);

//////    suggestionResponse = await client.GetChatCompletionsAsync(new ChatCompletionsOptions // ChatCompletionsOptions
//////    {
//////        DeploymentName = _deploymentOrModelName4, //_deploymentOrModelName35; 
//////        Temperature = completionOverrides.Temperature,
//////        MaxTokens = completionOverrides.MaxOutputTokens,
//////        NucleusSamplingFactor = completionOverrides.Nucleus,
//////        Messages = { new ChatRequestUserMessage(followupTemplate) }
//////    });
//////}

//ChatCompletionOptions chatCompletionOptions = new ChatCompletionOptions();
//var completionOptions = new ChatCompletionsOptions();

//// Add the system prompt first
//completionOptions.Messages.Add(systemPrompt);

//// Add the chat history
//foreach (var message in completionOverrides.ChatMessages.Take(completionOverrides.ChatMessages.Count))
//{
//    if (message.Role == Role.Assistant)
//    {
//        completionOptions.Messages.Add(new AssistantChatMessage(message.Content));
//    }
//    else
//    {
//        completionOptions.Messages.Add(new UserChatMessage(message.Content));
//    }
//}

//// Lastly, add the main prompt last and AFTER the chat history
//// (Rule: System prompt first, chat history second, main prompt last)
//var mainTempate = PromptTemplates.MainPromptTemplate
//   .Replace("{{$prompt}}", userPrompt.Content);

// Add main prompt to end of list
//completionOptions.Messages.Add(new UserChatMessage(mainTempate));
//completionOptions.DeploymentName = _deploymentOrModelName4; //_deploymentOrModelName35; 
//completionOptions.Temperature = completionOverrides.Temperature;
//completionOptions.MaxTokens = completionOverrides.MaxOutputTokens;
//completionOptions.NucleusSamplingFactor = completionOverrides.Nucleus;

// Call to LLM for completion
//var completionResponse = await client. .GetChatCompletionsAsync(completionOptions);

// Add subsequent call to LLM to generate follow-up suggestions
//Response<ChatCompletion> suggestionResponse = null;

//if (completionOverrides.Suggestions)
//{
//    var followupTemplate = PromptTemplates.SugestionsPromptTemplate
//        .Replace("{{$answer}}", completionResponse.Value.Choices.FirstOrDefault()?.Message?.Content);

//    suggestionResponse = await client.GetChatCompletionsAsync(new ChatCompletionsOptions // ChatCompletionsOptions
//    {
//        DeploymentName = _deploymentOrModelName4, //_deploymentOrModelName35; 
//        Temperature = completionOverrides.Temperature,
//        MaxTokens = completionOverrides.MaxOutputTokens,
//        NucleusSamplingFactor = completionOverrides.Nucleus,
//        Messages = { new ChatRequestUserMessage(followupTemplate) }
//    });
//}

//// Reconstruct Optimized ChatHistory to return to caller
//// Add System Prompt to beginning of list
//chatMessages.Insert(0, new ChatMessage(Role.System, PromptTemplates.SystemPromptTemplate, 0));

////// Right after System Prompt, insert the main prompt and answer
//chatMessages.Insert(1, new ChatMessage(Role.User,
//                                       userPrompt.Content,
//                                       completionResponse.Value.Usage.PromptTokens));

//// Right after the main prompt, insert the completion response 
//chatMessages.Insert(2, new ChatMessage(Role.Assistant,
//                                       completionResponse.Value.Choices.FirstOrDefault()?.Message?.Content,
//                                       completionResponse.Value.Usage.CompletionTokens));

//_logger.LogInformation("Executed Completion:" + completionResponse.Value.Choices.FirstOrDefault()?.Message?.Content);


//return new Completion
//{
//    Response = completionResponse.Value.Choices.FirstOrDefault()?.Message?.Content,
//    PromptTokens = completionResponse.Value.Usage.PromptTokens,
//    ResponseTokens = completionResponse.Value.Usage.CompletionTokens,
//    ChatHistory = chatMessages,
//    SuggestionTokens = completionOverrides.Suggestions ? suggestionResponse.Value.Usage.TotalTokens : 0,
//    Suggestions = completionOverrides.Suggestions ? suggestionResponse.Value.Choices.FirstOrDefault()?.Message?.Content : string.Empty
//};