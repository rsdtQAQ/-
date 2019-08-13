using Smod2;
using Smod2.Attributes;
using IEventHandler;

namespace scp035
{
    [PluginDetails(
    author = "cushaw",
    name = "scp035",
    description = "scp035",
    id = "rsdt.scp035",
    version = "1.3",
    SmodMajor = 3,
    SmodMinor = 2,
    SmodRevision = 22
    )]
    class PlayerListTitle : Plugin
    {
        public override void OnDisable()
        {
            this.Info("scp035 读取失败");
        }

        public override void OnEnable()
        {
            this.Info("scp035 读取成功");
        }

        public override void Register()
        {
            this.AddEventHandlers(new EventHandler(this));
            this.AddConfig(new Smod2.Config.ConfigSetting("scp035_enable", true, Smod2.Config.SettingType.BOOL, true, "035 plugin enable/disable"));
            this.AddConfig(new Smod2.Config.ConfigSetting("scp035_spawn", 30, Smod2.Config.SettingType.NUMERIC, true, "	035 spawn probability(%)"));
            this.AddConfig(new Smod2.Config.ConfigSetting("scp035_hp", 500, Smod2.Config.SettingType.NUMERIC, true, "035 hp"));
            this.AddConfig(new Smod2.Config.ConfigSetting("scp035_number", 2, Smod2.Config.SettingType.NUMERIC, true, "035 max number"));
            this.AddConfig(new Smod2.Config.ConfigSetting("scp035_item", new int[] { -1 }, Smod2.Config.SettingType.NUMERIC_LIST, true, ""));
            this.AddConfig(new Smod2.Config.ConfigSetting("scp035_door", new string[] { "914" }, Smod2.Config.SettingType.LIST, true, ""));
        }
    }
}
