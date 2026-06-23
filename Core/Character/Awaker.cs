using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.ValueProps;
using MinionLib.Powers.Patches;
using Morimens.Core.ExEnergy;
using STS2RitsuLib.Combat.SecondaryResources;
using STS2RitsuLib.Scaffolding.Characters;
using STS2RitsuLib.Scaffolding.Godot;

namespace Morimens.Core.Character;

public abstract class Awaker<TCardPool, TRelicPool, TPotionPool> : ModCharacterTemplate<TCardPool, TRelicPool, TPotionPool>, IAwaker
    where TCardPool : CardPoolModel
    where TRelicPool : RelicPoolModel
    where TPotionPool : PotionPoolModel
{
    // ─── 自動化路徑約定 ───
    // 預設會拿類別名字去掉 "Awaker"，例如 DollAwaker 就會自動回傳 "Doll"
    public virtual string CharacterFolderName => GetType().Name.Replace("Awaker", "");

    protected string SceneRoot => $"{Entry.ScenePath}/{CharacterFolderName}";
    protected string ImageRoot => $"{Entry.ImagePath}/{CharacterFolderName}/ui";

    protected virtual string CharacterScenePath => $"{SceneRoot}/character.tscn";
    protected virtual string MerchantScenePath => $"{SceneRoot}/merchant.tscn";
    protected virtual string RestSiteScenePath => $"{SceneRoot}/rest_site.tscn";
    protected virtual string CharacterSelectBgScenePath => $"{SceneRoot}/character_select_bg.tscn";

    protected virtual string SharedEnergyCounterScenePath => $"{Entry.ScenePath}/Shared/energy_counter.tscn";

    // ─── 統一管理的資產配置 ───
    public override CharacterAssetProfile AssetProfile => new(
        Scenes: new CharacterSceneAssetSet(
            VisualsPath: CharacterScenePath,
            EnergyCounterPath: SharedEnergyCounterScenePath, // 🎯 帶入共用能量
            MerchantAnimPath: MerchantScenePath,
            RestSiteAnimPath: RestSiteScenePath),
        Ui: new CharacterUiAssetSet(
            IconTexturePath: $"{ImageRoot}/character_icon.png",
            IconOutlineTexturePath: $"{ImageRoot}/character_icon_outline.png",
            IconPath: $"{SceneRoot}/character_icon.tscn",
            CharacterSelectBgPath: CharacterSelectBgScenePath,
            CharacterSelectIconPath: $"{ImageRoot}/character_select.png",
            CharacterSelectLockedIconPath: $"{ImageRoot}/character_select_locked.png",
            MapMarkerPath: $"{ImageRoot}/map_marker.png"),
        Audio: new CharacterAudioAssetSet(
            AttackSfx: $"event:/Morimens/sfx/{CharacterFolderName}/Attack",
            CastSfx: $"event:/Morimens/sfx/{CharacterFolderName}/Cast",
            DeathSfx: $"event:/Morimens/sfx/{CharacterFolderName}/Death"
        ));

    // 让 RitsuLib 把普通 Godot 场景转换成游戏需要的 NCreatureVisuals。
    // 自动转换人物场景，让你不需要手动挂脚本。复制即可。
    protected override NCreatureVisuals? TryCreateCreatureVisuals()
    {
        return RitsuGodotNodeFactories.CreateFromScenePath<NCreatureVisuals>(CharacterScenePath);
    }

    public virtual int BaseAliemus => 100;
    public virtual int BaseKeyflare => 1000;
    public virtual int KeyflareGain => 25;

    // ─── 快取欄位 ───
    private CombatState? _cachedCombatState;
    private CardModel? _cachedExaltCard;
    private CardModel? _cachedOverExaltCard;

    // ─── 工廠方法 (Factory Methods) ───
    // 子類別只需要實作這兩個方法，回傳對應的卡牌範本即可
    protected abstract CardModel CreateExaltCardInstance();
    protected abstract CardModel CreateOverExaltCardInstance();

    // ─── 核心輔助方法：獲取並動態更新快取的卡牌 ───
    protected CardModel GetExaltCard()
    {
        // 只有第一次會執行 ToMutable() 分配記憶體，後續皆重複使用
        var combatState = CombatManager.Instance._state;

        // 當戰鬥對局改變（例如重開局），或是快取尚未建立時，重新調用工廠獲取綁定新環境的卡牌
        if (_cachedExaltCard is null || _cachedCombatState != combatState)
        {
            _cachedCombatState = combatState;
            _cachedExaltCard = CreateExaltCardInstance(); // 重新 ToMutable() 完美向新對局注入 CombatState
        }

        _cachedExaltCard.UpgradePreviewType = CardUpgradePreviewType.Combat;

        if (combatState is not null)
        {
            _cachedExaltCard.Owner ??= LocalContext.GetMe(combatState)!;
            _cachedExaltCard.DynamicVars.ClearPreview();
            _cachedExaltCard.UpdateDynamicVarPreview(CardPreviewMode.Normal, null, _cachedExaltCard.DynamicVars);
        }

        return _cachedExaltCard;
    }

    protected CardModel GetOverExaltCard()
    {
        var combatState = CombatManager.Instance._state;

        if (_cachedOverExaltCard is null || _cachedCombatState != combatState)
        {
            _cachedCombatState = combatState;
            _cachedOverExaltCard = CreateOverExaltCardInstance();
        }

        _cachedOverExaltCard.UpgradePreviewType = CardUpgradePreviewType.Combat;

        if (combatState is not null)
        {
            _cachedOverExaltCard.Owner ??= LocalContext.GetMe(combatState)!;
            _cachedOverExaltCard.DynamicVars.ClearPreview();
            _cachedOverExaltCard.UpdateDynamicVarPreview(CardPreviewMode.Normal, null, _cachedOverExaltCard.DynamicVars);
        }

        return _cachedOverExaltCard;
    }

    public virtual string ExaltTitle => GetExaltCard().Title;
    public virtual string ExaltDescription => GetExaltCard().GetDescriptionForPile(PileType.Hand);
    public virtual async Task Exalt() => await ((IExaltCard)GetExaltCard()).Execute();
    public virtual string OverExaltTitle => GetOverExaltCard().Title;
    public virtual string OverExaltDescription => GetOverExaltCard().GetDescriptionForPile(PileType.Hand);
    public virtual async Task OverExalt() => await ((IExaltCard)GetExaltCard()).Execute();
}
