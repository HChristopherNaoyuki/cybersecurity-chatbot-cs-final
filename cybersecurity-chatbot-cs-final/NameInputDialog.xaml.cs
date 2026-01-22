using System.Windows;

namespace cybersecurity_chatbot_cs_final
{
    public partial class NameInputDialog : Window
    {
        public string UserName { get; private set; }

        public NameInputDialog()
        {
            InitializeComponent();
            UserName = null;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            UserName = NameTextBox.Text?.Trim();
            DialogResult = !string.IsNullOrWhiteSpace(UserName);
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}