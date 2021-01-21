using System;
namespace lms.Models.Catalog
{
    public class AssetIndexListingModel
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string AuthorOrDirector { get; set; }
        public string Type { get; set; }
        public string DeweyIndex { get; set; }
        public string NumberOfCopies { get; set; }

        public AssetIndexListingModel()
        {
        }
    }
}
