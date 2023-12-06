using HojadeRuta2K23.Paginas;
using HojadeRuta2K23.Singleton;
using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using static HojadeRuta2K23.Paginas.Tareas;
namespace HojadeRuta2K23.Windows;

public partial class EditarTarea : Window
{
    private bool isDragging = false;
    private Point offset;
    Tarea tarea = new Tarea();
    byte[] fileBytes;
    int Asignador = App.MiVariableGlobal;
    public EditarTarea(Tarea tareas)
    {
        InitializeComponent();
        tarea = tareas;
        datepickerTarea.SelectedDate = tarea.FechaLimite;

        if(tarea.Prioridad == "Alta")
        {
            Prioridad.SelectedIndex = 0;
        }
        else if(tarea.Prioridad == "Media")
        {
            Prioridad.SelectedIndex = 1;
        }
        else
        {
            Prioridad.SelectedIndex = 2;
        }

        CargarDatosComboBox();

        Destinatario.SelectedIndex = tarea.DestinatarioId - 2;
        Descripcion.Text = tarea.Descripcion;

        this.MouseLeftButtonDown += Window_MouseLeftButtonDown;
        this.MouseLeftButtonUp += Window_MouseLeftButtonUp;
        this.MouseMove += Window_MouseMove;
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
    private void ModificarTarea_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            int tareaId = tarea.Id;
            MessageBox.Show("asd " + tareaId);
            DateTime? selectedDate = datepickerTarea.SelectedDate;

            if (selectedDate.HasValue && Destinatario.Text != "" && Prioridad.Text != "" && Descripcion.Text != "")
            {
                int destinatarioId = (int)Destinatario.SelectedValue;
                DateTime date = selectedDate.Value.Date;
                string descripcion = Descripcion.Text;
                string prioridad = Prioridad.Text;

                var BaseDeDatos = Coneccion.Instance;

                string query = "UPDATE TAREAS SET " +
                             "DestinatarioId = @DestinatarioId, " +
                             "Prioridad = @PrioridadTarea, " +
                             "Descripcion = @Descripcion, " +
                             "FechaLimite = @FechaLimite " +
                             "WHERE IdTarea = @IdTarea;";

                using (var connection = BaseDeDatos.GetConnection())
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DestinatarioId", destinatarioId);
                        command.Parameters.AddWithValue("@PrioridadTarea", prioridad);
                        command.Parameters.AddWithValue("@Descripcion", descripcion);
                        command.Parameters.AddWithValue("@FechaLimite", date);
                        command.Parameters.AddWithValue("@IdTarea", tareaId);



                        command.ExecuteNonQuery();

                        // Obtener el IdTarea recién insertado
                        //string getMaxTareaIdQuery = "SELECT MAX(IdTarea) FROM TAREAS";

                        //using (SqlCommand getMaxTareaIdCommand = new SqlCommand(getMaxTareaIdQuery, connection))
                        //{
                        //    var result = getMaxTareaIdCommand.ExecuteScalar();
                        //    tareaId = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        //}

                        // Verificar si hay un archivo adjunto
                        // Verificar si hay un archivo adjunto
                        if (fileBytes != null && fileBytes.Length > 0)
                        {
                            // Verificar si ya existe un anexo para la tarea
                            string selectAnexoQuery = "SELECT IdAnexoTarea FROM ANEXO_TAREAS WHERE TareaId = @TareaId";
                            int? idAnexoExistente = null;

                            using (SqlCommand commandSelectAnexo = new SqlCommand(selectAnexoQuery, connection))
                            {
                                commandSelectAnexo.Parameters.AddWithValue("@TareaId", tareaId);

                                using (SqlDataReader reader = commandSelectAnexo.ExecuteReader())
                                {
                                    // Verificar si hay resultados antes de intentar obtener el valor
                                    if (reader.Read())
                                    {
                                        // Verificar si no hay un anexo existente
                                        if (!reader.IsDBNull(0))
                                        {
                                            idAnexoExistente = reader.GetInt32(0);

                                            
                                        }
                                    }
                                }
                            }
                            MessageBox.Show("" + idAnexoExistente);
                            // Si ya existe un anexo, actualizar en lugar de insertar
                            if (idAnexoExistente.HasValue)
                            {
                                string updateAnexoQuery = "UPDATE ANEXO_TAREAS SET Archivo = @Archivo, FechaRegistro = @FechaRegistro WHERE IdAnexoTarea = @IdAnexoTarea";

                                using (SqlCommand anexoCommand = new SqlCommand(updateAnexoQuery, connection))
                                {
                                    anexoCommand.Parameters.AddWithValue("@IdAnexoTarea", idAnexoExistente.Value);
                                    anexoCommand.Parameters.AddWithValue("@Archivo", fileBytes);
                                    anexoCommand.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);

                                    anexoCommand.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                // Si no existe un anexo, insertar uno nuevo
                                string insertAnexoQuery = "INSERT INTO ANEXO_TAREAS (RemitenteId, TareaId, Archivo, FechaRegistro, EstadoRegistro)" +
                                                          "VALUES (@RemitenteId, @TareaId, @Archivo, @FechaRegistro, 1)";

                                using (SqlCommand anexoCommand = new SqlCommand(insertAnexoQuery, connection))
                                {
                                    anexoCommand.Parameters.AddWithValue("@RemitenteId", Asignador);
                                    anexoCommand.Parameters.AddWithValue("@TareaId", tareaId);
                                    anexoCommand.Parameters.AddWithValue("@Archivo", fileBytes);
                                    anexoCommand.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);

                                    anexoCommand.ExecuteNonQuery();
                                }
                            }
                        }/*Tarea nuevaTarea = new Tarea
                        {
                            Id = tareaId,
                            RemitenteId = Asignador,
                            DestinatarioId = destinatarioId,
                            CodigoTarea = codigo,
                            EstadoTarea = "Recepcionada",
                            Accion = "Falta hacer???",
                            Prioridad = prioridad,
                            Descripcion = descripcion,
                            FechaAsignacion = fechaAsignacion,
                            FechaLimite = date,
                            FechaFinalizacion = null,  // O ajusta según sea necesario
                            EstadoRegistro = 1  // O ajusta según sea necesario
                        };

                        // Agregar la nueva tarea a la lista existente
                        tareas.Add(nuevaTarea);
                        membersDataGrid.ItemsSource = tareas;*/
                        Close();

                    }
                }

                Descripcion.Text = string.Empty;
                Prioridad.SelectedIndex = 0;
                datepickerTarea.SelectedDate = null;
                Destinatario.SelectedIndex = 0;

            }
            else
            {
                System.Windows.MessageBox.Show("Falta llenar datos");
            }
        }
        catch(Exception x) {
            MessageBox.Show("a:" + x);
        }   
    }
    private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        isDragging = true;
        offset = e.GetPosition(this);
    }

    private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        isDragging = false;
    }

    private void Window_MouseMove(object sender, MouseEventArgs e)
    {
        if (isDragging)
        {
            Point currentMousePosition = e.GetPosition(this);
            double newX = currentMousePosition.X - offset.X + Left;
            double newY = currentMousePosition.Y - offset.Y + Top;

            Left = newX;
            Top = newY;
        }
    }
    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
       
        Close();
    }
}