﻿using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace ImageCaptionService
{
    internal static class PromptTemplates
    {
        /// <summary>
        /// Prompt template for for extracting keyword search from user question to be used 
        /// against the search engine to build retriever.
        /// </summary>

        // For Database Files
        public const string SystemPromptTemplate = """
            You are an image recognition assistant. Your task is to examine images provided by the user and respond with only the label naming the main object in the picture. Avoid adding any descriptive details, explanations, or additional context. Respond with only a single word or phrase if possible.
            """;

        /// <summary>
        /// Prompt template for for extracting keyword search from user question to be used 
        /// against the search engine to build retriever.
        /// </summary>
        public const string MainPromptTemplate = """
            Please identify the main object in the provided image. Repond only with concise, short term describing the object.
            """
        ;


        /// <summary>
        /// Prompt template for for extracting keyword search from user question to be used 
        /// against the search engine to build retriever.
        /// </summary>
        public const string SugestionsPromptTemplate = """
            <|im_start|>system
            Generate three follow-up questions based on the answer you just generated.
            # Answer
            {{$answer}}
 
            Remove quotes, brackets and any other special characters from the answer.
            Remove all preceding and trailing characters, including single quotes and periods.
            Each follow-up question should be no more than 15 words

            Here's a few examples of good search queries:
            # Format of the response
            Return the follow-up question as a json string list.
            e.g.
            [
                "What is the deductible?",
                "What is the co-pay?",
                "What is the out-of-pocket maximum?"
            ]
            <|im_end|>
            """;



        /// <summary>
        /// Prompt template for for extracting keyword search from user question to be used 
        /// against the search engine to build retriever.
        /// </summary>
        public const string QueryPromptTemplate = """
            <|im_start|>system
            Chat history:
            {{$chat_history}}
            
            Here's a few examples of good search queries:
            ### Good example 1 ###
            Prices and payment methods
            ### Good example 2 ###
            Menu
            ### Good example 3 ###
            Wifi service
            ###


            <|im_end|>
            <|im_start|>system
            Generate search query for followup question. You can refer to chat history for context information. Just return search query and don't include any other information.
            {{$question}}
            <|im_end|>
            <|im_start|>assistant
            """;

        /// <summary>
        /// Prompt template for generating answer to user question based on response return from the retriever.
        /// </summary>
        public const string AnswerPromptTemplate = """
            <|im_start|>system
            You are a system assistant who helps the company employees with their healthcare plan questions, and questions about the employee handbook. Be brief in your answers.
            Answer ONLY with the facts listed in the list of sources below. If there isn't enough information below, say you don't know. Do not generate answers that don't use the sources below.


            For tabular information return it as an html table. Do not return markdown format.
            Each source has a name followed by colon and the actual information, ALWAYS reference source for each fact you use in the response. Use square brakets to reference the source. List each source separately.
     


            Here're a few examples:
            ### Good Example 1 (include source) ###
            Apple is a fruit[reference1.pdf].
            ### Good Example 2 (include multiple source) ###
            Apple is a fruit[reference1.pdf][reference2.pdf].
            ### Good Example 2 (include source and use double angle brackets to reference question) ###
            Microsoft is a software company[reference1.pdf].  <<followup question 1>> <<followup question 2>> <<followup question 3>>
            ### END ###
            Sources:
            {{$sources}}

            Chat history:
            {{$chat_history}}
            <|im_end|>
            <|im_start|>user
            {{$question}}
            <|im_end|>
            <|im_start|>assistant
            """;

        public const string SystemMessage = @"You are a virtual assistant from a Microsoft Partner. Your goal is to answer all customer questions to help them understand the Azure cloud platform. Responses to customers should be friendly and include appropriate emojis. Your responses should be in the same language as the user's question.";
        //public const string SystemMessage = @"You are a virtual assistant from the Marriott Hotel. Your goal is to answer all customer questions to help them make a reservation. Responses to customers should be friendly and include appropriate emojis. Your responses should be in the same language as the user's question.";
    }
}
