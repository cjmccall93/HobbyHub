using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using HobbyHub.Models;

namespace HobbyHub.Models
{
    
    public class Hobbies{
        [Key]        
        public int HobbyId {get; set;}
        [Required(ErrorMessage="A Title is Required")]
        public string Title {get; set;}
        [Required(ErrorMessage="A Description is Required")]
        public string Description{get; set;}
        public Users Creator {get; set;}
        public int UserId {get;set;}

        public List<HobbyHubAssoc> JoiningUser{ get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt {get;set;}
    }
}