using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Morimens.Characters.Doll.CardTags;
using Morimens.Characters.Doll.Definition;
using Morimens.Characters.Doll.Minions;
using Morimens.Characters.Doll.Powers.Tmp;
using Morimens.Characters.Doll.Targeting;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Morimens.Characters.Doll.Cards;

[RegisterCard(typeof(DollCardPool))]
public sealed class MinionAttack() : AbstractDollCard(1, CardType.Attack, CardRarity.Common, DollTargetType.AllDollMinions)
{
    protected override HashSet<CardTag> CanonicalTags => [DollCardTag.MinionCmd];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<MinionAttackPower>(1m)];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        List<DollMinion> minions = await DollMinionCmd.CheckMinionExistAndSummon(choiceContext, Owner, this);

        if (DynamicVars["MinionAttackPower"].BaseValue > 0)
        {
            await PowerCmd.Apply<MinionAttackPower>(choiceContext,
                minions.Select(m => m.Creature),
                DynamicVars["MinionAttackPower"].BaseValue,
                Owner.Creature, this);
        }

        foreach (var minion in minions)
            await DollMinionCmd.AttackRandomEnemy(choiceContext, minion, null);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["MinionAttackPower"].UpgradeValueBy(1m);
    }
}
