using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryServices
{
    public class CheckoutService : ICheckout
    {
        private LibraryContext _context;
        public CheckoutService(LibraryContext context)
        {
            _context = context;
        }
        public void Add(Checkout checkout)
        {
            _context.Add(checkout);
            _context.SaveChanges();
        }

        public IEnumerable<Checkout> GetAll()
        {
            return _context.Checkouts;
        }

        public Checkout GetById(int checkoutId)
        {
            return GetAll().FirstOrDefault(checkout => checkout.Id == checkoutId);
        }

        public Checkout GetLatestCheckout(int assetId)
        {
            return _context.Checkouts
                .Where(c => c.LibraryAsset.Id == assetId)
                .OrderByDescending(c => c.Since)
                .FirstOrDefault();
        }

        public IEnumerable<CheckoutHistory> GetCheckoutHistories(int id)
        {
            return _context.CheckoutHistories
                .Include(h => h.LibraryAsset)
                .Include(h => h.LibraryCard)
                .Where(h => h.LibraryAsset.Id == id);
        }

        public IEnumerable<Hold> GetCurrentHolds(int id)
        {
            return _context.Holds
                .Include(h => h.LibraryAsset)
                .Where(h => h.LibraryAsset.Id == id);
        }

        public string GetCurrentHoldStudentName(int holdId)
        {
            var hold = _context.Holds
                .Include(h => h.LibraryCard)
                .Include(h => h.LibraryAsset)
                .FirstOrDefault(h => h.Id == holdId);

            var cardId = hold?.LibraryCard.Id;

            var student = _context.Students.Include(s => s.LibraryCard).FirstOrDefault(s => s.LibraryCard.Id == cardId);

            return student?.FirstName + " " + student?.LastName;
        }

        public void MarkFound(int assetId)
        {
            UpdateAssetStatus(assetId, "Available");
            RemoveExistingCheckouts(assetId);
            CloseExistingCheckoutHistory(assetId);
            _context.SaveChanges();
        }

        private void UpdateAssetStatus(int assetId, string status)
        {
            var item = _context.LibraryAssets.FirstOrDefault(asset => asset.Id == assetId);

            _context.Update(item);

            item.Status = _context.Statuses.FirstOrDefault(status => status.Name == "Available");
        }

        private void CloseExistingCheckoutHistory(int assetId)
        {
            var now = DateTime.Now;
            var history = _context.CheckoutHistories.FirstOrDefault(ch => ch.LibraryAsset.Id == assetId && ch.CheckedIn == null);

            if (history != null)
            {
                _context.Update(history);
                history.CheckedIn = now;
            }
        }

        private void RemoveExistingCheckouts(int assetId)
        {
            var checkout = _context.Checkouts.FirstOrDefault(ch => ch.LibraryAsset.Id == assetId);

            if (checkout != null)
            {
                _context.Remove(checkout);
            }
        }

        public void MarkLost(int assetId)
        {
            UpdateAssetStatus(assetId, "Lost");
            _context.SaveChanges();
        }

        public void PlaceHold(int assetId, int libraryCardId)
        {
            var now = DateTime.Now;

            var item = _context.LibraryAssets.FirstOrDefault(ass => ass.Id == assetId);
            var libraryCard = _context.LibraryCards.FirstOrDefault(card => card.Id == libraryCardId);

            if (item.Status.Name == "Available")
            {
                UpdateAssetStatus(assetId, "On Hold");
            }

            var hold = new Hold
            {
                HoldPlaced = now,
                LibraryAsset = item,
                LibraryCard = libraryCard
            };

            _context.Add(hold);
            _context.SaveChanges();
        }

        public void CheckInItem(int assetId, int libraryCardId)
        {
            var now = DateTime.Now;

            var item = _context.LibraryAssets.FirstOrDefault(ass => ass.Id == assetId);

            RemoveExistingCheckouts(item.Id);
            CloseExistingCheckoutHistory(item.Id);

            var currentHolds = _context.Holds
                .Include(h => h.LibraryAsset)
                .Include(h => h.LibraryCard)
                .Where(h => h.LibraryAsset.Id == assetId);

            if (currentHolds.Any())
            {
                CheckoutToEarliestHold(assetId, currentHolds);
            }

            UpdateAssetStatus(assetId, "Available");
            _context.SaveChanges();
        }

        private void CheckoutToEarliestHold(int assetId, IQueryable<Hold> currentHolds)
        {
            var earliestHold = currentHolds.OrderBy(holds => holds.HoldPlaced).FirstOrDefault();

            var card = earliestHold.LibraryCard;

            _context.Remove(earliestHold);
            _context.SaveChanges();

            CheckOutItem(assetId, card.Id);
        }

        public void CheckOutItem(int assetId, int libraryCardId)
        {
            if (IsCheckedOut(assetId))
            {
                return;
            }

            var item = _context.LibraryAssets.FirstOrDefault(ass => ass.Id == assetId);

            UpdateAssetStatus(assetId, "Checked Out");
            var now = DateTime.Now;
            var libraryCard = _context.LibraryCards.Include(card => card.Checkouts).FirstOrDefault(card => card.Id == libraryCardId);

            var checkout = new Checkout
            {
                LibraryAsset = item,
                LibraryCard = libraryCard,
                Since = now,
                Until = GetDefaultCheckoutTime(now)
            };

            _context.Add(checkout);

            var checkoutHistory = new CheckoutHistory
            {
                LibraryAsset = item,
                LibraryCard = libraryCard,
                CheckedOut = now,
            };

            _context.Add(checkoutHistory);
            _context.SaveChanges();
        }

        private DateTime GetDefaultCheckoutTime(DateTime now)
        {
            return now.AddDays(21);
        }

        private bool IsCheckedOut(int assetId)
        {
            return _context.Checkouts.Where(co => co.LibraryAsset.Id == assetId).Any();
        }

        public DateTime GetCurrentHoldPlaced(int id)
        {
            return _context.Holds
                .Include(h => h.LibraryAsset)
                .Include(h => h.LibraryCard)
                .FirstOrDefault(h => h.LibraryAsset.Id == id).HoldPlaced;
        }

        public string GetCurrentCheckoutStudent(int assetId)
        {
            var checkout = GetCheckoutByAssetId(assetId);
            if(checkout == null)
            {
                return "";
            }

            var cardId = checkout?.LibraryCard.Id;

            var student = _context.Students.Include(s => s.LibraryCard).FirstOrDefault(s => s.LibraryCard.Id == cardId);

            return student?.FirstName + " " + student?.LastName;
        }

        private Checkout GetCheckoutByAssetId(int assetId)
        {
            return _context.Checkouts
                .Include(co => co.LibraryAsset)
                .Include(co => co.LibraryCard)
                .FirstOrDefault(co => co.LibraryAsset.Id == assetId);
        }
    }
}
