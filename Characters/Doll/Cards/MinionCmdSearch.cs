using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Morimens.Characters.Doll.CardTags;
using Morimens.Characters.Doll.Definition;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Morimens.Characters.Doll.Cards;

[RegisterCard(typeof(DollCardPool))]
public sealed class MinionCmdSearch() : AbstractDollCard(0, CardType.Skill, CardRarity.Rare, TargetType.None)
{
    protected override HashSet<CardTag> CanonicalTags => [DollCardTag.MinionCmd];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, DollSpine.State.Cast, DollSpine.CastAnimDelay);

        IEnumerable<CardPile> targetPiles = [PileType.Draw.GetPile(Owner), PileType.Discard.GetPile(Owner)];
        List<CardModel> combinedCards = [.. targetPiles.SelectMany(p => p.Cards).Where(c => c.Tags.Contains(DollCardTag.MinionCmd))];
        if (combinedCards.Count == 0)
            return;

        await CardPileCmd.Add(
            await CardSelectCmd.FromSimpleGrid(choiceContext, combinedCards, Owner,
                new CardSelectorPrefs(SelectionScreenPrompt, DynamicVars.Cards.IntValue)),
            PileType.Hand);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}
