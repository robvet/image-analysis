using Microsoft.Extensions.Configuration;
using OpenAI;

//namespace DescrptionEnhancementServices
//{
//    internal class EmbedingService
//    {
//        private readonly IConfiguration _configuration;
//        private readonly string _endpoint;
//        private readonly string _key;
//        private readonly string _deploymentOrModelName;
//        private readonly string _openAIEmbeddingDeployment;
//        private OpenAIClient _openAIClient;

//        public EmbedingService(IConfiguration configuration)
//        {
//            _configuration = configuration;

//            _endpoint = _configuration["OpenAiEndpoint"] ?? throw new ArgumentException("OpenAiEndpoint is Missing");
//            _key = _configuration["OpenAiKey"] ?? throw new ArgumentException("OpenAiKey is Missing"); ;
//            _deploymentOrModelName = _configuration["OpenAiModel"] ?? throw new ArgumentException("OpenAiModel is Missing");
//            _openAIEmbeddingDeployment = _configuration["OpenAiEmbeddingModel"] ?? throw new ArgumentException("OpenAiEmbeddingModel Missing");
//        }

//        public async Task<float[]> EmbedSingleAsync(string value)
//        {
//            //var surveyEmbedding = new SurveyEmbeddingModel
//            //{
//            //    id = Guid.NewGuid().ToString(),
//            //    surveyId = surveyResponse.surveyId,
//            //    response = surveyResponse.response,
//            //            embedding = await GetEmbeddingsAsync(surveyResponse.response)
//            //        };

//            return await EmbedValueAsync(value);
//        }
//    }
//}
