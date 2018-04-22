using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SAMarineAndBoatSupplies.Models
{
    public class Order
    {
        [BindNever]
        public int OrderId { get; set; }
        public List<OrderDetail> OrderLines { get; set; }
        [Display(Name = "First name")]
        [StringLength(50)]
        [Required(ErrorMessage = "Please enter your first name")]
        public string FirstName { get; set; }
        [Display(Name = "Last name")]
        [StringLength(50)]
        [Required(ErrorMessage = "Please enter your last name")]
        public string LastName { get; set; }
        [Display(Name = "Address Line 1")]
        [StringLength(100)]
        [Required(ErrorMessage = "Please enter the first line of your address")]
        public string AddressLine1 { get; set; }
        [Display(Name = "Address Line 2")]
        [StringLength(100)]
        public string AddressLine2 { get; set; }
        [StringLength(20)]
        [Required(ErrorMessage = "Please enter the name of your city")]
        public string City { get; set; }
        public string County { get; set; }
        [Display(Name = "Post Code")]
        [StringLength(9)]
        [DataType(DataType.PostalCode)]
        [Required(ErrorMessage = "Please enter your post code")]
        public string PostCode { get; set; }
        [Display(Name = "Contact Number")]
        [StringLength(15)]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Please enter a number for us to contact you to complete your order")]
        public string PhoneNumber { get; set; }
        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please enter your email address in a correct format")]
        public string Email { get; set; }
        [Display(Name = "Oder Total")]
        [ScaffoldColumn(false)]
        public decimal OrderTotal { get; set; }
        public DateTime OrderPlaced { get; set; }
    }
}
