using Config;
using Constants.Paths;
using Data.Models.Entities;
using Data.Models.Player;
using Settings;
using Utils;

namespace Data.Database
{
    public static class Configs
    {
        public static GameConfig GameConfig { get; private set; }
        public static PlayerConfig PlayerConfig { get; private set; }
        public static ItemEntityConfig ItemEntityConfig { get; private set; }
        public static SettingsConfig SettingsConfig { get; private set; }
        public static TooltipConfig TooltipConfig { get; private set; }
        
        public static void LoadAll()
        {
            GameConfig = ResourcesHelper.LoadJson<GameConfig>("Data/game_config");
            PlayerConfig = ResourcesHelper.LoadJson<PlayerConfig>(PlayerPaths.PlayerConfigPath);
            ItemEntityConfig = ResourcesHelper.LoadJson<ItemEntityConfig>("Data/ItemEntity/config");
            SettingsConfig = ResourcesHelper.LoadJson<SettingsConfig>("Data/settings_config");
            TooltipConfig = ResourcesHelper.LoadJson<TooltipConfig>("Data/tooltip_config");
        }
    }
}