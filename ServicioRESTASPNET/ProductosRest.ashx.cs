using Dal;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.IO;

namespace ServicioRESTASPNET
{
    /// <summary>
    /// Descripción breve de ProductosRest
    /// </summary>
    public class ProductosRest : IHttpHandler
    {
        private static readonly IDao<Producto> dao = DaoSqlServerProducto.ObtenerDao();


        public void ProcessRequest(HttpContext context)
        {
            HttpRequest req = context.Request;
            HttpResponse res = context.Response;

            string metodo = req.RequestType;
            string argumento = req.PathInfo.Replace("/", "");
            long.TryParse(argumento, out long id);

            context.Response.ContentType = "application/json";

            string json = null;

            Producto producto;

            switch (metodo)
            {
                case "GET":
                    if (argumento == "")
                    {
                        json = JsonConvert.SerializeObject(dao.ObtenerTodos());
                    }
                    else
                    {
                        json = JsonConvert.SerializeObject(dao.ObtenerPorId(id));
                    }
                    break;
                case "POST":
                    producto = JsonConvert.DeserializeObject<Producto>(new StreamReader(req.InputStream).ReadToEnd());
                    producto = dao.Insertar(producto);
                    json = JsonConvert.SerializeObject(producto);

                    res.StatusCode = 201;
                    break;
                case "PUT":
                    producto = JsonConvert.DeserializeObject<Producto>(new StreamReader(req.InputStream).ReadToEnd());
                    json = JsonConvert.SerializeObject(producto);

                    dao.Modificar(producto);
                    break;
                case "DELETE":
                    dao.Borrar(id);
                    res.StatusCode = 204;
                    break;
            }

            res.Write(json);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}