using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raza.Model
{
    public class RatePageMetaTag
    {
        public List<MetaTagInfo> RatePageMetaTags { get; set; }
    }

    public class MetaTagInfo
    {
        public string PageName { get; set; }
        public int CountryFrom { get; set; }
        public int CountryTo { get; set; }
        public string Title { get; set; }
        public string PageDescription { get; set; }
        public string PageKeywords { get; set; }
    }
}
