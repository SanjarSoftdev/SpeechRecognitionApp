using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading.Tasks;

namespace SpeechRecognitionApp
{
    public class SpeechHandler
    {
        private SpeechRecognitionEngine speechRecognizer;
        private SpeechSynthesizer speechSynthesizer;
        private Dictionary<string, string> AssistantCommands;
        public ObservableCollection<string> RecognizedPhrases { get; private set; }
        public event Action<string, bool> StatusUpdatedEvent;

        public SpeechHandler()
        {
            RecognizedPhrases = new ObservableCollection<string>();
            speechSynthesizer = new SpeechSynthesizer();
            InitializeSpeechRecognition();
        }

        private void InitializeSpeechRecognition()
        {
            speechRecognizer = new SpeechRecognitionEngine();
            speechRecognizer.SetInputToDefaultAudioDevice();
            SpeechRecognizerLoadGrammar();
            speechRecognizer.SpeechRecognized += SpeechRecognizer_SpeechRecognized;
        }

        public void SpeechRecognizerLoadGrammar()
        {
            AssistantCommands = VoiceCommandsPathHelper.LoadCommands();
            speechRecognizer.LoadGrammar(BuildGrammar());
        }

        private Grammar BuildGrammar()
        {
            var choices = new Choices(AssistantCommands.Keys.ToArray());
            var grammarBuilder = new GrammarBuilder(choices);
            return new Grammar(grammarBuilder);
        }

        private async void SpeechRecognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            // Update status to "Listening..." and wait for 100ms
            UpdateStatus("Listening...", true);
            await Task.Delay(100);

            var recognizedPhrase = e.Result.Text;
            string response = string.Empty;

            // Process recognized phrase and perform actions
            if (AssistantCommands.TryGetValue(recognizedPhrase, out response))
            {
                response = ReplacePlaceholders(response);
                this.PerformAction(recognizedPhrase);
                await Task.Run(async () =>
                {
                    Speak(response);
                    await Task.Delay(100);
                });
            }

            // Update status to "Recognition stopped" and wait for 100ms
            UpdateStatus("Recognition stopped", false);
            await Task.Delay(100);

            // Add recognized phrase and response to the list
            RecognizedPhrases.Add($"{recognizedPhrase} : {response}");
        }

        private void Speak(string response)
        {
            //speechSynthesizer.SpeakAsyncCancelAll();
            speechSynthesizer.Speak(response);
        }

        private string ReplacePlaceholders(string response)
        {
            if (response.Contains("-time-"))
                response = response.Replace("-time-", DateTime.Now.ToString("h:mm tt"));
            else if (response.Contains("-datetoday-"))
                response = response.Replace("-datetoday-", DateTime.Now.ToString("MMMM dd, yyyy"));

            return response;
        }

        private bool isRecognizing = false;
        public void StartRecognition()
        {
            if (!isRecognizing)
            {
                speechRecognizer.RecognizeAsync(RecognizeMode.Multiple);
                isRecognizing = true;
            }
        }

        public void StopRecognition()
        {
            if (isRecognizing)
            {
                speechRecognizer.RecognizeAsyncStop();
                isRecognizing = false;
            }
        }

        public void SetVoice(string voiceName) => speechSynthesizer.SelectVoice(voiceName);

        public void SetVoiceGender(VoiceGender gender) => speechSynthesizer.SelectVoiceByHints(gender);

        public void SetVoiceRate(int rate) => speechSynthesizer.Rate = rate;

        public void SetVoiceVolume(int volume) => speechSynthesizer.Volume = volume;

        public IEnumerable<string> GetInstalledVoices() => speechSynthesizer.GetInstalledVoices().Select(v => v.VoiceInfo.Name);

        private void UpdateStatus(string message, bool isActive) => StatusUpdatedEvent?.Invoke(message, isActive);
    }
}
