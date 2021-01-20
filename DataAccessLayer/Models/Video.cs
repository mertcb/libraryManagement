using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Models
{
    public class Video : LibraryAsset
    {
        [Required]
        public string Director { get; set; }
        public Video()
        {
        }
    }
}
