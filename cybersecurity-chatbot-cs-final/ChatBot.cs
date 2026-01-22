using System;

namespace cybersecurity_chatbot_cs_final
{
    public class ChatBot
    {
        private readonly MainWindow _mainWindow;
        private readonly UserInterface _ui;
        private readonly KnowledgeBase _knowledgeBase;
        private readonly ConversationManager _conversationManager;
        private readonly MemoryManager _memoryManager;

        public ChatBot(MainWindow mainWindow)
        {
            _mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));

            _ui = new UserInterface(_mainWindow);
            _knowledgeBase = new KnowledgeBase();
            _memoryManager = new MemoryManager();
            _conversationManager = new ConversationManager(_knowledgeBase, _memoryManager, _ui);
        }

        public MemoryManager MemoryManager => _memoryManager;

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

        public void ProcessInput(string input)
        {
            try
            {
                string response = _conversationManager.ProcessUserInput(input);

                if (response == "COMMAND_EXIT")
                {
                    _mainWindow.AddMessage("Goodbye. Stay safe online.", false);
                    Application.Current.Shutdown();
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