using MegaCrit.Sts2.Core.Entities.Players;

namespace Morimens.Characters;

public interface IAwaker
{
    int BaseAliemus { get; }
    Task Exalt(Player player);
    Task SuperExalt(Player player);
}
