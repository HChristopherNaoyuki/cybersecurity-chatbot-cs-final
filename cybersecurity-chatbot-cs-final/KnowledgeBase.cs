using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace cybersecurity_chatbot_cs_final
{
    public class KnowledgeBase
    {
        private readonly Dictionary<string, string> _knowledgeBase = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _ignoreWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        private readonly string[] _systemCommands = {
            "help", "how are you", "purpose", "what can you do", "topics"
        };

        public KnowledgeBase()
        {
            InitializeIgnoreWords();
            InitializeKnowledgeBase();
        }

        public string GetResponse(string topic)
        {
            if (string.IsNullOrWhiteSpace(topic))
            {
                return string.Empty;
            }

            if (_knowledgeBase.TryGetValue(topic, out string response))
            {
                return response;
            }

            foreach (var kvp in _knowledgeBase)
            {
                if (topic.IndexOf(kvp.Key, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return kvp.Value;
                }
            }

            return string.Empty;
        }

        public bool ShouldIgnoreWord(string word)
        {
            return string.IsNullOrWhiteSpace(word) || _ignoreWords.Contains(word);
        }

        public ArrayList GetAllTopics()
        {
            var topics = new ArrayList();

            foreach (var topic in _knowledgeBase.Keys)
            {
                if (!_systemCommands.Contains(topic))
                {
                    topics.Add(topic);
                }
            }

            return topics;
        }

        private void InitializeKnowledgeBase()
        {
            AddKnowledgeEntry("password",
                "Use passwords with at least 12 characters including uppercase, lowercase, numbers, and symbols. Never reuse passwords.");

            AddKnowledgeEntry("phishing",
                "Verify sender addresses before responding to emails. Legitimate organizations never ask for passwords via email.");

            AddKnowledgeEntry("privacy",
                "Review privacy settings on all accounts monthly. Limit personal information shared online.");

            AddKnowledgeEntry("malware",
                "Keep antivirus updated. Malware includes viruses, ransomware, and spyware that can steal data.");

            AddKnowledgeEntry("vpn",
                "VPNs encrypt internet connections, protecting data on public networks. Use reputable VPN services.");

            AddKnowledgeEntry("wifi",
                "Avoid sensitive transactions on public WiFi. Use VPNs for added security on unknown networks.");

            AddKnowledgeEntry("authentication",
                "Two-factor authentication adds security by requiring password plus device verification.");

            AddKnowledgeEntry("2fa",
                "Two-factor authentication combines something you know (password) with something you have (device).");

            AddKnowledgeEntry("browsing",
                "Check for HTTPS padlock icons. Avoid suspicious links and downloads from unknown sources.");

            AddKnowledgeEntry("update",
                "Install software updates promptly. Updates patch security vulnerabilities exploited by hackers.");

            AddKnowledgeEntry("backup",
                "Regular backups protect against data loss from ransomware or hardware failure. Use 3-2-1 rule.");

            AddKnowledgeEntry("encryption",
                "Encryption scrambles data so only authorized parties can read it. Use for sensitive files.");

            AddKnowledgeEntry("firewall",
                "Firewalls monitor network traffic. Enable both software and hardware firewalls when possible.");

            AddKnowledgeEntry("social engineering",
                "Attackers manipulate people into revealing information. Verify identities before sharing data.");

            AddKnowledgeEntry("ransomware",
                "Ransomware encrypts files until payment. Prevent with backups and cautious email handling.");
        }

        private void InitializeIgnoreWords()
        {
            string[] wordsToIgnore =
            {
                "a", "an", "the", "i", "you", "he", "she", "it", "we", "they",
                "is", "are", "was", "were", "be", "being", "been", "have", "has",
                "had", "do", "does", "did", "can", "could", "would", "should",
                "may", "might", "must", "what", "how", "why", "when", "where",
                "who", "which", "please", "thanks", "thank", "tell", "me", "about",
                "explain", "know", "want", "need", "like", "help", "understand"
            };

            foreach (string word in wordsToIgnore)
            {
                _ignoreWords.Add(word);
            }
        }

        private void AddKnowledgeEntry(string topic, string response)
        {
            if (!string.IsNullOrWhiteSpace(topic) && !string.IsNullOrWhiteSpace(response))
            {
                _knowledgeBase[topic] = response;
            }
        }
    }
}