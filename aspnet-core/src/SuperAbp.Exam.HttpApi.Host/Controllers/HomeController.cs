using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using Volo.Abp.AspNetCore.Mvc;

namespace SuperAbp.Exam.Controllers;

public class HomeController(IHostEnvironment environment) : AbpController
{
    public ActionResult Index()
    {
        if (environment.IsDevelopment())
        {
            return Redirect("~/swagger");
        }

        return Content("Hello Baby!");
    }
}