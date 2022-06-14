using System;
using System.Runtime.InteropServices;

namespace VControllerHelper.Helpers
{
    internal class WindowTextInsertHelper
    {
        #region Public Methods

        public static void SetText(IntPtr hwnd, string text)
        {
            Win32ApiHelper.SendMessage(hwnd, 0x000C, IntPtr.Zero, Marshal.StringToHGlobalAnsi(text));
        }

        #endregion Public Methods
    }
}