using HandyControl.Controls;
using HojadeRuta2K23.Singleton;
using HojadeRuta2K23.Windows;
using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace HojadeRuta2K23.Paginas;

public partial class Tareas : Page
{
    int TareaID_ParaOperaciones; //id tarea para aceptar,rechazar,etc.
    int cargo = App.MiVariableGlobal; //variable global del cargo
    byte[] fileBytes;
    private bool isDetailsVisible = false;
    private List<Tarea> tareas;
    int Asignador = App.MiVariableGlobal;
    public class Tarea
    {
        public int Id { get; set; }
        public int RemitenteId { get; set; }
        public int DestinatarioId { get; set; }
        public string EstadoTarea { get; set; }
        public string Accion { get; set; }
        public string Prioridad { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public DateTime FechaLimite { get; set; }
        public DateTime? FechaFinalizacion { get; set; }
        public int EstadoRegistro { get; set; }
        public string CodigoTarea { get; set; }
        public string NombreDestinatario { get; set; }
    }
    private bool isPanelVisible = false;
    private void ToggleButton_Click(object sender, RoutedEventArgs e)
    {
        DoubleAnimation animation = new DoubleAnimation();

        if (isPanelVisible)
        {
            animation.From = 150;
            animation.To = 0;
        }
        else
        {
            animation.From = 0;
            animation.To = 150;
        }

        animation.Duration = new Duration(TimeSpan.FromSeconds(0.5));

        Storyboard.SetTargetName(animation, hola3.Name); // Asegúrate de que 'hola3' es el nombre correcto del StackPanel
        Storyboard.SetTargetProperty(animation, new PropertyPath(StackPanel.HeightProperty));

        Storyboard storyboard = new Storyboard();
        storyboard.Children.Add(animation);

        hola3.BeginAnimation(StackPanel.HeightProperty, animation);

        isPanelVisible = !isPanelVisible;
    }
    public class Cargo
    {
        public int IdCargo { get; set; }
        public string NombreCargo { get; set; }
    }
    public Tareas()
    {
        InitializeComponent();
        //ComboBoxVistas.Items.Add("Vista Cuadriculada");
        //ComboBoxVistas.Items.Add("Vista Lista");
        tareas = new List<Tarea>();
        CargarDatosTareasDesdeBD();

        CargarDatosComboBox();
    }

    static string GenerarCodigoUnico()
    {
        const string prefijo = "TR-";
        const int longitudCodigo = 10;
        const string caracteresPermitidos = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        Random random = new Random();
        char[] codigoArray = new char[longitudCodigo];

        // Asegura que el código comience con el prefijo
        for (int i = 0; i < prefijo.Length; i++)
        {
            codigoArray[i] = prefijo[i];
        }

        // Genera caracteres aleatorios para el resto del código
        for (int i = prefijo.Length; i < longitudCodigo; i++)
        {
            codigoArray[i] = caracteresPermitidos[random.Next(caracteresPermitidos.Length)];
        }

        return new string(codigoArray);
    }

    private void CargarDatosComboBox()
    {
        List<Cargo> cargos = new List<Cargo>();

        var BaseDeDatos = Coneccion.Instance;

        string query = "SELECT IdCargo, NombreCargo FROM CARGOS"; // Selecciona IdCargo y NombreCargo de la tabla CARGOS

        using (SqlConnection connection = BaseDeDatos.GetConnection())
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int idCargo = reader.GetInt32(0);
                        string nombreCargo = reader.GetString(1);

                        if (idCargo != App.MiVariableGlobal)
                        {
                            // Crear un objeto Cargo y agregarlo a la lista
                            Cargo cargo = new Cargo
                            {
                                IdCargo = idCargo,
                                NombreCargo = nombreCargo
                            };

                            cargos.Add(cargo);
                        }
                    }
                }
            }
        }

        // Una vez que tengas la lista de cargos, asígnalos al ComboBox
        Destinatario.ItemsSource = cargos;
        Destinatario.DisplayMemberPath = "NombreCargo";
        Destinatario.SelectedValuePath = "IdCargo";

    }


    private void CargarDatosTareasDesdeBD()
    {
        var BaseDeDatos = Coneccion.Instance;

        // Modifica la consulta SQL para incluir una condición WHERE que filtre por idCargo
        string query = "SELECT T.IdTarea, T.RemitenteId, T.DestinatarioId, T.CodigoTarea, T.EstadoTarea, T.Accion, T.Prioridad, T.Descripcion, " +
                       "T.FechaAsignacion, T.FechaLimite, T.FechaFinalizacion, T.EstadoRegistro " +
                       "FROM TAREAS T " +
                       "JOIN FUNCIONARIOS F ON T.RemitenteId = F.IdFuncionario " +
                       "JOIN FUNCIONARIO_CARGO FC ON F.IdFuncionario = FC.FuncionarioId ";
        using (SqlConnection connection = BaseDeDatos.GetConnection())
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand(query, connection))
            {


                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Resto del código sigue igual...
                        Tarea tarea = new Tarea
                        {
                            Id = reader.GetInt32(0),
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
                            EstadoRegistro = reader.GetInt32(11)
                        };

                        tareas.Add(tarea);
                    }
                }
            }
        }
    }



    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        if (NavigationService != null)
        {
            NavigationService.Navigate(new NuevaTarea());
        }
    }

    private void AddDocumento_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Archivos PDF (*.pdf)|*.pdf";
        if (openFileDialog.ShowDialog() == true)
        {
            string filePath = openFileDialog.FileName;
            string fileName = System.IO.Path.GetFileName(filePath);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                fileBytes = new byte[fileStream.Length];
                fileStream.Read(fileBytes, 0, fileBytes.Length);
            }

            //string fileBytesString = BitConverter.ToString(fileBytes);
            //MessageBox.Show("Contenido del archivo PDF en bytes: " + fileBytesString);

            //nv_documento2.Text = fileName;
        }
    }

    private void ScannaDocumento_Click(object sender, RoutedEventArgs e)
    {
        System.Windows.MessageBox.Show("Esto aún está en proceso");
    }

    private void CrearTarea_Click(object sender, RoutedEventArgs e)
    {
        string fechaActual = DateTime.Today.ToString("dd/MM/yyyy");
        DateTime? selectedDate = datepickerTarea.SelectedDate;

        if (selectedDate.HasValue && Destinatario.Text != "" && Prioridad.Text != "" && Descripcion.Text != "")
        {
            DateTime date = selectedDate.Value;
            string usuario = "";
            string descripcion = Descripcion.Text;
            DateTime fechaAsignacion = DateTime.Now;
            string fecha_asignacion = fechaAsignacion.ToString("yyyy-MM-dd HH:mm:ss");
            string fecha_conclusion = date.ToString("yyyy-MM-dd HH:mm:ss");
            string estado = "Recepcionada";
            string prioridad = Prioridad.Text;

            string codigo = GenerarCodigoUnico();

            int destinatarioId = (int)Destinatario.SelectedValue;

            var BaseDeDatos = Coneccion.Instance;

            string query = "INSERT INTO TAREAS (RemitenteId, DestinatarioId, CodigoTarea, EstadoTarea, Accion, Prioridad, Descripcion, FechaAsignacion, FechaLimite, FechaFinalizacion, EstadoRegistro)" +
                "VALUES (@RemitenteId, @DestinatarioId, @CodigoTarea,@EstadoTarea, 'Falta hacer???', @PrioridadTarea, @Descripcion, @FechaAsignacion, @FechaConclusion, NULL, 1)";

            using (var connection = BaseDeDatos.GetConnection())
            {
                connection.Open();

                // Después de insertar la tarea en TAREAS
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RemitenteId", Asignador);
                    command.Parameters.AddWithValue("@DestinatarioId", destinatarioId);
                    command.Parameters.AddWithValue("@CodigoTarea", codigo);
                    command.Parameters.AddWithValue("@Descripcion", descripcion);
                    command.Parameters.AddWithValue("@FechaAsignacion", fecha_asignacion);
                    command.Parameters.AddWithValue("@FechaConclusion", fecha_conclusion);
                    command.Parameters.AddWithValue("@EstadoTarea", "Recepcionada");
                    command.Parameters.AddWithValue("@PrioridadTarea", prioridad);

                    command.ExecuteNonQuery();

                    // Obtener el IdTarea recién insertado
                    int tareaId;
                    string getMaxTareaIdQuery = "SELECT MAX(IdTarea) FROM TAREAS";

                    using (SqlCommand getMaxTareaIdCommand = new SqlCommand(getMaxTareaIdQuery, connection))
                    {
                        var result = getMaxTareaIdCommand.ExecuteScalar();
                        tareaId = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                    }

                    // Verificar si hay un archivo adjunto
                    if (fileBytes != null && fileBytes.Length > 0)
                    {
                        // Insertar el archivo adjunto en la tabla ANEXO_TAREAS
                        string insertAnexoQuery = "INSERT INTO ANEXO_TAREAS (RemitenteId, TareaId, Archivo, FechaRegistro, EstadoRegistro)" +
                                                  "VALUES (@RemitenteId, @TareaId, @Archivo, @FechaRegistro, 1)";

                        using (SqlCommand anexoCommand = new SqlCommand(insertAnexoQuery, connection))
                        {
                            anexoCommand.Parameters.AddWithValue("@RemitenteId", Asignador);
                            anexoCommand.Parameters.AddWithValue("@TareaId", tareaId);  // Usar el IdTarea obtenido
                            anexoCommand.Parameters.AddWithValue("@Archivo", fileBytes);
                            anexoCommand.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);

                            anexoCommand.ExecuteNonQuery();
                        }
                    }
                }
            }

            Descripcion.Text = string.Empty;
            Prioridad.Text = string.Empty;
            datepickerTarea.SelectedDate = null;
            Destinatario.SelectedIndex = -1;
            System.Windows.MessageBox.Show("Tarea insertada");
        }
        else
        {
            System.Windows.MessageBox.Show("Falta llenar datos");
        }
    }

    private void propiedades_Click(object sender, RoutedEventArgs e)
    {
        Button btn = sender as Button;
        if (btn != null)
        {
            int taskId = (int)btn.Tag; // Obtener el ID de la tarea desde el Tag del botón

            if (isDetailsVisible)
            {
                DoubleAnimation hideAnimation = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(0.5)));
                VistaDetallesTarea.BeginAnimation(WidthProperty, hideAnimation);
            }
            else
            {
                // Buscar la tarea correspondiente en la lista de tareas por el ID
                Tarea tareaSeleccionada = tareas.FirstOrDefault(t => t.Id == taskId);
                TareaID_ParaOperaciones = tareaSeleccionada.Id;
                if (tareaSeleccionada != null)
                {
                    if (tareaSeleccionada.DestinatarioId == cargo)
                    {
                        jajaja.IsEnabled = false;
                    }
                    else
                    {
                        jajaja.IsEnabled = true;
                    }
                    CodigoTarea.Content = tareaSeleccionada.CodigoTarea;
                    NombreDestinatario.Content = tareaSeleccionada.DestinatarioId;
                    PrioridadLBL.Content = tareaSeleccionada.Prioridad;

                    FechaAsignacion.Content = tareaSeleccionada.FechaAsignacion.ToString();
                    FechaLimite.Content = tareaSeleccionada.FechaLimite.ToString();
                    Descripcio.Text = tareaSeleccionada.Descripcion.ToString();
                    Estado.Content = tareaSeleccionada.EstadoTarea.ToString();
                    //Observaciones.Text = tareaSeleccionada.Observaciones.ToString();
                }

                DoubleAnimation showAnimation = new DoubleAnimation(500, new Duration(TimeSpan.FromSeconds(0.5)));
                VistaDetallesTarea.BeginAnimation(WidthProperty, showAnimation);
            }

            isDetailsVisible = !isDetailsVisible;


        }
    }

    bool a = false;

    private void Filtros_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (Filtros.SelectedItem != null)
        {
            // Obtener el valor seleccionado en el ComboBox directamente
            ComboBoxItem itemSeleccionado = (ComboBoxItem)Filtros.SelectedItem;
            ComboBoxItem itemSeleccionado2 = (ComboBoxItem)FiltrosAsignacion.SelectedItem;

            string filtroSeleccionado = itemSeleccionado.Content.ToString();
            string filtroSeleccionado2 = itemSeleccionado2.Content.ToString();

            System.Windows.MessageBox.Show("Filtro seleccionado: " + filtroSeleccionado);
            System.Windows.MessageBox.Show("Filtro seleccionado: " + filtroSeleccionado2);

            /*if (filtroSeleccionado == "Todos" && filtroSeleccionado2 == "Asignados por mi")
            {
                membersDataGrid.ItemsSource = tareas;
            }
            if (filtroSeleccionado == "Todos" && filtroSeleccionado2 == "Asignados hacia mi")
            {
                membersDataGrid.ItemsSource = tareas;
            }*/

            if (filtroSeleccionado == "Todos" && filtroSeleccionado2 == "Asignados por mi")
            {
                List<Tarea> tareasAsignadasPorMi = tareas.Where(t => t.RemitenteId == Asignador).ToList();
                membersDataGrid.ItemsSource = tareasAsignadasPorMi; // Muestra todas las tareas asignadas por el usuario actual
            }
            if (filtroSeleccionado == "Todos" && filtroSeleccionado2 == "Asignados hacia mi")
            {
                List<Tarea> tareasAsignadasHaciaMi = tareas.Where(t => t.DestinatarioId == Asignador).ToList();
                membersDataGrid.ItemsSource = tareasAsignadasHaciaMi; // Muestra todas las tareas asignadas hacia el usuario actual
            }



            if (filtroSeleccionado == "Finalizado" && filtroSeleccionado2 == "Asignados por mi")
            {
                List<Tarea> tareasFinalizadasPorMi = tareas.Where(t => t.EstadoTarea == "Finalizado" && t.RemitenteId == Asignador).ToList();
                membersDataGrid.ItemsSource = tareasFinalizadasPorMi;
            }
            if (filtroSeleccionado == "Finalizado" && filtroSeleccionado2 == "Asignados hacia mi")
            {
                List<Tarea> tareasFinalizadasHaciaMi = tareas.Where(t => t.EstadoTarea == "Finalizado" && t.DestinatarioId == Asignador).ToList();
                membersDataGrid.ItemsSource = tareasFinalizadasHaciaMi;
            }


            if (filtroSeleccionado == "En Curso" && filtroSeleccionado2 == "Asignados por mi")
            {
                List<Tarea> tareasEnCursoPorMi = tareas.Where(t => t.EstadoTarea == "Recepcionada" && t.RemitenteId == Asignador).ToList();
                membersDataGrid.ItemsSource = tareasEnCursoPorMi; // Muestra las tareas en curso asignadas por el usuario actual
            }
            if (filtroSeleccionado == "En Curso" && filtroSeleccionado2 == "Asignados hacia mi")
            {
                List<Tarea> tareasEnCursoHaciaMi = tareas
                    .Where(t => t.EstadoTarea == "Recepcionada" && t.DestinatarioId == cargo)
                    .ToList();

                membersDataGrid.ItemsSource = tareasEnCursoHaciaMi; // Muestra las tareas en curso asignadas hacia el usuario actual 
                System.Windows.MessageBox.Show("Cargo " + cargo);

                // Mostrar cada dato de la lista
                foreach (var tarea in tareasEnCursoHaciaMi)
                {
                    System.Windows.MessageBox.Show($"ID: {tarea.Id}, Descripción: {tarea.Descripcion}");
                    // Agrega más propiedades según la estructura de tu clase Tarea
                }
            }





        }
    }

    private void RechazarTarea_Click(object sender, RoutedEventArgs e)
    {

    }

    private void EditarTarea_Click(object sender, RoutedEventArgs e)
    {
        System.Windows.MessageBox.Show("asd");
    }

    private void AceptarTarea_Click(object sender, RoutedEventArgs e)
    {

    }

    private void DevolverTarea_Click(object sender, RoutedEventArgs e)
    {

    }


    //COSITAS DEL DANIEL
    private void rechazar_OnClick(object sender, RoutedEventArgs e)
    {
        RechazarTarea rechazar = new RechazarTarea();
        rechazar.ShowDialog();
    }

    private void Modificar_OnClick(object sender, RoutedEventArgs e)
    {
        EditarTarea edit = new EditarTarea();
        edit.ShowDialog();
    }

    private void ocultar_OnClick(object sender, RoutedEventArgs e)
    {
        DoubleAnimation animation = new DoubleAnimation();
        animation.From = 500;
        animation.To = 0;
        animation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
        VistaDetallesTarea.BeginAnimation(StackPanel.WidthProperty, animation);
    }

    private void MiBotonV_OnClick(object sender, RoutedEventArgs e)
    {
        DoubleAnimation animation = new DoubleAnimation();
        animation.From = 0;
        animation.To = 250;
        animation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
        hola3.BeginAnimation(StackPanel.HeightProperty, animation);
        miBotonV.Visibility = Visibility.Collapsed;
        miBotonV1.Visibility = Visibility.Visible;
    }

    private void MiBotonV1_OnClick(object sender, RoutedEventArgs e)
    {
        DoubleAnimation animation = new DoubleAnimation();
        animation.From = 250;
        animation.To = 0;
        animation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
        hola3.BeginAnimation(StackPanel.HeightProperty, animation);
        miBotonV.Visibility = Visibility.Visible;
        miBotonV1.Visibility = Visibility.Collapsed;
    }

    private void ver_OnClick(object sender, RoutedEventArgs e)
    {
        DoubleAnimation animation = new DoubleAnimation();
        animation.From = 0;
        animation.To = 500;
        animation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
        VistaDetallesTarea.BeginAnimation(StackPanel.WidthProperty, animation);

    }
}