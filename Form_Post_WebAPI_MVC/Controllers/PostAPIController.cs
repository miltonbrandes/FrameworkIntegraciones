using Form_Post_WebAPI_MVC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Form_Post_WebAPI_MVC.Controllers
{
    public class PostAPIController : ApiController
    {
        [HttpPost]
        [Route("api/PostAPI/SaveCase")]
        public HttpResponseMessage SaveCase(CaseModel caso)
        {
            string nombreCaso = caso.CaseName;
            string comercial = caso.Comercial;
            string tipoPagos = caso.Pagos;
            string integrador = caso.Integrador;

            var client = new HttpClient();
            //Creacion Card
            try
            {
                dynamic body = new System.Dynamic.ExpandoObject();
                var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                string description = "Comercial:" + comercial + "---" + "Integrador:" + integrador;
                string uri = "https://api.trello.com/1/cards?idMembers=59c2f251c929d5e77a2f37da&name=" + nombreCaso + "&desc=" + description + "&idList=5b9a6ceeb8076b37908695d4&keepFromSource=all&key=a9f027bba71c5ed49727bf65b25b6b93&token=47282984b2d62a89240b9742f94a5d9a050d3ad1d1b42a1db3a397a9ad85cc0d";
                Task.Factory.StartNew(async () =>
                {
                    HttpResponseMessage response = await client.PostAsync(uri, content);
                    string respuestaDe = await response.Content.ReadAsStringAsync();
                    dynamic respuesta = JsonConvert.DeserializeObject(respuestaDe);
                    string id = respuesta.id;
                    string uriChecklist = "https://api.trello.com/1/cards/" + id + "/checklists?key=a9f027bba71c5ed49727bf65b25b6b93&token=47282984b2d62a89240b9742f94a5d9a050d3ad1d1b42a1db3a397a9ad85cc0d";
                    HttpResponseMessage responseChecklist = await client.PostAsync(uriChecklist, content);
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
