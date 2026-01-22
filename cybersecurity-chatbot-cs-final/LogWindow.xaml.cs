using System;
using System.Collections.Generic;
using System.Windows;

namespace cybersecurity_chatbot_cs_final
{
    /// <summary>
    /// Window for displaying user activity history
    /// </summary>
    public partial class LogWindow : Window
    {
        private readonly MemoryManager _memoryManager;

        /// <summary>
        /// Initializes the log window
        /// </summary>
        /// <param name="memoryManager">Memory manager instance</param>
        public LogWindow(MemoryManager memoryManager)
        {
            InitializeComponent();
            _memoryManager = memoryManager ?? throw new ArgumentNullException(nameof(memoryManager));
            LoadActivityLog();
        }

        /// <summary>
        /// Loads and displays activity log entries
        /// </summary>
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
                MessageBox.Show($"Failed to load history: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Handles Close button click
        /// </summary>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}