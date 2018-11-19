using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using TresEmpanadas.Api.Models;
using TresEmpanadas.Services;

namespace TresEmpanadas.Api.Pedidos
{
    public class PedidosController : ApiController
    {
        // GET: api/Pedidos
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Pedidos/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Pedidos
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Pedidos/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Pedidos/5
        public void Delete(int id)
        {
        }

        [Route("api/Pedidos/ConfirmarPedido")]
        [ResponseType(typeof(RespuestaGenerica))]
        [HttpPost]
        public HttpResponseMessage ConfirmarPedido([FromBody]ConfirmarPedido confirmarPedido)
        {
            var pedidoService = new PedidoService();
            Boolean result;
            try
            {
                result = pedidoService.ConfirmarPedido(confirmarPedido);
            }
            catch (InvalidOperationException e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new RespuestaGenerica("No encontrado", e.Message));
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
            if (result == true)
            {
                return Request.CreateResponse(HttpStatusCode.Created, new RespuestaGenerica("Creado", "Pedido Confirmado"));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new RespuestaGenerica("No encontrado", "Pedido no encontrado"));
            }

        }

    }
}
