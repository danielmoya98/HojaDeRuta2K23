using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using HojadeRuta2K23.Windows;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using Microsoft.Data.SqlClient;
using HojadeRuta2K23.Singleton;

namespace HojadeRuta2K23.Paginas;

public partial class Tramites : Page
{
    List<Tramite> listaTramites = new List<Tramite>();
    byte[] fileBytes;
    string ci, nombre, appPaterno, appMaterno, celular, celularRef, correo, diccion, tipoTramite, descripcion;
    public Tramites()
    {
        InitializeComponent();
        CargarTramitesEnDataGrid();
    }
    private void CargarTramitesEnDataGrid()
    {
        listaTramites = ObtenerTramitesDesdeLaBaseDeDatos();
        membersDataGrid.ItemsSource = listaTramites;
    }
    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        if (NavigationService != null)
        {
            NavigationService.Navigate(new NuevaTarea());
        }
    }
    public class Persona
    {
        public int IdPersona { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public int CI { get; set; }
        public int Celular { get; set; }
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

    public class Cliente
    {
        public int IdCliente { get; set; }
        public int PersonaId { get; set; }
        public string Correo { get; set; }
        public string Direccion { get; set; }
        public int CelularReferencia { get; set; }
        public int EstadoRegistro { get; set; }
    }
    private void CheckBox1_OnChecked(object sender, RoutedEventArgs e)
    {
        if (CheckBox1.IsChecked == true)
        {
            tipo.Visibility = Visibility.Visible;
        }

    }

    private void CheckBox1_OnUnchecked(object sender, RoutedEventArgs e)
    {
        if (CheckBox1.IsChecked == false)
        {
            tipo.Visibility = Visibility.Hidden;
        }
    }

    private void Admitir_OnClick(object sender, RoutedEventArgs e)
    {
        BtnAdmitir admitir = new BtnAdmitir();
        admitir.ShowDialog();
    }

    private void vdoc_OnClick(object sender, RoutedEventArgs e)
    {
        DocumentosAdjuntos doc = new DocumentosAdjuntos();
        doc.ShowDialog();
    }

    private void Rechazar_OnClick(object sender, RoutedEventArgs e)
    {
        RechazarTramite tra = new RechazarTramite();
        tra.ShowDialog();
    }

    private void Observar_OnClick(object sender, RoutedEventArgs e)
    {
        ObservarRechazarTramite tra = new ObservarRechazarTramite();
        tra.ShowDialog();
    }

    //BOTÓN PARA CREAR EL TRÁMITE
    private void cod_OnClick(object sender, RoutedEventArgs e)
    {
        ci = txtCI.Text;
        nombre = txtNombre.Text;
        appPaterno = txtApellidoP.Text;
        appMaterno = txtApellidoM.Text;
        celular = txtCelular.Text;
        celularRef = txtCelularRef.Text;
        correo = txtCorreoElectronico.Text;
        diccion = txtDireccion.Text;
        tipoTramite = txtTipoTramite.Text;
        descripcion = txtDescripcionTramite.Text;

        Cod_tramite cod = new Cod_tramite();
        cod.ShowDialog();
    }

    private void MiBotonV_OnClick(object sender, RoutedEventArgs e)
    {
        DoubleAnimation animation = new DoubleAnimation();
        animation.From = 0;
        animation.To = 280;
        animation.Duration = new Duration(TimeSpan.FromSeconds(0.5));

        hola3.BeginAnimation(StackPanel.HeightProperty, animation);
        miBotonV.Visibility = Visibility.Collapsed;
        miBotonV1.Visibility = Visibility.Visible;
    }

    private void MiBotonV1_OnClick(object sender, RoutedEventArgs e)
    {
        DoubleAnimation animation = new DoubleAnimation();
        animation.From = 280;
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

        VistaDetallesTramite.BeginAnimation(StackPanel.WidthProperty, animation);
    }

    private void ocultar_OnClick(object sender, RoutedEventArgs e)
    {
        DoubleAnimation animation = new DoubleAnimation();
        animation.From = 500;
        animation.To = 0;
        animation.Duration = new Duration(TimeSpan.FromSeconds(0.5));

        VistaDetallesTramite.BeginAnimation(StackPanel.WidthProperty, animation);

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

    private void TxtCI_TextChanged(object sender, TextChangedEventArgs e)
    {
        string ciText = txtCI.Text;

        // Verificar si el CI tiene 7 u 8 caracteres
        if (ciText.Length == 6 || ciText.Length == 8)
        {
            // Realizar la búsqueda en la tabla PERSONAS
            if (BuscarPersonaPorCI(ciText, out Persona persona))
            {
                // Si se encontró la persona, llenar los demás campos
                LlenarCamposConPersona(persona);
            }
        }
        if (ciText == "")
        {
            LimpiarCamposAdicionales();
            LimpiarTodosLosCampos();
        }
    }


    private void LlenarCamposConPersona(Persona persona)
    {
        if (persona != null)
        {
            // Llenar los campos con la información de la persona
            txtNombre.Text = persona.Nombres;
            txtApellidoP.Text = persona.ApellidoPaterno;
            txtApellidoM.Text = persona.ApellidoMaterno;
            txtCelular.Text = persona.Celular.ToString();

            // Buscar el cliente asociado a esta persona
            Cliente cliente = BuscarClientePorIdPersona(persona.IdPersona);

            if (cliente != null)
            {
                // Llenar los campos adicionales con la información del cliente
                txtCorreoElectronico.Text = cliente.Correo;
                txtDireccion.Text = cliente.Direccion;
                txtCelularRef.Text = cliente.CelularReferencia.ToString();


               
            }
            else
            {
                // Limpiar los campos adicionales si no se encontró el cliente
                LimpiarCamposAdicionales();
            }
        }
        else
        {
            // Limpiar todos los campos si la persona no se encontró
            LimpiarTodosLosCampos();
        }
    }


    private Cliente BuscarClientePorIdPersona(int idPersona)
    {
        using (var connection = Coneccion.Instance.GetConnection())
        {
            connection.Open();

            string query = "SELECT IdCliente, PersonaId, Correo, Direccion, CelularReferencia " +
                           "FROM CLIENTES " +
                           "WHERE PersonaId = @IdPersona";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@IdPersona", idPersona);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Cliente
                        {
                            IdCliente = reader.GetInt32(0),
                            PersonaId = reader.GetInt32(1),
                            Correo = reader.GetString(2),
                            Direccion = reader.GetString(3),
                            CelularReferencia = reader.GetInt32(4),
                            // Otros campos según tu modelo
                        };
                    }
                }
            }
        }

        return null;
    }

    private void LimpiarCamposAdicionales()
    {
        // Limpiar los campos adicionales
        txtCorreoElectronico.Text = string.Empty;
        txtDireccion.Text = string.Empty;
        txtCelularRef.Text = string.Empty;
        // Otros campos según tu interfaz
    }

    private void LimpiarTodosLosCampos()
    {
        // Limpiar todos los campos
        LimpiarCamposAdicionales();
        // Limpiar los campos de la persona
        txtNombre.Text = string.Empty;
        txtApellidoP.Text = string.Empty;
        txtApellidoM.Text = string.Empty;
        txtCelular.Text = string.Empty;
    }

    private bool BuscarPersonaPorCI(string ci, out Persona persona)
    {
        persona = null;

        try
        {
            using (var connection = Coneccion.Instance.GetConnection())
            {
                connection.Open();

                // Consulta SQL para buscar en la tabla PERSONAS
                string query = "SELECT IdPersona, Nombres, ApellidoPaterno, ApellidoMaterno, CI, Celular " +
                               "FROM PERSONAS " +
                               "WHERE CI = @CI";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CI", ci);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Se encontró la persona, crea un objeto Persona
                            persona = new Persona
                            {
                                IdPersona = reader.GetInt32(0),
                                Nombres = reader.GetString(1),
                                ApellidoPaterno = reader.GetString(2),
                                ApellidoMaterno = reader.GetString(3),
                                CI = reader.GetInt32(4),
                                Celular = reader.GetInt32(5)
                            };

                            return true; // Se encontró la persona
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Manejar la excepción (puedes registrarla, mostrar un mensaje, etc.)
            Console.WriteLine("Error al buscar persona por CI: " + ex.Message);
        }

        return false; // No se encontró la persona o ocurrió un error
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
}