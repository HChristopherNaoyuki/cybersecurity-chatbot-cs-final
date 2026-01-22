using System;
using System.Windows.Data;
using System.Windows.Media;

namespace cybersecurity_chatbot_cs_final
{
    public class MessageBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool isUserMessage)
            {
                return isUserMessage
                    ? new SolidColorBrush(Color.FromArgb(255, 220, 240, 255))  // Light blue for user
                    : new SolidColorBrush(Color.FromArgb(255, 245, 245, 245)); // Light gray for bot
            }
            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MessageForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}