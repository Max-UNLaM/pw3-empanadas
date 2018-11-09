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
        Entities contexto = new Entities();

        // Iniciar Pedido
        public ActionResult IniciarPedido()
        {
            //ViewBag.estadosPedido = servicioPedido.ListarEstadosPedidos();
            ViewBag.gustosEmpanadas = servicioPedido.ListarGustosEmpanadas();
            ViewBag.usuariosDisponibles = servicioUsuario.ListarUsuarios();
            System.Web.HttpContext.Current.Session["IdUsuario"] = 1;
            return View();
        } 

        //Guardar Pedido
        [HttpPost]
        public ActionResult GuardarPedido(Pedido pedido, int?[] gustos, int?[] usuariosInvitados) {
                servicioPedido.GuardarPedido(pedido, gustos, usuariosInvitados);           
            return View();
        }

        //Listado Pedidos
        public ActionResult ListadoPedidos() {
            var listadoPedidos = servicioPedido.listadoPedidosAsociadosUsuario();
            ViewBag.pedidosUsuario = listadoPedidos;
            return View();
        }

        //Detalle Pedidos
        public ActionResult DetallePedido(int idPedido) {
            var detallePedido = servicioPedido.BuscarPedidoPorId(idPedido);
            ViewBag.detallePedido = detallePedido; 
            return View(detallePedido);
        }


    }
}
