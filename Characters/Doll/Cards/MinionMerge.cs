using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Morimens.Characters.Doll.Cards.Abstracts;
using Morimens.Characters.Doll.CardTags;
using Morimens.Characters.Doll.Definition;
using Morimens.Characters.Doll.Minions;
using Morimens.Characters.Doll.Targeting;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Morimens.Characters.Doll.Cards;

[RegisterCard(typeof(DollCardPool))]
public sealed class MinionMerge() : AbstractDollCard(1, CardType.Skill, CardRarity.Rare, DollTargetType.AllDollMinions)
{
    protected override HashSet<CardTag> CanonicalTags => [DollCardTag.MinionCmd];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        List<DollMinion> minions = await DollMinionCmd.CheckMinionExistAndSummon(choiceContext, Owner, this);
        if (minions.Count == 0)
            return;

        await CreatureCmd.TriggerAnim(Owner.Creature, DollSpine.State.Skill2, DollSpine.Skill2AnimDelay);
        await DollMinionCmd.MergeAllDollMinions(choiceContext, Owner, this);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
}
