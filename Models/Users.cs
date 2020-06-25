using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using HobbyHub.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace HobbyHub.Models
{
    public class Users {
        [Key]
        public int UserId { get; set; }
        [Required]
        [MinLength(2)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(2)]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Must enter Username")]
        [Display(Name = "Username")]
        [MinLength(3)]
        [MaxLength(15)]
        public string Username { get; set; }
        [Required(ErrorMessage = "Must Enter password")]
        [MinLength(8)]
        [Display(Name = "Password:")]
        public string Password { get; set; }
        [NotMapped]
        [Compare("Password", ErrorMessage="Passwords do not match")]
        [Display(Name = "Confirm Password:")]
        public string Confirm { get; set; }
        public List<Hobbies>   FavoriteHobbies {get;set;}
        public List<HobbyHubAssoc> DoingHobbies{ get; set;}

    }
}
