using System;
using System.Collections.Generic;
using HumanResourceHelth.Model;

namespace HumanResourceHelth.Web.Models
{
    public class BuilderViewModel
    {
        public List<Section> Sections { get; set; }
        public string CompanyName { get; set; }
        public string DocumentName { get; set; }
        public string LogoFile { get; set; }
    }
}