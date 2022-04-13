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
    public class TermsConditionsViewModel
    {
        public TermsConditions TermCondition { get; set; }
        public List<TermsConditions>  TermsConditions { get; set; }
        public List<Country> Countries { get; set; }


    }
}