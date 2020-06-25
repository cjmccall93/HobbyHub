using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HobbyHub.Models
{
    public class LoginUser
    {
        [Required (ErrorMessage = "Username is required")]
        public string LoginUsername { get; set; }

        [Required (ErrorMessage = "Password is required")]
        [DataType ("Password")]
        public string LoginPassword { get; set; }
    }
}