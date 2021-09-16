using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace HumanResourceHelth.Web.Areas.Admin.Data
{
    public class GridFilesViewModel
    {
        public DirectoryInfo[] PlanDirectory { get; set; }
        public string Language { get; set; }
    }
}