using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TresEmpanadas.Models.ViewModels
{
    public class PedidoGusto
    {
        public Pedido Pedido { get; set; }
        public ICollection<GustoEmpanada> GustoEmpanada { get; set; }
    }
}