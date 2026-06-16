using MegaCrit.Sts2.Core.Models;
using Morimens.Characters;

namespace Morimens.UI;

public static class CharacterBgManager
{
    // 🔍 在這裡定義你想輪流切換的背景 .tscn 路徑
    private static readonly List<string> BackgroundPaths =
    [
        "res://Morimens/scenes/characters/Doll_character_select_bg.tscn",
        "res://Morimens/scenes/characters/Doll_character_select_bg_2.tscn"
    ];

    private static int _currentIndex = 0;

    /// <summary>
    /// 輪流切換到下一個背景
    /// </summary>
    public static void CycleToNext()
    {
        if (BackgroundPaths.Count == 0) return;
        _currentIndex = (_currentIndex + 1) % BackgroundPaths.Count;
    }

    /// <summary>
    /// 獲取當前選中的背景路徑
    /// </summary>
    public static string GetCurrentBackgroundPath()
    {
        if (BackgroundPaths.Count == 0) return "";
        return BackgroundPaths[_currentIndex];
    }

    /// <summary>
    /// 判斷這個角色是不是你的 Mod 角色（只有你的角色才允許切換背景）
    /// </summary>
    public static bool IsSupportedCharacter(CharacterModel? character)
    {
        return character is IAwaker;
    }
}
