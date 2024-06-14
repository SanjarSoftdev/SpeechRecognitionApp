using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SpeechRecognitionApp
{
    public static class ApplicationManager
    {
        public static void OpenApplication(string applicationName)
        {
            string processPath = GetApplicationPath(applicationName);

            if (string.IsNullOrEmpty(processPath))
            {
                Console.WriteLine($"Application '{applicationName}' is not supported.");
                return;
            }

            try
            {
                if (Uri.IsWellFormedUriString(processPath, UriKind.Absolute))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = processPath,
                        UseShellExecute = true
                    });
                }
                else
                {
                    Process.Start(processPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting process: {ex.Message}");
            }
        }

        public static async Task OpenApplicationAsync(string applicationName)
        {
            string processPath = GetApplicationPath(applicationName);

            if (string.IsNullOrEmpty(processPath))
            {
                Console.WriteLine($"Application '{applicationName}' is not supported.");
                return;
            }

            try
            {
                if (Uri.IsWellFormedUriString(processPath, UriKind.Absolute))
                {
                    await Task.Run(() =>
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = processPath,
                            UseShellExecute = true
                        });
                    });
                }
                else
                {
                    await Task.Run(() =>
                    {
                        Process.Start(processPath);
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting process: {ex.Message}");
            }
        }

        public static void CloseApplication(string applicationName)
        {
            try
            {
                string processName = GetProcessNameForClose(applicationName) ?? GetApplicationName(applicationName);
                foreach (var process in Process.GetProcessesByName(processName))
                    process.Kill();
            }
            catch { }
        }

        private static string GetApplicationPath(string applicationName)
        {
            return applicationName.ToLower() switch
            {
                "word" => @"C:\Program Files\Microsoft Office\root\Office16\WINWORD.EXE",
                "excel" => @"C:\Program Files\Microsoft Office\root\Office16\EXCEL.EXE",
                "notepad" => @"C:\Windows\System32\notepad.exe",
                "paint" => @"C:\Program Files\WindowsApps\Microsoft.Paint_11.2309.30.0_x64__8wekyb3d8bbwe\PaintApp\mspaint.exe",
                "music" => @"C:\Program Files\AIMP\AIMP.exe",
                "some music" => @"C:\Program Files\AIMP\AIMP.exe",
                "news" => "https://asiaplustj.info/",
                "weather" => "https://www.weather-atlas.com/en/tajikistan",
                "weather forecast" => "https://www.weather-atlas.com/en/tajikistan",
                _ => GetApplicationName(applicationName)
            };
        }

        private static string GetApplicationName(string applicationName)
        {
            return applicationName.ToLower() switch
            {
                "word" => "winword",
                "excel" => "excel",
                "powerpoint" => "powerpnt",
                "outlook" => "outlook",
                "notepad" => "notepad",
                "paint" => "mspaint",
                "calc" => "calculator",
                "calculator" => "calc",
                "cmd" => "cmd",
                "explorer" => "explorer",
                "news" => "msedge",
                "music" => "AIMP",
                "browser" => "msedge",
                "chrome" => "chrome",
                "firefox" => "firefox",
                _ => applicationName
            };
        }

        private static string GetProcessNameForClose(string applicationName)
        {
            return applicationName.ToLower() switch
            {
                "calculator" => "calculator",
                _ => null
            };
        }
    }
}
