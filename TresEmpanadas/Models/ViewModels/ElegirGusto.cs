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
    }
}