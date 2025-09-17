using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Volo.Abp.AspNetCore.Mvc;

namespace SuperAbp.Exam.Admin.Controllers;

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