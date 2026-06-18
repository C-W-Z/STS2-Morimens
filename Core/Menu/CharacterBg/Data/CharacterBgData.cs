namespace Morimens.Core.Menu.CharacterBg.Data;

// 定义我们要保存的数据结构
public sealed class CharacterBgData
{
    public const string DataStoreKey = "character_bg";

    // Key: 角色 ID (CharacterModel.Id), Value: 當前選中的背景索引
    public Dictionary<string, int> BgId { get; set; } = [];
}
