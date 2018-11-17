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
            if (Session["idUsuario"] == null)
            {
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Index(Usuario usu)
        {
            if(ModelState.IsValid)
            {
                bool usuarioEncontrado = login.verificarDatos(usu);

                if(!usuarioEncontrado) {
                    ViewBag.msg = "Usuario y/o Contraseña inválidos.";

                    return View();
                }
                else
                {
                    if(Request.QueryString["redirigir"] != null)
                    {
                        return Redirect(Request.QueryString["redirigir"]);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                
            }

            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}