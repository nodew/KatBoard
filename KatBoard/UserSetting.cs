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
