using System.Collections.Generic;
using System.Linq;
using DataAccessLayer;
using DataAccessLayer.Models;
using lms.DataAccessLayer;
using Microsoft.EntityFrameworkCore;

namespace LibraryServices
{
    public class StudentService : IStudentService
    {
        private readonly LibraryDbContext _context;

        public StudentService(LibraryDbContext context)
        {
            _context = context;
        }

        public void Add(Student student)
        {
            _context.Add(student);
            _context.SaveChanges();
        }

        public Student Get(int id)
        {
            return _context.Students
                .Include(a => a.LibraryCard)
                .Include(a => a.HomeLibraryBranch)
                .FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Student> GetAll()
        {
            return _context.Students
                .Include(a => a.LibraryCard)
                .Include(a => a.HomeLibraryBranch);
            // Eager load this data.
        }

        public IEnumerable<CheckoutHistory> GetCheckoutHistory(int studentId)
        {
            var cardId = _context.Students
                .Include(a => a.LibraryCard)
                .FirstOrDefault(a => a.Id == studentId)?
                .LibraryCard.Id;

            return _context.CheckoutHistories
                .Include(a => a.LibraryCard)
                .Include(a => a.LibraryAsset)
                .Where(a => a.LibraryCard.Id == cardId)
                .OrderByDescending(a => a.CheckedOut);
        }

        public IEnumerable<Checkout> GetCheckouts(int id)
        {
            var studentCardId = Get(id).LibraryCard.Id;
            return _context.Checkouts
                .Include(a => a.LibraryCard)
                .Include(a => a.LibraryAsset)
                .Where(v => v.LibraryCard.Id == studentCardId);
        }

        public IEnumerable<Hold> GetHolds(int studentId)
        {
            var cardId = _context.Students
                .Include(a => a.LibraryCard)
                .FirstOrDefault(a => a.Id == studentId)?
                .LibraryCard.Id;

            return _context.Holds
                .Include(a => a.LibraryCard)
                .Include(a => a.LibraryAsset)
                .Where(a => a.LibraryCard.Id == cardId)
                .OrderByDescending(a => a.HoldPlaced);
        }
    }
}