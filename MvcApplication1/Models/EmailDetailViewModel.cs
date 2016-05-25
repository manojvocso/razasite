using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
{
    using Raza.Model;

    public class EmailDetailViewModel : BaseRazaViewModel
    {

        public string FullName { get; set; }

        public List<EmailModel> Emails { get; set; }

        public EmailModel SelectedEmail { get; set; }

    }
}
    