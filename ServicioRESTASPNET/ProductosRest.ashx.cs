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
                        producto = dao.ObtenerPorId(id);

                        if (producto == null)
                        {
                            res.StatusCode = 404;
                            return;
                        }

                        json = JsonConvert.SerializeObject(producto);
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

                    try
                    {
                        dao.Modificar(producto);
                    }
                    catch
                    {
                        res.StatusCode = 404;
                        return;
                    }
                    break;
                case "DELETE":
                    try
                    {
                        dao.Borrar(id);
                    }
                    catch
                    {
                        res.StatusCode = 404;
                        return;
                    }
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