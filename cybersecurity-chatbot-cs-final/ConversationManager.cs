using System;
using System.Collections.Generic;
using System.Linq;

namespace cybersecurity_chatbot_cs_final
{
    public class ConversationManager
    {
        private readonly KnowledgeBase _knowledgeBase;
        private readonly MemoryManager _memory;
        private readonly UserInterface _ui;

        public ConversationManager(KnowledgeBase knowledgeBase, MemoryManager memory, UserInterface ui)
        {
            _knowledgeBase = knowledgeBase ?? throw new ArgumentNullException(nameof(knowledgeBase));
            _memory = memory ?? throw new ArgumentNullException(nameof(memory));
            _ui = ui ?? throw new ArgumentNullException(nameof(ui));
        }

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

        private string ProcessNaturalLanguage(string input)
        {
            string sentiment = DetectInputSentiment(input);
            List<string> keywords = ExtractRelevantKeywords(input);

            if (keywords.Count == 0) return null;

            foreach (string keyword in keywords)
            {
                _memory.RememberKeyword(keyword);
            }

            return GenerateContextualResponse(keywords, sentiment);
        }

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

        private string HandleNameQuery()
        {
            return !string.IsNullOrEmpty(_memory.UserName)
                ? $"I remember you, {_memory.UserName}."
                : "I don't know your name yet.";
        }

        private string GenerateHelpResponse()
        {
            var topics = _knowledgeBase.GetAllTopics();
            return topics.Count > 0
                ? "I can help with:\n- " + string.Join("\n- ", topics.Cast<string>())
                : "No topics available.";
        }

        private string DetectInputSentiment(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "neutral";

            string lowerInput = input.ToLowerInvariant();

            if (ContainsAny(lowerInput, "worried", "concerned", "scared"))
                return "worried";
            if (ContainsAny(lowerInput, "happy", "excited", "great"))
                return "positive";
            if (ContainsAny(lowerInput, "angry", "frustrated", "upset"))
                return "negative";
            if (ContainsAny(lowerInput, "what", "how", "why", "explain", "?"))
                return "curious";

            return "neutral";
        }

        private List<string> ExtractRelevantKeywords(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return new List<string>();

            return input.Split(new[] { ' ', ',', '.', '?', '!' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(word => word.Trim().ToLowerInvariant())
                .Where(word => word.Length > 2 && !_knowledgeBase.ShouldIgnoreWord(word))
                .ToList();
        }

        private string GetSentimentPrefix(string sentiment)
        {
            return sentiment.ToLowerInvariant() switch
            {
                "worried" => "I understand this can be concerning. ",
                "positive" => "Great. ",
                "negative" => "I'm sorry you're feeling this way. ",
                "curious" => "Good question. ",
                _ => ""
            };
        }

        private bool IsExitCommand(string input) =>
            new[] { "exit", "quit", "bye", "goodbye" }.Contains(input.ToLowerInvariant());

        private bool IsHelpCommand(string input) =>
            new[] { "help", "options", "topics", "what can you do" }.Contains(input.ToLowerInvariant());

        private bool IsNameQuery(string input) =>
            input.IndexOf("what is my name", StringComparison.OrdinalIgnoreCase) >= 0 ||
            input.IndexOf("who am i", StringComparison.OrdinalIgnoreCase) >= 0;

        private bool ContainsAny(string input, params string[] terms) =>
            terms.Any(term => input.Contains(term));
    }
}