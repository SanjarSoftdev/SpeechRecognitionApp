using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
namespace SpeechRecognitionApp
{
    public static class VoiceCommandsPathHelper
    {
        private const string CommandsFileName = "AssistantCommands.json";
        private static readonly string filePath = Path.Combine(GetCurrentProjectPath(), CommandsFileName);

        public static Dictionary<string, string> LoadCommands()
        {
            try
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException("The specified file was not found.");

                string json = File.ReadAllText(filePath);
                var commands = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                return commands;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading commands: {ex.Message}");
                return null;
            }
        }

        public static void RewriteCommands(Dictionary<string, string> commands)
        {
            try
            {
                string json = JsonConvert.SerializeObject(commands);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while writing commands: {ex.Message}");
            }
        }

        public static string GetCurrentProjectPath()
        {
            // Get the base directory
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Split the directory into parts
            string[] pathParts = baseDirectory.Split(Path.DirectorySeparatorChar);

            // Find the index of the 'bin' directory
            int binIndex = Array.IndexOf(pathParts, "bin");

            if (binIndex == -1)
            {
                throw new InvalidOperationException("The 'bin' directory was not found in the current path.");
            }

            // Combine the parts up to the 'bin' directory to get the project path
            string projectPath = Path.Combine(pathParts[..binIndex]);

            // Convert to full path
            return Path.GetFullPath(projectPath);
        }
    }
}
