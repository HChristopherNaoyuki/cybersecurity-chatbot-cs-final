using System;
using System.Windows;

namespace cybersecurity_chatbot_cs_final
{
    /// <summary>
    /// Main chatbot controller coordinating all components
    /// </summary>
    public class ChatBot
    {
        private readonly MainWindow _mainWindow;
        private readonly UserInterface _ui;
        private readonly KnowledgeBase _knowledgeBase;
        private readonly ConversationManager _conversationManager;
        private readonly MemoryManager _memoryManager;

        /// <summary>
        /// Initializes a new ChatBot instance
        /// </summary>
        /// <param name="mainWindow">Reference to main application window</param>
        public ChatBot(MainWindow mainWindow)
        {
            _mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));

            _ui = new UserInterface(_mainWindow);
            _knowledgeBase = new KnowledgeBase();
            _memoryManager = new MemoryManager();
            _conversationManager = new ConversationManager(_knowledgeBase, _memoryManager, _ui);
        }

        /// <summary>
        /// Gets the memory manager instance
        /// </summary>
        public MemoryManager MemoryManager
        {
            get { return _memoryManager; }
        }

        /// <summary>
        /// Starts the chatbot session
        /// </summary>
        public void Run()
        {
            try
            {
                _ui.PlayVoiceGreeting();
                _ui.DisplayAsciiArt();

                string userName = _ui.GetUserName();
                _memoryManager.UserName = userName;
                _mainWindow.AddMessage($"Welcome {userName}. How can I help you with cybersecurity?", false);
            }
            catch (Exception ex)
            {
                _ui.DisplayError($"Initialization error: {ex.Message}");
            }
        }

        /// <summary>
        /// Processes user input and generates responses
        /// </summary>
        /// <param name="input">User input text</param>
        public void ProcessInput(string input)
        {
            try
            {
                string response = _conversationManager.ProcessUserInput(input);

                if (response == "COMMAND_EXIT")
                {
                    _mainWindow.AddMessage("Goodbye. Stay safe online.", false);
                    System.Windows.Application.Current.Shutdown();
                    return;
                }

                _mainWindow.AddMessage(response, false);
            }
            catch (Exception ex)
            {
                _ui.DisplayError($"Error processing input: {ex.Message}");
            }
        }
    }
}