using System;
using System.Collections.Generic;
using System.Windows;

namespace cybersecurity_chatbot_cs_final
{
    public partial class LogWindow : Window
    {
        private readonly MemoryManager _memoryManager;

        public LogWindow(MemoryManager memoryManager)
        {
            InitializeComponent();
            _memoryManager = memoryManager ?? throw new ArgumentNullException(nameof(memoryManager));
            LoadActivityLog();
        }

        private void LoadActivityLog()
        {
            try
            {
                var history = _memoryManager.GetActivityHistory();

                if (history.Count == 0)
                {
                    LogEntries.ItemsSource = new List<string> { "No activities recorded" };
                    return;
                }

                LogEntries.ItemsSource = history;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading history: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}