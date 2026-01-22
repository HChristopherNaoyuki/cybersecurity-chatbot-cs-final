using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace cybersecurity_chatbot_cs_final
{
    /// <summary>
    /// Repository of cybersecurity knowledge and responses
    /// </summary>
    public class KnowledgeBase
    {
        private readonly Dictionary<string, string> _knowledgeBase = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _ignoreWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        private readonly string[] _systemCommands = {
            "help", "how are you", "purpose", "what can you do", "topics"
        };

        /// <summary>
        /// Initializes the knowledge base with cybersecurity information
        /// </summary>
        public KnowledgeBase()
        {
            InitializeIgnoreWords();
            InitializeKnowledgeBase();
        }

        /// <summary>
        /// Gets response for a given topic
        /// </summary>
        /// <param name="topic">Topic to get information about</param>
        /// <returns>Response string or empty if topic not found</returns>
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

        /// <summary>
        /// Checks if a word should be ignored during keyword extraction
        /// </summary>
        /// <param name="word">Word to check</param>
        /// <returns>True if word should be ignored</returns>
        public bool ShouldIgnoreWord(string word)
        {
            return string.IsNullOrWhiteSpace(word) || _ignoreWords.Contains(word);
        }

        /// <summary>
        /// Gets all available topics
        /// </summary>
        /// <returns>ArrayList of topic names</returns>
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

        /// <summary>
        /// Initializes the cybersecurity knowledge base
        /// </summary>
        private void InitializeKnowledgeBase()
        {
            // System commands
            AddKnowledgeEntry("help",
                "I can explain these topics: passwords, phishing, privacy, malware, VPNs, WiFi security, authentication, encryption");
            AddKnowledgeEntry("how are you",
                "I'm functioning optimally! Ready to discuss cybersecurity.");
            AddKnowledgeEntry("purpose",
                "I provide cybersecurity education to help you stay safe online.");
            AddKnowledgeEntry("what can you do",
                "I can educate you about cybersecurity best practices and test your knowledge.");

            // Core cybersecurity topics
            AddKnowledgeEntry("password",
                "Strong passwords should be at least 12 characters with mixed case, numbers and symbols. Use a password manager to store them securely.");
            AddKnowledgeEntry("phishing",
                "Be cautious of unexpected emails asking for personal information. Always verify sender addresses before responding or clicking links.");
            AddKnowledgeEntry("privacy",
                "Review privacy settings monthly on social media and online accounts. Limit personal information shared publicly.");
            AddKnowledgeEntry("malware",
                "Malware includes viruses, worms, ransomware, and spyware. Keep antivirus software updated and avoid suspicious downloads.");
            AddKnowledgeEntry("vpn",
                "A VPN encrypts your internet connection, protecting data on public WiFi. Use reputable VPN services with no-log policies.");
            AddKnowledgeEntry("wifi",
                "Public WiFi can be risky. Use VPNs and avoid accessing sensitive accounts on unsecured networks.");
            AddKnowledgeEntry("authentication",
                "Two-factor authentication adds security beyond passwords. Use authenticator apps instead of SMS when possible.");
            AddKnowledgeEntry("2fa",
                "Two-factor authentication requires something you know (password) and something you have (phone/device).");
            AddKnowledgeEntry("browsing",
                "Safe browsing includes checking for HTTPS padlock icons, avoiding suspicious links, and keeping browsers updated.");
            AddKnowledgeEntry("update",
                "Regular software updates patch security vulnerabilities. Enable automatic updates when available.");
            AddKnowledgeEntry("backup",
                "Follow the 3-2-1 backup rule: 3 copies, 2 different media, 1 offsite location. Test backups regularly.");
            AddKnowledgeEntry("encryption",
                "Encryption scrambles data so only authorized parties can read it. Use full-disk encryption for mobile devices.");
            AddKnowledgeEntry("firewall",
                "Firewalls monitor network traffic. Enable both software and hardware firewalls for layered protection.");
            AddKnowledgeEntry("social engineering",
                "Attackers manipulate people into revealing information. Verify identities before sharing sensitive data.");
            AddKnowledgeEntry("ransomware",
                "Ransomware encrypts files until payment. Prevent with regular backups and cautious email handling.");
            AddKnowledgeEntry("phishing email",
                "Phishing emails often have urgent language, poor grammar, and suspicious links. Hover over links to check URLs.");
            AddKnowledgeEntry("password manager",
                "Password managers generate and store strong passwords. Use a master password and enable 2FA for the manager.");
            AddKnowledgeEntry("data breach",
                "If affected by a data breach, change passwords immediately and monitor accounts for suspicious activity.");
            AddKnowledgeEntry("public computer",
                "Never save passwords on public computers. Use private browsing mode and clear history after use.");
        }

        /// <summary>
        /// Initializes the list of words to ignore during processing
        /// </summary>
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

        /// <summary>
        /// Adds a knowledge entry to the dictionary
        /// </summary>
        /// <param name="topic">Topic name</param>
        /// <param name="response">Response text</param>
        private void AddKnowledgeEntry(string topic, string response)
        {
            if (!string.IsNullOrWhiteSpace(topic) && !string.IsNullOrWhiteSpace(response))
            {
                _knowledgeBase[topic] = response;
            }
        }
    }
}