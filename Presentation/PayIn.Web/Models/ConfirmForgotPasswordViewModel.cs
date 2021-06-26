using PayIn.Common.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PayIn.Web.Models
{
    public class ConfirmForgotPasswordViewModel:ValidateEmailViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword")]
        [Compare("Password",ErrorMessageResourceType = typeof(SecurityResources), ErrorMessageResourceName = "ComparePassword")]
        public string ConfirmPassword { get; set; }
    }
}