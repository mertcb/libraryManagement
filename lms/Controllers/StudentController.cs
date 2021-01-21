using System.Linq;
using DataAccessLayer;
using lms.DataAccessLayer;
using lms.Models.Student;
using Microsoft.AspNetCore.Mvc;

namespace lms.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public IActionResult Index()
        {
            var allStudents = _studentService.GetAll();

            var studentModels = allStudents
                .Select(p => new StudentDetailModel
                {
                    Id = p.Id,
                    LastName = p.LastName ?? "No First Name Provided",
                    FirstName = p.FirstName ?? "No Last Name Provided",
                    LibraryCardId = p.LibraryCard?.Id,
                    OverdueFees = p.LibraryCard?.Fees,
                    HomeLibrary = p.HomeLibraryBranch?.Name
                }).ToList();

            var model = new StudentIndexModel
            {
                Students = studentModels
            };

            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var student = _studentService.Get(id);

            var model = new StudentDetailModel
            {
                Id = student.Id,
                LastName = student.LastName ?? "No Last Name Provided",
                FirstName = student.FirstName ?? "No First Name Provided",
                Address = student.Address ?? "No Address Provided",
                HomeLibrary = student.HomeLibraryBranch?.Name ?? "No Home Library",
                MemberSince = student.LibraryCard?.Created,
                OverdueFees = student.LibraryCard?.Fees,
                LibraryCardId = student.LibraryCard?.Id,
                Telephone = string.IsNullOrEmpty(student.Telephone) ? "No Telephone Number Provided" : student.Telephone,
                CheckoutHistory = _studentService.GetCheckoutHistory(id),
                Holds = _studentService.GetHolds(id)
            };

            return View(model);
        }
    }
}