using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TresEmpanadas.Models.Metadata;

namespace TresEmpanadas
{
    [MetadataType(typeof(PedidoMetadata))]
    public partial class Pedido
    {
        public Pedido(Pedido pedido)
        {
            this.Descripcion = pedido.Descripcion;
            this.FechaCreacion =  DateTime.Now;          
            this.NombreNegocio = pedido.NombreNegocio;
            this.PrecioDocena = pedido.PrecioDocena;
            this.PrecioUnidad = pedido.PrecioUnidad;
        }
    }
}