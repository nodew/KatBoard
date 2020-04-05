using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Hardcodet.Wpf.TaskbarNotification;

namespace KatBoard
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon trayIcon;
        private MenuItem lockMenuItem;
        private bool keyboardLocked;

        protected override void OnStartup(StartupEventArgs e)
        {
            trayIcon = new TaskbarIcon();
            trayIcon.IconSource = new BitmapImage(ResourceAccessor.Get(@"Assets\Cat.ico"));
            trayIcon.ToolTipText = "KatBoard is running";
            trayIcon.ContextMenu = GetTrayContextMenu();
            this.MainWindow = new Window();
            this.MainWindow.Visibility = Visibility.Hidden;
            keyboardLocked = false;
        }

        private ContextMenu GetTrayContextMenu()
        {
            ContextMenu contextMenu = new ContextMenu();

            lockMenuItem = new MenuItem()
            {
                Header = "Lock Keyboard"
            };

            lockMenuItem.Click += HandleLockRequest;

            MenuItem exitMenuItem = new MenuItem()
            {
                Header = "Exit"
            };
            exitMenuItem.Click += (object sender, RoutedEventArgs e) =>
            {
                Application.Current.Shutdown();
            };

            contextMenu.Items.Add(lockMenuItem);
            contextMenu.Items.Add(exitMenuItem);

            return contextMenu;
        }

        private void HandleLockRequest(object sender, RoutedEventArgs e)
        {
            if (!keyboardLocked)
            {

                if (SystemManager.LockKeyboard())
                {
                    lockMenuItem.Header = "Unlock Keyboard";
                    keyboardLocked = true;
                }
            }
            else
            {
                if(SystemManager.UnlockKeyboard())
                {
                    lockMenuItem.Header = "Lock Keyboard";
                    keyboardLocked = false;
                }
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }
    }
}
