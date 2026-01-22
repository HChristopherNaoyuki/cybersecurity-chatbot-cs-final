using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;

namespace cybersecurity_chatbot_cs_final
{
    /// <summary>
    /// Converts message ownership to background color
    /// - User messages: Light blue
    /// - Bot messages: Light gray
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Brush))]
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
                    // Light blue for user messages
                    return new SolidColorBrush(Color.FromArgb(255, 220, 240, 255));
                }
                else
                {
                    // Light gray for bot messages
                    return new SolidColorBrush(Color.FromArgb(255, 245, 245, 245));
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
    [ValueConversion(typeof(bool), typeof(Brush))]
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
    [ValueConversion(typeof(bool), typeof(HorizontalAlignment))]
    public class MessageAlignmentConverter : IValueConverter
    {
        /// <summary>
        /// Converts boolean isUserMessage to horizontal alignment
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isUserMessage)
            {
                return isUserMessage ? HorizontalAlignment.Right : HorizontalAlignment.Left;
            }
            return HorizontalAlignment.Left;
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