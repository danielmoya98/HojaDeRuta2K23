using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using HojadeRuta2K23.Singleton;
using System.Windows.Media.Imaging;
using HojadeRuta2K23.Windows;
using MaterialDesignThemes.Wpf;
using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using System.Security.Cryptography;

namespace HojadeRuta2K23.Paginas;

public partial class AdministracionUsuarios : Page
{
    private string connectionString = "Server=(LocalDB)\\MSSQLLocalDB; Database=SIVALLE_DB; Integrated Security=true; TrustServerCertificate=true";

    SqlConnection conexion = new SqlConnection("Server= (LocalDB)\\MSSQLLocalDB;Database=SIVALLE_DB;Integrated Security=true; TrustServerCertificate=True");


    public AdministracionUsuarios()
    {
        InitializeComponent();
        // ComboBoxVistas.Items.Add("Vista Cuadriculada");
        // ComboBoxVistas.Items.Add("Vista Lista");
        //
        Combo.Items.Add("Funcionario");
        Combo.Items.Add("cliente");
        Loaded += DataGridPersonas_Load;
    }

    private void CargarDatosPersonas()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = @"SELECT * FROM ObtenerDatosPersonasFC()";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Asignar el DataTable directamente al ItemsSource del DataGrid
                membersDataGrid.ItemsSource = dataTable.DefaultView;
            }
        }
    }

    private void DataGridPersonas_Load(object sender, RoutedEventArgs e)
    {
        CargarDatosPersonas();
    }
    private string CalcularHash(string contraseña)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            // Convertir la contraseña en un array de bytes
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(contraseña));

            // Convertir el array de bytes en una cadena hexadecimal
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
    private byte[] fotoBytesGlobal;
    private byte[] ObtenerBytesDesdeArchivo(string rutaArchivo)
    {
        try
        {
            return File.ReadAllBytes(rutaArchivo);
        }
        catch (IOException ex)
        {
            MessageBox.Show($"Error de entrada/salida al leer el archivo: {ex.Message}");
        }
        catch (UnauthorizedAccessException ex)
        {
            MessageBox.Show($"No se tiene acceso al archivo: {ex.Message}");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al leer el archivo: {ex.Message}");
        }

        return null;
    }

    private void btnFOTOfuncionario_Click_1(object sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.gif";

        if (openFileDialog.ShowDialog() == true)
        {
            // Actualizar dinámicamente el ImageSource del ImageBrush
            imgBrush.ImageSource = new BitmapImage(new Uri(openFileDialog.FileName));

            // Obtener los bytes de la imagen seleccionada
            byte[] fotoBytes = ObtenerBytesDesdeArchivo(openFileDialog.FileName);

            if (fotoBytes != null)
            {
                // Guardar los bytes en una variable global o en el contexto según sea necesario
                // Supongamos que tienes una variable global llamada fotoBytes en tu clase
                fotoBytesGlobal = fotoBytes;
            }
            else
            {
                MessageBox.Show("No se pudieron obtener los bytes de la imagen.");
            }
        }
    }
    private byte[] ObtenerBytesDesdeImagen(byte[] fotoBytesGlobal)
    {
        // Supongamos que la imagen ya está en formato de bytes
        return fotoBytesGlobal;
    }
    private void btnGuardarCliente_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtCi.Text) || string.IsNullOrWhiteSpace(txtname.Text) || string.IsNullOrWhiteSpace(txtAPaterno.Text) ||
            string.IsNullOrWhiteSpace(txtAMaterno.Text) || string.IsNullOrWhiteSpace(txtNumCelular.Text) ||
            string.IsNullOrWhiteSpace(txtCorreo.Text) || string.IsNullOrWhiteSpace(txtCelReferencia.Text) ||
            string.IsNullOrWhiteSpace(txtDireccion.Text) || fotoBytesGlobal == null ||
            string.IsNullOrWhiteSpace(txtusuario1.Text) || string.IsNullOrWhiteSpace(txtcontraseña1.Text))
        {
            MessageBox.Show("Por favor, complete todos los campos antes de guardar.");
            return;
        }

        try
        {
            // Obtener datos del formulario
            string ci = $"{txtCi.Text}";
            string nombre = $"{txtname.Text}";
            string apellidoPaterno = $"{txtAPaterno.Text}";
            string apellidoMaterno = $"{txtAMaterno.Text}";
            int numCelular = int.Parse(txtNumCelular.Text);
            string CorreoCliente = $"{txtCorreo.Text}";
            string Direccion = $"{txtDireccion.Text}";
            int CelReferencia = int.Parse(txtCelReferencia.Text);
            string usuario1 = $"{txtusuario1.Text}";
            string contrasena1 = $"{txtcontraseña1.Text}";


            // Convertir la imagen a bytes si es necesario
            byte[] fotoBytes = ObtenerBytesDesdeImagen(fotoBytesGlobal); // Asegúrate de tener una función que haga esto

            // Llamar al procedimiento almacenado
            using (SqlConnection connection = new SqlConnection("Server= (LocalDB)\\MSSQLLocalDB;Database=SIVALLE_DB;Integrated Security=true; TrustServerCertificate=True"))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("InsertarDatosPersonaClienteLogin", connection))
                {
                    string contraseñaEncriptada = CalcularHash(contrasena1);
                    command.CommandType = CommandType.StoredProcedure;

                    // Parámetros del procedimiento almacenado
                    command.Parameters.AddWithValue("@Nombres", nombre);
                    command.Parameters.AddWithValue("@ApellidoPaterno", apellidoPaterno);
                    command.Parameters.AddWithValue("@ApellidoMaterno", apellidoMaterno);
                    command.Parameters.AddWithValue("@CI", ci);
                    command.Parameters.AddWithValue("@Celular", numCelular);
                    command.Parameters.AddWithValue("@Correo", CorreoCliente);
                    command.Parameters.AddWithValue("@Direccion", Direccion);
                    command.Parameters.AddWithValue("@CelularReferencia", CelReferencia);
                    command.Parameters.AddWithValue("@Usuario", usuario1);
                    command.Parameters.AddWithValue("@Contrasena", contraseñaEncriptada);
                    command.Parameters.AddWithValue("@CodigoVerificacion", ""); // Puedes ajustar según tus necesidades
                    command.Parameters.AddWithValue("@Foto", fotoBytes);

                    // Ejecutar el procedimiento almacenado
                    command.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Los datos se guardaron correctamente");
            LimpiarCampos();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al guardar los datos: {ex.Message}");
        }
    }
    private void btnGuardarFuncionario_Click_1(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtCi.Text) || string.IsNullOrWhiteSpace(txtname.Text) || string.IsNullOrWhiteSpace(txtAPaterno.Text) ||
            string.IsNullOrWhiteSpace(txtAMaterno.Text) || string.IsNullOrWhiteSpace(txtNumCelular.Text) ||
            string.IsNullOrWhiteSpace(txtCorreoInstitucional.Text) || fotoBytesGlobal == null ||
            string.IsNullOrWhiteSpace(txtusuario.Text) || string.IsNullOrWhiteSpace(txtcontraseña.Text))
        {
            MessageBox.Show("Por favor, complete todos los campos antes de guardar.");
            return;
        }

        try
        {
            // Obtener datos del formulario
            string ci = $"{txtCi.Text}";
            string nombre = $"{txtname.Text}";
            string apellidoPaterno = $"{txtAPaterno.Text}";
            string apellidoMaterno = $"{txtAMaterno.Text}";
            int numCelular = int.Parse(txtNumCelular.Text);
            string correoInstitucional = $"{txtCorreoInstitucional.Text}";
            string usuario = $"{txtusuario.Text}";
            string contrasena = $"{txtcontraseña.Text}";

            // Convertir la imagen a bytes si es necesario
            byte[] fotoBytes = ObtenerBytesDesdeImagen(fotoBytesGlobal); // Asegúrate de tener una función que haga esto

            // Llamar al procedimiento almacenado
            using (SqlConnection connection = new SqlConnection("Server=(LocalDB)\\MSSQLLocalDB;Database=SIVALLE_DB;Integrated Security=true; TrustServerCertificate=True"))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("InsertarDatosPersonaFuncionarioLogin", connection))
                {
                    string contraseñaEncriptada = CalcularHash(contrasena);
                    command.CommandType = CommandType.StoredProcedure;

                    // Parámetros del procedimiento almacenado
                    command.Parameters.AddWithValue("@Nombres", nombre);
                    command.Parameters.AddWithValue("@ApellidoPaterno", apellidoPaterno);
                    command.Parameters.AddWithValue("@ApellidoMaterno", apellidoMaterno);
                    command.Parameters.AddWithValue("@CI", ci);
                    command.Parameters.AddWithValue("@Celular", numCelular);
                    command.Parameters.AddWithValue("@CorreoInstitucional", correoInstitucional);
                    command.Parameters.AddWithValue("@Usuario", usuario);
                    command.Parameters.AddWithValue("@Contrasena", contraseñaEncriptada);
                    command.Parameters.AddWithValue("@CodigoVerificacion", ""); // Puedes ajustar según tus necesidades
                    command.Parameters.AddWithValue("@Foto", fotoBytes);

                    // Ejecutar el procedimiento almacenado
                    command.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Los datos se guardaron correctamente");
            LimpiarCampos();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al guardar los datos: {ex.Message}");
        }
    }

    private void Combo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (Combo.SelectedIndex == 0)
        {
            funcionario.Visibility = Visibility.Visible;
            cliente.Visibility = Visibility.Collapsed;
            btnGuardarCliente.Visibility = Visibility.Collapsed;
            btnGuardarFuncionario.Visibility = Visibility.Visible;

        }
        if (Combo.SelectedIndex == 1)
        {
            cliente.Visibility = Visibility.Visible;
            funcionario.Visibility = Visibility.Collapsed;
            btnGuardarCliente.Visibility = Visibility.Visible;
            btnGuardarFuncionario.Visibility = Visibility.Collapsed;

        }
    }

    private void CrearPerson_OnClick(object sender, RoutedEventArgs e)
    {
        DoubleAnimation animation = new DoubleAnimation();
        animation.From = 0;
        animation.To = 250;
        animation.Duration = new Duration(TimeSpan.FromSeconds(0.5));

        hola3.BeginAnimation(StackPanel.HeightProperty, animation);
        funcionario.Visibility = Visibility.Visible;
        cliente.Visibility = Visibility.Collapsed;
        CrearPerson.Visibility = Visibility.Collapsed;
        OcultarCrearPerson.Visibility = Visibility.Visible;
        btnGuardarCliente.Visibility = Visibility.Collapsed;
        btnGuardarFuncionario.Visibility = Visibility.Visible;
        OcultarCrear.Visibility = Visibility.Visible;
        EditarFuncionario.Visibility = Visibility.Collapsed;
        EditarCliente.Visibility = Visibility.Collapsed;
        PERSON6.Visibility = Visibility.Visible;
        PERSON7.Visibility = Visibility.Collapsed;
        LimpiarCampos();
    }
    private void OcultarCrearPerson_OnClick(object sender, RoutedEventArgs e)
    {
        DoubleAnimation animation = new DoubleAnimation();
        animation.From = 250;
        animation.To = 0;
        animation.Duration = new Duration(TimeSpan.FromSeconds(0.5));

        hola3.BeginAnimation(StackPanel.HeightProperty, animation);
        CrearPerson.Visibility = Visibility.Visible;
        OcultarCrearPerson.Visibility = Visibility.Collapsed;
        OcultarCrear.Visibility = Visibility.Visible;
        funcionario.Visibility = Visibility.Visible;
        cliente.Visibility = Visibility.Collapsed;
        PERSON6.Visibility = Visibility.Visible;
        PERSON7.Visibility = Visibility.Collapsed;

    }

    private void MiBotonV_OnClick(object sender, RoutedEventArgs e)
    {
        DoubleAnimation animation = new DoubleAnimation();
        animation.From = 0;
        animation.To = 250;
        animation.Duration = new Duration(TimeSpan.FromSeconds(0.5));

        hola.BeginAnimation(StackPanel.WidthProperty, animation);
        // miBotonV1.Visibility = Visibility.Visible;
        // miBotonV.Visibility = Visibility.Collapsed;
    }

    private void MiBotonV1_OnClick(object sender, RoutedEventArgs e)
    {
        DoubleAnimation animation = new DoubleAnimation();
        animation.From = 250;
        animation.To = 0;
        animation.Duration = new Duration(TimeSpan.FromSeconds(0.5));

        hola.BeginAnimation(StackPanel.WidthProperty, animation);
        // miBotonV.Visibility = Visibility.Visible;
        // miBotonV1.Visibility = Visibility.Collapsed;
    }

    private void editarUsuario_OnClick(object sender, RoutedEventArgs e)
    {
        btnGuardarCliente.Visibility = Visibility.Collapsed;
        EditarFuncionario.Visibility = Visibility.Visible;
        EditarCliente.Visibility = Visibility.Visible;
        DoubleAnimation animation = new DoubleAnimation();
        animation.From = 0;
        animation.To = 250;
        animation.Duration = new Duration(TimeSpan.FromSeconds(0.5));

        hola3.BeginAnimation(StackPanel.HeightProperty, animation);
    }

    private void OcultarCrear_OnClick(object sender, RoutedEventArgs e)
    {
        DoubleAnimation animation = new DoubleAnimation();
        animation.From = 250;
        animation.To = 0;
        animation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
        hola3.BeginAnimation(StackPanel.HeightProperty, animation);
    }
    private void OcultarVerDatos_OnClick(object sender, RoutedEventArgs e)
    {
        DoubleAnimation animation = new DoubleAnimation();
        animation.From = 250;
        animation.To = 0;
        animation.Duration = new Duration(TimeSpan.FromSeconds(0.5));

        hola.BeginAnimation(StackPanel.WidthProperty, animation);
    }
    private void EditarPersonaFuncionarioCliente_OnClick(object sender, RoutedEventArgs e)
    {

        DoubleAnimation animation = new DoubleAnimation();
        animation.From = 0;
        animation.To = 250;
        animation.Duration = new Duration(TimeSpan.FromSeconds(0.5));

        hola3.BeginAnimation(StackPanel.HeightProperty, animation);

        // Obtener el IdPersona desde algún lugar (puede ser seleccionado desde una lista, etc.)
        int idPersonaSeleccionada = ObtenerIdPersonaSeleccionada();


        // Consultar la base de datos para obtener los datos de la persona
        string consulta = "SELECT * FROM ObtenerDatosPersonasFC() WHERE IdPersona = @IdPersona";

        try
        {
            using (SqlConnection connection = new SqlConnection("Server=(LocalDB)\\MSSQLLocalDB;Database=SIVALLE_DB;Integrated Security=true; TrustServerCertificate=True"))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(consulta, connection))
                {
                    // Agregar el parámetro
                    command.Parameters.AddWithValue("@IdPersona", idPersonaSeleccionada);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Asignar los valores a los TextBox
                            txtid.Content = reader["IdPersona"].ToString();
                            txtCi.Text = reader["CI"].ToString();
                            txtname.Text = reader["Nombres"].ToString();
                            txtAPaterno.Text = reader["ApellidoPaterno"].ToString();
                            txtAMaterno.Text = reader["ApellidoMaterno"].ToString();
                            txtNumCelular.Text = reader["Celular"].ToString();
                            EstadoRegistroPersona.SelectedItem = reader["EstadoRegistro"].ToString();

                            if (EsCliente(idPersonaSeleccionada))
                            {
                                // Mostrar componentes específicos para clientes
                                txtCorreo.Text = reader["Correo"].ToString();
                                txtDireccion.Text = reader["DireccionCliente"].ToString();
                                txtCelReferencia.Text = reader["CelularReferenciaCliente"].ToString();
                                EstadoRegistroCliente.SelectedItem = reader["EstadoRegistro"].ToString();
                                txtcontraseña.Text = reader["Contrasena"].ToString();
                                txtusuario.Text = reader["Usuario"].ToString();
                                // Asignar la imagen a imgBrush
                                byte[] fotoBytes = (byte[])reader["Foto"];
                                if (fotoBytes != null && fotoBytes.Length > 0)
                                {
                                    using (MemoryStream stream = new MemoryStream(fotoBytes))
                                    {
                                        BitmapImage image = new BitmapImage();
                                        image.BeginInit();
                                        image.CacheOption = BitmapCacheOption.OnLoad;
                                        image.StreamSource = stream;
                                        image.EndInit();

                                        imgBrush.ImageSource = image;
                                    }
                                }
                                else
                                {
                                    // Si no hay imagen, puedes asignar un valor predeterminado o manejarlo según tus necesidades
                                    imgBrush.ImageSource = null; // o asigna una imagen por defecto
                                }


                                // Ocultar componentes específicos para clientes
                                cliente.Visibility = Visibility.Visible;
                                // Ocultar componentes específicos para funcionarios
                                funcionario.Visibility = Visibility.Collapsed;
                                EditarFuncionario.Visibility = Visibility.Collapsed;
                                EditarCliente.Visibility = Visibility.Visible;
                                btnGuardarFuncionario.Visibility = Visibility.Collapsed;
                                btnGuardarCliente.Visibility = Visibility.Collapsed;
                                OcultarCrear.Visibility = Visibility.Visible;
                                PERSON6.Visibility = Visibility.Collapsed;
                                PERSON7.Visibility = Visibility.Visible;

                            }
                            else
                            {
                                // Mostrar componentes específicos para funcionarios
                                txtCorreoInstitucional.Text = reader["Correo"].ToString();
                                EstadoFuncioanrio.SelectedItem = reader["EstadoRegistro"].ToString();
                                EstadoRegistroFuncioanrio.SelectedItem = reader["EstadoRegistro"].ToString();
                                txtcontraseña1.Text = reader["Contrasena"].ToString();
                                txtusuario1.Text = reader["Usuario"].ToString();
                                // Asignar la imagen a imgBrush
                                byte[] fotoBytes = (byte[])reader["Foto"];
                                if (fotoBytes != null && fotoBytes.Length > 0)
                                {
                                    using (MemoryStream stream = new MemoryStream(fotoBytes))
                                    {
                                        BitmapImage image = new BitmapImage();
                                        image.BeginInit();
                                        image.CacheOption = BitmapCacheOption.OnLoad;
                                        image.StreamSource = stream;
                                        image.EndInit();

                                        imgBrush.ImageSource = image;
                                    }
                                }
                                else
                                {
                                    // Si no hay imagen, puedes asignar un valor predeterminado o manejarlo según tus necesidades
                                    imgBrush.ImageSource = null; // o asigna una imagen por defecto
                                }

                                // Ocultar componentes específicos para clientes
                                cliente.Visibility = Visibility.Collapsed;

                                // Ocultar componentes específicos para funcionarios
                                funcionario.Visibility = Visibility.Visible;
                                OcultarCrear.Visibility = Visibility.Visible;
                                EditarFuncionario.Visibility = Visibility.Visible;
                                EditarCliente.Visibility = Visibility.Collapsed;
                                btnGuardarFuncionario.Visibility = Visibility.Collapsed;
                                btnGuardarCliente.Visibility = Visibility.Collapsed;
                                PERSON6.Visibility = Visibility.Collapsed;
                                PERSON7.Visibility = Visibility.Visible;

                            }
                        }
                        else
                        {
                            MessageBox.Show("No se encontraron datos para la persona seleccionada.");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al obtener datos de la persona: {ex.Message}");
        }
    }

    private bool EsCliente(int idPersona)
    {
        // Lógica para determinar si la persona con el id especificado es un cliente
        // Puedes realizar una consulta a la base de datos u otro método según tus necesidades
        // Aquí se asume que existe una columna llamada "IdCliente" en la tabla CLIENTES

        using (SqlConnection connection = new SqlConnection("Server= (LocalDB)\\MSSQLLocalDB;Database=SIVALLE_DB;Integrated Security=true; TrustServerCertificate=True"))
        {
            connection.Open();

            string query = $"SELECT IdCliente FROM CLIENTES WHERE PersonaId = {idPersona}";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                object result = command.ExecuteScalar();
                return result != null && result != DBNull.Value;
            }
        }
    }

    private int ObtenerIdPersonaSeleccionada()
    {
        if (membersDataGrid.SelectedItem != null)
        {
            // Obtener la fila seleccionada desde el DataGrid
            DataRowView selectedRow = (DataRowView)membersDataGrid.SelectedItem;

            // Obtener el valor del campo IdPersona de la fila seleccionada
            if (selectedRow.Row["IdPersona"] != DBNull.Value)
            {
                return Convert.ToInt32(selectedRow.Row["IdPersona"]);
            }
        }

        // Devolver un valor predeterminado o lanzar una excepción según tus necesidades
        return -1; // O ajusta según tus necesidades
    }

    private void BorrarPersonaFuncionarioCliente_OnClick(object sender, RoutedEventArgs e)
    {

    }
    private void ActualizarEstadoRegistro(int idPersona)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Sentencia SQL para actualizar el estado a "Inactivo" para la persona específica
                string updateQuery = $"UPDATE PERSONAS SET EstadoRegistro = 'Inactivo' WHERE IdPersona = {idPersona}";

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    // Ejecutar la sentencia SQL
                    int rowsAffected = command.ExecuteNonQuery();

                    // Comprobar si se actualizaron registros
                    if (rowsAffected > 0)
                    {
                        // Éxito: Registros actualizados
                        MessageBox.Show("Se actualizó el registro correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        // Advertencia: No se actualizó ningún registro
                        MessageBox.Show("No se actualizó ningún registro.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Manejar cualquier error que pueda ocurrir durante la ejecución
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    private void VerPersoanFuncionarioCliente_OnClick(object sender, RoutedEventArgs e)
    {
        // Ocultar o mostrar los componentes según sea necesario

        DoubleAnimation animation = new DoubleAnimation();
        animation.From = 0;
        animation.To = 250;
        animation.Duration = new Duration(TimeSpan.FromSeconds(0.5));

        hola.BeginAnimation(StackPanel.WidthProperty, animation);

        // Obtener el IdPersona desde algún lugar (puede ser seleccionado desde una lista, etc.)
        int idPersonaSeleccionada = ObtenerIdPersonaSeleccionada();


        // Consultar la base de datos para obtener los datos de la persona
        string consulta = "SELECT * FROM ObtenerDatosPersonasFC() WHERE IdPersona = @IdPersona";

        try
        {
            using (SqlConnection connection = new SqlConnection("Server=(LocalDB)\\MSSQLLocalDB;Database=SIVALLE_DB;Integrated Security=true; TrustServerCertificate=True"))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(consulta, connection))
                {
                    // Agregar el parámetro
                    command.Parameters.AddWithValue("@IdPersona", idPersonaSeleccionada);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Asignar los valores a los TextBox
                            ciPerson.Content = reader["CI"].ToString();
                            nameComplet.Content = $"{reader["Nombres"]} {reader["ApellidoPaterno"]} {reader["ApellidoMaterno"]}".Trim();
                            namePerson.Content = reader["Nombres"].ToString();
                            celPerson.Content = reader["Celular"].ToString();
                            EstadoRegistroPersona.SelectedItem = reader["EstadoRegistro"].ToString();

                            if (EsCliente(idPersonaSeleccionada))
                            {
                                // Mostrar componentes específicos para clientes
                                emailPerson.Content = reader["Correo"].ToString();
                                ubiPerson.Content = reader["DireccionCliente"].ToString();

                                byte[] fotoBytes = (byte[])reader["Foto"];
                                if (fotoBytes != null && fotoBytes.Length > 0)
                                {
                                    using (MemoryStream stream = new MemoryStream(fotoBytes))
                                    {
                                        BitmapImage image = new BitmapImage();
                                        image.BeginInit();
                                        image.CacheOption = BitmapCacheOption.OnLoad;
                                        image.StreamSource = stream;
                                        image.EndInit();

                                        imgBrush1.ImageSource = image;
                                    }
                                }
                                else
                                {
                                    // Si no hay imagen, puedes asignar un valor predeterminado o manejarlo según tus necesidades
                                    imgBrush1.ImageSource = null; // o asigna una imagen por defecto
                                }


                                // Ocultar componentes específicos para clientes
                                UBIPEROSN1.Visibility = Visibility.Visible;
                                animation.From = 250;
                                animation.To = 0;
                                animation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                                hola3.BeginAnimation(StackPanel.HeightProperty, animation);

                            }
                            else
                            {
                                emailPerson.Content = reader["Correo"].ToString();

                                byte[] fotoBytes = (byte[])reader["Foto"];
                                if (fotoBytes != null && fotoBytes.Length > 0)
                                {
                                    using (MemoryStream stream = new MemoryStream(fotoBytes))
                                    {
                                        BitmapImage image = new BitmapImage();
                                        image.BeginInit();
                                        image.CacheOption = BitmapCacheOption.OnLoad;
                                        image.StreamSource = stream;
                                        image.EndInit();

                                        imgBrush1.ImageSource = image;
                                    }
                                }
                                else
                                {
                                    // Si no hay imagen, puedes asignar un valor predeterminado o manejarlo según tus necesidades
                                    imgBrush1.ImageSource = null; // o asigna una imagen por defecto
                                }

                                // Ocultar componentes específicos para clientes
                                UBIPEROSN1.Visibility = Visibility.Collapsed;
                                animation.From = 250;
                                animation.To = 0;
                                animation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                                hola3.BeginAnimation(StackPanel.HeightProperty, animation);
                                // Ocultar componentes específicos para funcionarios
                                //funcionario.Visibility = Visibility.Visible;

                            }
                        }
                        else
                        {
                            MessageBox.Show("No se encontraron datos para la persona seleccionada.");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al obtener datos de la persona: {ex.Message}");
        }
    }
    private void EditarCliente_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            int idPersona = ObtenerIdPersonaEdit();

            if (idPersona == -1)
            {
                MessageBox.Show("Por favor, seleccione un funcionario para editar.");
                return;
            }

            string ci = $"{txtCi.Text}";
            //int ci = int.Parse(txtCi.Text);
            string nombre = $"{txtname.Text}";
            string apellidoPaterno = $"{txtAPaterno.Text}";
            string apellidoMaterno = $"{txtAMaterno.Text}";
            string numCelular = $"{txtNumCelular.Text}";
            //int numCelular = int.Parse(txtNumCelular.Text);

            string correo = $"{txtCorreo.Text}";
            string direccion = $"{txtDireccion.Text}";
            string celReferencia = $"{txtCelReferencia.Text}";
            //int celReferencia = int.Parse(txtCelReferencia.Text);

            string usuario1 = $"{txtusuario1.Text}";
            string contrasena1 = $"{txtcontraseña1.Text}";



            // Convertir la imagen a bytes si es necesario
            byte[] fotoBytes = ObtenerBytesDesdeImagen(fotoBytesGlobal); // Asegúrate de tener una función que haga esto

            // Llamar al procedimiento almacenado de actualización
            using (SqlConnection connection = new SqlConnection("Server=(LocalDB)\\MSSQLLocalDB;Database=SIVALLE_DB;Integrated Security=true; TrustServerCertificate=True"))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("EditarDatosUsuarioCliente", connection))
                {
                    string contraseñaEncriptada = CalcularHash(contrasena1);
                    command.CommandType = CommandType.StoredProcedure;

                    // Parámetros del procedimiento almacenado
                    command.Parameters.AddWithValue("@IdPersona", idPersona);
                    command.Parameters.AddWithValue("@Nombres", nombre);
                    command.Parameters.AddWithValue("@ApellidoPaterno", apellidoPaterno);
                    command.Parameters.AddWithValue("@ApellidoMaterno", apellidoMaterno);
                    command.Parameters.AddWithValue("@CI", ci);
                    command.Parameters.AddWithValue("@Celular", numCelular);
                    command.Parameters.AddWithValue("@EstadoRegistroPersona", 2);

                    command.Parameters.AddWithValue("@Correo", correo);
                    command.Parameters.AddWithValue("@Direccion", direccion);
                    command.Parameters.AddWithValue("@CelularReferencia", celReferencia);
                    command.Parameters.AddWithValue("@EstadoRegistroCliente", 2);
                    command.Parameters.AddWithValue("@Foto", fotoBytes);

                    command.Parameters.AddWithValue("@Usuario", usuario1);
                    command.Parameters.AddWithValue("@Contrasena", contraseñaEncriptada);
                    command.Parameters.AddWithValue("@EstadoRegistroLogin", 1);
                    command.Parameters.AddWithValue("@CodigoVerificacion", "");

                    // Ejecutar el procedimiento almacenado
                    command.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Los datos se actualizaron correctamente");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al actualizar los datos: {ex.Message}");
        }

    }
    private int ObtenerIdPersonaEdit()
    {
        // Obtener el valor del Label txtid
        if (!string.IsNullOrWhiteSpace(txtid.Content?.ToString()))
        {
            if (int.TryParse(txtid.Content.ToString(), out int idPersona))
            {
                return idPersona;
            }
        }

        // Devolver un valor predeterminado o lanzar una excepción según tus necesidades
        return -1; // O ajusta según tus necesidades
    }

    private void EditarFuncionario_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            int idPersona = ObtenerIdPersonaEdit();

            if (idPersona == -1)
            {
                MessageBox.Show("Por favor, seleccione un funcionario para editar.");
                return;
            }

            string ci = $"{txtCi.Text}";
            string nombre = $"{txtname.Text}";
            string apellidoPaterno = $"{txtAPaterno.Text}";
            string apellidoMaterno = $"{txtAMaterno.Text}";
            string numCelular = $"{txtNumCelular.Text}";

            //int numCelular = int.Parse(txtNumCelular.Text);

            string correoInstitucional = $"{txtCorreoInstitucional.Text}";
            string usuario = $"{txtusuario.Text}";
            string contrasena = $"{txtcontraseña.Text}";

            // Convertir la imagen a bytes si es necesario
            byte[] fotoBytes = ObtenerBytesDesdeImagen(fotoBytesGlobal); // Asegúrate de tener una función que haga esto

            // Llamar al procedimiento almacenado de actualización
            using (SqlConnection connection = new SqlConnection("Server=(LocalDB)\\MSSQLLocalDB;Database=SIVALLE_DB;Integrated Security=true; TrustServerCertificate=True"))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("EditarDatosUsuarioFuncionario", connection))
                {
                    string contraseñaEncriptada = CalcularHash(contrasena);
                    command.CommandType = CommandType.StoredProcedure;

                    // Parámetros del procedimiento almacenado
                    command.Parameters.AddWithValue("@IdPersona", idPersona);
                    command.Parameters.AddWithValue("@Nombres", nombre);
                    command.Parameters.AddWithValue("@ApellidoPaterno", apellidoPaterno);
                    command.Parameters.AddWithValue("@ApellidoMaterno", apellidoMaterno);
                    command.Parameters.AddWithValue("@CI", ci);
                    command.Parameters.AddWithValue("@Celular", numCelular);
                    command.Parameters.AddWithValue("@EstadoRegistroPersona", 2);

                    command.Parameters.AddWithValue("@CorreoInstitucional", correoInstitucional);
                    command.Parameters.AddWithValue("@EstadoFuncionario", "asuente");
                    command.Parameters.AddWithValue("@EstadoRegistroFuncionario", 2);
                    command.Parameters.AddWithValue("@Foto", fotoBytes);

                    command.Parameters.AddWithValue("@Usuario", usuario);
                    command.Parameters.AddWithValue("@Contrasena", contraseñaEncriptada);
                    command.Parameters.AddWithValue("@EstadoRegistroLogin", 1);
                    command.Parameters.AddWithValue("@CodigoVerificacion", "");

                    // Ejecutar el procedimiento almacenado
                    command.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Los datos se actualizaron correctamente");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al actualizar los datos: {ex.Message}");
        }
    }

    private void btnActualizarUsuario_Click(object sender, RoutedEventArgs e)
    {
        // Obtén los nuevos valores de los TextBox y ComboBox en la página SubVClientes4
        //string nuevoCI = txtidUsuarios.Text;
        //string nuevosNombres = txtNombreUsuarios.Text;
        //string nuevosApellidos = txtApellidosUsuarios.Text;
        //string nuevoUserName = txtUserNameUsuarios.Text;
        //string nuevaContraseña = PassContraseñaNuevaUsuarios.Password; // Nueva contraseña ingresada por el usuario
        //string nuevosCargo = cbxCargoUsuarios.Text;
        //string nuevosEstado = cbxEstado.Text;

        //int estado = 0; // Valor predeterminado, debes definir tus propios valores numéricos

        // Asignar el valor numérico correspondiente al ComboBox de Estado
        /*if (nuevosEstado == "Activo")
        {
            estado = 1;
        }
        else if (nuevosEstado == "Inactivo")
        {
            estado = 2;
        }
        else if (nuevosEstado == "Ausente")
        {
            estado = 3;
        }

        string connectionString = "Server=(LocalDB)\\MSSQLLocalDB; database=BDInventarioVenta; integrated security=true; TrustServerCertificate=true";

        using (SqlConnection conexion = new SqlConnection(connectionString))
        {
            conexion.Open();

            // Verificar si la contraseña actual es correcta antes de continuar
            if (!VerificarContraseñaActual(nuevoCI, PassContraseñaActualUsuarios.Password))
            {
                MessageBox.Show("Es necesario la contraseña actual. No se puede realizar la actualización.");
                return; // Detener la actualización si la contraseña actual es incorrecta
            }

            // Construye la consulta SQL de actualización para los datos del usuario (nombres, apellidos, cargo, estado)
            string consulta = "UPDATE Usuario SET Nombre = @Nombres, Apellidos = @Apellidos, Cargo = @Cargo, Estado = @Estado, UserName = @UserName WHERE CI = @CI";
            SqlCommand comando = new SqlCommand(consulta, conexion);

            // Asigna los nuevos valores como parámetros
            comando.Parameters.AddWithValue("@Nombres", nuevosNombres);
            comando.Parameters.AddWithValue("@Apellidos", nuevosApellidos);
            comando.Parameters.AddWithValue("@Cargo", nuevosCargo);
            comando.Parameters.AddWithValue("@Estado", estado);
            comando.Parameters.AddWithValue("@UserName", nuevoUserName);
            comando.Parameters.AddWithValue("@CI", nuevoCI);

            int filasActualizadas = comando.ExecuteNonQuery();

            if (filasActualizadas > 0)
            {
                MessageBox.Show("Los datos se actualizaron correctamente");
                LimpiarCampos();
                //mainFrame.NavigationService.Navigate(new SubVUsuario2(mainFrame));

            }
            else
            {
                MessageBox.Show("No se encontró ningún usuario con el número de cédula (CI) proporcionado");
            }

            // Si se proporciona una nueva contraseña, actualiza la contraseña también
            if (!string.IsNullOrEmpty(nuevaContraseña))
            {
                // Calcula el hash de la nueva contraseña
                string nuevaContraseñaHash = CalcularHash(nuevaContraseña);

                // Construye una consulta SQL para actualizar la contraseña
                string consultaContraseña = "UPDATE Usuario SET Contraseña = @Contraseña WHERE CI = @CI";
                SqlCommand comandoContraseña = new SqlCommand(consultaContraseña, conexion);

                // Asigna la nueva contraseña hasheada como parámetro
                comandoContraseña.Parameters.AddWithValue("@Contraseña", nuevaContraseñaHash);
                comandoContraseña.Parameters.AddWithValue("@CI", nuevoCI);

                int filasActualizadasContraseña = comandoContraseña.ExecuteNonQuery();

                if (filasActualizadasContraseña > 0)
                {
                    MessageBox.Show("La contraseña se actualizó correctamente");
                }
                else
                {
                    MessageBox.Show("No se encontró ningún usuario con el número de cédula (CI) proporcionado");
                }
            }
        }*/
    }
    private bool VerificarContraseñaActual(string ci, string contraseñaActual)
    {
        string connectionString = "Server=(LocalDB)\\MSSQLLocalDB; database=BDInventarioVenta; integrated security=true; TrustServerCertificate=true";

        using (SqlConnection conexion = new SqlConnection(connectionString))
        {
            conexion.Open();
            string consulta = "SELECT Contraseña FROM Usuario WHERE CI = @CI";
            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@CI", ci);

            SqlDataReader reader = comando.ExecuteReader();

            if (reader.Read())
            {
                string contraseñaAlmacenada = reader["Contraseña"].ToString();
                // Verificar si la contraseña actual coincide con la contraseña almacenada
                return VerificarContraseña(contraseñaActual, contraseñaAlmacenada);
            }

            // Si no se encontró un usuario con el CI proporcionado, devuelve falso
            return false;
        }
    }
    private bool VerificarContraseña(string contraseña, string hash)
    {
        // Calcular el hash de la contraseña ingresada
        string contraseñaIngresadaHash = CalcularHash(contraseña);

        // Comparar los hashes
        StringComparer comparer = StringComparer.OrdinalIgnoreCase;
        return comparer.Compare(contraseñaIngresadaHash, hash) == 0;
    }
    private void LimpiarCampos()
    {
        txtCi.Text = "";
        txtname.Text = "";
        txtAMaterno.Text = "";
        txtAPaterno.Text = "";
        txtNumCelular.Text = "";
        txtCelReferencia.Text = "";
        txtDireccion.Text = "";
        txtCorreoInstitucional.Text = "";
        txtCorreo.Text = "";
        txtcontraseña.Text = "";
        txtcontraseña1.Text = "";
        txtusuario.Text = "";
        txtusuario1.Text = "";
        imgBrush.ImageSource = null;
        imgBrush1.ImageSource = null;
    }
    private void EstadoRegistroPersona_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Verifica que se haya seleccionado algún elemento
        if (e.AddedItems.Count > 0)
        {
            // Obtén el ComboBoxItem seleccionado
            ComboBoxItem selectedItem = (ComboBoxItem)e.AddedItems[0];

            // Obtén el contenido del ComboBoxItem (estado)
            string estadoSeleccionado = selectedItem.Content.ToString();

            // Asigna un valor numérico a cada estado
            int estadoNumerico = -1; // Valor por defecto o un valor que no tenga significado específico

            switch (estadoSeleccionado)
            {
                case "Activo":
                    estadoNumerico = 1;
                    break;

                case "Inactivo":
                    estadoNumerico = 2;
                    break;

                case "Ausente":
                    estadoNumerico = 3;
                    break;

                // Puedes agregar más casos según tus necesidades

                default:
                    // Manejar el caso por defecto si el estado no coincide con ninguno de los casos anteriores
                    break;
            }

            // Ahora puedes usar el valor de estadoNumerico según tus necesidades
            Console.WriteLine($"Estado Numérico: {estadoNumerico}");
        }
    }
    private void EstadoFuncionario_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Verifica que se haya seleccionado algún elemento
        if (e.AddedItems.Count > 0)
        {
            // Obtén el ComboBoxItem seleccionado
            ComboBoxItem selectedItem = (ComboBoxItem)e.AddedItems[0];

            // Obtén el contenido del ComboBoxItem (estado)
            string estadoSeleccionado = selectedItem.Content.ToString();

            // Asigna un valor numérico a cada estado
            int estadoNumerico = -1; // Valor por defecto o un valor que no tenga significado específico

            switch (estadoSeleccionado)
            {
                case "Activo":
                    estadoNumerico = 1;
                    break;

                case "Inactivo":
                    estadoNumerico = 2;
                    break;

                case "Ausente":
                    estadoNumerico = 3;
                    break;

                // Puedes agregar más casos según tus necesidades

                default:
                    // Manejar el caso por defecto si el estado no coincide con ninguno de los casos anteriores
                    break;
            }

            // Ahora puedes usar el valor de estadoNumerico según tus necesidades
            Console.WriteLine($"Estado Numérico: {estadoNumerico}");
        }
    }
    private void EstadoRegistroFuncionario_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Verifica que se haya seleccionado algún elemento
        if (e.AddedItems.Count > 0)
        {
            // Obtén el ComboBoxItem seleccionado
            ComboBoxItem selectedItem = (ComboBoxItem)e.AddedItems[0];

            // Obtén el contenido del ComboBoxItem (estado)
            string estadoSeleccionado = selectedItem.Content.ToString();

            // Asigna un valor numérico a cada estado
            int estadoNumerico = -1; // Valor por defecto o un valor que no tenga significado específico

            switch (estadoSeleccionado)
            {
                case "Activo":
                    estadoNumerico = 1;
                    break;

                case "Inactivo":
                    estadoNumerico = 2;
                    break;

                case "Ausente":
                    estadoNumerico = 3;
                    break;

                // Puedes agregar más casos según tus necesidades

                default:
                    // Manejar el caso por defecto si el estado no coincide con ninguno de los casos anteriores
                    break;
            }

            // Ahora puedes usar el valor de estadoNumerico según tus necesidades
            Console.WriteLine($"Estado Numérico: {estadoNumerico}");
        }
    }
    private void EstadoRegistroCliente_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Verifica que se haya seleccionado algún elemento
        if (e.AddedItems.Count > 0)
        {
            // Obtén el ComboBoxItem seleccionado
            ComboBoxItem selectedItem = (ComboBoxItem)e.AddedItems[0];

            // Obtén el contenido del ComboBoxItem (estado)
            string estadoSeleccionado = selectedItem.Content.ToString();

            // Asigna un valor numérico a cada estado
            int estadoNumerico = -1; // Valor por defecto o un valor que no tenga significado específico

            switch (estadoSeleccionado)
            {
                case "Activo":
                    estadoNumerico = 1;
                    break;

                case "Inactivo":
                    estadoNumerico = 2;
                    break;

                case "Ausente":
                    estadoNumerico = 3;
                    break;

                // Puedes agregar más casos según tus necesidades

                default:
                    // Manejar el caso por defecto si el estado no coincide con ninguno de los casos anteriores
                    break;
            }

            // Ahora puedes usar el valor de estadoNumerico según tus necesidades
            Console.WriteLine($"Estado Numérico: {estadoNumerico}");
        }
    }

}