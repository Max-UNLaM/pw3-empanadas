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
            System.Web.HttpContext.Current.Session["IdUsuario"] = 1;
            return View();
        }
        [HttpPost]
        public ActionResult GuardarPedido(Pedido pedido, int?[] gustos, int?[] usuariosInvitados) {
                servicioPedido.GuardarPedido(pedido, gustos, usuariosInvitados);           
            return View();
        }
        public ActionResult listadoPedidos() {
            var pedidoUsuario = contexto.Pedido.Join
                               (contexto.Usuario, pedido => pedido.IdUsuarioResponsable,
                                 usuario => usuario.IdUsuario, (pedido, usuario) => new { pedido })
                                 .OrderByDescending(pedido => pedido.pedido.FechaCreacion)
                                 .ToList().Where(ped => ped.pedido.IdUsuarioResponsable == 1);
            var Pedidos = contexto.Pedido.
                OrderByDescending(pedido => pedido.FechaCreacion).ToList()
                .Where(pedido => pedido.IdUsuarioResponsable == 1);

            ViewBag.Pedidos = Pedidos;
            ViewBag.pedidoUsuario = pedidoUsuario;

           // servicioPedido.listadoPedidosAsociadosUsuario();
            return View();
        }
    }
}
