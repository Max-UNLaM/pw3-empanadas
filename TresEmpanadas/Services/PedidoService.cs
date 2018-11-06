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

        public void GuardarPedido(Pedido pedido) {
            var valor = System.Web.HttpContext.Current.Session["IdUsuario"] as int?;
            pedido.IdUsuarioResponsable = (int)valor;
            pedido.IdEstadoPedido = 1;
            if (pedido.IdUsuarioResponsable==123)
            {
              
            }
            contexto.Pedido.Add(pedido);
            contexto.SaveChanges();
        }
    }
}