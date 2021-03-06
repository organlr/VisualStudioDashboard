﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dashboard;
using WebApplication.Extensions;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public partial class BurnDownController : Controller
    {
        private readonly IHistorian historian;
        private readonly Dictionary<string, string> colors = new Dictionary<string, string>
        {
            {"Ideal", "#000000"},
            {"Remaining", "#2ca02c"}
        };

        public BurnDownController(IHistorian historian)
        {
            this.historian = historian;
        }

        // GET: BurnDown
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual JsonResult GetData()
        {
            var burnupData = historian.GetBurnDown(new DateTime(2014, 7, 9, 23, 59, 59),
                new DateTime(2014, 11, 17, 23, 59, 59), @"BPS.Scrum\Dev -SEP Project");

            var series =
                burnupData.Select(d => CreateViewModel(d.Data, d.Title, colors[d.Title], d.Title == "Remaining"));

            return Json(series, JsonRequestBehavior.AllowGet);
        }

        private ChartSeriesViewModel CreateViewModel(IEnumerable<Metric> data, string seriesTitle, string colorString, bool area)
        {
            return new ChartSeriesViewModel
            {
                values =
                    data.Select(s => new PointViewModel { x = s.Date.ToInt(), y = s.Value })
                        .ToList(),
                key = seriesTitle,
                color = colorString,
                area = area ? true.ToString().ToLower() : string.Empty
            };
        }
    }
}