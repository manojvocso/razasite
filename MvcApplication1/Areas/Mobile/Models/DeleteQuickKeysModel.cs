using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcApplication1.Areas.Mobile.Models
{
    public class DeleteQuickKeysModel
    {
        [Required]
        public string DestinationNumber { get; set; }

        [Required]
        public string QuickKeys { get; set; }
    }
}