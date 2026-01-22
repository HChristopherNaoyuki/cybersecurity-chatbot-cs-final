using System;
using System.IO;
using System.Media;
using System.Windows;

namespace cybersecurity_chatbot_cs_final
{
    public class UserInterface
    {
        private readonly MainWindow _mainWindow;

        public UserInterface(MainWindow mainWindow)
        {
            _mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
            Directory.CreateDirectory("Audio");
        }

        public void PlayVoiceGreeting()
        {
            try
            {
                string audioPath = Path.Combine("Audio", "welcome.wav");

                if (File.Exists(audioPath))
                {
                    using (var player = new SoundPlayer(audioPath))
                    {
                        player.Play();
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public void DisplayAsciiArt()
        {
            _mainWindow.DisplayAsciiArt(@"
███████╗██╗   ██╗███████╗███████╗███████╗ ██████╗██╗   ██╗███████╗██████╗ ██╗████████╗██╗   ██╗
██╔════╝╚██╗ ██╔╝██╔════╝██╔════╝██╔════╝██╔════╝██║   ██║██╔════╝██╔══██╗██║╚══██╔══╝╚██╗ ██╔╝
███████╗ ╚████╔╝ ███████╗█████╗  ███████╗██║     ██║   ██║█████╗  ██████╔╝██║   ██║    ╚████╔╝ 
╚════██║  ╚██╔╝  ╚════██║██╔══╝  ╚════██║██║     ██║   ██║██╔══╝  ██╔══██╗██║   ██║     ╚██╔╝  
███████║   ██║   ███████║███████╗███████║╚██████╗╚██████╔╝███████╗██║  ██║██║   ██║      ██║   
╚══════╝   ╚═╝   ╚══════╝╚══════╝╚══════╝ ╚═════╝ ╚═════╝ ╚══════╝╚═╝  ╚═╝╚═╝   ╚═╝      ╚═╝   
");
        }

        public string GetUserName()
        {
            var dialog = new NameInputDialog();
            if (dialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(dialog.UserName))
            {
                return dialog.UserName.Trim();
            }
            return "User";
        }

        public void DisplayError(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                _mainWindow.AddMessage($"Error: {message}", false);
            }
        }
    }
}