using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MorimensDoll.Characters;
using MorimensDoll.Minion;
using MorimensDoll.Minions;
using MorimensDoll.Targeting;
using STS2RitsuLib.Interop.AutoRegistration;

namespace MorimensDoll.Cards;

[RegisterCard(typeof(DollCardPool))]
public sealed class MinionAttack() : AbstractMinionCard(1, CardType.Skill, CardRarity.Common, DollTargetTypes.AllDollMinions)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        List<DollMinion> pets = await CheckMinionExistAndSummon(choiceContext);

        foreach (var minion in pets)
            await DollMinionCmd.AttackRandomEnemy(choiceContext, minion, null);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
}
