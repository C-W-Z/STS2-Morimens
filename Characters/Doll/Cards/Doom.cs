using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Morimens.Characters.Doll.Definition;
using Morimens.Core.Card;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Morimens.Characters.Doll.Cards;

[RegisterCard(typeof(DollCardPool))]
public sealed class Doom() : AbstractMorimensCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override HashSet<CardTag> CanonicalTags => [];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DoomPower>(15m)];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromPower<DoomPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await CreatureCmd.TriggerAnim(Owner.Creature, DollSpine.State.Cast, DollSpine.CastAnimDelay);
        await PowerCmd.Apply<DoomPower>(choiceContext, cardPlay.Target, DynamicVars.Doom.BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Doom.UpgradeValueBy(5m);
    }
}
