namespace Morimens.Core.Character;

public interface IAwaker
{
    int BaseAliemus { get; }
    int BaseKeyflare { get; }
    string ExaltTitle { get; }
    string ExaltDescription { get; }
    Task Exalt();
    string OverExaltTitle { get; }
    string OverExaltDescription { get; }
    Task OverExalt();
}
