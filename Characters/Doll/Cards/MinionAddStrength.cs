using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Morimens.Characters.Doll.Cards.Abstracts;
using Morimens.Characters.Doll.CardTags;
using Morimens.Characters.Doll.Definition;
using Morimens.Characters.Doll.Minions;
using Morimens.Characters.Doll.Targeting;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Morimens.Characters.Doll.Cards;

[RegisterCard(typeof(DollCardPool))]
public sealed class MinionAddStrength() : AbstractDollCard(1, CardType.Skill, CardRarity.Common, DollTargetType.AllDollMinions)
{
    protected override HashSet<CardTag> CanonicalTags => [DollCardTag.MinionCmd];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StrengthPower>(1m)];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        List<DollMinion> minions = await DollMinionCmd.CheckMinionExistAndSummon(choiceContext, Owner, this);

        foreach (var minion in minions)
        {
            await CreatureCmd.TriggerAnim(minion.Creature, DollSpine.State.Skill2, DollSpine.Skill2AnimDelay);
            await PowerCmd.Apply<StrengthPower>(choiceContext, minion.Creature, DynamicVars.Strength.BaseValue, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Strength.UpgradeValueBy(1m);
    }
}
