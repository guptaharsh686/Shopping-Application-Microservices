using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

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

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto obj)
        {
            ResponseDto response = await _authService.LoginAsync(obj);

            if (response != null && response.IsSuccess)
            {
                LoginResponseDto loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(response.Result));
                TempData["success"] = "Login Sucessful";
                //on login sucessful redirect to login view
                return RedirectToAction("Index","Home");
            }
            else
            {
                ModelState.AddModelError("CustomError", response.Message);
                return View(obj);
            }
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
