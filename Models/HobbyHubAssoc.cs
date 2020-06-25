using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HobbyHub.Models;

namespace HobbyHub.Models
{
    public class HobbyHubAssoc
    {
        [Key]
        public int UserHobbyId {get; set;}
        public int UserId {get; set;}
        public int HobbyId {get; set;}
        public Users JoiningUser{get; set;}
        public Hobbies JoiningActivity {get; set;}
    }
}