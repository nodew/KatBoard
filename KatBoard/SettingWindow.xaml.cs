using System.Diagnostics;
using System.Windows;
using Microsoft.Win32;

namespace KatBoard
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingWindow : Window
    {
        private UserSetting userSetting;
        private const string AppName = "KatBoard";

        public SettingWindow()
        {
            InitializeComponent();
            userSetting = new UserSetting();
            StartupCheckBox.IsChecked = userSetting.AutoStartup;
            ShortcutKeyBinding.Text = userSetting.Shortcut;
        }

        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            bool enabled = StartupCheckBox.IsChecked.HasValue && StartupCheckBox.IsChecked.Value;
            if (userSetting.AutoStartup != enabled) {
                ConfigAutoStartup(enabled);
                userSetting.AutoStartup = enabled;
            }

            userSetting.Shortcut = ShortcutKeyBinding.Text.Trim();
            userSetting.Save();
            Close();
        }

        private void ConfigAutoStartup(bool enabled)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            string filename = Process.GetCurrentProcess().MainModule.FileName;
            if (enabled)
            {
                key.SetValue(AppName, filename);
            }
            else
            {
                key.DeleteValue(AppName);
            }
        }
    }
}
