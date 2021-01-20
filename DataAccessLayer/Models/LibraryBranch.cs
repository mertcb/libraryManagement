using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Models
{
    public class LibraryBranch
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Branch Name is Limited Up To 30 Characters")]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Telephone { get; set; }
        public string Description { get; set; }
        public DateTime OpenDate { get; set; }

        public virtual IEnumerable<Student> Students { get; set; }
        public virtual IEnumerable<LibraryAsset> LibraryAssets { get; set; }
        public string ImageUrl { get; set; }


        public LibraryBranch()
        {
        }
    }
}
