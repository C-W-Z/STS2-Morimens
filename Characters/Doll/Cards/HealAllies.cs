using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Morimens.Characters.Doll.Cards.Abstracts;
using Morimens.Characters.Doll.Definition;
using Morimens.Core.CardTags;
using Morimens.Core.Targeting;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Morimens.Characters.Doll.Cards;

// TODO: 測試聯機時是否作用在別人和別人的人偶身上
[RegisterCard(typeof(DollCardPool))]
public sealed class HealAllies() : AbstractDollCard(1, CardType.Skill, CardRarity.Common, GenericTargetType.AllAllies)
{
    protected override HashSet<CardTag> CanonicalTags => [MorimensCardTag.Heal];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(5m)];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(CombatState);
        await CreatureCmd.TriggerAnim(Owner.Creature, DollSpine.State.Skill2, DollSpine.Skill2AnimDelay);
        foreach (var ally in CombatState.Allies)
            await CreatureCmd.Heal(ally, DynamicVars.Heal.BaseValue, true);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Heal.UpgradeValueBy(2m);
    }
}
