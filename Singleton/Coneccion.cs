using Microsoft.Data.SqlClient;

namespace HojadeRuta2K23.Singleton;

public class Coneccion
{
    private static Coneccion instacia;
    private readonly string cadenaDeConeccion;

    private Coneccion()
    {
        cadenaDeConeccion = "Server=(LocalDB)\\MSSQLLocalDB;Database=SIVALLE_DB1;Integrated Security=true; TrustServerCertificate=True";
    }

    public static Coneccion Instance
    {
        get
        {
            if (instacia == null)
            {
                instacia = new Coneccion();
            }

            return instacia;
        }
    }

    public SqlConnection GetConnection()
    {
        return new SqlConnection(cadenaDeConeccion);
    }
}