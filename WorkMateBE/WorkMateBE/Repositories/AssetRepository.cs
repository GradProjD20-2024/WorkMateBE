using WorkMateBE.Data;
using WorkMateBE.Interfaces;
using WorkMateBE.Models;
using Microsoft.EntityFrameworkCore;

namespace WorkMateBE.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        private readonly DataContext _context;

        public AssetRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateAsset(Asset asset)
        {
            _context.Assets.Add(asset);
            return Save();
        }

        public bool DeleteAsset(int assetId)
        {
            var asset = _context.Assets.Find(assetId);
            if (asset != null)
            {
                _context.Assets.Remove(asset);
                return Save();
            }
            return false;
        }

        public ICollection<Asset> GetAll()
        {
            return _context.Assets.ToList();
        }

        public Asset GetAssetById(int assetId)
        {
            return _context.Assets.Find(assetId);
        }

        public bool UpdateAsset(int assetId, Asset updatedAsset)
        {
            var existingAsset = _context.Assets.Find(assetId);
            if (existingAsset != null)
            {
                existingAsset.Name = updatedAsset.Name;
                existingAsset.Description = updatedAsset.Description;
                existingAsset.Location = updatedAsset.Location;
                existingAsset.Status = updatedAsset.Status;
                existingAsset.EmployeeId = updatedAsset.EmployeeId;

                _context.Assets.Update(existingAsset);
                return Save();
            }
            return false;
        }
        
        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
