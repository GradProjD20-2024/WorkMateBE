using WorkMateBE.Models;

namespace WorkMateBE.Interfaces
{
    public interface IAssetRepository
    {
        bool CreateAsset(Asset asset);
        bool UpdateAsset(int assetId, Asset asset);
        bool DeleteAsset(int assetId);
        ICollection<Asset> GetAll();
        Asset GetAssetById(int assetId);
    }
}
