using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using Windows.Graphics;
using Windows.UI;
namespace SpeechRecognitionApp
{
    public sealed partial class MinimizedWindow : Window
    {
        private SpeechHandler speechHandler;
        public MinimizedWindow()
        {
            InitializeComponent();
            AppWindow.MoveAndResize(new RectInt32(100, 100, 400, 350));
            InitializeTitleBar();
            SetupListeningImage();
            InitializeSpeechHandler();
        }

        private void InitializeTitleBar()
        {
            var titleBar = AppWindow.TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Black;
            titleBar.ButtonForegroundColor = Colors.Black;
            titleBar.ExtendsContentIntoTitleBar = true;
        }

        private const string SpeekingImageSource = "ms-appx:///Assets/wave-sound.gif";
        private const string GiphyImageSource = "ms-appx:///Assets/giphy.gif";
        private const string CircleOutlineImageSource = "ms-appx:///Assets/circle-outline.png";
        private const string ListeningImageSource = "ms-appx:///Assets/load-33_128.gif";
        private void SetupListeningImage()
        {
            SpeekingImage.Source = CreateBitmapImage(CircleOutlineImageSource);
            SpeekingImage.Width = 200;
            SpeekingImage.Height = 300;

            ListeningImage.Source = CreateBitmapImage(ListeningImageSource);
            ListeningImage.Width = 120;
            ListeningImage.Height = 60;
            ListeningImage.Visibility = Visibility.Collapsed;
        }

        private void InitializeSpeechHandler()
        {
            speechHandler = new SpeechHandler();
            speechHandler.SetVoiceRate(2);
            speechHandler.SetVoiceVolume(80);
            speechHandler.StatusUpdatedEvent += UpdateImage;
        }

        private void UpdateImage(string message, bool isActive)
        {
            if (isActive)
                SpeekingImage.Source = new BitmapImage(new Uri(SpeekingImageSource));
            else
                SpeekingImage.Source = new BitmapImage(new Uri(GiphyImageSource));
        }

        BitmapImage CreateBitmapImage(string source)
        {
            return new BitmapImage(new Uri(source));
        }

        private void StackPanel_RightTapped(object sender, Microsoft.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            CreateAndShowMenuFlyout(sender);
        }

        void CreateAndShowMenuFlyout(object sender)
        {
            MenuFlyout myFlyout = new MenuFlyout();
            MenuFlyoutItem settingsMenu = new MenuFlyoutItem { Text = "Settings" };
            MenuFlyoutItem commandsMenu = new MenuFlyoutItem { Text = "Commands" };

            // Apply styles to the menu items
            SetMenuFlyoutItemStyle(settingsMenu);
            SetMenuFlyoutItemStyle(commandsMenu);

            settingsMenu.Click += SettingsMenu_Click;
            commandsMenu.Click += CommandsMenu_Click;

            myFlyout.Items.Add(settingsMenu);
            myFlyout.Items.Add(commandsMenu);

            FrameworkElement senderElement = sender as FrameworkElement;
            myFlyout.ShowMode = Microsoft.UI.Xaml.Controls.Primitives.FlyoutShowMode.Auto;
            myFlyout.ShowAt(senderElement);

            void SetMenuFlyoutItemStyle(MenuFlyoutItem flyoutItem)
            {
                flyoutItem.FontSize = 16;
                flyoutItem.Padding = new Thickness(10);
                flyoutItem.Foreground = new SolidColorBrush(Colors.White);
                flyoutItem.Background = new SolidColorBrush(Color.FromArgb(255, 98, 0, 238)); // #FF6200EE
                flyoutItem.Margin = new Thickness(5);
                flyoutItem.CornerRadius = new CornerRadius(5);
            }
        }

        private void CommandsMenu_Click(object sender, RoutedEventArgs e)
        {
            AssistantCommands assistantCommands = new AssistantCommands(speechHandler);
            assistantCommands.Activate();
        }

        private void SettingsMenu_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow(speechHandler);
            mainWindow.Activate();
        }

        private void Grid_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == Microsoft.UI.Input.PointerDeviceType.Mouse)
            {
                var properties = e.GetCurrentPoint((UIElement)sender).Properties;
                if (properties.IsLeftButtonPressed)
                {
                }
            }
        }

        private bool isRecognizing = false;
        private void MicrophoneButton_Click(object sender, RoutedEventArgs e)
        {
            if (isRecognizing)
            {
                speechHandler.StopRecognition();
                SpeekingImage.Source = CreateBitmapImage(CircleOutlineImageSource);
                ListeningImage.Visibility = Visibility.Collapsed;
                MicrophoneButtonSymbolIcon.Symbol = Symbol.Microphone;
                isRecognizing = false;
            }
            else
            {
                speechHandler.StartRecognition();
                SpeekingImage.Source = CreateBitmapImage(GiphyImageSource);
                ListeningImage.Visibility = Visibility.Visible;
                MicrophoneButtonSymbolIcon.Symbol = Symbol.Stop;
                isRecognizing = true;
            }
        }
    }
}
