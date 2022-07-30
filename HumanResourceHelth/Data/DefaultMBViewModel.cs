using HumanResourceHelth.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HumanResourceHelth.Web.Data
{
    public class DefaultMBViewModel
    {
        [MaxLength(50)]
        public string Title { get; set; }


        public int? ParenId { get; set; }
        public int CurrentCountry { get; set; }
        public int CurrentCompanySize { get; set; }
        public int CurrentCompanyIndustry { get; set; }

        [MaxLength(50)]
        public string Description { get; set; }

        [MaxLength]
        public string Content { get; set; }
        public int ParentConter { get; set; }
        public List<DefaultMB> DefaultMB { get; set; }
        public List<DefaultMB> DefaultMB1 { get; set; }
        public List<Country> Countries { get; set; }
        public List<Country> CountriesNotAdded { get; set; }
    }
}