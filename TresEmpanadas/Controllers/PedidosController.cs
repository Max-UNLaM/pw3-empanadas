using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TresEmpanadas.Models;
using TresEmpanadas.Models.ViewModels;
using TresEmpanadas.Services;
namespace TresEmpanadas.Controllers
{
    public class PedidosController : Controller
    {
        PedidoService servicioPedido = new PedidoService();
        UsuarioService servicioUsuario = new UsuarioService();
        //Entities contexto = new Entities();

        // Iniciar Pedido
        public ActionResult IniciarPedido(int? idPedido)
        {
            if (Session["idUsuario"] != null)
            {
                //System.Web.HttpContext.Current.Session["IdUsuario"] = 1;
                ViewBag.gustosEmpanadas = servicioPedido.ListarGustosEmpanadas();
                var usuariosDisponibles = servicioUsuario.ListarUsuariosParaInvitar((int)Session["idUsuario"]);
                ViewBag.usuariosDisponibles = usuariosDisponibles;
                    if (idPedido == null)
                    {
                        ViewBag.conModelo = false;
                    //PedidoGusto pedidoGusto = new PedidoGusto();
                    //pedidoGusto.Pedido = new Pedido();
                    //pedidoGusto.GustoEmpanada = servicioPedido.ListarGustosEmpanadas();
                    //Pedido pedido = new Pedido();

                    //return View(pedidoGusto);
                    return View();
                    }
                    else
                    {
                        ViewBag.conModelo = true;
                        int idParametro = (int)idPedido;
                        ViewBag.usuariosInvitados = servicioPedido.UsuariosInvitados((int)idPedido);
                        Pedido pedidoBuscado = servicioPedido.BuscarPedidoPorId(idParametro);
                        return View(pedidoBuscado);
                    }
            }
            else
            {
                return Redirect("/Home/Login?redirigir=/Pedidos/IniciarPedido/");
            }
        }

        //Guardar Pedido
        [HttpPost]
        public ActionResult GuardarPedido(Pedido pedido, int?[] gustos, string[] usuariosInvitados)
        {
            if (ModelState.IsValid)
            {
                var idPedidoRetornado = servicioPedido.GuardarPedido(pedido, gustos, usuariosInvitados);
                ViewBag.NombrePedido = servicioPedido.BuscarPedidoPorId(idPedidoRetornado).NombreNegocio;
                ViewBag.IdPedido = idPedidoRetornado;
                return View("PedidoIniciado");
            }
            else {
                return View("IniciarPedido", pedido);
            }
        }
        public ActionResult CerrarPedido(int idPedido) {
             servicioPedido.CerrarPedido(idPedido);
            var result = 1;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //Listado Pedidos
        public ActionResult ListadoPedidos()
        {
            if (Session["idUsuario"] != null)
            {
                ViewBag.pedidoEliminado = Session["pedidoEliminado"];
                //Session["pedidoEliminado"] = null;
                Session.Remove("pedidoEliminado");
                var listadoPedidos = servicioPedido.ListadoPedidosAsociadosUsuario();
                ViewBag.pedidosUsuario = listadoPedidos;
                return View();
            }
            else
            {
                return Redirect("/Home/Login?redirigir=/Pedidos/listadoPedidos/");
            }
        }

        //Detalle Pedidos
        public ActionResult DetallePedido(int? idPedido)
        {
            if (Session["idUsuario"] != null)
            {
                Pedido detallePedido;
                if (idPedido != null)
                {
                    detallePedido = servicioPedido.BuscarPedidoPorId((int)idPedido);
                }
                else
                {
                    int idRecibido = (int)TempData["idPedido"];
                    if (idRecibido > 0)
                    {
                        detallePedido = servicioPedido.BuscarPedidoPorId((int)idRecibido);
                    }
                    else
                    {
                        return View();
                    }
                }
                ViewBag.detallePedido = detallePedido;
                return View(detallePedido);
            }
            else
            {
                string url;
                if (idPedido != null)
                {
                    url = "/Home/Login?redirigir=/Pedidos/DetallePedido?idPedido=" + idPedido;

                }
                else
                {
                    int idRecibido = (int)TempData["idPedido"];
                    if (idRecibido > 0)
                    {
                        url = "/Home/Login?redirigir=/Pedidos/DetallePedido?idPedido=" + idRecibido;
                    }
                    else
                    {
                        return Redirect("/Home/Login?redirigir=/Pedidos/DetallePedido/");
                    }
                }
                return Redirect(url);
            }

        }

        //Eliminar Pedidos

        public RedirectToRouteResult Eliminar(int idPedido)
        {
            var nombrePedidoEliminado = servicioPedido.BuscarPedidoPorId(idPedido);
            servicioPedido.EliminarPedido(idPedido);
            Session["pedidoEliminado"] = nombrePedidoEliminado.NombreNegocio;
            return RedirectToAction("ListadoPedidos");
        }

        public ActionResult EliminarPedido(int idPedido)
        {
            var pedido = servicioPedido.BuscarPedidoPorId(idPedido);
            var invitacionesConfirmadas = servicioPedido.BuscarInvitacionesConfirmadas(idPedido);
            ViewBag.cantidadInvitaciones = invitacionesConfirmadas;
            return View(pedido);
        }
        // Eliminar con JavaScrit
        //[HttpGet]
        //public ActionResult Eliminar(int? idPedido)
        //{
        //    var valor = idPedido;
        //    var result = 0;
        //    if (valor != null) {
        //        servicioPedido.EliminarPedido((int)valor);
        //        // return RedirectToAction("ListadoPedidos");
        //        result = 1;
        //    }
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult EditarPedido(int idPedido)
        {
            Boolean estadoPedido = servicioPedido.EstadoPedido(idPedido);
            if (estadoPedido)
            {
                try
                {
                    ViewBag.gustosEmpanadas = servicioPedido.ListarGustosEmpanadas();
                    ViewBag.usuariosDisponibles = servicioUsuario.ListarUsuariosParaInvitar((int)Session["idUsuario"]);
                    ViewBag.usuariosInvitados = servicioPedido.UsuariosInvitados(idPedido);
                    ViewBag.conModelo = true;
                    int idParametro = (int)idPedido;
                    Pedido pedidoBuscado = servicioPedido.BuscarPedidoPorId(idParametro);
                    return View(pedidoBuscado);
                }
                catch
                {
                    return View("Error/Info", new DetailError
                    {
                        Title = "Error",
                        Body = "No fue posible editar el pedido.",
                        Link = ""
                    });
                }
                
            }
            else
            {
                //var detallePedido = servicioPedido.BuscarPedidoPorId(idPedido);
                //ViewBag.detallePedido = detallePedido;
                //return View("",detallePedido);
                TempData["idPedido"] = idPedido;
                return RedirectToAction("DetallePedido");
            }
        }
        [HttpPost]
        public ActionResult EditarPedido(Pedido pedido, int?[] gustos, string[] usuariosInvitados, int opcion_id)
        {

            servicioPedido.EditarPedido(pedido, gustos, usuariosInvitados, opcion_id);
            return RedirectToAction("ListadoPedidos");
        }

        [HttpGet]
        public ActionResult ElegirGustos(string idPedido = "", string token = "")
        {
            var lele = Int32.Parse(Request.QueryString["IdPedido"]);
            if (idPedido == "" && token == "")
            {
                return RedirectToAction("ListadoPedidos");
            }
            try
            {
                if (idPedido != "")
                {
                    return ElegirGustoPorIdUsuario(Int32.Parse(idPedido));
                }
                return ElegirGustosPorToken(new Guid(token));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return View("~/Views/Error/Info", new DetailError
                {
                    Title = "Error",
                    Body = "No fue posible actualizar el pedido.",
                    Link = ""
                });
            }
        }

        internal ActionResult ElegirGustoPorIdUsuario(int idPedido)
        {
            var elegirPedidoService = new ElegirPedidoService();
            return View(elegirPedidoService.BuildElegirPedido(idPedido, (int)Session["idUsuario"]));
        }


        internal ActionResult ElegirGustosPorToken(Guid token)
        {
            return null;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Error/Index.cshtml"
            };
        }
    }
}
