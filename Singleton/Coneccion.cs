using Microsoft.Data.SqlClient;

namespace HojadeRuta2K23.Singleton;

public class Coneccion
{
    private static Coneccion instacia;
    private readonly string cadenaDeConeccion;

    private Coneccion()
    {
        cadenaDeConeccion =
            
            "Server=Italo-Navia;Database=nashe1;Integrated Security=true; TrustServerCertificate=True"; // Reemplaza con tu cadena de conexión
   
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