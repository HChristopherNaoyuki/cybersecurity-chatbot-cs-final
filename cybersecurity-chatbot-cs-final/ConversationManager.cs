using System;
using System.Collections.Generic;
using System.Linq;

namespace cybersecurity_chatbot_cs_final
{
    /// <summary>
    /// Handles conversation flow and natural language processing
    /// </summary>
    public class ConversationManager
    {
        private readonly KnowledgeBase _knowledgeBase;
        private readonly MemoryManager _memory;
        private readonly UserInterface _ui;

        /// <summary>
        /// Initializes a new ConversationManager instance
        /// </summary>
        public ConversationManager(KnowledgeBase knowledgeBase, MemoryManager memory, UserInterface ui)
        {
            _knowledgeBase = knowledgeBase ?? throw new ArgumentNullException(nameof(knowledgeBase));
            _memory = memory ?? throw new ArgumentNullException(nameof(memory));
            _ui = ui ?? throw new ArgumentNullException(nameof(ui));
        }

        /// <summary>
        /// Processes user input and returns appropriate response
        /// </summary>
        /// <param name="input">User input text</param>
        /// <returns>Response string</returns>
        public string ProcessUserInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return "Please enter your question.";
            }

            string processedInput = input.Trim();
            _memory.LogActivity($"User input: {processedInput}");

            if (IsExitCommand(processedInput)) return "COMMAND_EXIT";
            if (IsHelpCommand(processedInput)) return GenerateHelpResponse();
            if (IsNameQuery(processedInput)) return HandleNameQuery();

            return ProcessNaturalLanguage(processedInput) ??
                   "I didn't understand that. Try 'help' for options.";
        }

        /// <summary>
        /// Processes natural language input
        /// </summary>
        private string ProcessNaturalLanguage(string input)
        {
            string sentiment = DetectInputSentiment(input);
            List<string> keywords = ExtractRelevantKeywords(input);

            if (keywords.Count == 0)
            {
                return null;
            }

            foreach (string keyword in keywords)
            {
                _memory.RememberKeyword(keyword);
            }

            return GenerateContextualResponse(keywords, sentiment);
        }

        /// <summary>
        /// Generates contextual response based on keywords and sentiment
        /// </summary>
        private string GenerateContextualResponse(List<string> keywords, string sentiment)
        {
            List<string> responses = new List<string>();

            foreach (string keyword in keywords.Distinct())
            {
                string response = _knowledgeBase.GetResponse(keyword);
                if (!string.IsNullOrEmpty(response))
                {
                    int count = _memory.GetKeywordCount(keyword);
                    if (count > 1)
                    {
                        response = $"About {keyword} (discussed before): {response}";
                    }

                    response = GetSentimentPrefix(sentiment) + response;
                    responses.Add(response);
                }
            }

            return responses.Count > 0 ? string.Join("\n\n", responses) : null;
        }

        /// <summary>
        /// Handles name query from user
        /// </summary>
        private string HandleNameQuery()
        {
            return !string.IsNullOrEmpty(_memory.UserName)
                ? $"I remember you, {_memory.UserName}."
                : "I don't know your name yet.";
        }

        /// <summary>
        /// Generates help response with available topics
        /// </summary>
        private string GenerateHelpResponse()
        {
            var topics = _knowledgeBase.GetAllTopics();
            return topics.Count > 0
                ? "I can help with:\n- " + string.Join("\n- ", topics.Cast<string>())
                : "No topics available.";
        }

        /// <summary>
        /// Detects sentiment from input text
        /// </summary>
        private string DetectInputSentiment(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return "neutral";
            }

            string lowerInput = input.ToLowerInvariant();

            if (ContainsAny(lowerInput, "worried", "concerned", "scared", "afraid"))
            {
                return "worried";
            }
            else if (ContainsAny(lowerInput, "happy", "excited", "great", "good"))
            {
                return "positive";
            }
            else if (ContainsAny(lowerInput, "angry", "frustrated", "upset", "mad"))
            {
                return "negative";
            }
            else if (ContainsAny(lowerInput, "what", "how", "why", "explain", "?"))
            {
                return "curious";
            }

            return "neutral";
        }

        /// <summary>
        /// Extracts relevant keywords from input
        /// </summary>
        private List<string> ExtractRelevantKeywords(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new List<string>();
            }

            return input.Split(new[] { ' ', ',', '.', '?', '!' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(word => word.Trim().ToLowerInvariant())
                .Where(word => word.Length > 2 && !_knowledgeBase.ShouldIgnoreWord(word))
                .ToList();
        }

        /// <summary>
        /// Gets sentiment prefix for responses
        /// </summary>
        private string GetSentimentPrefix(string sentiment)
        {
            string lowerSentiment = sentiment.ToLowerInvariant();

            if (lowerSentiment == "worried")
            {
                return "I understand this can be concerning. ";
            }
            else if (lowerSentiment == "positive")
            {
                return "Great. ";
            }
            else if (lowerSentiment == "negative")
            {
                return "I'm sorry you're feeling this way. ";
            }
            else if (lowerSentiment == "curious")
            {
                return "Good question. ";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Checks if input is an exit command
        /// </summary>
        private bool IsExitCommand(string input)
        {
            string[] exitCommands = { "exit", "quit", "bye", "goodbye" };
            return exitCommands.Contains(input.ToLowerInvariant());
        }

        /// <summary>
        /// Checks if input is a help command
        /// </summary>
        private bool IsHelpCommand(string input)
        {
            string[] helpCommands = { "help", "options", "topics", "what can you do" };
            return helpCommands.Contains(input.ToLowerInvariant());
        }

        /// <summary>
        /// Checks if input is a name query
        /// </summary>
        private bool IsNameQuery(string input)
        {
            return input.IndexOf("what is my name", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   input.IndexOf("who am i", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// Checks if input contains any of the specified terms
        /// </summary>
        private bool ContainsAny(string input, params string[] terms)
        {
            return terms.Any(term => input.Contains(term));
        }
    }
}