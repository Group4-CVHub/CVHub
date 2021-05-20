using CVHub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CVHub.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Enter a valid E-mail adress"), EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Enter a password"), PasswordPropertyText, StringLength(150, MinimumLength = 5), DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Enter first name"), StringLength(150, MinimumLength = 1, ErrorMessage = "Name to long!"), DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Enter last name"), StringLength(150, MinimumLength = 1, ErrorMessage = "Name to long!"), DisplayName("Last Name")]
        public string LastName { get; set; }
        [Phone(ErrorMessage = "Need to enter a phone number"), StringLength(12, MinimumLength = 10)]
        public string PhoneNumber { get; set; }
        [StringLength(75)]
        public string Country { get; set; }
        [StringLength(75)]
        public string City { get; set; }
        [StringLength(75)]
        public string State { get; set; }
        public string FacebookId { get; set; }
        //public string GoogleId { get; set; }
        public ICollection<Cv> Cvs { get; set; }
    }
}
