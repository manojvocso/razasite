namespace Raza.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EmailModel
    {

        public int EmailId { get; set; }

        public string Subject { get; set; }

        public string Sender { get; set; }

        public string Body { get; set; }

        public DateTime UpdatedTimestamp { get; set; }

        public string ShortSubject
        {
            get
            {
                int length = Subject.Length > 20 ? 20 : Subject.Length;
                return string.Format("{0}..{1}", Subject.Substring(0, length), UpdatedTimestamp.ToString("M/d/yyyy"));
            }
        }
    }
}
