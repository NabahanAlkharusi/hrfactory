using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public class IntroVedio : BaseEntity
    {
        public bool Uploaded { get; set; }
        public String EmbadedEnglish { get; set; }
        public String UploadedEnglish { get; set; }
        public String EmbadedArabic { get; set; }
        public String UploadedArabic { get; set; }


    }
}
