using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.Account.Web;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.AspNetCore.Mvc.MultiTenancy;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace SuperAbp.Exam.Pages.Account
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(LoginModel))]
    public class MyLoginModel : LoginModel
    {
        private readonly IAbpTenantAppService _tenantAppService;

        public MyLoginModel(IAuthenticationSchemeProvider schemeProvider,
            IOptions<AbpAccountOptions> accountOptions,
            IOptions<IdentityOptions> identityOptions,
            IdentityDynamicClaimsPrincipalContributorCache identityDynamicClaimsPrincipalContributorCache,
            IAbpTenantAppService tenantAppService)
            : base(schemeProvider, accountOptions, identityOptions, identityDynamicClaimsPrincipalContributorCache)
        {
            _tenantAppService = tenantAppService;
        }

        public override async Task<IActionResult> OnGetAsync()
        {
            FindTenantResultDto tenant = await _tenantAppService.FindTenantByNameAsync("Demo");
            ViewData["TenantId"] = tenant.TenantId;
            return await base.OnGetAsync();
        }
    }
}