using Morimens.Core.Menu.CharacterBg.Data;

namespace Morimens.Core;

// NOTE: 檔案會存在 %APPDATA%\SlayTheSpire2\steam\{玩家SteamID}\mod_data\Morimens
public static class DataRegistry
{
    public static void Register()
    {
        CharacterBgDataRegistry.Register();
    }
}
