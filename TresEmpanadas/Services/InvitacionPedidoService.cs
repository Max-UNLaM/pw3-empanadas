using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TresEmpanadas.Api.Models;

namespace TresEmpanadas.Services
{
    public class InvitacionPedidoService
    {
        Entities Entities = new Entities();

        public InvitacionPedido GetInvitacionPedido(int idUsuario, int idPedido)
        {
            return Entities.InvitacionPedido.First(
                (inv) =>
                    inv.IdPedido == idPedido && inv.IdUsuario == idUsuario
                );
        }

        internal void AddGustoEmpanada()
        {

        }
    }
}