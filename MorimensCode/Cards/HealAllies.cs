using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Morimens.Anims;
using Morimens.CardTags;
using Morimens.Characters;
using Morimens.Targeting;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Morimens.Cards;

// TODO: 測試聯機時是否作用在別人和別人的人偶身上
[RegisterCard(typeof(DollCardPool))]
[RegisterCharacterStarterCard(typeof(Doll), 1)]
public sealed class HealAllies() : AbstractDollCard(1, CardType.Skill, CardRarity.Common, GenericTargetType.AllAllies)
{
    protected override HashSet<CardTag> CanonicalTags => [DollCardTag.Heal];

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
