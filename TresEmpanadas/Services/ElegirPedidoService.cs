using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TresEmpanadas.Models.ViewModels;

namespace TresEmpanadas.Services
{
    public class ElegirPedidoService
    {
        public Entities Entities = new Entities();

        public ElegirGusto BuildElegirPedido(int idPedido)
        {
            var pedidoService = new PedidoService();
            Pedido pedido = pedidoService.BuscarPedidoPorId(idPedido);
            var gustoService = new GustoService();
            var gustosDelPedido = gustoService.GustosPedidos(pedido);
            return new ElegirGusto {
                GustoEmpanadas = Entities.GustoEmpanada.ToList(),
                GustosPedidos = gustoService.GustosPedidos(pedido),
                Pedido = pedido
            };
        }
    }
}