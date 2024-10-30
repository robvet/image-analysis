using Azure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Runtime.ConstrainedExecution;

namespace ImageCaptionService.ImageCaptionServices.Prompts
{
    internal static class PromptTemplates
    {
        /// <summary>
        /// Prompt template for for extracting keyword search from user question to be used 
        /// against the search engine to build retriever.
        /// </summary>

        // Prompting Tips
        // Encourage Conciseness: Instruct the LLM to respond concisely.The JSON format naturally keeps responses short.
        // Prompt Reinforcement: Emphasize the JSON format and fields (object and probability) to guide the model to follow the structure exactly.
        // Temperature and Top-p: Set temperature to a low value(e.g., 0.1) and top-p between 0.1 and 0.3 to minimize creative variance,
        // which is important when you need consistent JSON outputs and a straightforward probability estimate.

        // System Prompt
        //public const string SystemPromptTemplate = """
        //    You are an image recognition assistant. Your task is to examine images provided by the user and respond with only the label naming the main object in the picture. Avoid adding any descriptive details, explanations, or additional context. Respond with only a single word or phrase if possible.
        //    """;

        //public const string SystemPromptTemplate = """
        //    You are an image recognition assistant. Your task is to examine images provided by the user and respond with only the label naming the main object in the picture. Additionally, include an estimated probability score for the inference. Return your response as a JSON object with fields object for the label and probability for the probability estimate. Avoid adding any descriptive details, explanations, or additional context. Respond concisely.
        //    """;

        //public const string SystemPromptTemplate = """
        //    You are an image recognition assistant. Your task is to examine images provided by the user and respond with a JSON object. The JSON object should contain two fields: object, which is the main object label you infer, and probability, which is your estimated confidence in that inference as a decimal between 0.0 and 1.0. Always provide both fields, and do not leave any fields blank. Avoid any additional context or description outside of the JSON format.
        //    """;


        public const string SystemPromptTemplate = """
            You are an image recognition assistant. Your task is to examine images provided by the user and respond with a JSON object. 

            The JSON representation should be in the following format:

            {
                "object": "banana",
                "probability": 0.9,
                "color": "yellow",
                "count": 3,
                "volume": null,
                "isFood": true,
                "isDrink": false,
                "isAlcoholic": false,
                "UPC": 12345,
                "Manufacturer": "xyz company"
            }
            
            """;


        /// <summary>
        /// Prompt template for for extracting keyword search from user question to be used 
        /// against the search engine to build retriever.
        /// </summary>
        //public const string MainPromptTemplate = """
        //    Please identify the main object in the provided image. Respond only with concise, short term describing the object.
        //    """
        //;

        //public const string MainPromptTemplate = """
        //    Please identify the main object in the provided image and include a probability estimate for your inference. Return only the JSON response, with fields object for the label and probability for your confidence estimate (a decimal between 0.0 and 1.0)
        //    """;

        //public const string MainPromptTemplate = """
        //    Please identify the main object in the provided image and include a probability estimate in JSON format, even if you’re uncertain. If uncertain, set probability to 0.5.

        //    Return the analysis as an object named CaptionsCompletion in JSON format. Do not include the word JSON. 
        //    Ensure the CaptionsCompletion object includes the object and probability percentage.

        //    The JSON representation should be in the following format:

        //    { "object": "banana", "probability": 0.9 }

        //    Return only the CaptionsCompletion in a valid JSON format - nothing else.

        //    Replace values as appropriate and ensure both fields are always populated.

        //    DO NOT PUT ANY CHARACTERS OR STRINGS WHATSOEVER of ANY KIND BEFORE OR AFTER THE CaptionsCompletion object. This includes  ``` , ```, {, or } -- ABSOLUTELY NO CHARACTERS.
        //    As well, DO NOT include the word JSON anywhere in the response.
        //    """;



        public const string MainPromptTemplate = """
            Please identify the main object in the provided image and include a probability estimate in JSON format, even if you’re uncertain. If uncertain, set probability to 0.0.
            
            Infer the following information from the image:

            1- The object that appears in the image.
            2- The probability of your inference being correct(between 0.0 and 0.99).
            3 - What product category would this image fall into?

                Examples include:
                a- Food
                b- Beverage
                c- Cigarette

            4 - The color of the object.
            5 - The number of objects in the image.
            6 - The volume of the object.
            7 - Whether the object is food.
            8 - Whether the object is a drink.
            9 - Whether the object is alcoholic.
            10 - The widely-recognized UPC code of the object.
            11 - The manufacturer of the object.

            Return the analysis as an object named CaptionsCompletion in JSON format. Do not include the word JSON. 
        
            Ensure the CaptionsCompletion object includes the object and probability percentage.
                                    
            Return only the CaptionsCompletion in a valid JSON format - nothing else.
            
            Replace values as appropriate and ensure both fields are always populated.
            
            DO NOT PUT ANY CHARACTERS OR STRINGS WHATSOEVER of ANY KIND BEFORE OR AFTER THE CaptionsCompletion object. This includes  ``` , ```, {, or } -- ABSOLUTELY NO CHARACTERS.
            As well, DO NOT include the word JSON anywhere in the response.
                                    
            """;








        ///// <summary>
        ///// Prompt template for for extracting keyword search from user question to be used 
        ///// against the search engine to build retriever.
        ///// </summary>
        //public const string SugestionsPromptTemplate = """
        //    <|im_start|>system
        //    Generate three follow-up questions based on the answer you just generated.
        //    # Answer
        //    {{$answer}}

        //    Remove quotes, brackets and any other special characters from the answer.
        //    Remove all preceding and trailing characters, including single quotes and periods.
        //    Each follow-up question should be no more than 15 words

        //    Here's a few examples of good search queries:
        //    # Format of the response
        //    Return the follow-up question as a json string list.
        //    e.g.
        //    [
        //        "What is the deductible?",
        //        "What is the co-pay?",
        //        "What is the out-of-pocket maximum?"
        //    ]
        //    <|im_end|>
        //    """;



        ///// <summary>
        ///// Prompt template for for extracting keyword search from user question to be used 
        ///// against the search engine to build retriever.
        ///// </summary>
        //public const string QueryPromptTemplate = """
        //    <|im_start|>system
        //    Chat history:
        //    {{$chat_history}}

        //    Here's a few examples of good search queries:
        //    ### Good example 1 ###
        //    Prices and payment methods
        //    ### Good example 2 ###
        //    Menu
        //    ### Good example 3 ###
        //    Wifi service
        //    ###


        //    <|im_end|>
        //    <|im_start|>system
        //    Generate search query for followup question. You can refer to chat history for context information. Just return search query and don't include any other information.
        //    {{$question}}
        //    <|im_end|>
        //    <|im_start|>assistant
        //    """;

        ///// <summary>
        ///// Prompt template for generating answer to user question based on response return from the retriever.
        ///// </summary>
        //public const string AnswerPromptTemplate = """
        //    <|im_start|>system
        //    You are a system assistant who helps the company employees with their healthcare plan questions, and questions about the employee handbook. Be brief in your answers.
        //    Answer ONLY with the facts listed in the list of sources below. If there isn't enough information below, say you don't know. Do not generate answers that don't use the sources below.


        //    For tabular information return it as an html table. Do not return markdown format.
        //    Each source has a name followed by colon and the actual information, ALWAYS reference source for each fact you use in the response. Use square brakets to reference the source. List each source separately.



        //    Here're a few examples:
        //    ### Good Example 1 (include source) ###
        //    Apple is a fruit[reference1.pdf].
        //    ### Good Example 2 (include multiple source) ###
        //    Apple is a fruit[reference1.pdf][reference2.pdf].
        //    ### Good Example 2 (include source and use double angle brackets to reference question) ###
        //    Microsoft is a software company[reference1.pdf].  <<followup question 1>> <<followup question 2>> <<followup question 3>>
        //    ### END ###
        //    Sources:
        //    {{$sources}}

        //    Chat history:
        //    {{$chat_history}}
        //    <|im_end|>
        //    <|im_start|>user
        //    {{$question}}
        //    <|im_end|>
        //    <|im_start|>assistant
        //    """;

        //public const string SystemMessage = @"You are a virtual assistant from a Microsoft Partner. Your goal is to answer all customer questions to help them understand the Azure cloud platform. Responses to customers should be friendly and include appropriate emojis. Your responses should be in the same language as the user's question.";
        //public const string SystemMessage = @"You are a virtual assistant from the Marriott Hotel. Your goal is to answer all customer questions to help them make a reservation. Responses to customers should be friendly and include appropriate emojis. Your responses should be in the same language as the user's question.";
    }
}
