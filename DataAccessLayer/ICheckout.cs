using System;
using System.Collections.Generic;
using DataAccessLayer.Models;

namespace DataAccessLayer
{
    public interface ICheckout
    {
        IEnumerable<Checkout> GetAll();
        Checkout GetById(int checkoutId);
        void Add(Checkout checkout);
        void CheckOutItem(int assetId, int libraryCardId);
        void CheckInItem(int assetId);
        IEnumerable<CheckoutHistory> GetCheckoutHistories(int id);
        Checkout GetLatestCheckout(int assetId);
        void PlaceHold(int assetId, int libraryCardId);
        string GetCurrentHoldStudentName(int id);
        DateTime GetCurrentHoldPlaced(int id);
        IEnumerable<Hold> GetCurrentHolds(int id);
        string GetCurrentCheckoutStudent(int assetId);
        void MarkLost(int assetId);
        void MarkFound(int assetId);
        bool IsCheckedOut(int id);
    }
}
