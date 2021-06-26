//ASM 20150921
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PayIn.Web.Models
{
    public class ValidateEmailViewModel
    {
        [Required]
        [Display(Name = "UserId")]
        [DataType(DataType.Text)]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Code")]
        [DataType(DataType.Text)]
        public string Code { get; set; }

        [Display(Name = "Refresh")]
        public bool Refresh { get; set; }

    }
}