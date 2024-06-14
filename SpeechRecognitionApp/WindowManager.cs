using System;
using System.Runtime.InteropServices;

namespace SpeechRecognitionApp
{
    public static class WindowManager
    {
        public static void MinimizeWindow()
        {
            var hwnd = GetActiveWindow();
            ShowWindow(hwnd, SW_MINIMIZE);
        }

        public static void MaximizeWindow()
        {
            var hwnd = GetActiveWindow();
            ShowWindow(hwnd, SW_MAXIMIZE);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_MINIMIZE = 6;
        private const int SW_MAXIMIZE = 3;
    }
}
