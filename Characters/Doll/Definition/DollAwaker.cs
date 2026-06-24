using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Runs;
using Morimens.Core.Character;
using Morimens.Core.Character.Data;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Godot;

namespace Morimens.Characters.Doll.Definition;

[RegisterCharacter]
public sealed class DollAwaker : Awaker<DollCardPool, DollRelicPool, DollPotionPool>
{
    public static readonly Color ThemeColor = new("ECD3D1FF");

    // 角色名称颜色。
    public override Color NameColor => ThemeColor;
    // 能量图标轮廓颜色。
    public override Color EnergyLabelOutlineColor => new("917070FF");
    // 地图绘制颜色。
    public override Color MapDrawingColor => ThemeColor;

    // 人物性别（男女中立）。
    public override CharacterGender Gender => CharacterGender.Neutral;

    // 初始血量和金币。
    public override int StartingHp => 68;
    public override int StartingGold => 99;

    // 某个字段没写时，RitsuLib 会从占位角色配置里补齐。
    public override string? PlaceholderCharacterId => "necrobinder";
    // 如果你的人物不需要时间线小故事，加上这句。
    public override bool RequiresEpochAndTimeline => false;
    // 攻击和施法动画延迟，以对齐动画。静态占位资源不需要延迟。
    public override float AttackAnimDelay => DollSpine.AttackAnimDelay;
    public override float CastAnimDelay => DollSpine.CastAnimDelay;

    // 攻击建筑师的攻击特效列表。
    public override List<string> GetArchitectAttackVfx()
    {
        return
        [
            "vfx/vfx_attack_blunt",
            "vfx/vfx_heavy_blunt",
            "vfx/vfx_attack_slash",
            "vfx/vfx_bloody_impact",
            "vfx/vfx_rock_shatter"
        ];
    }

    // 用 AwakerRunStateRegistry.Data.Modify(player, data => data.AwakerVisualState = 1); 來達成切換spine（下一場戰鬥生效）
    protected override NCreatureVisuals? TryCreateCreatureVisuals()
    {
        Entry.Logger.Debug($"[TryCreateCreatureVisuals] RunManager.Instance.State is not null = {RunManager.Instance.State is not null}");
        Entry.Logger.Debug($"[TryCreateCreatureVisuals] LocalContext.NetId is not null = {LocalContext.NetId is not null}");
        Entry.Logger.Debug($"[TryCreateCreatureVisuals] AwakerVisualState = {AwakerRunStateRegistry.Data.Get(RunManager.Instance.State!, LocalContext.NetId!.Value).AwakerVisualState}");

        string path = AwakerRunStateRegistry.Data.Get(RunManager.Instance.State!, LocalContext.NetId!.Value).AwakerVisualState != 0
                ? $"{SceneRoot}/character2.tscn"
                : CharacterScenePath;

        Entry.Logger.Debug($"[TryCreateCreatureVisuals] CharacterScenePath = {path}");
        return RitsuGodotNodeFactories.CreateFromScenePath<NCreatureVisuals>(path);
    }

    protected override CreatureAnimator? SetupCustomCreatureAnimator(MegaSprite controller)
    {
        return DollSpine.GetCreatureAnimator(controller);
    }

    protected override AbstractExaltCard ExaltCard => ModelDb.Get<MorimensExaltDoll>();
    protected override AbstractExaltCard OverExaltCard => ModelDb.Get<MorimensOverExaltDoll>();
}
