using Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public class DaoSqlServerProducto : IDao<Producto>
    {
        #region Singleton
        private DaoSqlServerProducto() { }

        private const string CONNECTION_STRING = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=mf0966;Integrated Security=True";
        private static DaoSqlServerProducto dao = new DaoSqlServerProducto();
        public static DaoSqlServerProducto ObtenerDao()
        {
            return dao;
        }
        #endregion

        private IDbConnection ObtenerConexion()
        {
            IDbConnection con = new SqlConnection(CONNECTION_STRING);
            con.Open();
            return con;
        }
        
        public void Borrar(long id)
        {
            using (IDbConnection con = ObtenerConexion())
            {
                IDbCommand com = con.CreateCommand();

                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "ProductosBorrar";

                IDbDataParameter parId = com.CreateParameter();
                parId.ParameterName = "Id";
                parId.DbType = DbType.Int64;
                parId.Value = id;

                com.Parameters.Add(parId);

                com.ExecuteNonQuery();
            }
        }

        public Producto Insertar(Producto producto)
        {
            using (IDbConnection con = ObtenerConexion())
            {
                IDbCommand com = con.CreateCommand();

                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "ProductosInsertar";

                IDbDataParameter parNombre = com.CreateParameter();
                parNombre.ParameterName = "Nombre";
                parNombre.DbType = DbType.String;
                parNombre.Value = producto.Nombre;
                com.Parameters.Add(parNombre);

                IDbDataParameter parPrecio = com.CreateParameter();
                parPrecio.ParameterName = "Precio";
                parPrecio.DbType = DbType.Decimal;
                parPrecio.Value = producto.Precio;
                com.Parameters.Add(parPrecio);

                IDbDataParameter parCategoriaId = com.CreateParameter();
                parCategoriaId.ParameterName = "CategoriaId";
                parCategoriaId.DbType = DbType.Int64;
                parCategoriaId.Value = producto.Categoria.Id;
                com.Parameters.Add(parCategoriaId);

                producto.Id = (long?)com.ExecuteScalar();

                return producto;
            }
        }

        public Producto Modificar(Producto producto)
        {
            using (IDbConnection con = ObtenerConexion())
            {
                IDbCommand com = con.CreateCommand();

                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "ProductosModificar";

                IDbDataParameter parId = com.CreateParameter();
                parId.ParameterName = "Id";
                parId.DbType = DbType.Int64;
                parId.Value = producto.Id;
                com.Parameters.Add(parId);

                IDbDataParameter parNombre = com.CreateParameter();
                parNombre.ParameterName = "Nombre";
                parNombre.DbType = DbType.String;
                parNombre.Value = producto.Nombre;
                com.Parameters.Add(parNombre);

                IDbDataParameter parPrecio = com.CreateParameter();
                parPrecio.ParameterName = "Precio";
                parPrecio.DbType = DbType.Decimal;
                parPrecio.Value = producto.Precio;
                com.Parameters.Add(parPrecio);

                IDbDataParameter parCategoriaId = com.CreateParameter();
                parCategoriaId.ParameterName = "CategoriaId";
                parCategoriaId.DbType = DbType.Int64;
                parCategoriaId.Value = producto.Categoria.Id;
                com.Parameters.Add(parCategoriaId);
                
                com.ExecuteNonQuery();

                return producto;
            }
        }

        public Producto ObtenerPorId(long id)
        {
            using (IDbConnection con = ObtenerConexion())
            {
                IDbCommand com = con.CreateCommand();

                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "ProductosObtenerPorId";

                IDbDataParameter parId = com.CreateParameter();
                parId.ParameterName = "Id";
                parId.DbType = DbType.Int64;
                parId.Value = id;

                com.Parameters.Add(parId);

                IDataReader dr = com.ExecuteReader();

                Producto producto = null;
                Categoria categoria = null;

                if (dr.Read())
                {
                    categoria = new Categoria() { Id = (long?)dr["cId"], Nombre = (string)dr["cNombre"] };
                    producto = new Producto() { Id = (long?)dr["Id"], Nombre = (string)dr["Nombre"], Precio = (decimal)dr["Precio"], Categoria = categoria };
                }

                return producto;
            }
        }

        public IEnumerable<Producto> ObtenerTodos()
        {
            using(IDbConnection con = ObtenerConexion())
            {
                IDbCommand com = con.CreateCommand();

                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "ProductosObtenerTodos";

                IDataReader dr = com.ExecuteReader();

                List<Producto> productos = new List<Producto>();

                while(dr.Read())
                {
                    productos.Add(new Producto() { Id = (long?)dr["Id"], Nombre = (string)dr["Nombre"], Precio = (decimal)dr["Precio"] });
                }

                return productos;
            }
        }
    }
}
