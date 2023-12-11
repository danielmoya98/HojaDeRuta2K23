using HojadeRuta2K23.Paginas;
using HojadeRuta2K23.Singleton;
using Microsoft.Data.SqlClient;
using System;
using System.Windows;

namespace HojadeRuta2K23.Windows;

public partial class ObservarRechazarTramite : Window
    {
        Tramites tramite;
        public ObservarRechazarTramite(Tramites tramite)
        {
            this.tramite = tramite;
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {













            using (var connection = Coneccion.Instance.GetConnection())
            {
                int TramiteId = tramite.taskiid;

                string query2 = "SELECT ISNULL(MAX(NumeroInstancia), 0) FROM FLUJO_HOJA_DE_RUTA WHERE TramiteId = @TramiteId;";

                int numeroInstancia = 0;

                using (var command = new SqlCommand(query2, connection))
                {
                    command.Parameters.AddWithValue("@TramiteId", TramiteId);

                    connection.Open(); // Asegúrate de abrir la conexión antes de ejecutar la consulta

                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        numeroInstancia = Convert.ToInt32(result);
                    }

                    connection.Close(); // No olvides cerrar la conexión después de usarla
                }










                string query5 = "DELETE FROM FLUJO_HOJA_DE_RUTA WHERE TramiteId = @TramiteId AND NumeroInstancia = @NumeroInstancia;";

            
            
                    using (var command = new SqlCommand(query5, connection))
                    {
                        command.Parameters.AddWithValue("@TramiteId", TramiteId);
                        command.Parameters.AddWithValue("@NumeroInstancia", numeroInstancia);

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }




                using (var command = new SqlCommand(query2, connection))
                {
                    command.Parameters.AddWithValue("@TramiteId", TramiteId);

                    connection.Open(); // Asegúrate de abrir la conexión antes de ejecutar la consulta

                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        numeroInstancia = Convert.ToInt32(result);
                    }

                    connection.Close(); // No olvides cerrar la conexión después de usarla
                }




                string query = "SELECT DestinatarioId FROM FLUJO_HOJA_DE_RUTA WHERE TramiteId = @TramiteId AND NumeroInstancia = @NumeroInstancia;";

                int RemitenteId = 0; // Se inicializa en 0 para manejar posibles valores nulos o no encontrados

                using (var command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@TramiteId", TramiteId);
                    command.Parameters.AddWithValue("@NumeroInstancia", numeroInstancia);
                    connection.Open();

                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        RemitenteId = Convert.ToInt32(result);
                    }
                    else
                    {
                        // Manejar el caso cuando no se encuentra ningún RemitenteId para la TramiteId y NumeroInstancia proporcionadas
                        // Puede mostrar un mensaje de error, lanzar una excepción o tomar cualquier otra acción necesaria.
                    }
                    connection.Close();
                }


                string query3 = "SELECT TipoTramiteId FROM TRAMITES WHERE IdTramite = @TramiteId;";

                int tipoTramiteId = 0;

                using (var command = new SqlCommand(query3, connection))
                {

                    command.Parameters.AddWithValue("@TramiteId", TramiteId);
                    command.Parameters.AddWithValue("@NumeroInstancia", numeroInstancia);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    tipoTramiteId = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                    connection.Close();
                }








                int destinatarioId = tramite.EncontrarDestinatarioId(tipoTramiteId, RemitenteId);


                if (destinatarioId != 0)
                {



                    string query4 = "UPDATE FLUJO_HOJA_DE_RUTA " +
                   "SET EstadoTramite = 'Regresado' " +
                   "WHERE TramiteId = @TramiteId " +
                   "AND NumeroInstancia = @NumeroInstancia " +
                   "AND EstadoTramite NOT IN ('Devuelto', 'Rechazado');";

                    using (var command = new SqlCommand(query4, connection))
                    {
                        command.Parameters.AddWithValue("@TramiteId", TramiteId);
                        command.Parameters.AddWithValue("@NumeroInstancia", numeroInstancia);
                        connection.Open();
                        command.ExecuteNonQuery();

                        connection.Close();
                    }




                
                }


            }












        }




    
}