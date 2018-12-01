using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TresEmpanadas.Models.ViewModels
{
    public class ElegirGusto
    {
        public List<GustoEmpanada> GustoEmpanadas { get; set; }
        public List<GustoPedido> GustosPedidos { get; set; }
        public Pedido Pedido { get; set; }
        public InvitacionPedido InvitacionPedido { get; set; }
        public int CantidadEmpanadas { get; set; }
        public float PrecioTotal { get; set; }
        public Guid Token { get; set; }
        public int IdUsuario { get; set; }
    }
}