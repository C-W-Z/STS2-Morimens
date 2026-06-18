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
public sealed class MinionCopy() : AbstractDollMinionCard(2, CardType.Skill, CardRarity.Rare, DollTargetType.AnyDollMinionOrNone)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        List<DollMinion> minions = await CheckMinionExistAndSummon(choiceContext);
        if (minions.Count == 0)
            return;

        ArgumentNullException.ThrowIfNull(cardPlay.Target?.Monster);
        await CreatureCmd.TriggerAnim(Owner.Creature, DollSpine.State.Skill2, DollSpine.Skill2AnimDelay);
        await DollMinionCmd.SummonCopy(choiceContext, Owner, (DollMinion)cardPlay.Target.Monster, this);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
}
