using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new();

            return View(loginRequestDto);
        }

        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{ Text=StaticDetails.RoleAdmin, Value=StaticDetails.RoleAdmin },
                new SelectListItem{ Text=StaticDetails.RoleCustomer, Value=StaticDetails.RoleCustomer }

            };

            ViewBag.RoleList = roleList;

            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterationRequestDto obj)
        {
            ResponseDto response = await _authService.RegisterAsync(obj);
            ResponseDto assignRole;

            if(response != null && response.IsSuccess)
            {
                if (string.IsNullOrEmpty(obj.Role))
                {
                    obj.Role = StaticDetails.RoleCustomer;
                }
                assignRole = await _authService.AssignRoleAsync(obj);

                if (assignRole != null && assignRole.IsSuccess)
                {
                    TempData["success"] = "Registration Sucessful";
                    //on login sucessful redirect to login view
                    return RedirectToAction(nameof(Login));
                }
            }

            //if unsucessful then return back to view with obj values populated

            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{ Text=StaticDetails.RoleAdmin, Value=StaticDetails.RoleAdmin },
                new SelectListItem{ Text=StaticDetails.RoleCustomer, Value=StaticDetails.RoleCustomer }

            };
            TempData["error"] = "An error occured please try again";

            ViewBag.RoleList = roleList;

            return View(obj);
        }
    }
}
