using Microsoft.UI.Dispatching;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Speech.Synthesis;
using Windows.Graphics;

namespace SpeechRecognitionApp
{
    public partial class MainWindow : Window
    {
        private SpeechHandler speechHandler;

        public MainWindow(SpeechHandler _speechHandler)
        {
            speechHandler = _speechHandler;

            InitializeComponent();
            InitializeUI();
            InitializeSpeechHandler();
            InitializeVoiceSettings();
        }

        private void InitializeUI()
        {
            ResizeWindow();
            ExtendsContentIntoTitleBar = true;
        }

        private void ResizeWindow(int width = 700, int height = 700) => AppWindow.Resize(new SizeInt32 { Width = width, Height = height });

        private void InitializeSpeechHandler()
        {
            speechHandler.StatusUpdatedEvent += UpdateStatus;
            RecognizedPhrasesListBox.ItemsSource = speechHandler.RecognizedPhrases;
        }

        private void InitializeVoiceSettings()
        {
            foreach (var voice in speechHandler.GetInstalledVoices())
                VoiceComboBox.Items.Add(voice);

            if (VoiceComboBox.Items.Count > 0)
                VoiceComboBox.SelectedIndex = 0;
        }

        private void StartRecognitionButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatus("Listening...", isActive: true);
            speechHandler.StartRecognition();
        }

        private void StopRecognitionButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatus("Recognition stopped", isActive: false);
            speechHandler.StopRecognition();
        }

        private void VoiceSettingsSpeedValueChanged(object sender, Microsoft.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            speechHandler?.SetVoiceRate((int)VoiceSpeedSlider.Value);
        }

        private void VoiceGenderSettingsChanged(object sender, SelectionChangedEventArgs e)
        {
            SetVoiceGender();
        }

        private void VoiceSettingsChanged(object sender, SelectionChangedEventArgs e)
        {
            speechHandler?.SetVoice(VoiceComboBox.SelectedItem.ToString());
            SetVoiceGender();
        }

        private void SetVoiceGender()
        {
            if (GenderComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                var genderString = selectedItem.Content.ToString();
                if (Enum.TryParse(genderString, out VoiceGender gender))
                    speechHandler?.SetVoiceGender(gender);
            }
        }

        private void VoiceSettingsVolumeValueChanged(object sender, Microsoft.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            speechHandler?.SetVoiceVolume((int)VoiceVolumeSlider.Value);
        }

        private void UpdateStatus(string message, bool isActive)
        {
            DispatcherQueue.TryEnqueue(() =>
            {
                RecognizedTextBox.Text += $"{message}...\n";
                RecognitionProgressRing.IsActive = isActive;
                StatusTextBlock.Text = message;
            });
        }

        private void ClearTextButton_Click(object sender, RoutedEventArgs e)
        {
            RecognizedTextBox.Text = string.Empty;
            speechHandler.RecognizedPhrases.Clear();
            StatusTextBlock.Text = string.Empty;
        }
    }
}
