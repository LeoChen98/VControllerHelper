using System;

namespace VControllerHelper.Helpers
{
    internal class WindowButtonClickHelper
    {
        #region Public Methods

        public static void Click(IntPtr hwnd)
        {
            Win32ApiHelper.SendMessage(hwnd, 0x0201, IntPtr.Zero, IntPtr.Zero);
            Win32ApiHelper.SendMessage(hwnd, 0x0202, IntPtr.Zero, IntPtr.Zero);
        }

        #endregion Public Methods
    }
}