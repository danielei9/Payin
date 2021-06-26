//ASM 20150928
using PayIn.Common.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PayIn.Web.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

		//[Required]
        [Display(Name = "Birthday")]
        public string Birthday { get; set; }

        [Required]
        [Display(Name = "Mobile")]
        public string Mobile { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword")]
        [Compare("Password", ErrorMessageResourceType = typeof(SecurityResources), ErrorMessageResourceName = "ComparePassword")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "CheckTerms")]
        public bool CheckTerms { get; set; }


    }
}