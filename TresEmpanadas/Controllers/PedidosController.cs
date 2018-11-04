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

        public ActionResult IniciarPedido()
        {
            //ViewBag.estadosPedido = servicioPedido.ListarEstadosPedidos();
            ViewBag.gustosEmpanadas = servicioPedido.ListarGustosEmpanadas();
            return View();
        }
        [HttpPost]
        public ActionResult GuardarPedido(Pedido pedido) {
            servicioPedido.GuardarPedido(pedido);
            return View();
        }

    }
}
