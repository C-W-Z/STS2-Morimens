using Morimens.Characters.Shared.Definition;
using STS2RitsuLib.Content;

namespace Morimens.Characters.Shared;

public static class SharedRegistry
{
    public static void Register()
    {
        ModContentRegistry.For(Entry.ModId)
            .RegisterCardLibraryCompendiumSharedPoolFilter<SharedCardPool>(
                "morimens_shared_card_pool", // ID
                $"{Entry.ImagePath}/Shared/ui/icon_morimens.png", // 图标位置
                null // 放置顺序（可选）
            );
    }
}
