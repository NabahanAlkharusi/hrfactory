using HumanResourceHelth.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HumanResourceHelth.Web.Models
{
    public class RequestServiceViewModel 
    {
        [Required]
        public String Discription { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        

        [MaxLength(255, ErrorMessage = "must be less than 255 charcter....")]
        public string ContactPerson { get; set; }
        public DateTime LastLoginDate { get; set; }
       
        
        [Required]

        public int NumberOFEmployees { get; set; }
        public List<Country> Countries { get; set; }
        public List<Industry> Industries { get; set; }



    }
}