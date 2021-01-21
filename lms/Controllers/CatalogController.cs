using System;
using System.Linq;
using DataAccessLayer;
using lms.Models.Catalog;
using lms.Models.Checkout;
using Microsoft.AspNetCore.Mvc;
using static lms.Models.Catalog.AssetDetailModel;

namespace lms.Controllers
{
    public class CatalogController : Controller
    {
        private ICheckoutService _checkouts;
        private ILibraryAssetService _assets;
        public CatalogController(ILibraryAssetService assets, ICheckoutService checkouts)
        {
            _checkouts = checkouts;
            _assets = assets;
        }

        public IActionResult Index()
        {
            var assetModels = _assets.GetAll();
            var listingModels = assetModels.Select(result => new AssetIndexListingModel
            {
                Id = result.Id,
                ImageUrl = result.ImageUrl,
                AuthorOrDirector = _assets.GetAuthorOrDirector(result.Id),
                DeweyIndex = _assets.GetDeweyIndex(result.Id),
                Title = result.Title,
                Type = _assets.GetType(result.Id)
            });

            var model = new AssetIndexModel
            {
                Assets = listingModels
            };

            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var asset = _assets.Get(id);
            var currentHolds = _checkouts.GetCurrentHolds(id).Select(a => new AssetHoldModel
            {
                HoldPlaced = _checkouts.GetCurrentHoldPlaced(id),
                StudentName = _checkouts.GetCurrentHoldStudent(a.Id)
            });
            var model = new AssetDetailModel
            {
                AssetId = id,
                Title = asset.Title,
                Year = asset.Year,
                Cost = asset.Cost,
                Status = asset.Status.Name,
                ImageUrl = asset.ImageUrl,
                AuthorOrDirector = _assets.GetAuthorOrDirector(id),
                CurrentLocation = _assets.GetCurrentLocation(id).Name,
                DeweyIndex = _assets.GetDeweyIndex(id),
                ISBN = _assets.GetIsbn(id),
                StudentName = _checkouts.GetCurrentStudent(id),
                CurrentHolds = currentHolds,
                CheckoutHistory = _checkouts.GetCheckoutHistory(id)
            };

            return View(model);

        }

        public IActionResult Checkout(int id)
        {
            var asset = _assets.Get(id);

            var model = new CheckoutModels
            {
                AssetId = id,
                ImageUrl = asset.ImageUrl,
                Title = asset.Title,
                LibraryCardId = "",
                IsCheckedOut = _checkouts.IsCheckedOut(id)
            };

            return View(model);
        }

        public IActionResult CheckIn(int id)
        {
            _checkouts.CheckInItem(id);
            return RedirectToAction("Detail", new { id = id });
        }

        public IActionResult Hold(int id)
        {
            var asset = _assets.Get(id);

            var model = new CheckoutModels
            {
                AssetId = id,
                ImageUrl = asset.ImageUrl,
                Title = asset.Title,
                LibraryCardId = "",
                IsCheckedOut = _checkouts.IsCheckedOut(id),
                HoldCount = _checkouts.GetCurrentHolds(id).Count()
            };

            return View(model);
        }

        public IActionResult MarkLost(int assetId)
        {
            _checkouts.MarkLost(assetId);
            return RedirectToAction("Detail", new { id = assetId });
        }
        public IActionResult MarkFound(int assetId)
        {
            _checkouts.MarkFound(assetId);
            return RedirectToAction("Detail", new { id = assetId });
        }

        [HttpPost]
        public IActionResult PlaceCheckout(int assetId, int libraryCardId)
        {
            _checkouts.CheckoutItem(assetId, libraryCardId);
            return RedirectToAction("Detail", new { id = assetId });
        }

        [HttpPost]
        public IActionResult PlaceHold(int assetId, int libraryCardId)
        {
            _checkouts.PlaceHold(assetId, libraryCardId);
            return RedirectToAction("Detail", new { id = assetId });
        }
    }
}
