using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TresEmpanadas.Services;

namespace TresEmpanadas.Controllers
{
    public class HomeController : Controller
    {
        LoginService login = new LoginService();

        public ActionResult Index()
        {
            if (Session["idUsuario"] == null)
            {
                return RedirectToAction("Login");
            }

            return RedirectToAction("ListadoPedidos", "Pedidos");
        }

        // GET: Login
        public ActionResult Login()
        {
            if (Session["idUsuario"] == null)
            {
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Login(Usuario usu)
        {
            if (ModelState.IsValid)
            {
                bool usuarioEncontrado = login.verificarDatos(usu);

                if (!usuarioEncontrado)
                {
                    ViewBag.msg = "Usuario y/o Contraseña inválidos.";

                    return View();
                }
                else
                {
                    if (Request.QueryString["redirigir"] != null)
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
            return RedirectToAction("Login", "Home");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}