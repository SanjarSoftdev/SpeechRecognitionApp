using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeechRecognitionApp
{
    public class CommandManager
    {
        private SpeechHandler speechHandler;
        private Dictionary<string, string> assistantCommands;
        private Action updateCommandsListBox;

        public CommandManager(SpeechHandler speechHandler, Action updateCommandsListBox)
        {
            this.speechHandler = speechHandler;
            this.assistantCommands = VoiceCommandsPathHelper.LoadCommands();
            this.updateCommandsListBox = updateCommandsListBox;
        }

        public bool CommandExists(string question)
        {
            return assistantCommands.ContainsKey(question);
        }

        public void AddCommand(string question, string answer)
        {
            assistantCommands.Add(question, answer);
            VoiceCommandsPathHelper.RewriteCommands(assistantCommands);
            speechHandler.SpeechRecognizerLoadGrammar();
            updateCommandsListBox();
        }

        public bool DeleteCommand(string selectedCommand)
        {
            var question = ExtractQuestion(selectedCommand);
            if (assistantCommands.ContainsKey(question))
            {
                assistantCommands.Remove(question);
                VoiceCommandsPathHelper.RewriteCommands(assistantCommands);
                speechHandler.SpeechRecognizerLoadGrammar();
                updateCommandsListBox();
                return true;
            }
            return false;
        }

        public List<string> GetFilteredCommands(string filter)
        {
            return assistantCommands
                .Where(c => c.Key.ToLower().Contains(filter) || c.Value.ToLower().Contains(filter))
                .Select(c => $"Q: {c.Key}\nA: {c.Value}")
                .ToList();
        }

        private string ExtractQuestion(string selectedCommand)
        {
            const string startMarker = "Q: ";
            const string endMarker = "\nA: ";

            int startIndex = selectedCommand.IndexOf(startMarker) + startMarker.Length;
            int endIndex = selectedCommand.IndexOf(endMarker);
            return selectedCommand.Substring(startIndex, endIndex - startIndex).Trim();
        }
    }
}
