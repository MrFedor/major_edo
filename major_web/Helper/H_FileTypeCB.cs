using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace major_web.Helper
{
    public static class H_FileTypeCB
    {
        public static string Label(this HtmlHelper helper, string target, string text)
        {
            return String.Format("<label for='{0}'>{1}</label>", target, text);

        }
    }
}