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

        public ElegirGusto BuildElegirPedido(int idPedido, int idUsuario)
        {
            var pedidoService = new PedidoService();
            var gustoService = new GustoService();
            var invitacionPedidoService = new InvitacionPedidoService();
            Pedido pedido = pedidoService.BuscarPedidoPorId(idPedido);
            var gustosDelPedido = gustoService.GustosPedidos(pedido);
            int cantidadEmpa;
            try
            { 
                cantidadEmpa = pedidoService.CantidadEmpanadas(pedido.IdPedido);
            }
            catch
            {
                cantidadEmpa = 0;
            }
            int precioTotal = cantidadEmpa * pedido.PrecioUnidad;
            return new ElegirGusto {
                GustoEmpanadas = GustosEmpanadasDelPedido(idPedido),
                GustosPedidos = gustoService.GustosPedidos(pedido),
                Pedido = pedido,
                CantidadEmpanadas = cantidadEmpa,
                PrecioTotal = precioTotal,
                Token = invitacionPedidoService.GetInvitacionPedido(idUsuario, pedido.IdPedido).Token
            };
        }

        internal List<GustoEmpanada> GustosEmpanadasDelPedido (int idPedido)
        {
            var pedido = Entities.Pedido.Find(idPedido);
            return pedido.GustoEmpanada.ToList();
        }
    }
}