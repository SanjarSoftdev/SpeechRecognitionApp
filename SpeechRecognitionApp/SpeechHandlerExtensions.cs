namespace SpeechRecognitionApp
{
    public static class SpeechHandlerExtensions
    {
        private const string OpenCommand = "open";
        private const string ShowCommand = "show";
        private const string CommandsPage = "commands";
        private const string SettingsPage = "settings";
        private const string CloseCommand = "close";
        private const string MinimizeCommand = "minimize";
        private const string MaximizeCommand = "maximize";
        private const string PlayCommand = "play";

        public static void PerformAction(this SpeechHandler speechHandler, string recognizedPhrase)
        {
            if (recognizedPhrase.StartsWith(ShowCommand))
            {
                var recognizedCommand = recognizedPhrase.Substring(ShowCommand.Length).Trim();
                if (recognizedCommand.Equals(CommandsPage))
                {
                    var assistantCommands = new AssistantCommands(speechHandler);
                    assistantCommands.Activate();
                }
                else if (recognizedCommand.Equals(SettingsPage))
                {
                    var mainWindow = new MainWindow(speechHandler);
                    mainWindow.Activate();
                }
                else
                {
                    ApplicationManager.OpenApplication(ProcessPhrase(recognizedPhrase, ShowCommand));
                }
            }
            else if (recognizedPhrase.StartsWith(OpenCommand))
            {
                ApplicationManager.OpenApplication(ProcessPhrase(recognizedPhrase, OpenCommand));
            }
            else if (recognizedPhrase.StartsWith(CloseCommand))
            {
                ApplicationManager.CloseApplication(ProcessPhrase(recognizedPhrase, CloseCommand));
            }
            else if (recognizedPhrase.Contains(PlayCommand))
            {
                ApplicationManager.OpenApplication(ProcessPhrase(recognizedPhrase, PlayCommand));
            }
            else if (recognizedPhrase.StartsWith(MinimizeCommand))
            {
                WindowManager.MinimizeWindow();
            }
            else if (recognizedPhrase.StartsWith(MaximizeCommand))
            {
                WindowManager.MaximizeWindow();
            }
        }

        private static string ProcessPhrase(string recognizedPhrase, string command)
        {
            return recognizedPhrase.Substring(command.Length).Trim();
        }
    }
}
