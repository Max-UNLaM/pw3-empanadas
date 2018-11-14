using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            System.Web.HttpContext.Current.Session["IdUsuario"] = 1;
            ViewBag.gustosEmpanadas = servicioPedido.ListarGustosEmpanadas();
            ViewBag.usuariosDisponibles = servicioUsuario.ListarUsuarios();
            if (idPedido == null)
            {
                ViewBag.conModelo = false;
                return View();
            }
            else {
                ViewBag.conModelo = true;
                int idParametro = (int)idPedido;
                Pedido pedidoBuscado = servicioPedido.BuscarPedidoPorId(idParametro);
                return View(pedidoBuscado);
            }
        } 

        //Guardar Pedido
        [HttpPost]
        public ActionResult GuardarPedido(Pedido pedido, int?[] gustos, int?[] usuariosInvitados) {
                servicioPedido.GuardarPedido(pedido, gustos, usuariosInvitados);           
            return View("PedidoIniciado");
        }

        //Listado Pedidos
        public ActionResult ListadoPedidos() {
            var listadoPedidos = servicioPedido.ListadoPedidosAsociadosUsuario();
            ViewBag.pedidosUsuario = listadoPedidos;
            return View();
        }

        //Detalle Pedidos
        public ActionResult DetallePedido(int? idPedido) {
            int idRecibido =  (int)TempData["idPedido"];
            Pedido detallePedido;
            if (idPedido != null)
            {
                detallePedido = servicioPedido.BuscarPedidoPorId((int)idPedido);
            }
            else if (idRecibido > 0)
            {
                detallePedido = servicioPedido.BuscarPedidoPorId((int)idRecibido);
            }else {
                return View();
            }
            ViewBag.detallePedido = detallePedido;
            return View(detallePedido);
        }

        //Eliminar Pedidos
        public RedirectToRouteResult EliminarPedido(int idPedido) {
           servicioPedido.EliminarPedido(idPedido);
            return RedirectToAction("ListadoPedidos");
        }

        public ActionResult EditarPedido(int idPedido) {
            Boolean estadoPedido = servicioPedido.EstadoPedido(idPedido);
            if (estadoPedido)
            {
                System.Web.HttpContext.Current.Session["IdUsuario"] = 1;
                ViewBag.gustosEmpanadas = servicioPedido.ListarGustosEmpanadas();
                ViewBag.usuariosDisponibles = servicioUsuario.ListarUsuarios();
                ViewBag.conModelo = true;
                int idParametro = (int)idPedido;
                Pedido pedidoBuscado = servicioPedido.BuscarPedidoPorId(idParametro);
                return View(pedidoBuscado);
            }
            else {
                //var detallePedido = servicioPedido.BuscarPedidoPorId(idPedido);
                //ViewBag.detallePedido = detallePedido;
                //return View("",detallePedido);
                TempData["idPedido"] = idPedido; 
                return RedirectToAction("DetallePedido");
            }
        }
    }
}
