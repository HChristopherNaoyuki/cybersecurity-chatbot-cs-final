using System;
using System.IO;
using System.Media;
using System.Windows;

namespace cybersecurity_chatbot_cs_final
{
    /// <summary>
    /// Handles user interface interactions
    /// </summary>
    public class UserInterface
    {
        private readonly MainWindow _mainWindow;

        /// <summary>
        /// Initializes user interface handler
        /// </summary>
        /// <param name="mainWindow">Main window reference</param>
        public UserInterface(MainWindow mainWindow)
        {
            _mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
            Directory.CreateDirectory("Audio");
        }

        /// <summary>
        /// Plays welcome voice greeting
        /// </summary>
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
                // Silent fail if audio can't play
            }
        }

        /// <summary>
        /// Displays cybersecurity-themed ASCII art
        /// </summary>
        public void DisplayAsciiArt()
        {
            _mainWindow.DisplayAsciiArt(@"
   _____      _                _____           _              
  / ____|    | |              / ____|         | |             
 | |    _   _| |__  _   _ ___| (___  _   _ ___| |_ ___ _ __   
 | |   | | | | '_ \| | | / __|\___ \| | | / __| __/ _ \ '_ \  
 | |___| |_| | |_) | |_| \__ \____) | |_| \__ \ ||  __/ | | | 
  \_____\__, |_.__/ \__,_|___/_____/ \__, |___/\__\___|_| |_| 
         __/ |                        __/ |                    
        |___/                        |___/                     
");
        }

        /// <summary>
        /// Gets user name via dialog
        /// </summary>
        /// <returns>User name or "User" if canceled</returns>
        public string GetUserName()
        {
            var dialog = new NameInputDialog();
            if (dialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(dialog.UserName))
            {
                return dialog.UserName.Trim();
            }
            return "User";
        }

        /// <summary>
        /// Displays error message in chat
        /// </summary>
        /// <param name="message">Error message</param>
        public void DisplayError(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                _mainWindow.AddMessage($"Error: {message}", false);
            }
        }
    }
}