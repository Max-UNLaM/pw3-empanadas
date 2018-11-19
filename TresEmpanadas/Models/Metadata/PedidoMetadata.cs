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

        [Required(ErrorMessage = "Descripcion requerida")]
        [StringLength(1024, ErrorMessage = "Debe tener como máximo 1024 caracteres")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Precio unitario requerido")]
        [DataType(DataType.Currency)]
        public int PrecioUnidad { get; set; }

        [Required(ErrorMessage = "Precio por docena es requerido")]
        [DataType(DataType.Currency)]
        public int PrecioDocena { get; set; }

             
    }
}