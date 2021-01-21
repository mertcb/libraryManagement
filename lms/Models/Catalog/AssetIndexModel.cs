using System;
using System.Collections.Generic;

namespace lms.Models.Catalog
{
    public class AssetIndexModel
    {
        public IEnumerable<AssetIndexListingModel> Assets { get; set; }

        public AssetIndexModel()
        {
        }
    }
}
