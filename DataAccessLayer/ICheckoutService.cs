using System;
using System.Collections.Generic;
using DataAccessLayer.Models;

namespace DataAccessLayer
{
    public interface ICheckoutService
    {
        IEnumerable<Checkout> GetAll();
        Checkout Get(int id);
        void Add(Checkout newCheckout);
        IEnumerable<CheckoutHistory> GetCheckoutHistory(int id);
        void PlaceHold(int assetId, int libraryCardId);
        void CheckoutItem(int id, int libraryCardId);
        void CheckInItem(int id);
        Checkout GetLatestCheckout(int id);
        int GetNumberOfCopies(int id);
        bool IsCheckedOut(int id);

        string GetCurrentHoldStudent(int id);
        string GetCurrentHoldPlaced(int id);
        string GetCurrentStudent(int id);
        IEnumerable<Hold> GetCurrentHolds(int id);

        void MarkLost(int id);
        void MarkFound(int id);
    }
}