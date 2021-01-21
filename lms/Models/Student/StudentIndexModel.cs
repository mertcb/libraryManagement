using System.Collections.Generic;

namespace lms.Models.Student
{
    public class StudentIndexModel
    {
        public IEnumerable<StudentDetailModel> Students { get; set; }
    }
}