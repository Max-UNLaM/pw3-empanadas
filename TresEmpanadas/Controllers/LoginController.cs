using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TresEmpanadas.Services;

namespace TresEmpanadas.Controllers
{
    public class LoginController : Controller
    {
        LoginService login = new LoginService();

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Usuario usu)
        {
            if(ModelState.IsValid)
            {
                bool usuarioEncontrado = login.verificarDatos(usu);

                if(!usuarioEncontrado) {
                    ViewBag.msg = "Usuario y/o Contraseña inválidos.";
                }
                
                return View();
            }

            return Redirect("/Home/Index");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return Redirect("/Login/Index");
        }
    }
}