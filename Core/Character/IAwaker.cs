namespace Morimens.Core.Character;

public interface IAwaker
{
    int BaseAliemus { get; }
    int BaseKeyflare { get; }
    int KeyflareGain { get; }
    AbstractExaltCard Exalt { get; }
    AbstractExaltCard OverExalt { get; }
}
