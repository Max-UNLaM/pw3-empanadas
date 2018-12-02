using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TresEmpanadas.Models.Metadata
{
    public class PedidoMetadata
    {

        [Required(ErrorMessage = "Nombre requerido")]
        [StringLength(200, ErrorMessage = "Debe tener como máximo 200 caracteres")]
        public string NombreNegocio { get; set; }
           
        [Required(ErrorMessage = "Precio unitario requerido")]
        public int PrecioUnidad { get; set; }

        [Required(ErrorMessage = "Precio por docena es requerido")]
        public int PrecioDocena { get; set; }

        [Required(ErrorMessage = "Debe Elegir un gusto de empanada")]
        [DataType(DataType.Currency)]
        public ICollection<InvitacionPedido> GustoEmpanada { get; set; }

        [Required(ErrorMessage = "Debe Elegir un gusto de empanada")]
        [DataType(DataType.Currency)]
        public virtual ICollection<InvitacionPedido> InvitacionPedido { get; set; }
    }
}