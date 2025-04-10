using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Notifications;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace SuperAbp.Exam.Blazor.Pages.Account.Setting.Components
{
    public partial class InfoView(ICurrentUser currentUser, IIdentityUserAppService userService, IUiNotificationService notification)
    {
        protected IdentityUserDto UserDto;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            UserDto = await userService.GetAsync(currentUser.GetId());
        }

        private async Task HandleFinish()
        {
            await userService.UpdateAsync(currentUser.GetId(),
                new IdentityUserUpdateDto
                {
                    Name = UserDto.Name,
                    UserName = UserDto.UserName,
                    Surname = UserDto.Surname,
                    Email = UserDto.Email,
                    PhoneNumber = UserDto.PhoneNumber
                });
            await notification.Success("Updated");
        }
    }
}