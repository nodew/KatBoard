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

using System.Configuration;

namespace KatBoard
{
    public class UserSetting: ApplicationSettingsBase
    {
        private const string AUTO_STARTUP_KEY = "AUTOSTARTUP";
        private const string SHORTCUT_KEY = "SHORTCUT";

        [UserScopedSetting()]
        [DefaultSettingValue("false")]
        public bool AutoStartup
        {
            get => (bool)this[AUTO_STARTUP_KEY];
            set => this[AUTO_STARTUP_KEY] = value;
        }

        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string Shortcut
        {
            get => (string)this[SHORTCUT_KEY];
            set => this[SHORTCUT_KEY] = value;
        }
    }
}
