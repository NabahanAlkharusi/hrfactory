using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public class Coupons
    {
        //creation of Coupon Model that will handel Coupons data
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] /* this line is to avoid this error 
                                                               * (System.Data.SqlClient.SqlException: 
                                                               * Cannot insert explicit value for 
                                                               * identity column in table 'Coupons' 
                                                               * when IDENTITY_INSERT is set to OFF.)*/
        [Key]
        public int ID { get; set; } // automatic Id which is auto-increment id
       
        [Display(Name = "CouponCode", ResourceType = typeof(Resources.Coupons.Coupons))] // used for Localization and Globalization 
        [Required]
        public string coupon_code { get; set; } // Coupon Code field 
        [Display(Name = "coupon_discount_rate", ResourceType = typeof(Resources.Coupons.Coupons))]// used for Localization and Globalization 
        [Required]
        [Range(0, 100)]
        public Decimal coupon_discount_rate { get; set; } // Coupon Discount rate field

        [Display(Name = "coupon_stat", ResourceType = typeof(Resources.Coupons.Coupons))]// used for Localization and Globalization 
        [Required]
        public int coupon_stat { get; set; } // represent Coupon Status 
        [DataType(DataType.Date)]  // used to make editfor in the forms with date type
        [Display(Name = "coupon_satrt_date", ResourceType = typeof(Resources.Coupons.Coupons))]// used for Localization and Globalization 
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")] // Sepcify date format
        [Required]
        public DateTime coupon_satrt_date { get; set; }
        [DataType(DataType.Date)]// used to make editfor in the forms with date type
        [Display(Name = "coupon_end_date", ResourceType = typeof(Resources.Coupons.Coupons))]// used for Localization and Globalization 
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]// Sepcify date format
        [Required]
        public DateTime coupon_end_date { get; set; }
        [Display(Name = "coupon_for", ResourceType = typeof(Resources.Coupons.Coupons))]// used for Localization and Globalization 
        [Required]
        public string coupon_for { get; set; } // user can sepcify each coupon for sepcific service plan 
        [Display(Name = "coupon_created_at", ResourceType = typeof(Resources.Coupons.Coupons))]// used for Localization and Globalization 
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime coupon_created_at { get; set; } // Coupon Created at feild which is an automatic field (system will fill this field or data)
        

    }
}
