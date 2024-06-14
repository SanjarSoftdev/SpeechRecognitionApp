using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;

namespace SpeechRecognitionApp
{
    public static class UIHelper
    {
        public static void SetMessage(TextBlock textBlock, string text, bool isSuccess = false)
        {
            textBlock.Text = text;
            textBlock.Foreground = string.IsNullOrEmpty(text)
                ? new SolidColorBrush(Colors.Transparent)
                : new SolidColorBrush(isSuccess ? Colors.Green : Colors.Red);
        }

        public static void ClearMessage(TextBlock textBlock)
        {
            textBlock.Text = string.Empty;
            textBlock.Foreground = new SolidColorBrush(Colors.Transparent);
        }
    }
}
