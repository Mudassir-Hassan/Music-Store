using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStore1.Models
{
    [Bind(Exclude="OrderId")]
    public partial class Order
    {
        [ScaffoldColumn(false)]
        public int OrderID { get; set; }
        
        [ScaffoldColumn(false)]
        public string Username { get; set; }
        
        [Required(ErrorMessage = "First Name is Required")]
        [DisplayName("First Name")]
        [StringLength(160)]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Last Name is Required")]
        [DisplayName("Last Name")]
        [StringLength(160)]
        public string LastName { get; set; }
        
        [Required(ErrorMessage="Address is required")]
        [StringLength(160)]
        public string Address { get; set; }
        
        [Required(ErrorMessage="City is Required")]
        [StringLength(40)]
        public string City { get; set; }
        
        [Required(ErrorMessage="State is Required")]
        [StringLength(40)]
        public string State { get; set; }

        [Required(ErrorMessage = "Postal Code is Required")]
        [DisplayName("PostalCode")]
        [StringLength(10)]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Country is Required")]
        [StringLength(40)]
        public string Country { get; set; }

        [Required(ErrorMessage = "Phone is Required")]
        [StringLength(24)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [DisplayName("Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Email is not valid")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [ScaffoldColumn(false)]
        public decimal Total { get; set; }
        
        [ScaffoldColumn(false)]
        public System.DateTime OrderDate { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}