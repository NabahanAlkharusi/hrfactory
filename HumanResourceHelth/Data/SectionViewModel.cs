using HumanResourceHelth.DataAccess.Migrations;
using HumanResourceHelth.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanResourceHelth.Web.Data
{
    public class SectionViewModel
    {
        [MaxLength(50)]
        public string Title { get; set; }
        
        
        public int? ParenId { get; set; }
        
        [MaxLength(50)]
        public string Description { get; set; }
        
        [MaxLength]
        public string Content { get; set; }
        public List<Section>  Sections { get; set; }
        public List<Section> DefualtSections { get; set; }
        public List<DefaultMB> DefualtMBs { get; set; }
        public List<Country> Countries { get; set; }
        public List<Country> CountriesNotAdded { get; set; }

    }
}