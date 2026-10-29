using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace SuperAbp.Exam.Admin.Blazor.Controllers;

public class AccountController : Controller
{
    [HttpGet("/Account/Login")]
    public IActionResult Login(string? returnUrl = null)
    {
        return Challenge(
            new AuthenticationProperties { RedirectUri = GetLocalReturnUrl(returnUrl) },
            "oidc");
    }

    [HttpGet("/Account/Logout")]
    public IActionResult Logout(string? returnUrl = null)
    {
        return SignOut(
            new AuthenticationProperties { RedirectUri = GetLocalReturnUrl(returnUrl) },
            "Cookies",
            "oidc");
    }

    private string GetLocalReturnUrl(string? returnUrl)
    {
        return Url.IsLocalUrl(returnUrl) ? returnUrl : "/";
    }
}
