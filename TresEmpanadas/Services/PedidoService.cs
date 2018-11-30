using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using TresEmpanadas.Api.Models;
using TresEmpanadas.Models.ViewModels;

namespace TresEmpanadas.Services
{
    public class
        PedidoService
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
        public MultiSelectList ListarGustos()
        {
            var listadoGustos = new MultiSelectList(Contexto.GustoEmpanada, "IdGustoEmpanada", "Nombre");
            return listadoGustos;
        }

        // Guardar Pedido
        public int GuardarPedido(Pedido pedido, int?[] gustos, string[] usuariosInvitados)
        {
            var valor = HttpContext.Current.Session["IdUsuario"] as int?;
            pedido.IdUsuarioResponsable = (int)valor;
            pedido.IdEstadoPedido = 1;
            pedido.FechaCreacion = DateTime.Now;
            foreach (var item in gustos)
            {
                GustoEmpanada gustoEmpanada = Contexto.GustoEmpanada.Find(item);
                pedido.GustoEmpanada.Add(gustoEmpanada);
            }
            Contexto.Pedido.Add(pedido);
            Contexto.SaveChanges();
            foreach (var invitados in usuariosInvitados)
            {
                Boolean crearUsuario = true;
                foreach (var usuario in Contexto.Usuario)
                {
                    if (usuario.Email.Equals(invitados))
                    {
                        crearUsuario = false;
                    }
                }
                if (crearUsuario)
                {
                    Usuario usuarioCrear = new Usuario();
                    usuarioCrear.Email = invitados;
                    usuarioCrear.Password = "test1234";
                    Contexto.Usuario.Add(usuarioCrear);
                    Contexto.SaveChanges();
                }
            }
            // Guardo una invitacion para el usuario responsable
            InvitacionPedido miInvitacion = new InvitacionPedido();
            var guid1 = Guid.NewGuid();
            miInvitacion.IdPedido = pedido.IdPedido;
            miInvitacion.IdUsuario = (int)valor;
            miInvitacion.Token = guid1;
            miInvitacion.Completado = true;
            Contexto.InvitacionPedido.Add(miInvitacion);
            Contexto.SaveChanges();
            foreach (var item in usuariosInvitados)
            {
                InvitacionPedido invitacion = new InvitacionPedido();
                var guid = Guid.NewGuid();
                invitacion.IdPedido = pedido.IdPedido;
                var usu = Contexto.Usuario.Where(emailUsu => emailUsu.Email.Equals(item)).First();
                invitacion.IdUsuario = usu.IdUsuario;
                invitacion.Token = guid;
                invitacion.Completado = true;
                //this.enviarEmail(invitacion, usu, null);
                Contexto.InvitacionPedido.Add(invitacion);
                Contexto.SaveChanges();
            }

            var responsable = Contexto.InvitacionPedido.Where(i => i.IdUsuario == valor && i.IdPedido == pedido.IdPedido).Select(i => i.Token).FirstOrDefault();

            //this.enviarEmail(null, null, responsable);

            int idGenerado = pedido.IdPedido;
            return idGenerado;
        }
        // Confirma terminar Pedio
        public void CerrarPedido(int idPedido)
        {
            var pedidoBuscado = BuscarPedidoPorId(idPedido);
            pedidoBuscado.IdEstadoPedido = 2;
            Contexto.SaveChanges();
        }

        public void EditarPedido(Pedido pedido, int?[] gustos, string[] usuariosInvitados)
        { 
            var pedidoBuscado = BuscarPedidoPorId(pedido.IdPedido);
            pedidoBuscado.GustoEmpanada.Clear();
            foreach (var item in gustos)
            {
                GustoEmpanada gustoEmpanada = Contexto.GustoEmpanada.Find(item);
                pedidoBuscado.GustoEmpanada.Add(gustoEmpanada);
            }
            pedidoBuscado.NombreNegocio = pedido.NombreNegocio;
            pedidoBuscado.PrecioDocena = pedido.PrecioDocena;
            pedidoBuscado.PrecioUnidad = pedido.PrecioUnidad;
            //pedidoBuscado.FechaCreacion = pedido.FechaCreacion;
            pedidoBuscado.FechaModificacion = DateTime.Now;
            pedidoBuscado.Descripcion = pedido.Descripcion;
            Contexto.SaveChanges();
            if (usuariosInvitados != null)
            {
                foreach (var invitados in usuariosInvitados)
                {
                    Boolean crearUsuario = true;
                    foreach (var usuario in Contexto.Usuario)
                    {
                        if (usuario.Email.Equals(invitados))
                        {
                            crearUsuario = false;
                        }
                    }
                    if (crearUsuario)
                    {
                        Usuario usuarioCrear = new Usuario();
                        usuarioCrear.Email = invitados;
                        usuarioCrear.Password = "test1234";
                        Contexto.Usuario.Add(usuarioCrear);
                        Contexto.SaveChanges();
                    }
                }
            }
            // Lista de invitaciones del pedido a editar
            var listaInvitacionesPedido = Contexto.InvitacionPedido.Where(ped => ped.IdPedido == pedidoBuscado.IdPedido).ToList();

            //Debe crear invitacion solo si la invitacion no existe 
            if (usuariosInvitados != null)
            {
                foreach (var item in usuariosInvitados)
                {
                    Boolean crearInvitacion = true;
                    var usu = Contexto.Usuario.Where(emailUsu => emailUsu.Email.Equals(item)).First();
                    foreach (var usuarioInvitacion in listaInvitacionesPedido)
                    {
                        if (usu.IdUsuario == usuarioInvitacion.IdUsuario)
                        {
                            crearInvitacion = false;
                            break;
                        }
                    }
                    if (crearInvitacion)
                    {
                        InvitacionPedido invitacion = new InvitacionPedido();
                        var guid = Guid.NewGuid();
                        invitacion.IdUsuario = usu.IdUsuario;
                        invitacion.IdPedido = pedido.IdPedido;
                        invitacion.Token = guid;
                        invitacion.Completado = true;
                        Contexto.InvitacionPedido.Add(invitacion);
                        Contexto.SaveChanges();
                    }
                }
            }

        }

        public void enviarEmail(InvitacionPedido inv, Usuario usu, Guid? responsable)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("TresEmpanadas@gmail.com");

            if (responsable != null)
                mail.To.Add((string)HttpContext.Current.Session["email"]);
            else
                mail.To.Add(usu.Email);


            mail.Subject = "Pedido en TresEmpanadas";
            var link = "";

            if (responsable != null)
                link = HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + "/Pedidos/elegir/" + responsable;
            else
                link = HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + "/Pedidos/elegir/" + inv.Token;

            mail.Body = "<a href=" + link + ">" + link + "</a>";
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("tresempanadaspw3@gmail.com", "pruebapw3");
            smtp.EnableSsl = true;
            smtp.Send(mail);

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

        public int CantidadEmpanadas(int id)
        {
            var pedidos = Contexto.InvitacionPedidoGustoEmpanadaUsuario.Where(
                inv => inv.IdPedido == id);
            return pedidos.Sum(p => p.Cantidad);
        }
        public string[] CargarOpciones()
        {
            string[] opciones = { "A Nadie", "Re-enviar Invitación a Todos", "Enviar sólo a los Nuevos", "Re - enviar sólo a los que no eligieron gustos" };
            return opciones;
        }

        public List<UsuariosInvitados> UsuariosInvitados(int pedidoId)
        {
            var invitaciones = Contexto.InvitacionPedido.Where(inv => inv.IdPedido == pedidoId).ToList();
            var usuarios = new List<Usuario>();
            var usuariosInvitados = new List<UsuariosInvitados>();
            foreach (var inv in invitaciones)
            {
                usuarios.Add(Contexto.Usuario.Single(us => us.IdUsuario == inv.IdUsuario));
            }
            foreach (var usr in usuarios)
            {
                var invPedido = Contexto.InvitacionPedido.First(inv => inv.IdUsuario == usr.IdUsuario && inv.IdPedido == pedidoId);
                usuariosInvitados.Add(new UsuariosInvitados
                {
                    Email = usr.Email,
                    Estado = invPedido.Completado == false ? "NO" : "SI"
                });
            }
            return usuariosInvitados;
        }




    }
}