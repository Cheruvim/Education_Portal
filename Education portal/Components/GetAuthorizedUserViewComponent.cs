using System.Threading.Tasks;
using Education_portal.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Education_portal.Components
{
    public class GetAuthorizedUserViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync()
        {
            var currentUser = UserCookieUtility.GetUserInfoFromCookies(HttpContext);
            return Task.FromResult<IViewComponentResult>(View(currentUser));
        }
    }
}