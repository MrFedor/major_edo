﻿using System.Web;
using System.Web.Optimization;

namespace major_web
{
    public class BundleConfig
    {
        //Дополнительные сведения об объединении см. по адресу: http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/signalR").Include(
                        "~/Scripts/jquery.signalR-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/sitejs").Include(
                        "~/Scripts/sitejs.js"));

            bundles.Add(new ScriptBundle("~/bundles/dhtmlxcombo").Include(
                        "~/Scripts/dhtmlxcombo.js",
                        "~/Scripts/dhtmlxlayout.js"));

            bundles.Add(new ScriptBundle("~/bundles/material").Include(
                        "~/Scripts/material.min.js"));

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
                      "~/Content/daterangepicker.css",
                      "~/Content/dhtmlxcombo.css",
                      "~/Content/dhtmlxlayout.css",
                      "~/Content/toastr.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/daterangepicker").Include(
                      "~/Scripts/moment.min.js",
                      "~/Scripts/moment-with-locales.min.js",
                      "~/Scripts/daterangepicker.js",
                      "~/Scripts/toastr.min.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/material").Include(
                      "~/Content/material-css.css",
                      "~/Content/material.min.css"));
        }
    }
}
