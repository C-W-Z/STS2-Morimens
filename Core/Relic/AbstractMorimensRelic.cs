using Morimens.Core.Utils;
using STS2RitsuLib.Scaffolding.Content;

namespace Morimens.Core.Relic;

public abstract class AbstractMorimensRelic : ModRelicTemplate
{
    private RelicAssetProfile? _cachedAssetProfile;

    // 图片资源统一放在 AssetProfile 里配置。
    // 三个路径可以先指向同一张图。后续有高清图或轮廓图时再拆开。
    public override RelicAssetProfile AssetProfile
    {
        get
        {
            if (_cachedAssetProfile != null)
                return _cachedAssetProfile;

            string folder = GetType().GetCharacterFolderName();

            // 例如："Morimens/images/Doll/relics/RelicA.png"
            string targetPath = $"{Entry.ImagePath}/{folder}/relics/{GetType().Name}.png";

            if (!Godot.ResourceLoader.Exists(targetPath))
            {
                Entry.Logger.Debug($"Missing relic art for '{GetType().Name}'. Falling back to placeholder. (Expected path: {targetPath})");
                targetPath = $"{Entry.ImagePath}/shared/relics/missing.png";
            }

            _cachedAssetProfile = new RelicAssetProfile(IconPath: targetPath, IconOutlinePath: targetPath, BigIconPath: targetPath);
            return _cachedAssetProfile;
        }
    }
}
