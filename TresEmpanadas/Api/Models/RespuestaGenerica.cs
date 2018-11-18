using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TresEmpanadas.Api.Models
{
    public class RespuestaGenerica
    {
        public String Resultado { get; set; }
        public String Mensaje { get; set; }
        public RespuestaGenerica(){}
        public RespuestaGenerica(String resultado, String mensaje)
        {
            Resultado = resultado;
            Mensaje = mensaje;
        }
    }
}