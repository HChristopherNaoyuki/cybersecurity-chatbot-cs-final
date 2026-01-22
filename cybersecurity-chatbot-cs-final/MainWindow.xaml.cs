using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace cybersecurity_chatbot_cs_final
{
    /// <summary>
    /// Main application window handling chat interface
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ChatBot _chatBot;
        public ObservableCollection<ChatMessage> Messages { get; }

        /// <summary>
        /// Initializes main window and chatbot
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            Messages = new ObservableCollection<ChatMessage>();
            ChatHistory.ItemsSource = Messages;

            _chatBot = new ChatBot(this);
            _chatBot.Run();
        }

        /// <summary>
        /// Adds a message to chat history
        /// </summary>
        /// <param name="text">Message content</param>
        /// <param name="isUserMessage">True if from user</param>
        public void AddMessage(string text, bool isUserMessage)
        {
            Dispatcher.Invoke(() =>
            {
                string prefix = isUserMessage ? "You" : "Bot";
                Messages.Add(new ChatMessage($"{prefix}: {text}", isUserMessage));

                if (Messages.Count > 0)
                {
                    ChatHistory.ScrollIntoView(Messages[Messages.Count - 1]);
                }
            });
        }

        /// <summary>
        /// Displays ASCII art in header
        /// </summary>
        public void DisplayAsciiArt(string asciiArt)
        {
            Dispatcher.Invoke(() =>
            {
                AsciiArtBox.Text = asciiArt ?? string.Empty;
            });
        }

        /// <summary>
        /// Processes user input from text box
        /// </summary>
        private void ProcessInput()
        {
            if (!string.IsNullOrWhiteSpace(UserInput.Text))
            {
                string input = UserInput.Text.Trim();
                AddMessage(input, true);
                _chatBot.ProcessInput(input);
                UserInput.Clear();
                UserInput.Focus();
            }
        }

        /// <summary>
        /// Handles Enter key in input box
        /// </summary>
        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ProcessInput();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handles Send button click
        /// </summary>
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessInput();
        }

        /// <summary>
        /// Opens task management window
        /// </summary>
        private void TasksButton_Click(object sender, RoutedEventArgs e)
        {
            new TaskWindow(_chatBot).ShowDialog();
        }

        /// <summary>
        /// Opens quiz window
        /// </summary>
        private void QuizButton_Click(object sender, RoutedEventArgs e)
        {
            new QuizWindow(_chatBot).ShowDialog();
        }

        /// <summary>
        /// Opens activity history window
        /// </summary>
        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            new LogWindow(_chatBot.MemoryManager).ShowDialog();
        }
    }

    /// <summary>
    /// Represents a single chat message
    /// </summary>
    public class ChatMessage
    {
        public string Text { get; }
        public bool IsUserMessage { get; }

        /// <summary>
        /// Creates a new chat message
        /// </summary>
        public ChatMessage(string text, bool isUserMessage)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
            IsUserMessage = isUserMessage;
        }
    }
}