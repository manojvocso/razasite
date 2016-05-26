using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcApplication1.Areas.Mobile.Models
{
    public class DeletePinlessSetupModel
    {
        [Required]
        public string PinlessNumber { get; set; }
        [Required]
        public string CountryCode { get; set; }
        
    }
}