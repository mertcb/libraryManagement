using System.Collections.Generic;
using DataAccessLayer.Models;

namespace lms.DataAccessLayer
{
    public interface IStudentService
    {
        IEnumerable<Student> GetAll();
        Student Get(int id);
        void Add(Student newBook);
        IEnumerable<CheckoutHistory> GetCheckoutHistory(int patronId);
        IEnumerable<Hold> GetHolds(int patronId);
        IEnumerable<Checkout> GetCheckouts(int id);
    }
}