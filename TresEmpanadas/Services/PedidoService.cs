using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TresEmpanadas.Api.Models;

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
            foreach (var item in gustos) {
                GustoEmpanada gustoEmpanada = contexto.GustoEmpanada.Find(item);
                pedido.GustoEmpanada.Add(gustoEmpanada);       
            }
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

        public Boolean AgregarGustoAInvitacion(InvitacionPedidoGustoEmpanadaUsuario inv)
        {
            var ipgeu = contexto.InvitacionPedidoGustoEmpanadaUsuario;
            Pedido pedido = contexto.Pedido.Single(p => p.IdPedido == inv.IdPedido);
            if (!PedidoAbierto(pedido))
            {
                return false;
            }
            ipgeu.Add(inv);
            return true;
        }

        public Boolean LimpiarGustosPedido(int idPedido)
        {
            var ipgeu = contexto.InvitacionPedidoGustoEmpanadaUsuario;
            Pedido pedido = contexto.Pedido.Single(p => p.IdPedido == idPedido);
            if (!PedidoAbierto(pedido))
            {
                return false;
            }
            List<InvitacionPedidoGustoEmpanadaUsuario> actual = ipgeu.Where(
                invi => invi.IdPedido == pedido.IdPedido).ToList();
            actual.ForEach(invi => ipgeu.Remove(invi));
            return true;
        }

        internal Boolean PedidoAbierto(int idPedido)
        {
            var pedido = contexto.Pedido.Find(idPedido);
            return pedido.IdEstadoPedido == 1 ? true : false;
        }

        internal Boolean PedidoAbierto(Pedido pedido)
        {
            return pedido.IdEstadoPedido == 1 ? true : false;
        }
        public Pedido EditarPedido(int idPedido)
        {
            throw new NotImplementedException();
        }

        public bool EstadoPedido(int idPedido)
        {
            bool estado;
            Pedido pedidoBuscado = BuscarPedidoPorId(idPedido);
            String nombreEstado = pedidoBuscado.EstadoPedido.Nombre;
            if (nombreEstado.Equals("Abierto"))
            {
                estado = true;
            }
            else{
                estado = false;
            }
            
            return estado;
        }

        internal void EliminarPedido(int idPedido)
        {
            // Trae las invitaciones que tiene un pedido
            var listaInvitacionesPedido = contexto.InvitacionPedido.Where(inv => inv.IdPedido == idPedido).ToList();
            foreach (var item in listaInvitacionesPedido) {
                var invitacionEliminar = contexto.InvitacionPedido.Find(item.IdInvitacionPedido);
                if (invitacionEliminar != null)
                {
                    contexto.InvitacionPedido.Remove(invitacionEliminar);
                    contexto.SaveChanges();
                }
            }
            // Trae las invitaciones con gustos de empanada por usuario
            var listaInvitacionGustoEmpanadaPedido = contexto.InvitacionPedidoGustoEmpanadaUsuario
                                                     .Where(invUsuPed => invUsuPed.IdPedido == idPedido).ToList();
            foreach (var item in listaInvitacionGustoEmpanadaPedido)
            {
                var invitacionGustoUsuarioEliminar = contexto.InvitacionPedido.Find(item.IdInvitacionPedidoGustoEmpanadaUsuario);
                if (invitacionGustoUsuarioEliminar != null)
                {
                    contexto.InvitacionPedido.Remove(invitacionGustoUsuarioEliminar);
                    contexto.SaveChanges();
                }
            }
           
            //Traigo el pedido que voy a eliminar
            Pedido pedidoEliminar = contexto.Pedido.Find(idPedido);
            // Borra todos los registros de gustos de empanadas de ese pedido
            pedidoEliminar.GustoEmpanada.Clear();
            //Borra el pedido y guarda los cambios
            contexto.Pedido.Remove(pedidoEliminar);
            contexto.SaveChanges();
            //throw new NotImplementedException();
        }

        public int BuscarInvitacionesConfirmadas(int idPedido) {
            var listaInvitacionGustoEmpanadaPedido = contexto.InvitacionPedido
                                                    .Where(invUsuPed => invUsuPed.IdPedido == idPedido && invUsuPed.Completado == true).Count();
            return listaInvitacionGustoEmpanadaPedido;
        }

        public Pedido BuscarPedidoPorId(int idPedido) {
            var pedidoDetalle = contexto.Pedido.Find(idPedido);
            return pedidoDetalle;
        }

    }
}