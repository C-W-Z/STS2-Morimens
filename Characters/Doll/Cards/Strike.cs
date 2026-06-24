using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Morimens.Characters.Doll.Definition;
using Morimens.Core.ExEnergy;
using STS2RitsuLib.Combat.SecondaryResources;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Morimens.Characters.Doll.Cards;

// RegisterCard 会把这张牌交给 RitsuLib 自动注册。
// RegisterCharacterStarterCard 会把它追加进 Doll 的初始卡组。
[RegisterCard(typeof(DollCardPool))]
[RegisterCharacterStarterCard(typeof(DollAwaker), 4)]
public sealed class Strike() : AbstractDollCard(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
{
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];

    // CanonicalVars 翻译是“规范值”，指卡牌的基础数值。
    // 添加一个 DamageVar 意为指定卡牌的基础伤害是多少；它会自动绑定到本地化里的 {Damage:diff()} 占位符。
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(6, ValueProp.Move),
        SecondaryResourceVars.For("Aliemus", ExEnergyManager.AliemusId, 5)
    ];

    // 打出时的效果逻辑。
    // 尖塔2使用了 async 和 await 来控制效果逻辑顺序执行，和尖塔1的 action 类似。
    // DamageCmd.Attack 会按当前 DynamicVars.Damage 的值造成攻击伤害。
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
        await SecondaryResourceCmd.Gain(Owner, ExEnergyManager.AliemusId, DynamicVars["Aliemus"].IntValue);
    }

    // 升级后的效果逻辑。
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
        DynamicVars["Aliemus"].UpgradeValueBy(5);
    }
}
