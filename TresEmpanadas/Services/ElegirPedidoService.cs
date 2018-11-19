﻿using System;
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
            var gustoService = new GustoService();
            var invitacionPedidoService = new InvitacionPedidoService();
            Pedido pedido = pedidoService.BuscarPedidoPorId(idPedido);
            var gustosDelPedido = gustoService.GustosPedidos(pedido);
            int cantidadEmpa = pedidoService.CantidadEmpanadas(pedido.IdPedido);
            int precioTotal = cantidadEmpa * pedido.PrecioUnidad;
            return new ElegirGusto {
                GustoEmpanadas = Entities.GustoEmpanada.ToList(),
                GustosPedidos = gustoService.GustosPedidos(pedido),
                Pedido = pedido,
                CantidadEmpanadas = cantidadEmpa,
                PrecioTotal = precioTotal,
                Token = invitacionPedidoService.GetInvitacionPedido(1, pedido.IdPedido).Token
            };
        }
    }
}