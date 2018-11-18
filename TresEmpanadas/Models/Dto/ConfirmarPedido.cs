using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TresEmpanadas.Api.Dto;

namespace TresEmpanadas.Api.Models
{
    public class ConfirmarPedido
    {
        public int IdUsuario { get; set; }
        public int TokenInvitacion { get; set; }
        public List<GustosEmpanadaCantidad> GustosEmpanadaCantidades { get; set; }
    }
}