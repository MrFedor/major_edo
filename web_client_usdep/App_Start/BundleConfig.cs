using System.Web;
using System.Web.Optimization;

namespace web_client_usdep
{
    public class BundleConfig
    {
        //Дополнительные сведения об объединении см. по адресу: http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));            

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(                    
                    "~/Scripts/jquery.validate.min.js",
                    "~/Scripts/jquery.validate.unobtrusive.js",
                    "~/Scripts/jquery.validate.unobtrusive.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/global").Include(
                "~/Scripts/cldr.js",
                "~/Scripts/globalize.js",
                "~/Scripts/globalize/*.js"));

            bundles.Add(new ScriptBundle("~/bundles/globalLastFile").Include(
             "~/Scripts/jquery.validate.globalize*"));

            bundles.Add(new ScriptBundle("~/bundles/dhtmlxcombo").Include(
                        "~/Scripts/dhtmlxcombo.js",
                        "~/Scripts/dhtmlxvault.js"));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // используйте средство сборки на сайте http://modernizr.com, чтобы выбрать только нужные тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/daterangepicker-bs3.css",
                      "~/Content/dhtmlxcombo.css",
                      "~/Content/dhtmlxvault.css"));

            bundles.Add(new ScriptBundle("~/bundles/daterangepicker").Include(
                      "~/Scripts/moment.min.js",
                      "~/Scripts/moment-with-locales.min.js",
                      "~/Scripts/daterangepicker.js"));
        }
    }
}
