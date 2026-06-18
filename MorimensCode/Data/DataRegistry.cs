using STS2RitsuLib;
using STS2RitsuLib.Utils.Persistence;

namespace Morimens.Data;

// NOTE: 檔案會存在 %APPDATA%\SlayTheSpire2\steam\{玩家SteamID}\mod_data\Morimens
public static class DataRegistry
{
    public static void Register()
    {
        using (RitsuLibFramework.BeginModDataRegistration(Entry.ModId))
        {
            var store = RitsuLibFramework.GetDataStore(Entry.ModId);

            // 向磁盘注册一项数据
            store.Register(
                key: CharacterBgData.DataStoreKey,               // ID键值，一旦确定不要轻易改动
                fileName: "morimens_character_bg.json", // 决定它在硬盘里的名字
                scope: SaveScope.Global,           // 这是一份通用的全局数据
                defaultFactory: () => new CharacterBgData(), // 首次创建的默认值
                autoCreateIfMissing: true          // 是否在玩家第一次挂载 Mod 后自动帮他在硬盘里生成文件
            );
        }
    }
}
