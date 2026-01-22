using System.Windows;

namespace cybersecurity_chatbot_cs_final
{
    /// <summary>
    /// Dialog for collecting user's name
    /// </summary>
    public partial class NameInputDialog : Window
    {
        /// <summary>
        /// Gets the entered user name
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Initializes the name input dialog
        /// </summary>
        public NameInputDialog()
        {
            InitializeComponent();
            UserName = null;
        }

        /// <summary>
        /// Handles OK button click
        /// </summary>
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            UserName = NameTextBox.Text?.Trim();
            DialogResult = !string.IsNullOrWhiteSpace(UserName);
            Close();
        }

        /// <summary>
        /// Handles Cancel button click
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}