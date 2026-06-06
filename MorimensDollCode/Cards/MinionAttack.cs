using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MorimensDoll.Characters;
using MorimensDoll.Minion;
using MorimensDoll.Minions;
using MorimensDoll.Powers.Tmp;
using MorimensDoll.Targeting;
using STS2RitsuLib.Interop.AutoRegistration;

namespace MorimensDoll.Cards;

[RegisterCard(typeof(DollCardPool))]
public sealed class MinionAttack() : AbstractMinionCard(1, CardType.Skill, CardRarity.Common, DollTargetTypes.AllDollMinions)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<TmpStrengthPower>(1m)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        List<DollMinion> minions = await CheckMinionExistAndSummon(choiceContext);

        if (DynamicVars["TmpStrengthPower"].BaseValue > 0)
        {
            await PowerCmd.Apply<TmpStrengthPower>(choiceContext,
                minions.Select(m => m.Creature),
                DynamicVars["TmpStrengthPower"].BaseValue,
                Owner.Creature, this);
        }

        foreach (var minion in minions)
            await DollMinionCmd.AttackRandomEnemy(choiceContext, minion, null);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["TmpStrengthPower"].UpgradeValueBy(1m);
    }
}
