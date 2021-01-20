using System;
namespace DataAccessLayer.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string TelephoneNumber  { get; set; }

        //public virtual LibraryCard LibraryCard { get; set; }
        public Student()
        {
        }
    }
}
