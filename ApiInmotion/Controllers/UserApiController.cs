using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace ApiInmotion.Controllers
{
    public class UserApiController : ApiController
    {
        private InmotionEntities _inmotionContext = new InmotionEntities();

        public HttpResponseMessage Get()
        {
            List<Person> data = _inmotionContext.Person.ToList();
            return new HttpResponseMessage()
            {
                //Content = new StringContent("[{'id':1,'name':'Ulises','address':'Puruandiro','rol':1,'gender':1}]")
                Content = new JsonContent(data)
            };
        }

        public HttpResponseMessage Post(Person user)
        {
            //user.Name = "Ulises";
            //user.Address = "Puru";
            //user.Rol = 1;
            //user.Gender = 1;
            _inmotionContext = new InmotionEntities();
            int idU = _inmotionContext.Person.Count();
            user.Id = idU + 1;
            _inmotionContext.Person.Add(user);
            int savedRecords = _inmotionContext.SaveChanges();
            var result = new { savedRecords = savedRecords };
            List<Person> data = _inmotionContext.Person.ToList();
            return new HttpResponseMessage()
            {
                //Content = new StringContent("[{'id':1,'name':'Ulises','address':'Puruandiro','rol':1,'gender':1}]")
                Content = new JsonContent(data)
            };
        }
    }

    public class JsonContent : HttpContent
    {

        private readonly MemoryStream _Stream = new MemoryStream();
        public JsonContent(List<Person> value)
        {

            Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var jw = new JsonTextWriter(new StreamWriter(_Stream));
            jw.Formatting = Formatting.Indented;
            var serializer = new JsonSerializer();
            serializer.Serialize(jw, value);
            jw.Flush();
            _Stream.Position = 0;

        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            return _Stream.CopyToAsync(stream);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = _Stream.Length;
            return true;
        }
    }
}
