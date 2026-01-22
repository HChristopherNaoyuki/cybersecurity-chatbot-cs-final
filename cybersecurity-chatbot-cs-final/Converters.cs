using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace cybersecurity_chatbot_cs_final
{
    /// <summary>
    /// Converts message ownership to background color
    /// - User messages: Light blue
    /// - Bot messages: Light gray
    /// </summary>
    public class MessageBackgroundConverter : IValueConverter
    {
        /// <summary>
        /// Converts boolean isUserMessage to background brush
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isUserMessage)
            {
                if (isUserMessage)
                {
                    return new SolidColorBrush(Color.FromArgb(255, 220, 240, 255)); // Light blue for user
                }
                else
                {
                    return new SolidColorBrush(Color.FromArgb(255, 245, 245, 245)); // Light gray for bot
                }
            }

            return Brushes.White;
        }

        /// <summary>
        /// Not implemented for one-way conversion
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts message type to text color (always black)
    /// </summary>
    public class MessageForegroundConverter : IValueConverter
    {
        /// <summary>
        /// Always returns black brush for text
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Brushes.Black;
        }

        /// <summary>
        /// Not implemented for one-way conversion
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts message ownership to horizontal alignment
    /// - User messages: Right aligned
    /// - Bot messages: Left aligned
    /// </summary>
    public class MessageAlignmentConverter : IValueConverter
    {
        /// <summary>
        /// Converts boolean isUserMessage to horizontal alignment
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isUserMessage)
            {
                if (isUserMessage)
                {
                    return System.Windows.HorizontalAlignment.Right;
                }
                else
                {
                    return System.Windows.HorizontalAlignment.Left;
                }
            }

            return System.Windows.HorizontalAlignment.Left;
        }

        /// <summary>
        /// Not implemented for one-way conversion
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}