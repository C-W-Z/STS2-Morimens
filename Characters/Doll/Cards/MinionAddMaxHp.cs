using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Morimens.Characters.Doll.Cards.Abstracts;
using Morimens.Characters.Doll.Definition;
using Morimens.Characters.Doll.Minions;
using Morimens.Characters.Doll.Targeting;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Morimens.Characters.Doll.Cards;

[RegisterCard(typeof(DollCardPool))]
public sealed class MinionAddMaxHp() : AbstractMinionCard(1, CardType.Skill, CardRarity.Common, DollTargetType.AllDollMinions)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new MaxHpVar(5m)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        List<DollMinion> minions = await CheckMinionExistAndSummon(choiceContext);

        foreach (var minion in minions)
        {
            await CreatureCmd.TriggerAnim(minion.Creature, DollSpine.State.Skill2, DollSpine.Skill2AnimDelay);
            await CreatureCmd.GainMaxHp(minion.Creature, DynamicVars.MaxHp.BaseValue);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.MaxHp.UpgradeValueBy(3m);
    }
}
