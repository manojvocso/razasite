using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcApplication1.Areas.Mobile.Models
{
    public class DeleteOnetouchSetupModel
    {
        [Required]
        public string OnetouchNumber { get; set; }
    }
}