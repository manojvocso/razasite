using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raza.Model;

namespace MvcApplication1.Models
{
    public class ReadLogFileModel : BaseRazaViewModel
    {
        public string FileDate { get; set; }

        public string LogData { get; set; }
        public List<Country> list { get; set; } 
    }
}