using System.Collections.Generic;
using DataAccessLayer.Models;

namespace DataAccessLayer
{
    public interface IHoldService
    {
        IEnumerable<AssetType> GetAll();
        AssetType Get(int id);
        void Add(AssetType newType);
    }
}