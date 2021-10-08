using System.Collections.Generic;

namespace Entidades
{
    public class Categoria
    {
        public long? Id { get; set; }
        public string Nombre { get; set; }
        public IEnumerable<Producto> Productos { get; set; }
    }
}