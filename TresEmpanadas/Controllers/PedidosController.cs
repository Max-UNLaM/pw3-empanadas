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
        Entities contexto = new Entities();
        public ActionResult IniciarPedido()
        {
            //ViewBag.estadosPedido = servicioPedido.ListarEstadosPedidos();
            ViewBag.gustosEmpanadas = servicioPedido.ListarGustosEmpanadas();
            ViewBag.usuariosDisponibles = usuarioService.ListarUsuarios();
            System.Web.HttpContext.Current.Session["IdUsuario"] = 2;
            return View();
        }
        [HttpPost]
        public ActionResult GuardarPedido(Pedido pedido, int?[] gustos, int?[] usuariosInvitados) {
                servicioPedido.GuardarPedido(pedido, gustos, usuariosInvitados);           
            return View();
        }
        public ActionResult ListadoPedidos() {
            var listadoPedidos = servicioPedido.listadoPedidosAsociadosUsuario();
            ViewBag.pedidosUsuario = listadoPedidos;

           // servicioPedido.listadoPedidosAsociadosUsuario();
            return View();
        }
    }
}
