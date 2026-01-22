using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace cybersecurity_chatbot_cs_final
{
    public partial class MainWindow : Window
    {
        private readonly ChatBot _chatBot;
        public ObservableCollection<ChatMessage> Messages { get; }

        public MainWindow()
        {
            InitializeComponent();

            Messages = new ObservableCollection<ChatMessage>();
            ChatHistory.ItemsSource = Messages;

            _chatBot = new ChatBot(this);
            _chatBot.Run();
        }

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

        public void DisplayAsciiArt(string asciiArt)
        {
            Dispatcher.Invoke(() =>
            {
                AsciiArtBox.Text = asciiArt ?? string.Empty;
            });
        }

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

        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ProcessInput();
                e.Handled = true;
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessInput();
        }

        private void TasksButton_Click(object sender, RoutedEventArgs e)
        {
            new TaskWindow(_chatBot).ShowDialog();
        }

        private void QuizButton_Click(object sender, RoutedEventArgs e)
        {
            new QuizWindow(_chatBot).ShowDialog();
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            new LogWindow(_chatBot.MemoryManager).ShowDialog();
        }
    }

    public class ChatMessage
    {
        public string Text { get; }
        public bool IsUserMessage { get; }

        public ChatMessage(string text, bool isUserMessage)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
            IsUserMessage = isUserMessage;
        }
    }

    public class MessageAlignmentConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool isUserMessage)
            {
                return isUserMessage ? System.Windows.HorizontalAlignment.Right : System.Windows.HorizontalAlignment.Left;
            }
            return System.Windows.HorizontalAlignment.Left;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}