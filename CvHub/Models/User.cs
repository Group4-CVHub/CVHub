using CVHub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CvHub.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Enter a viable E-mail adress"), EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Enter a password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Enter first name"), Range(1, 50, ErrorMessage = "Name to long!")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Enter last name"), Range(1, 50, ErrorMessage = "Name to long!")]
        public string LastName { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public ICollection<Cv> Cvs { get; set; }
    }
}
