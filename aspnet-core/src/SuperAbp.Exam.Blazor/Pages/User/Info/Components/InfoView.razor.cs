using System.Threading.Tasks;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace SuperAbp.Exam.Blazor.Pages.User.Info.Components
{
    public partial class InfoView(ICurrentUser currentUser, IIdentityUserAppService userService)
    {
        protected IdentityUserDto UserDto;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            UserDto = await userService.GetAsync(currentUser.GetId());
        }

        private async Task HandleFinish()
        {
            IdentityUserUpdateDto updateDto = new IdentityUserUpdateDto();
            await userService.UpdateAsync(currentUser.GetId(), UserDto);
        }
    }
}