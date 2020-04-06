using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Hardcodet.Wpf.TaskbarNotification;
using NHotkey;
using NHotkey.Wpf;

namespace KatBoard
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IDisposable
    {
        private TaskbarIcon trayIcon;
        private MenuItem lockMenuItem;
        private bool keyboardLocked;
        private SettingWindow settingWindow;
        private UserSetting userSetting;
        private Mutex mutex;

        public App()
        {
            CheckSingleton();
            userSetting = new UserSetting();
            MainWindow = new Window();
            MainWindow.Visibility = Visibility.Hidden;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            trayIcon = new TaskbarIcon();
            trayIcon.IconSource = new BitmapImage(ResourceAccessor.Get(@"Assets\Cat.ico"));
            trayIcon.ToolTipText = "KatBoard is running";
            trayIcon.ContextMenu = GetTrayContextMenu();

            keyboardLocked = false;

            if (!string.IsNullOrEmpty(userSetting.Shortcut))
            {
                SetShortcutKeyBinding();
            }
        }

        private void CheckSingleton()
        {
            bool isNew;
            mutex = new Mutex(true, "KatBoardApp", out isNew);

            if (!isNew)
            {
                mutex = null;
                Shutdown();
            }
        }

        private ContextMenu GetTrayContextMenu()
        {
            ContextMenu contextMenu = new ContextMenu();

            lockMenuItem = new MenuItem()
            {
                Header = "Lock Keyboard"
            };

            lockMenuItem.Click += HandleToggleLockRequest;

            MenuItem settingMenu = new MenuItem()
            {
                Header = "Settings"
            };
            settingMenu.Click += HandleSettingMenuClicked;


            MenuItem exitMenuItem = new MenuItem()
            {
                Header = "Exit"
            };
            exitMenuItem.Click += (object sender, RoutedEventArgs e) =>
            {
                Shutdown();
            };

            contextMenu.Items.Add(lockMenuItem);
            contextMenu.Items.Add(settingMenu);
            contextMenu.Items.Add(exitMenuItem);

            return contextMenu;
        }

        private void HandleToggleLockRequest(object sender, RoutedEventArgs e)
        {
            if (!keyboardLocked)
            {

                if (SystemManager.LockKeyboard())
                {
                    trayIcon.ToolTipText = "Keyboard has been locked";
                    lockMenuItem.Header = "Unlock Keyboard";
                    keyboardLocked = true;
                }
            }
            else
            {
                if(SystemManager.UnlockKeyboard())
                {
                    trayIcon.ToolTipText = "KatBoard is running";
                    lockMenuItem.Header = "Lock Keyboard";
                    keyboardLocked = false;
                }
            }
        }

        private void HandleLockKeyboardRequest(object sender, HotkeyEventArgs e)
        {
            if (!keyboardLocked)
            {
                if (SystemManager.LockKeyboard())
                {
                    trayIcon.ToolTipText = "Keyboard has been locked";
                    lockMenuItem.Header = "Unlock Keyboard";
                    keyboardLocked = true;
                }
            }
        }

        private void HandleSettingMenuClicked(object sender, RoutedEventArgs e)
        {
            if (settingWindow == null)
            {
                settingWindow = new SettingWindow();
                settingWindow.Show();
                settingWindow.Closed += HandleSettingWindowClosed;
                return;
            }

            if (settingWindow.WindowState == WindowState.Minimized)
            {
                settingWindow.WindowState = WindowState.Normal;
            }

            if (!settingWindow.IsActive)
            {
                settingWindow.Activate();
            }
        }

        private void HandleSettingWindowClosed(object sender, EventArgs e)
        {
            settingWindow = null;
        }

        private void SetShortcutKeyBinding()
        {
            KeyGestureConverter keyGestureConverter = new KeyGestureConverter();
            KeyGesture keyGesture = (KeyGesture)keyGestureConverter.ConvertFrom(userSetting.Shortcut);

            if (keyGesture != null)
            {
                try
                {
                    HotkeyManager.Current.AddOrReplace("lockKeyboard", keyGesture.Key, keyGesture.Modifiers, HandleLockKeyboardRequest);
                }
                catch
                {
                }
            }
        }

        public void Dispose()
        {
            if(mutex != null)
            {
                mutex.ReleaseMutex();
                mutex.Close();
                mutex = null;
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Dispose();
            base.OnExit(e);
        }
    }
}
