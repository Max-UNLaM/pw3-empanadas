using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TresEmpanadas.Models;

namespace TresEmpanadas.Services
{
    public class GustoService
    {
        Entities Contexto = new Entities();
        public List<GustoPedido> GustosPedidos(Pedido pedido)
        {
            var gustosDelPedido = Contexto.InvitacionPedidoGustoEmpanadaUsuario.Where(iv => iv.IdPedido == pedido.IdPedido).ToList();
            var gustosPedidos = new List<GustoPedido>();
            foreach (var gusto in gustosDelPedido)
            {
                gustosPedidos.Add(new GustoPedido
                {
                    NombreGustoEmpanada = GetGustoEmpanada(gusto.IdGustoEmpanada).Nombre,
                    NombreUsuario = NombreUsuario(gusto.IdUsuario),
                    Cantidad = gusto.Cantidad
                });
            }
            return gustosPedidos;
        }

        public GustoEmpanada GetGustoEmpanada(int id)
        {
            return Contexto.GustoEmpanada.Find(id);
        }


        internal String NombreUsuario(int idUsuario)
        {
            return Contexto.Usuario.Find(idUsuario).Email;
        }
    }
}