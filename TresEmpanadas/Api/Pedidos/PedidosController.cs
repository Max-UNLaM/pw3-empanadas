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

            return Request.CreateResponse(HttpStatusCode.Created, new RespuestaGenerica("lele", "lelelele"));
        }

    }
}
