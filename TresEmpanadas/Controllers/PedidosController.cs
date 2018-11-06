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
        UsuarioService usuarioService = new UsuarioService();
        public ActionResult IniciarPedido()
        {
            //ViewBag.estadosPedido = servicioPedido.ListarEstadosPedidos();
            ViewBag.gustosEmpanadas = servicioPedido.ListarGustosEmpanadas();
            ViewBag.usuariosDisponibles = usuarioService.ListarUsuarios();
            System.Web.HttpContext.Current.Session["IdUsuario"] = 1;
            return View();
        }
        [HttpPost]
        public ActionResult GuardarPedido(Pedido pedido) {
            servicioPedido.GuardarPedido(pedido);
            return View();
        }

    }
}
