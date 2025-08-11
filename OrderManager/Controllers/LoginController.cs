using Microsoft.AspNetCore.Mvc;
using OrderManager.Services;

namespace OrderManager.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public LoginController (IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View("LoginPage");
        }

        [HttpPost]
        public IActionResult Validar (string username, string password)
        {
            var teste = _userService.Signup("Vinícius Monteiro", "vinicius.monteiro", "admin@@123", "admin");

            var validationResponse = _userService.Validar(username, password);

            if (validationResponse.Success)
            {
                var token = _tokenService.GenerateToken(validationResponse.Data);

                Response.Cookies.Append("AuthToken", token, new CookieOptions
                {
                    HttpOnly = true, 
                    Secure = true, 
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                });

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return BadRequest(validationResponse.Message);
            }
        }
    }
}
