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
        Entities Contexto = new Entities();
        //Listado de Estado de los pedidos
        public List<EstadoPedido> ListarEstadosPedidos()
        {
            var estados = Contexto.EstadoPedido.ToList();
            return estados;
        }
        //Listado de gustos de empanadas
        public List<GustoEmpanada> ListarGustosEmpanadas()
        {
            var gustosEmpanadas = Contexto.GustoEmpanada.ToList();
            return gustosEmpanadas;
        }
        // Guardar Pedido
        public void GuardarPedido(Pedido pedido, int?[] gustos, int?[] usuariosInvitados)
        {
            var valor = HttpContext.Current.Session["IdUsuario"] as int?;
            pedido.IdUsuarioResponsable = (int)valor;
            pedido.IdEstadoPedido = 1;
            foreach (var item in gustos)
            {
                GustoEmpanada gustoEmpanada = Contexto.GustoEmpanada.Find(item);
                pedido.GustoEmpanada.Add(gustoEmpanada);
            }
            Contexto.Pedido.Add(pedido);
            Contexto.SaveChanges();
            foreach (var item in usuariosInvitados)
            {
                InvitacionPedido invitacion = new InvitacionPedido();
                var guid = Guid.NewGuid();
                invitacion.IdPedido = pedido.IdPedido;
                invitacion.IdUsuario = (int)item;
                invitacion.Token = guid;
                invitacion.Completado = true;
                Contexto.InvitacionPedido.Add(invitacion);
                Contexto.SaveChanges();
            }
            int idGenerado = pedido.IdPedido;
        }

        // Listado de pedidos que estan asociados a un usuario
        public List<Pedido> ListadoPedidosAsociadosUsuario()
        {
            //Ejemplo Linq Join 
            //var pedidoUsuario = contexto.Pedido.Join
            //                    (contexto.InvitacionPedido, pedido => pedido.IdPedido,
            //                      invitacion => invitacion.IdPedido, (pedido, invitacion) => new { pedido })
            //                     .OrderByDescending(pedido => pedido.pedido.FechaCreacion)
            //                     .Where(ped => ped.pedido.IdUsuarioResponsable  == 1).ToList();
            List<Pedido> pedidosResultado = new List<Pedido>();
            //PEDIDOS DONDE ES RESPONSABLE
            int idUsuarioLogueado = (Convert.ToInt32(System.Web.HttpContext.Current.Session["IdUsuario"]));
            var pedidosResponsable = Contexto.Pedido
              .Where(pedido => pedido.IdUsuarioResponsable == idUsuarioLogueado)
              .OrderByDescending(pedido => pedido.FechaCreacion)
              .ToList();
            //inserto en mi lista resultado
            pedidosResultado.AddRange(pedidosResponsable);
            //PEDIDOS DONDE ES INVITADO
            var invitacionesUsuario = Contexto.InvitacionPedido
            .Where(inv => inv.IdUsuario == idUsuarioLogueado).ToList();
            foreach (var inv in invitacionesUsuario)
            {
                Boolean agregarPedido = true;
                foreach (var pedResponsable in pedidosResponsable)
                {
                    if (inv.IdPedido == pedResponsable.IdPedido)
                    {
                        agregarPedido = false;
                    }
                }
                if (agregarPedido)
                {
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

        public Boolean ConfirmarPedido(ConfirmarPedido confirmarPedido)
        {
            UsuarioService usuarioService = new UsuarioService();
            var gustoEmpanadaContext = Contexto.GustoEmpanada;
            var invitacionPedido = Contexto.InvitacionPedido.Single(ip => ip.Token == confirmarPedido.TokenInvitacion);
            var pedido = Contexto.Pedido.Single(ped => ped.IdPedido == invitacionPedido.IdPedido);
            if (!PedidoAbierto(pedido))
            {
                return false;
            }
            LimpiarGustosPedido(pedido.IdPedido);
            foreach (var invPedido in confirmarPedido.GustosEmpanadaCantidades)
            {
                AgregarGustoAInvitacion(new InvitacionPedidoGustoEmpanadaUsuario
                {
                    Cantidad = invPedido.Cantidad,
                    IdUsuario = confirmarPedido.IdUsuario,
                    IdGustoEmpanada = invPedido.IdGustoEmpanada,
                    IdPedido = pedido.IdPedido
                });
            }
            return true;
        }

        public Boolean AgregarGustoAInvitacion(InvitacionPedidoGustoEmpanadaUsuario inv)
        {
            var ipgeu = Contexto.InvitacionPedidoGustoEmpanadaUsuario;
            Pedido pedido = Contexto.Pedido.Single(p => p.IdPedido == inv.IdPedido);
            if (!PedidoAbierto(pedido))
            {
                return false;
            }
            ipgeu.Add(inv);
            Contexto.SaveChanges();
            return true;
        }

        public Boolean LimpiarGustosPedido(int idPedido)
        {
            var ipgeu = Contexto.InvitacionPedidoGustoEmpanadaUsuario;
            Pedido pedido = Contexto.Pedido.Single(p => p.IdPedido == idPedido);
            if (!PedidoAbierto(pedido))
            {
                return false;
            }
            List<InvitacionPedidoGustoEmpanadaUsuario> actual = ipgeu.Where(
                invi => invi.IdPedido == pedido.IdPedido).ToList();
            actual.ForEach(invi => ipgeu.Remove(invi));
            Contexto.SaveChanges();
            return true;
        }

        internal Boolean PedidoAbierto(Pedido pedido)
        {
            return pedido.IdEstadoPedido == 1 ? true : false;
        }

        internal Boolean PedidoAbierto(int idPedido)
        {
            var pedido = Contexto.Pedido.Find(idPedido);
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
            else
            {
                estado = false;
            }

            return estado;
        }

        internal void EliminarPedido(int idPedido)
        {
            // Trae las invitaciones que tiene un pedido
            var listaInvitacionesPedido = Contexto.InvitacionPedido.Where(inv => inv.IdPedido == idPedido).ToList();
            foreach (var item in listaInvitacionesPedido)
            {
                InvitacionPedido invitacionEliminar = Contexto.InvitacionPedido.Find(item.IdInvitacionPedido);
                if (invitacionEliminar != null)
                {
                    Contexto.InvitacionPedido.Remove(invitacionEliminar);
                    Contexto.SaveChanges();
                }
            }
            // Trae las invitaciones con gustos de empanada por usuario
            var listaInvitacionGustoEmpanadaPedido = Contexto.InvitacionPedidoGustoEmpanadaUsuario
                                                     .Where(invUsuPed => invUsuPed.IdPedido == idPedido).ToList();
            foreach (var item in listaInvitacionGustoEmpanadaPedido)
            {
                InvitacionPedido invitacionGustoUsuarioEliminar = Contexto.InvitacionPedido.Find(item.IdInvitacionPedidoGustoEmpanadaUsuario);
                if (invitacionGustoUsuarioEliminar != null)
                {
                    Contexto.InvitacionPedido.Remove(invitacionGustoUsuarioEliminar);
                    Contexto.SaveChanges();
                }
            }

            //Traigo el pedido que voy a eliminar
            Pedido pedidoEliminar = Contexto.Pedido.Find(idPedido);
            // Borra todos los registros de gustos de empanadas de ese pedido
            pedidoEliminar.GustoEmpanada.Clear();
            //Borra el pedido y guarda los cambios
            Contexto.Pedido.Remove(pedidoEliminar);
            Contexto.SaveChanges();
            //throw new NotImplementedException();
        }

        public int BuscarInvitacionesConfirmadas(int idPedido)
        {
            var listaInvitacionGustoEmpanadaPedido = Contexto.InvitacionPedido
                                                    .Where(invUsuPed => invUsuPed.IdPedido == idPedido && invUsuPed.Completado == true).Count();
            return listaInvitacionGustoEmpanadaPedido;
        }

        public Pedido BuscarPedidoPorId(int idPedido)
        {
            var pedidoDetalle = Contexto.Pedido.Find(idPedido);
            return pedidoDetalle;
        }

    }
}