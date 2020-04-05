using System;
using System.Runtime.InteropServices;

namespace KatBoard
{
    internal enum HookType : uint
    {
        WH_JOURNALRECORD = 0,
        WH_JOURNALPLAYBACK = 1,
        WH_KEYBOARD = 2,
        WH_GETMESSAGE = 3,
        WH_CALLWNDPROC = 4,
        WH_CBT = 5,
        WH_SYSMSGFILTER = 6,
        WH_MOUSE = 7,
        WH_HARDWARE = 8,
        WH_DEBUG = 9,
        WH_SHELL = 10,
        WH_FOREGROUNDIDLE = 11,
        WH_CALLWNDPROCRET = 12,
        WH_KEYBOARD_LL = 13,
        WH_MOUSE_LL = 14
    }

    internal static class SystemManager
    {
        private delegate int HookProc(int code, IntPtr wParam, IntPtr lParam);
        static IntPtr keyboardHook;

        public static bool LockKeyboard()
        {
            var hMod = Marshal.GetHINSTANCE(typeof(SystemManager).Module);
            keyboardHook = SetWindowsHookEx((int)HookType.WH_KEYBOARD_LL, HandleKeyboardProc, hMod, 0);
            if (keyboardHook == null)
            {
                return false;
            }
            return true;
        }

        public static bool UnlockKeyboard()
        {
            if (keyboardHook != null)
            {
                return UnhookWindowsHookEx(keyboardHook);
            }
            return false;
        }

        private static int HandleKeyboardProc(int code, IntPtr wParam, IntPtr lParam)
        {
            return -1;
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowsHookEx", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", EntryPoint = "UnhookWindowsHookEx", SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);
    }
}
