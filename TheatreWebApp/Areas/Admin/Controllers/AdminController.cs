using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static TheatreWebApp.Areas.Admin.AdminConstants;

namespace TheatreWebApp.Areas.Admin.Controllers
{
    [Area(AreaName)]
    [Authorize(Roles = AdminRoleName)]
    public class AdminController : Controller
    {
    }
}
