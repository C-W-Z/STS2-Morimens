using MegaCrit.Sts2.Core.Entities.Relics;
using Morimens.Characters.Doll.Definition;
using Morimens.Core.Relic;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Morimens.Characters.Doll.Relics;

// RegisterRelic 会把遗物注册进指定遗物池。
// RegisterCharacterStarterRelic 会把它作为 Doll 的初始遗物。
[RegisterRelic(typeof(DollRelicPool))]
[RegisterCharacterStarterRelic(typeof(DollAwaker))]
public sealed class SilverTech : AbstractMorimensRelic
{
    // 稀有度。
    public override RelicRarity Rarity => RelicRarity.Starter;

    // 遗物的数值。这里会替换本地化中的 {Cards}。
    // protected override IEnumerable<DynamicVar> CanonicalVars =>
    // [
    //     new CardsVar(1)
    // ];

    // 每回合开始时，抽一张牌。
    // 这里使用 DynamicVars.Cards.IntValue，保证效果和本地化显示保持一致。
    // public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    // {
    //     await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, player);
    // }
}
