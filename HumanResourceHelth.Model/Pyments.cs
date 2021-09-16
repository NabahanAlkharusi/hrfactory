using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HumanResourceHelth.Model
{
   public   class Pyments
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int PymentID { get; set; }
        public int CourseID { get; set; }
        public string  UserID { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddT24:mm:ssK}", ApplyFormatInEditMode = true)]
        public DateTime PymentDate { get; set; }
        public decimal  Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string  cardname { get; set; }
        public string  cardnumber { get; set; }
        public string  expmonth { get; set; }
        public string  expyear { get; set; }
        public string  cvv { get; set; }

    }
}
