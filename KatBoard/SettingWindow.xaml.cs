// Copyright (C) 2020 Joe Wang
//
// This file is part of KatBoard.
//
// KatBoard is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// KatBoard is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with KatBoard.  If not, see <http://www.gnu.org/licenses/>.

using System.Diagnostics;
using System.Windows;
using Microsoft.Win32;

namespace KatBoard
{
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
