using MegaCrit.Sts2.Core.Models;
using Morimens.Characters.Doll.Definition;
using Morimens.Core.Character;
using Morimens.Core.Menu.CharacterBg.Data;
using STS2RitsuLib;

namespace Morimens.Core.Menu.CharacterBg;

public static class CharacterBgManager
{
    // 在這裡為「每個角色」配置各自專屬的背景路徑清單
    private static readonly Dictionary<string, List<string>> CharacterBackgroundMaps = new()
    {
        {
            ModelDb.Get<DollAwaker>().Id.Entry, // MORIMENS_CHARACTER_DOLL_AWAKER
            new()
            {
                $"{Entry.ScenePath}/characters/Doll_character_select_bg.tscn",
                $"{Entry.ScenePath}/characters/Doll_character_select_bg_2.tscn"
            }
        }
    };

    /// <summary>
    /// 輪流切換指定角色的下一個背景，並儲存至 ModDataStore
    /// </summary>
    public static void CycleToNext(CharacterModel character)
    {
        if (character is null || !CharacterBackgroundMaps.TryGetValue(character.Id.Entry, out var bgList) || bgList.Count == 0)
            return;

        var store = RitsuLibFramework.GetDataStore(Entry.ModId);

        // 使用 RitsuLib 的 Modify 閉包安全更新資料
        store.Modify<CharacterBgData>(CharacterBgData.DataStoreKey, data =>
        {
            data.BgId ??= [];

            // 取得目前的索引（若無則默認 0）
            int currentIndex = data.BgId.TryGetValue(character.Id.Entry, out var idx) ? idx : 0;

            // 計算下一個索引
            data.BgId[character.Id.Entry] = (currentIndex + 1) % bgList.Count;
        });

        // ⚠️ 必須顯式呼叫 Save 才會安全寫入物理磁碟
        store.Save(CharacterBgData.DataStoreKey);
    }

    /// <summary>
    /// 讀取硬碟存檔，獲取該角色上次選擇的背景路徑
    /// </summary>
    public static string? GetCurrentBackgroundPath(CharacterModel character)
    {
        if (character is null || !CharacterBackgroundMaps.TryGetValue(character.Id.Entry, out var bgList) || bgList.Count == 0)
            return null;

        var store = RitsuLibFramework.GetDataStore(Entry.ModId);
        var data = store.Get<CharacterBgData>(CharacterBgData.DataStoreKey);

        int targetIndex = 0;
        if (data?.BgId is not null && data.BgId.TryGetValue(character.Id.Entry, out var savedIndex))
        {
            // 使用 % bgList.Count 防止未來你縮減背景陣列時導致存檔索引越界
            targetIndex = savedIndex % bgList.Count;
        }

        return bgList[targetIndex];
    }

    /// <summary>
    /// 判斷角色是否支援背景切換
    /// </summary>
    public static bool IsSupportedCharacter(CharacterModel? character)
    {
        // 同時滿足是你的 Mod 角色 (IAwaker) 且我們有幫它配置背景清單
        return character is IAwaker && CharacterBackgroundMaps.ContainsKey(character.Id.Entry);
    }
}
