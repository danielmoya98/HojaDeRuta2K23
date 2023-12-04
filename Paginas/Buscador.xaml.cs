using HojadeRuta2K23.Singleton;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace HojadeRuta2K23.Paginas
{
    public partial class Buscador : Page
    {
        List<Tramite> listaTramites = new List<Tramite>();
        public Buscador()
        {
            InitializeComponent();
            buscador.Items.Add("Tareas");
            buscador.Items.Add("Tramites");
            List<Tarea> listaTareas = ObtenerTareasDesdeLaBaseDeDatos();

            membersDataGrid.ItemsSource = listaTareas;
            CargarCombobox();
            CargarTramitesEnDataGrid();
            PrioridadTareaBuscar.Items.Add("Todos");
            PrioridadTareaBuscar.Items.Add("Alta");
            PrioridadTareaBuscar.Items.Add("Media");
            PrioridadTareaBuscar.Items.Add("Baja");

            EstadoTareaBuscar.Items.Add("Todos");
            EstadoTareaBuscar.Items.Add("Recepcionada");
            EstadoTareaBuscar.Items.Add("Observada");
            EstadoTareaBuscar.Items.Add("Finalizado");

            TipoTramiteBuscar.Items.Add("Solicitud de cambio de carrera");
            TipoTramiteBuscar.Items.Add("Solicitud programacion tardia");
            TipoTramiteBuscar.Items.Add("Solicitud de certificado de notas");
            TipoTramiteBuscar.Items.Add("Solicitud de inicio de grado");
            TipoTramiteBuscar.Items.Add("Certificado de trabajo");
            TipoTramiteBuscar.Items.Add("Solicitud de Licencia");
            TipoTramiteBuscar.Items.Add("Otros");
            TipoTramiteBuscar.Items.Add("Todos");
            TipoTramiteBuscar.Items.Add("Tipo de Trámite Ejemplo");



            EstadoTramiteBuscar.Items.Add("En Proceso");
            EstadoTramiteBuscar.Items.Add("Finalizado");
            EstadoTramiteBuscar.Items.Add("Observado");
            EstadoTramiteBuscar.Items.Add("Rechazado");
            EstadoTramiteBuscar.Items.Add("Todos");

        }

        public class Tramite
        {
            public int IdTramite { get; set; }
            public int TipoTramiteId { get; set; }
            public int ClienteId { get; set; }
            public string CodigoTramite { get; set; }
            public string EstadoTramite { get; set; }
            public DateTime FechaInicio { get; set; }
            public DateTime? FechaFinalizacion { get; set; }
            public int EstadoRegistro { get; set; }
        }




        private void CargarTramitesEnDataGrid()
        {
            listaTramites = ObtenerTramitesDesdeLaBaseDeDatos();
            membersDataGridd.ItemsSource = listaTramites;
        }

        private List<Tramite> ObtenerTramitesDesdeLaBaseDeDatos()
        {

            using (var connection = Coneccion.Instance.GetConnection())
            {
                connection.Open();

                string query = "SELECT IdTramite, TipoTramiteId, ClienteId, CodigoTramite, EstadoTramite, " +
                               "FechaInicio, FechaFinalizacion, EstadoRegistro " +
                               "FROM TRAMITES";

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Tramite tramite = new Tramite
                            {
                                IdTramite = reader.GetInt32(0),
                                TipoTramiteId = reader.GetInt32(1),
                                ClienteId = reader.GetInt32(2),
                                CodigoTramite = reader.GetString(3),
                                EstadoTramite = reader.GetString(4),
                                FechaInicio = reader.GetDateTime(5),
                                FechaFinalizacion = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6),
                                EstadoRegistro = reader.GetInt32(7),
                            };

                            listaTramites.Add(tramite);
                        }
                    }
                }
            }

            return listaTramites;
        }
        public class Tarea
        {
            public int IdTarea { get; set; }
            public int RemitenteId { get; set; }
            public int DestinatarioId { get; set; }
            public string CodigoTarea { get; set; }
            public string EstadoTarea { get; set; }
            public string Accion { get; set; }
            public string Prioridad { get; set; }
            public string Descripcion { get; set; }
            public DateTime FechaAsignacion { get; set; }
            public DateTime FechaLimite { get; set; }
            public DateTime? FechaFinalizacion { get; set; }
            public int EstadoRegistro { get; set; }
        }

        private List<Tarea> ObtenerTareasDesdeLaBaseDeDatos()
        {
            List<Tarea> listaTareas = new List<Tarea>();

            using (var connection = Coneccion.Instance.GetConnection())
            {
                connection.Open();

                string query = "SELECT IdTarea, RemitenteId, DestinatarioId, CodigoTarea, EstadoTarea, Accion, " +
                               "Prioridad, Descripcion, FechaAsignacion, FechaLimite, FechaFinalizacion, EstadoRegistro " +
                               "FROM TAREAS";

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Tarea tarea = new Tarea
                            {
                                IdTarea = reader.GetInt32(0),
                                RemitenteId = reader.GetInt32(1),
                                DestinatarioId = reader.GetInt32(2),
                                CodigoTarea = reader.GetString(3),
                                EstadoTarea = reader.GetString(4),
                                Accion = reader.GetString(5),
                                Prioridad = reader.GetString(6),
                                Descripcion = reader.GetString(7),
                                FechaAsignacion = reader.GetDateTime(8),
                                FechaLimite = reader.GetDateTime(9),
                                FechaFinalizacion = reader.IsDBNull(10) ? (DateTime?)null : reader.GetDateTime(10),
                                EstadoRegistro = reader.GetInt32(11),
                            };

                            listaTareas.Add(tarea);
                        }
                    }
                }
            }

            return listaTareas;
        }

        private void CodigoTareaBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            FiltrarTareas();
        }

        private void RemitenteTareaBuscar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FiltrarTareas();
        }

        private void EstadoTareaBuscar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FiltrarTareas();
        }

        private void PrioridadTareaBuscar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FiltrarTareas();
        }

        private void membersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Lógica relacionada con la selección en el DataGrid, si es necesario
        }

        private void Buscador_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (buscador.SelectedIndex == 0)
            {
                tareas.Visibility = Visibility.Visible;
                membersDataGrid.Visibility = Visibility.Visible;
                membersDataGridd.Visibility = Visibility.Collapsed;
                tramites.Visibility = Visibility.Hidden;
            }

            if (buscador.SelectedIndex == 1)
            {
                tramites.Visibility = Visibility.Visible;
                membersDataGrid.Visibility = Visibility.Collapsed;
                membersDataGridd.Visibility = Visibility.Visible;
                tareas.Visibility = Visibility.Hidden;
            }
        }

        private void CargarNombresCargos()
        {
            var BaseDeDatos = Coneccion.Instance;

            string query = "SELECT NombreCargo FROM CARGOS";

            using (var connection = BaseDeDatos.GetConnection())
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string nombreCargo = reader["NombreCargo"].ToString();
                            RemitenteTareaBuscar.Items.Add(nombreCargo);
                        }
                    }
                }
            }
        }

        private void CargarCombobox()
        {
            RemitenteTareaBuscar.Items.Clear(); // Limpiar los elementos previos, si los hay
            CargarNombresCargos();
        }

        private void BuscarYMostrarTareasPorRemitente(string nombreRemitente)
        {
            var BaseDeDatos = Coneccion.Instance;

            string query = @"SELECT TAREAS.*
                            FROM TAREAS
                            INNER JOIN FUNCIONARIOS ON TAREAS.RemitenteId = FUNCIONARIOS.IdFuncionario
                            INNER JOIN FUNCIONARIO_CARGO ON FUNCIONARIOS.IdFuncionario = FUNCIONARIO_CARGO.FuncionarioId
                            INNER JOIN CARGOS ON FUNCIONARIO_CARGO.CargoId = CARGOS.IdCargo
                            WHERE CARGOS.NombreCargo = @NombreRemitente";

            using (var connection = BaseDeDatos.GetConnection())
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NombreRemitente", nombreRemitente);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);

                        // Actualiza la interfaz de usuario en el hilo de la interfaz de usuario
                        Dispatcher.Invoke(() => membersDataGrid.ItemsSource = dataTable.DefaultView);
                    }
                }
            }
        }

        private void FiltrarTareas()
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(membersDataGrid.ItemsSource);

            if (view != null)
            {
                view.Filter = item =>
                {
                    Tarea tarea = (Tarea)item;

                    string filtroCodigo = CodigoTareaBuscar.Text.ToUpper();
                    string filtroRemitente = RemitenteTareaBuscar.SelectedItem?.ToString();
                    string filtroEstado = EstadoTareaBuscar.SelectedItem?.ToString();
                    string filtroPrioridad = PrioridadTareaBuscar.SelectedItem?.ToString();

                    bool codigoMatch = string.IsNullOrEmpty(filtroCodigo) || tarea.CodigoTarea.Contains(filtroCodigo);
                    bool remitenteMatch = string.IsNullOrEmpty(filtroRemitente) || filtroRemitente == "Todos" || ObtenerNombreCargo(tarea.RemitenteId) == filtroRemitente;
                    bool estadoMatch = string.IsNullOrEmpty(filtroEstado) || filtroEstado == "Todos" || tarea.EstadoTarea == filtroEstado;
                    bool prioridadMatch = string.IsNullOrEmpty(filtroPrioridad) || filtroPrioridad == "Todos" || tarea.Prioridad == filtroPrioridad;

                    return codigoMatch && remitenteMatch && estadoMatch && prioridadMatch;
                };
            }
        }


        private string ObtenerNombreCargo(int remitenteId)
        {
            // Obtener el nombre del cargo del remitente
            var BaseDeDatos = Coneccion.Instance;

            string query = @"SELECT CARGOS.NombreCargo
                    FROM FUNCIONARIOS
                    INNER JOIN FUNCIONARIO_CARGO ON FUNCIONARIOS.IdFuncionario = FUNCIONARIO_CARGO.FuncionarioId
                    INNER JOIN CARGOS ON FUNCIONARIO_CARGO.CargoId = CARGOS.IdCargo
                    WHERE FUNCIONARIOS.IdFuncionario = @RemitenteId";

            using (var connection = BaseDeDatos.GetConnection())
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RemitenteId", remitenteId);

                    var result = command.ExecuteScalar();
                    return result != null ? result.ToString() : null;
                }
            }
        }

        private void FiltrarTareasPorEstado(string estado)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(membersDataGrid.ItemsSource);

            if (view != null)
            {
                // Filtrar según EstadoTarea
                view.Filter = item =>
                {
                    Tarea tarea = (Tarea)item;
                    return tarea.CodigoTarea.Contains(CodigoTareaBuscar.Text) &&
                           (RemitenteTareaBuscar.SelectedItem == null || ObtenerNombreCargo(tarea.RemitenteId) == RemitenteTareaBuscar.SelectedItem.ToString()) &&
                           tarea.EstadoTarea == estado &&
                           (PrioridadTareaBuscar.SelectedItem == null || tarea.Prioridad == PrioridadTareaBuscar.SelectedItem.ToString());
                };
            }
        }

        private void CodigoHRBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            FiltrarTramites();
        }

        private void ApellidoPaternoTramiteBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            FiltrarTramites();
        }

        private void ApellidoMaternoTramiteBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            FiltrarTramites();
        }

        private void FechaInicioTramiteBuscar_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            FiltrarTramites();
        }

        private void TipoTramiteBuscar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FiltrarTramites();
        }

        private void EstadoTramiteBuscar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FiltrarTramites();
        }

        private void FiltrarTramites()
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(membersDataGridd.ItemsSource);

            if (view != null)
            {
                view.Filter = item =>
                {
                    Tramite tramite = (Tramite)item;

                    string filtroCodigo = CodigoHRBuscar.Text.ToUpper();
                    string filtroApellidoPaterno = ApellidoPaternoTramiteBuscar.Text.ToLower();
                    string filtroApellidoMaterno = ApellidoMaternoTramiteBuscar.Text.ToLower();
                    DateTime? filtroFechaInicio = FechaInicioTramiteBuscar.SelectedDate;
                    string filtroTipoTramite = TipoTramiteBuscar.SelectedItem?.ToString();
                    string filtroEstadoTramite = EstadoTramiteBuscar.SelectedItem?.ToString();

                    bool codigoMatch = string.IsNullOrEmpty(filtroCodigo) || tramite.CodigoTramite.Contains(filtroCodigo);
                    bool apellidoPaternoMatch = string.IsNullOrEmpty(filtroApellidoPaterno) || ObtenerApellidoPaterno(tramite.ClienteId).ToLower().StartsWith(filtroApellidoPaterno);
                    bool apellidoMaternoMatch = string.IsNullOrEmpty(filtroApellidoMaterno) || ObtenerApellidoMaterno(tramite.ClienteId).ToLower().StartsWith(filtroApellidoMaterno);
                    bool fechaInicioMatch = !filtroFechaInicio.HasValue || tramite.FechaInicio.Date == filtroFechaInicio.Value.Date;
                    bool tipoTramiteMatch = string.IsNullOrEmpty(filtroTipoTramite) || filtroTipoTramite == "Todos" || ObtenerNombreTipoTramite(tramite.TipoTramiteId) == filtroTipoTramite;
                    bool estadoTramiteMatch = string.IsNullOrEmpty(filtroEstadoTramite) || filtroEstadoTramite == "Todos" || tramite.EstadoTramite == filtroEstadoTramite;

                    return codigoMatch && apellidoPaternoMatch && apellidoMaternoMatch && fechaInicioMatch && tipoTramiteMatch && estadoTramiteMatch;
                };
            }
        }


        private string ObtenerApellidoPaterno(int clienteId)
        {
            using (var connection = Coneccion.Instance.GetConnection())
            {
                connection.Open();

                string query = "SELECT ApellidoPaterno " +
                               "FROM PERSONAS " +
                               "INNER JOIN CLIENTES ON PERSONAS.IdPersona = CLIENTES.PersonaId " +
                               "WHERE CLIENTES.IdCliente = @ClienteId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClienteId", clienteId);

                    var result = command.ExecuteScalar();
                    return result != null ? result.ToString() : null;
                }
            }
        }

        private string ObtenerApellidoMaterno(int clienteId)
        {
            using (var connection = Coneccion.Instance.GetConnection())
            {
                connection.Open();

                string query = "SELECT P.ApellidoMaterno " +
                               "FROM CLIENTES C " +
                               "INNER JOIN PERSONAS P ON C.PersonaId = P.IdPersona " +
                               "WHERE C.IdCliente = @ClienteId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClienteId", clienteId);

                    var result = command.ExecuteScalar();
                    return result != null ? result.ToString() : null;
                }
            }
        }

        private string ObtenerNombreTipoTramite(int tipoTramiteId)
        {
            using (var connection = Coneccion.Instance.GetConnection())
            {
                connection.Open();

                string query = "SELECT NombreTramite " +
                               "FROM TIPO_TRAMITE " +
                               "WHERE IdTipoTramite = @IdTipoTramite";  // Corregir aquí

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdTipoTramite", tipoTramiteId);  // Corregir aquí

                    var result = command.ExecuteScalar();
                    return result != null ? result.ToString() : null;
                }
            }
        }
    }
}