using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

        public void GuardarPedido(Pedido pedido, int[] gustos , int[] usuariosInvitados) {
           
        
            foreach (var item in pedido.InvitacionPedido) {
                //invitacion.IdUsuario = item;
            }
            //invitacion.IdUsuario = pedido.InvitacionPedido;
            // Pedido pedido1 = new Pedido();
            var valor = HttpContext.Current.Session["IdUsuario"] as int?;
            pedido.IdUsuarioResponsable = (int)valor;
            pedido.IdEstadoPedido = 1;
            if (pedido.IdUsuarioResponsable==123)
            {
              
            }
            contexto.Pedido.Add(pedido);
            contexto.SaveChanges();
            foreach (var item in usuariosInvitados) {
                InvitacionPedido invitacion = new InvitacionPedido();
                var guid = Guid.NewGuid();
                invitacion.IdPedido = pedido.IdPedido;
                invitacion.IdUsuario = item;
                invitacion.Token = guid;
                invitacion.Completado = true;
                contexto.InvitacionPedido.Add(invitacion);
                contexto.SaveChanges();
            }
            
            int idGenerado = pedido.IdPedido;
        }
    }
}