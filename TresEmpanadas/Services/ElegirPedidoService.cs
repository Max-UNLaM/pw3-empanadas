using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Web;
using TresEmpanadas.Models.ViewModels;

namespace TresEmpanadas.Services
{
    public class ElegirPedidoService
    {
        public Entities Entities = new Entities();

        public ElegirGusto BuildElegirGusto(int idPedido, int idUsuario)
        {
            var pedidoService = new PedidoService();
            Pedido pedido = pedidoService.BuscarPedidoPorId(idPedido);
            return BuildElegirGusto(pedido, idUsuario);
        }

        public ElegirGusto BuildElegirGusto(Guid token, int idUsuario)
        {
            var pedidoService = new PedidoService();
            Pedido pedido = pedidoService.BuscarPedido(token);
            return BuildElegirGusto(pedido, idUsuario);
        }

        internal ElegirGusto BuildElegirGusto(Pedido pedido, int idUsuario)
        {
            var gustoService = new GustoService();
            var invitacionPedidoService = new InvitacionPedidoService();
            var pedidoService = new PedidoService();
            ValidateToken(invitacionPedidoService.GetInvitacionPedido(idUsuario, pedido.IdPedido), idUsuario);
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
            return new ElegirGusto
            {
                GustoEmpanadas = GustosEmpanadasDelPedido(pedido.IdPedido),
                GustosPedidos = gustoService.GustosPedidos(pedido),
                Pedido = pedido,
                CantidadEmpanadas = cantidadEmpa,
                PrecioTotal = precioTotal,
                IdUsuario = idUsuario,
                Token = invitacionPedidoService.GetInvitacionPedido(idUsuario, pedido.IdPedido).Token
            };
        }

        internal void ValidateToken(InvitacionPedido invitacionPedido, int idUsuario)
        {
            if (invitacionPedido.IdUsuario != idUsuario)
            {
                throw new AuthenticationException("Token inválido");
            }
        }


        internal List<GustoEmpanada> GustosEmpanadasDelPedido (int idPedido)
        {
            var pedido = Entities.Pedido.Find(idPedido);
            return pedido.GustoEmpanada.ToList();
        }
    }
}