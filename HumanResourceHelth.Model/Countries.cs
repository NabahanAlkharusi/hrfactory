using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HumanResourceHelth.Model

{
  public  class Countries
    {
        [Key]
        public int CountryID { get; set; }
        public string CountryNameAr { get; set; }
        public string CountryNameEn { get; set; }

    }
}
