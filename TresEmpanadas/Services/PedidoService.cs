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
                                     //Listado de Estado de los pedidos
        public List<EstadoPedido> ListarEstadosPedidos() {
            var estados = contexto.EstadoPedido.ToList();
            return estados;
        }
                                          //Listado de gustos de empanadas
        public List<GustoEmpanada> ListarGustosEmpanadas() {
            var gustosEmpanadas = contexto.GustoEmpanada.ToList();
            return gustosEmpanadas;
        }
                                                // Guardar Pedido
        public void GuardarPedido(Pedido pedido, int?[] gustos, int?[] usuariosInvitados) {
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

                         // Listado de pedidos que estan asociados a un usuario
        public List<Pedido> ListadoPedidosAsociadosUsuario() {
            //Ejemplo Linq Join 
            //var pedidoUsuario = contexto.Pedido.Join
            //                    (contexto.InvitacionPedido, pedido => pedido.IdPedido,
            //                      invitacion => invitacion.IdPedido, (pedido, invitacion) => new { pedido })
            //                     .OrderByDescending(pedido => pedido.pedido.FechaCreacion)
            //                     .Where(ped => ped.pedido.IdUsuarioResponsable  == 1).ToList();


            List<Pedido> pedidosResultado = new List<Pedido>();

            //PEDIDOS DONDE ES RESPONSABLE
            int idUsuarioLogueado = (Convert.ToInt32(System.Web.HttpContext.Current.Session["IdUsuario"])) ;
            var pedidosResponsable = contexto.Pedido
              .Where(pedido => pedido.IdUsuarioResponsable == idUsuarioLogueado)
              .OrderByDescending(pedido => pedido.FechaCreacion)
              .ToList();

            //inserto en mi lista resultado
            pedidosResultado.AddRange(pedidosResponsable);

            //PEDIDOS DONDE ES INVITADO
            var invitacionesUsuario = contexto.InvitacionPedido
            .Where(inv => inv.IdUsuario == idUsuarioLogueado).ToList();

            foreach (var inv in invitacionesUsuario)
            {
                Boolean agregarPedido = true;
                foreach (var pedResponsable in pedidosResponsable) {
                    if (inv.IdPedido == pedResponsable.IdPedido) {
                         agregarPedido = false; 
                    }
                }
                if (agregarPedido) {
                    pedidosResultado.Add(inv.Pedido);
                }
                
            }
                            ////Si quiero ordenar la lista completa

            //List<Pedido> listaordenada = new List<Pedido>();
            //var listaOrdenada = from o in pedidosResultado
            //                    orderby o.FechaCreacion descending
            //                    select o;
            //pedidosResultado = listaOrdenada.ToList<Pedido>();

            return pedidosResultado;
        }

        public Pedido BuscarPedidoPorId(int idPedido) {
            var pedidoDetalle = contexto.Pedido.Find(idPedido);
            return pedidoDetalle;
        }

    }
}