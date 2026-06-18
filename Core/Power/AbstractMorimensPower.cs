using Morimens.Core.Utils;
using STS2RitsuLib.Scaffolding.Content;

namespace Morimens.Core.Power;

public abstract class AbstractMorimensPower : ModPowerTemplate
{
    private PowerAssetProfile? _cachedAssetProfile;

    // 原版大圖通常 256x256，小圖通常 64x64。
    public override PowerAssetProfile AssetProfile
    {
        get
        {
            if (_cachedAssetProfile != null)
                return _cachedAssetProfile;

            string folder = GetType().GetCharacterFolderName();

            // 例如："Morimens/images/Doll/powers/DollPowerA.png"
            string targetPath = $"{Entry.ImagePath}/{folder}/powers/{GetType().Name}.png";

            if (!Godot.ResourceLoader.Exists(targetPath))
            {
                Entry.Logger.Debug($"Missing power art for '{GetType().Name}'. Falling back to placeholder. (Expected path: {targetPath})");
                targetPath = $"{Entry.ImagePath}/shared/powers/missing.png";
            }

            _cachedAssetProfile = new PowerAssetProfile(IconPath: targetPath, BigIconPath: targetPath);
            return _cachedAssetProfile;
        }
    }
}
