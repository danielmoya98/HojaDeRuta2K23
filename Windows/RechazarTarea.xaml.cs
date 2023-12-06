using HojadeRuta2K23.Singleton;
using System;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using static HojadeRuta2K23.Paginas.Tareas;
using System.Windows;
using HojadeRuta2K23.Paginas;
using System.Windows.Navigation;

namespace HojadeRuta2K23.Windows
{
    public partial class RechazarTarea : Window
    {
        private Tarea tarea;

        public RechazarTarea()
        {
            InitializeComponent();
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            // Este evento se ejecutará cuando la ventana se cierre
            // Puedes realizar acciones adicionales si es necesario

            // Obtén el NavigationService del Frame padre
            NavigationService navigationService = NavigationService.GetNavigationService(this);

            // Navega a la página Tareas y recarga el Frame
            navigationService.Navigate(new Tareas());
        }
        public RechazarTarea(Tarea tarea)
        {
            this.tarea = tarea;
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string observacion = Observacion.Text;

            // Realizar la actualización en la base de datos
            ActualizarTareaRechazada();

            // Agregar observación en la tabla OBSERVACIONES_TAREAS
            AgregarObservacion(observacion);

            // Puedes realizar otras acciones después de rechazar la tarea si es necesario
            Close();
        }

        private void ActualizarTareaRechazada()
        {
            var conexion = Coneccion.Instance;

            string query = "UPDATE TAREAS SET " +
                           "DestinatarioId = @AntiguoDestinatarioId, " +
                           "RemitenteId = @DestinatarioId, " +
                           "EstadoTarea = 'Rechazado' " +
                           "WHERE IdTarea = @TareaId";

            using (SqlConnection connection = conexion.GetConnection())
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TareaId", tarea.Id);
                    command.Parameters.AddWithValue("@DestinatarioId", tarea.DestinatarioId);
                    command.Parameters.AddWithValue("@AntiguoDestinatarioId", tarea.RemitenteId);

                    command.ExecuteNonQuery();
                }
            }
        }

        private void AgregarObservacion(string observacion)
        {
            var conexion = Coneccion.Instance;

            // Verificar si ya existe una observación para la tarea
            string selectQuery = "SELECT IdObservTarea FROM OBSERVACIONES_TAREAS WHERE TareaId = @TareaId";
            int? idObservacionExistente = null;

            using (SqlConnection connectionSelect = conexion.GetConnection())
            {
                connectionSelect.Open();

                using (SqlCommand commandSelect = new SqlCommand(selectQuery, connectionSelect))
                {
                    commandSelect.Parameters.AddWithValue("@TareaId", tarea.Id);

                    using (SqlDataReader reader = commandSelect.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            idObservacionExistente = reader.GetInt32(0);
                        }
                    }
                }
            }

            // Si ya existe una observación, actualizar en lugar de insertar
            if (idObservacionExistente.HasValue)
            {
                string updateQuery = "UPDATE OBSERVACIONES_TAREAS SET Observacion = @Observacion, FechaObservacion = @FechaObservacion WHERE IdObservTarea = @IdObservTarea";

                using (SqlConnection connectionUpdate = conexion.GetConnection())
                {
                    connectionUpdate.Open();

                    using (SqlCommand commandUpdate = new SqlCommand(updateQuery, connectionUpdate))
                    {
                        commandUpdate.Parameters.AddWithValue("@IdObservTarea", idObservacionExistente.Value);
                        commandUpdate.Parameters.AddWithValue("@Observacion", observacion);
                        commandUpdate.Parameters.AddWithValue("@FechaObservacion", DateTime.Now);

                        commandUpdate.ExecuteNonQuery();
                    }
                }
            }
            else
            {
                // Si no existe una observación, insertar una nueva
                string insertQuery = "INSERT INTO OBSERVACIONES_TAREAS (RemitenteId, TareaId, Observacion, FechaObservacion, EstadoRegistro) " +
                                     "VALUES (@RemitenteId, @TareaId, @Observacion, @FechaObservacion, 1)";

                using (SqlConnection connectionInsert = conexion.GetConnection())
                {
                    connectionInsert.Open();

                    using (SqlCommand commandInsert = new SqlCommand(insertQuery, connectionInsert))
                    {
                        commandInsert.Parameters.AddWithValue("@RemitenteId", tarea.DestinatarioId);
                        commandInsert.Parameters.AddWithValue("@TareaId", tarea.Id);
                        commandInsert.Parameters.AddWithValue("@Observacion", observacion);
                        commandInsert.Parameters.AddWithValue("@FechaObservacion", DateTime.Now);

                        commandInsert.ExecuteNonQuery();
                    }
                }
            }
        }


    }
}
