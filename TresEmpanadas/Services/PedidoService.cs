using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TresEmpanadas.Services
{
    public class PedidoService
    {
        Entities contexto = new Entities();
        public List<EstadoPedido> ListarEstadosPedidos() {
            var estados = contexto.EstadoPedido.ToList();
            return estados;
        }
        public List<GustoEmpanada> ListarGustosEmpanadas() {
            var gustosEmpanadas = contexto.GustoEmpanada.ToList();
            return gustosEmpanadas;
        }

        public void GuardarPedido(Pedido pedido , int?[] gustos , int?[] usuariosInvitados) {
            var valor = HttpContext.Current.Session["IdUsuario"] as int?;
            pedido.IdUsuarioResponsable = (int)valor;
            pedido.IdEstadoPedido = 1;
            contexto.Pedido.Add(pedido);
            contexto.SaveChanges();
            foreach (var item in usuariosInvitados) {
                InvitacionPedido invitacion = new InvitacionPedido();
                var guid = Guid.NewGuid();
                invitacion.IdPedido = pedido.IdPedido;
                invitacion.IdUsuario = (int)item;
                invitacion.Token = guid;
                invitacion.Completado = true;
                contexto.InvitacionPedido.Add(invitacion);
                contexto.SaveChanges();
            }
            
            int idGenerado = pedido.IdPedido;
        }

        public void listadoPedidosAsociadosUsuario() {
            var pedidoUsuario = contexto.Pedido.Join
                                (contexto.Usuario, pedido => pedido.IdUsuarioResponsable,
                                  usuario => usuario.IdUsuario, (pedido, usuario) => new { pedido })
                                  .OrderByDescending(pedido => pedido.pedido.FechaCreacion)
                                  .ToList().Where(ped => ped.pedido.IdUsuarioResponsable  == 1);
            var Pedidos = contexto.Pedido.
                OrderByDescending(pedido => pedido.FechaCreacion).ToList()
                .Where(pedido => pedido.IdUsuarioResponsable ==1);
        }
    }
}