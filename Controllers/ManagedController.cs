using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PracticaMvcCore2APJ.Models;
using PracticaMvcCore2APJ.Repositories;
using System.Security.Claims;

namespace PracticaMvcCore2APJ.Controllers
{
    public class ManagedController : Controller
    {
        private RepositoryLibros repo;
        public ManagedController(RepositoryLibros repo)
        {
            this.repo = repo;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            Usuario usuario = await this.repo.LogInUsuarioAsync(email, password);
            if(usuario != null)
            {
                ClaimsIdentity identity = new ClaimsIdentity(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    ClaimTypes.Name, ClaimTypes.Role);

                Claim claimName = new Claim(ClaimTypes.Name, usuario.Nombre);
                identity.AddClaim(claimName);
                Claim claimId = new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString());
                identity.AddClaim(claimId);
                Claim claimApellidos = new Claim(ClaimTypes.Role, usuario.Apellidos);
                identity.AddClaim(claimApellidos);
                Claim claimEmail = new Claim("Email", usuario.Email);
                identity.AddClaim(claimEmail);
                Claim claimFoto = new Claim("Foto", usuario.Foto);
                identity.AddClaim(claimFoto);

                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);

                string controller = TempData["controller"].ToString();
                string action = TempData["action"].ToString();
                return RedirectToAction(action, controller);
            }
            else
            {
                ViewData["MENSAJE"] = "Usuario/Password incorrectos";
                return View();
            }
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ErrorAcceso()
        {
            return View();
        }
    }
}
