﻿using System;
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
using System.Linq;
using static HojadeRuta2K23.Paginas.Tareas;
using static HojadeRuta2K23.Paginas.Tramites;
using static HojadeRuta2K23.Paginas.Buscador;
using System.Runtime.InteropServices;
using FireSharp;
using FireSharp.Config;
using System.Threading.Tasks;

namespace HojadeRuta2K23.Paginas;

public partial class Tramites : Page
{
    public List<Tramite> listaTramites = new List<Tramite>();
    public List<Persona> listaPersonas = new List<Persona>();
    public List<Cliente> listaCliente = new List<Cliente>();
    public byte[] fileBytes = new byte[] { 0x00 };
    public bool isDetailsVisible = false;
    public string ci, nombre, appPaterno, appMaterno, celular, celularRef, correo, diccion, descripcion;
    public int tipoTramite;

    public FirebaseConfig config = new FirebaseConfig
    {
        AuthSecret = "J9UqvtxZbtcl8hEDkZvVkPnfQOsTbyS5iwNbXtuF",
        BasePath = "https://tramites-y-tareas-default-rtdb.firebaseio.com/"
    };
    public FirebaseClient client;
    public Tramites()
    {
        InitializeComponent();
        CargarTramitesEnDataGrid();
        ObtenerPersonasDesdeLaBaseDeDatos
            ();
        ObtenerClientesDesdeLaBaseDeDatos();

        CargarDatosComboBoxTipoTramite();

        client = new FirebaseClient(config);
    }
    public class TipoTramite
    {
        public int IdTipoTramite { get; set; }
        public string NombreTramite { get; set; }
    }

    public void CargarDatosComboBoxTipoTramite()
    {
        List<TipoTramite> tiposTramite = new List<TipoTramite>();

        var baseDeDatos = Coneccion.Instance;

        string query = "SELECT IdTipoTramite, NombreTramite FROM TIPO_TRAMITE";

        using (SqlConnection connection = baseDeDatos.GetConnection())
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int idTipoTramite = reader.GetInt32(0);
                        string nombreTramite = reader.GetString(1);

                        // Crear un objeto TipoTramite y agregarlo a la lista
                        TipoTramite tipoTramite = new TipoTramite
                        {
                            IdTipoTramite = idTipoTramite,
                            NombreTramite = nombreTramite
                        };

                        tiposTramite.Add(tipoTramite);
                    }
                }
            }
        }

        // Asignar los datos al ComboBox
        txtTipoTramite.ItemsSource = tiposTramite;
        txtTipoTramite.DisplayMemberPath = "NombreTramite";
        txtTipoTramite.SelectedValuePath = "IdTipoTramite";
    }

    public void CargarTramitesEnDataGrid()
    {
        listaTramites = ObtenerTramitesDesdeLaBaseDeDatos();
        membersDataGrid.ItemsSource = listaTramites;
    }
    public void ButtonBase_OnClick(object sender, RoutedEventArgs e)
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
        public int EstadoRegistro { get; set; }
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


    public void CheckBox1_OnChecked(object sender, RoutedEventArgs e)
    {
        if (CheckBox1.IsChecked == true)
        {
            tipo.Visibility = Visibility.Visible;
        }

    }

    public void CheckBox1_OnUnchecked(object sender, RoutedEventArgs e)
    {
        if (CheckBox1.IsChecked == false)
        {
            tipo.Visibility = Visibility.Hidden;
        }
    }



    public void vdoc_OnClick(object sender, RoutedEventArgs e)
    {
        DocumentosAdjuntos doc = new DocumentosAdjuntos();
        doc.ShowDialog();
    }





    //BOTÓN PARA CREAR EL TRÁMITE
    public async void cod_OnClick(object sender, RoutedEventArgs e)
    {
        ci = txtCI.Text;
        nombre = txtNombre.Text;
        appPaterno = txtApellidoP.Text;
        appMaterno = txtApellidoM.Text;
        celular = txtCelular.Text;
        celularRef = txtCelularRef.Text;
        correo = txtCorreoElectronico.Text;
        tipoTramite = (int)txtTipoTramite.SelectedValue;
        diccion = txtDireccion.Text;
        descripcion = txtDescripcionTramite.Text;
        int clienteId = ObtenerClienteIdPorCI(ci);

        int funcionario = ObtenerFuncionarioIdPorCargo(App.MiVariableGlobal);

        bool usuarioExiste = VerificarExistenciaUsuario(ci);

        if (usuarioExiste)
        {
            if (clienteId != -1)
            {
                int nuevoTramiteId = InsertarNuevoTramite(tipoTramite, clienteId, descripcion);

                if (nuevoTramiteId != -1)
                {
                    if (fileBytes != null)
                    {
                        InsertarNuevoAnexoTramite(funcionario, nuevoTramiteId, fileBytes);
                        fileBytes = null;
                    }
                }
                else
                {
                    MessageBox.Show("Error1");
                }
            }
            else
            {
                MessageBox.Show("Error al obtener el ID del cliente existente.");
            }
        }
        else
        {
            int personaId = InsertarNuevoUsuario(nombre, appPaterno, appMaterno, int.Parse(ci), int.Parse(celular));

            if (personaId != -1)
            {
                int nuevoClienteId = InsertarNuevoCliente(personaId, correo, diccion, int.Parse(celularRef));

                if (nuevoClienteId != -1)
                {
                    int nuevoTramiteId = InsertarNuevoTramite(tipoTramite, nuevoClienteId, descripcion);

                    if (nuevoTramiteId != -1)
                    {
                        if (fileBytes != null)
                        {
                            InsertarNuevoAnexoTramite(funcionario, nuevoTramiteId, fileBytes);
                            fileBytes = null;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error2");
                    }
                }
                else
                {
                    MessageBox.Show("Error al insertar el nuevo cliente en la base de datos.");
                }
            }
            else
            {
                MessageBox.Show("Error al insertar el nuevo usuario en la base de datos.");
            }
        }
    }


    public int ObtenerFuncionarioIdPorCargo(int cargoId)
    {
        using (var connection = Coneccion.Instance.GetConnection())
        {
            connection.Open();

            string query = "SELECT FuncionarioId FROM FUNCIONARIO_CARGO WHERE CargoId = @CargoId";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CargoId", cargoId);

                int funcionarioId = Convert.ToInt32(command.ExecuteScalar());
                return funcionarioId;
            }
        }
    }
    public static string GenerarCodigoUnico()
    {
        const string prefijo = "TRM-";
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

    public void InsertarNuevoAnexoTramite(int funcionario, int nuevoTramiteId, byte[] fileBytes)
    {
        try
        {
            using (var connection = Coneccion.Instance.GetConnection())
            {
                connection.Open();

                // Comando SQL para insertar el anexo en la tabla ANEXO_TRAMITES
                string sqlQuery = "INSERT INTO ANEXO_TRAMITES (RemitenteId, TramiteId, Archivo, FechaRegistro, EstadoRegistro) " +
                                  "VALUES (@RemitenteId, @TramiteId, @Archivo, GETDATE(), 1);";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@RemitenteId", funcionario);
                    command.Parameters.AddWithValue("@TramiteId", nuevoTramiteId);
                    command.Parameters.AddWithValue("@Archivo", fileBytes);

                    command.ExecuteNonQuery();
                }


            }
        }
        catch (Exception ex)
        {

        }
    }
    public int InsertarNuevoTramite(int tipoTramiteId, int clienteId, string descripcion)
    {
        string codigo = GenerarCodigoUnico();
        DateTime fechaIni = DateTime.Now;
        try
        {
            using (var connection = Coneccion.Instance.GetConnection())
            {
                connection.Open();

                string query = "INSERT INTO TRAMITES (TipoTramiteId, ClienteId, CodigoTramite, EstadoTramite, Descripcion, FechaInicio, FechaFinalizacion, EstadoRegistro)" +
                               "VALUES (@TipoTramiteId, @ClienteId, @CodigoTramite, 'En Proceso', @Descripcion, GETDATE(),NULL ,1);" +
                "SELECT SCOPE_IDENTITY();";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TipoTramiteId", tipoTramiteId);
                    command.Parameters.AddWithValue("@ClienteId", clienteId);
                    command.Parameters.AddWithValue("@Descripcion", descripcion);
                    command.Parameters.AddWithValue("@CodigoTramite", codigo);

                    int tramiteId = Convert.ToInt32(command.ExecuteScalar()); // Ejecutar y obtener el ID generado

                    InsertarFireSharp(clienteId, codigo, tipoTramiteId, "En Proceso", descripcion, 1, fechaIni,null, tramiteId);


                    int RemitenteId = 9;
                    MessageBox.Show(RemitenteId + "");
                    
                    int destinatarioId = EncontrarDestinatarioId(tipoTramiteId, RemitenteId);

                    int numerodelaInstancia = encontrarNumeroInstancia(tipoTramiteId, destinatarioId);

                    InsertarHojaDeRuta(RemitenteId, destinatarioId, tramiteId, "Nuevo", numerodelaInstancia, "Normal", "Nose", "Noise", 1);




                    return tramiteId;
                }



            }
        }
        catch (Exception ex)
        {
            // Manejar la excepción
            Console.WriteLine("Error al insertar nuevo trámite: " + ex.Message);
            return -1;
        }
    }
    public int encontrarNumeroInstancia(int tipoTramiteId, int destinatarioId)
    {
        int numeroInstancia = 0;

        using (var connection = Coneccion.Instance.GetConnection())
        {
            connection.Open();

            string query = "SELECT NumeroInstancia FROM TIPO_TRAMITE_FLUJO WHERE TipoTramiteId = @TipoTramiteId AND CargoId = @DestinatarioId;";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TipoTramiteId", tipoTramiteId);
                command.Parameters.AddWithValue("@DestinatarioId", destinatarioId);

                object result = command.ExecuteScalar();
                numeroInstancia = result != DBNull.Value ? Convert.ToInt32(result) : 0;
            }
        }

        return numeroInstancia;
    }


    public void InsertarHojaDeRuta(int remitenteId, int destinatarioId, int tramiteId, string estadoTramite, int numeroInstancia, string prioridad, string detallesObservaciones, string tipoFlujo, int estadoRegistro)
    {
        MessageBox.Show("Llego aca");
        try
        {
            MessageBox.Show("Llego aca");
            using (var connection = Coneccion.Instance.GetConnection())
            {
                connection.Open();

                // Buscar el último ID y sumarle uno
                int nuevoId;
                string queryGetMaxId = "SELECT ISNULL(MAX(IdFlujo), 0) FROM FLUJO_HOJA_DE_RUTA;";
                using (var commandMaxId = new SqlCommand(queryGetMaxId, connection))
                {
                    nuevoId = Convert.ToInt32(commandMaxId.ExecuteScalar()) + 1;
                }

                string query = "INSERT INTO FLUJO_HOJA_DE_RUTA (RemitenteId, DestinatarioId, TramiteId, EstadoTramite, NumeroInstancia, Prioridad, DetallesYObservaciones, FechaFinalizacion, TipoFlujo, EstadoRegistro)" +
                               "VALUES (@RemitenteId, @DestinatarioId, @TramiteId, @EstadoTramite, @NumeroInstancia, @Prioridad, @DetallesYObservaciones, GETDATE(), @TipoFlujo, @EstadoRegistro);";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", nuevoId);
                    command.Parameters.AddWithValue("@RemitenteId", remitenteId);
                    command.Parameters.AddWithValue("@DestinatarioId", destinatarioId);
                    command.Parameters.AddWithValue("@TramiteId", tramiteId);
                    command.Parameters.AddWithValue("@EstadoTramite", estadoTramite);
                    command.Parameters.AddWithValue("@NumeroInstancia", numeroInstancia);
                    command.Parameters.AddWithValue("@Prioridad", prioridad);
                    command.Parameters.AddWithValue("@DetallesYObservaciones", detallesObservaciones);
                    command.Parameters.AddWithValue("@TipoFlujo", tipoFlujo);
                    command.Parameters.AddWithValue("@EstadoRegistro", estadoRegistro);

                    command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(remitenteId + " " + destinatarioId);
            MessageBox.Show($"Se produjo un error al intentar insertar en la tabla FLUJO_HOJA_DE_RUTA: {ex.Message}");
        }
    }


    public int EncontrarDestinatarioId(int tipoTramiteId, int cargoId)
    {
        using (var connection = Coneccion.Instance.GetConnection())
        {
            connection.Open();

            string query = "SELECT NumeroInstancia FROM TIPO_TRAMITE_FLUJO WHERE TipoTramiteId = @TipoTramiteId AND CargoId = @CargoId;";

            int numeroInstanciaActual;

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TipoTramiteId", tipoTramiteId);
                command.Parameters.AddWithValue("@CargoId", cargoId);

                object result = command.ExecuteScalar();
                numeroInstanciaActual = result != DBNull.Value ? Convert.ToInt32(result) : 0;
            }

            string query2 = "SELECT MAX(NumeroInstancia) AS NumeroInstanciaMayor FROM TIPO_TRAMITE_FLUJO WHERE TipoTramiteId = @TipoTramiteId;";

            int numeroInstanciaMayor;

            using (var command = new SqlCommand(query2, connection))
            {
                command.Parameters.AddWithValue("@TipoTramiteId", tipoTramiteId);

                object result = command.ExecuteScalar();
                numeroInstanciaMayor = result != DBNull.Value ? Convert.ToInt32(result) : 0;
            }

            numeroInstanciaActual++;

            while (true && numeroInstanciaActual != numeroInstanciaMayor)
            {
                query = "SELECT CargoId FROM TIPO_TRAMITE_FLUJO WHERE TipoTramiteId = @TipoTramiteId AND NumeroInstancia = @NumeroInstanciaActual;";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TipoTramiteId", tipoTramiteId);
                    command.Parameters.AddWithValue("@NumeroInstanciaActual", numeroInstanciaActual);

                    object result = command.ExecuteScalar();

                    if (result != DBNull.Value)
                    {
                        cargoId = Convert.ToInt32(result);
                        break; // Salir del bucle al encontrar un CargoId
                    }

                    numeroInstanciaActual++;
                }
            }

            if (numeroInstanciaActual == numeroInstanciaMayor)
            {
                MessageBox.Show("¡Has llegado al final del flujo!");
                return 0;
            }

            return cargoId; // Devolver el último CargoId encontrado o el inicial
        }
    }


    public async Task InsertarFireSharp(int clienteId,string codigo, int tipoTramiteId, string estadoTramite, string descripcion, int estadoRegistro, DateTime fechaIni, DateTime? fechaFin, int tramiteId)
    {
        var tramiteData = new
        {
            ClienteId = Convert.ToString(clienteId),
            CodigoTramite = codigo,
            TipoTramite = tipoTramiteId,
            Descripcion = descripcion,
            EstadoRegistro = estadoRegistro,
            EstadoTramite = estadoTramite,
            FechaInicio = fechaIni,
            FechaFinalizacion = fechaFin
        };

        await client.SetAsync("tramites/" + tramiteId, tramiteData);
    }

    public int ObtenerClienteIdPorCI(string ci)
    {
        try
        {
            using (var connection = Coneccion.Instance.GetConnection())
            {
                connection.Open();

                string query = "SELECT IdCliente FROM CLIENTES " +
                               "WHERE PersonaId = (SELECT IdPersona FROM PERSONAS WHERE CI = @CI)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CI", ci);

                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Manejar la excepción
            Console.WriteLine("Error al obtener el ID del cliente por CI: " + ex.Message);
        }

        return -1; // Valor predeterminado si no se encontró el ID del cliente o ocurrió un error
    }

    public int InsertarNuevoUsuario(string nombres, string apellidoPaterno, string apellidoMaterno, int ci, int celular)
    {
        try
        {
            using (var connection = Coneccion.Instance.GetConnection())
            {
                connection.Open();

                // Insertar datos en la tabla PERSONAS
                string query = "INSERT INTO PERSONAS (Nombres, ApellidoPaterno, ApellidoMaterno, CI, Celular, EstadoRegistro) " +
                               "VALUES (@Nombres, @ApellidoPaterno, @ApellidoMaterno, @CI, @Celular, 1);" +
                               "SELECT SCOPE_IDENTITY();"; // Obtener el ID generado

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombres", nombres);
                    command.Parameters.AddWithValue("@ApellidoPaterno", apellidoPaterno);
                    command.Parameters.AddWithValue("@ApellidoMaterno", apellidoMaterno);
                    command.Parameters.AddWithValue("@CI", ci);
                    command.Parameters.AddWithValue("@Celular", celular);

                    int personaId = Convert.ToInt32(command.ExecuteScalar()); // Ejecutar y obtener el ID generado

                    return personaId;
                }
            }
        }
        catch (Exception ex)
        {
            // Manejar la excepción
            Console.WriteLine("Error al insertar nuevo usuario: " + ex.Message);
            return -1;
        }
    }

    public int InsertarNuevoCliente(int personaId, string correo, string direccion, int celularReferencia)
    {
        try
        {
            using (var connection = Coneccion.Instance.GetConnection())
            {
                connection.Open();

                // Insertar datos en la tabla CLIENTES
                string query = "INSERT INTO CLIENTES (PersonaId, Correo, Direccion, CelularReferencia, EstadoRegistro) " +
                               "VALUES (@PersonaId, @Correo, @Direccion, @CelularReferencia, 1);" +
                               "SELECT SCOPE_IDENTITY();"; // Obtener el ID generado

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonaId", personaId);
                    command.Parameters.AddWithValue("@Correo", correo);
                    command.Parameters.AddWithValue("@Direccion", direccion);
                    command.Parameters.AddWithValue("@CelularReferencia", celularReferencia);

                    int clienteId = Convert.ToInt32(command.ExecuteScalar()); // Ejecutar y obtener el ID generado

                    return clienteId;
                }
            }
        }
        catch (Exception ex)
        {
            // Manejar la excepción
            Console.WriteLine("Error al insertar nuevo cliente: " + ex.Message);
            return -1;
        }
    }

    public bool VerificarExistenciaUsuario(string ci)
    {
        bool existeUsuario = false;

        try
        {
            using (var connection = Coneccion.Instance.GetConnection())
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM PERSONAS WHERE CI = @CI";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CI", ci);

                    int count = (int)command.ExecuteScalar();

                    if (count > 0)
                    {
                        existeUsuario = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Manejar la excepción (puedes registrarla, mostrar un mensaje, etc.)
            Console.WriteLine("Error al verificar la existencia del usuario: " + ex.Message);
        }

        return existeUsuario;
    }


    public void MiBotonV_OnClick(object sender, RoutedEventArgs e)
    {
        DoubleAnimation animation = new DoubleAnimation();
        animation.From = 0;
        animation.To = 280;
        animation.Duration = new Duration(TimeSpan.FromSeconds(0.5));

        hola3.BeginAnimation(StackPanel.HeightProperty, animation);
        miBotonV.Visibility = Visibility.Collapsed;
        miBotonV1.Visibility = Visibility.Visible;
    }

    public void MiBotonV1_OnClick(object sender, RoutedEventArgs e)
    {
        DoubleAnimation animation = new DoubleAnimation();
        animation.From = 280;
        animation.To = 0;
        animation.Duration = new Duration(TimeSpan.FromSeconds(0.5));

        hola3.BeginAnimation(StackPanel.HeightProperty, animation);
        miBotonV.Visibility = Visibility.Visible;
        miBotonV1.Visibility = Visibility.Collapsed;
    }



    public void AddDocumento_Click(object sender, RoutedEventArgs e)
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

    public void TxtCI_TextChanged(object sender, TextChangedEventArgs e)
    {
        string ciText = txtCI.Text;
        // Verificar si el CI tiene 7 u 8 caracteres
        if (ciText.Length == 6 || ciText.Length == 7 || ciText.Length == 8)
        {
            // Realizar la búsqueda en la tabla PERSONAS
            if (BuscarPersonaPorCI(ciText, out Persona persona))
            {
                // Si se encontró la persona, llenar los demás campos
                LlenarCamposConPersona(persona);
            }
            else
            {
                LimpiarCamposAdicionales();
                LimpiarTodosLosCampos();
            }
        }
        else
        {
            LimpiarCamposAdicionales();
            LimpiarTodosLosCampos();
        }
    }


    public void LlenarCamposConPersona(Persona persona)
    {
        // Habilitar todos los TextBox para que sean editables


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


    public Cliente BuscarClientePorIdPersona(int idPersona)
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

    public void LimpiarCamposAdicionales()
    {
        // Limpiar los campos adicionales
        txtCorreoElectronico.Text = string.Empty;
        txtDireccion.Text = string.Empty;
        txtCelularRef.Text = string.Empty;
        // Otros campos según tu interfaz
    }

    public void LimpiarTodosLosCampos()
    {
        // Limpiar todos los campos
        LimpiarCamposAdicionales();
        // Limpiar los campos de la persona
        txtNombre.Text = string.Empty;
        txtApellidoP.Text = string.Empty;
        txtApellidoM.Text = string.Empty;
        txtCelular.Text = string.Empty;
    }

    public bool BuscarPersonaPorCI(string ci, out Persona persona)
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



    public List<Tramite> ObtenerTramitesDesdeLaBaseDeDatos()
    {

        using (var connection = Coneccion.Instance.GetConnection())
        {
            connection.Open();
            int personaDestinada = 9;

            string query = "SELECT t.IdTramite, t.TipoTramiteId, t.ClienteId, t.CodigoTramite, t.EstadoTramite, " +
                       "t.FechaInicio, t.FechaFinalizacion, t.EstadoRegistro " +
                       "FROM TRAMITES t " +
                       "INNER JOIN FLUJO_HOJA_DE_RUTA f ON t.IdTramite = f.TramiteId " +
                       "WHERE f.RemitenteId = @PersonaDestinada AND f.EstadoTramite != 'Mandado' AND t.EstadoRegistro != 0;";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PersonaDestinada", personaDestinada);
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



    public List<Cliente> ObtenerClientesDesdeLaBaseDeDatos()
    {


        try
        {
            using (var connection = Coneccion.Instance.GetConnection())
            {
                connection.Open();

                string query = "SELECT IdCliente, PersonaId, Correo, Direccion, CelularReferencia, EstadoRegistro " +
                               "FROM CLIENTES";

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Cliente cliente = new Cliente
                            {
                                IdCliente = reader.GetInt32(0),
                                PersonaId = reader.GetInt32(1),
                                Correo = reader.GetString(2),
                                Direccion = reader.GetString(3),
                                CelularReferencia = reader.GetInt32(4),
                                EstadoRegistro = reader.GetInt32(5)
                            };

                            listaCliente.Add(cliente);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Manejar la excepción (puedes registrarla, mostrar un mensaje, etc.)
            Console.WriteLine("Error al obtener clientes desde la base de datos: " + ex.Message);
        }

        return listaCliente;
    }


    public List<Persona> ObtenerPersonasDesdeLaBaseDeDatos()
    {

        try
        {
            using (var connection = Coneccion.Instance.GetConnection())
            {
                connection.Open();

                string query = "SELECT IdPersona, Nombres, ApellidoPaterno, ApellidoMaterno, CI, Celular, EstadoRegistro " +
                               "FROM PERSONAS";

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Persona persona = new Persona
                            {
                                IdPersona = reader.GetInt32(0),
                                Nombres = reader.GetString(1),
                                ApellidoPaterno = reader.GetString(2),
                                ApellidoMaterno = reader.GetString(3),
                                CI = reader.GetInt32(4),
                                Celular = reader.GetInt32(5),
                                EstadoRegistro = reader.GetInt32(6)
                            };

                            listaPersonas.Add(persona);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Manejar la excepción (puedes registrarla, mostrar un mensaje, etc.)
            Console.WriteLine("Error al obtener personas desde la base de datos: " + ex.Message);
        }

        return listaPersonas;
    }


    public void ver_OnClick(object sender, RoutedEventArgs e)
    {
        DoubleAnimation animation = new DoubleAnimation();
        animation.From = 0;
        animation.To = 500;
        animation.Duration = new Duration(TimeSpan.FromSeconds(0.5));

        VistaDetallesTramite.BeginAnimation(StackPanel.WidthProperty, animation);
    }

    public void ocultar_OnClick(object sender, RoutedEventArgs e)
    {
        DoubleAnimation animation = new DoubleAnimation();
        animation.From = 500;
        animation.To = 0;
        animation.Duration = new Duration(TimeSpan.FromSeconds(0.5));

        VistaDetallesTramite.BeginAnimation(StackPanel.WidthProperty, animation);

    }

































    public int taskiid = 0;

    public void Admitir_OnClick(object sender, RoutedEventArgs e)
    {
        // BtnAdmitir admitir = new BtnAdmitir();
        //admitir.ShowDialog();





        using (var connection = Coneccion.Instance.GetConnection())
        {
            int TramiteId = taskiid;

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




            



            int destinatarioId = EncontrarDestinatarioId(tipoTramiteId, RemitenteId);


            if(destinatarioId != 0)
            {



                string query4 = "UPDATE FLUJO_HOJA_DE_RUTA " +
               "SET EstadoTramite = 'Mandado' " +
               "WHERE TramiteId = @TramiteId " +
               "AND NumeroInstancia = @NumeroInstancia " +
               "AND EstadoTramite NOT IN ('Devuelto', 'Rechazado', 'Mandado');";

                using (var command = new SqlCommand(query4, connection))
                {
                    command.Parameters.AddWithValue("@TramiteId", TramiteId);
                    command.Parameters.AddWithValue("@NumeroInstancia", numeroInstancia);
                    connection.Open();
                    command.ExecuteNonQuery();

                    connection.Close();
                }




                MessageBox.Show(RemitenteId + " " + destinatarioId + " " + numeroInstancia + " " + taskiid);


                int numerodelaInstancia = encontrarNumeroInstancia(tipoTramiteId, destinatarioId);

                InsertarHojaDeRuta(RemitenteId, destinatarioId, TramiteId, "Nuevo", numerodelaInstancia, "Normal", "Nose", "Noise", 1);
            }

            
        }








    }
            


    private void propiedades_Click(object sender, RoutedEventArgs e)
    {
        Button btn = sender as Button;
        if (btn != null)
        {
            int taskId = (int)btn.Tag;
            taskiid = taskId;
            MessageBox.Show("" + taskId);


            if (isDetailsVisible)
            {
                DoubleAnimation hideAnimation = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(0.5)));
                VistaDetallesTramite.BeginAnimation(WidthProperty, hideAnimation);
            }
            else
            {
                Tramite tramite = listaTramites.FirstOrDefault(t => t.IdTramite == taskId);
                int idCliente = tramite.ClienteId;

                Cliente cliente = listaCliente.FirstOrDefault(t => t.IdCliente == idCliente);

                int idPersona = cliente.PersonaId;
                Persona persona = listaPersonas.FirstOrDefault(t => t.IdPersona == idPersona);

                if (tramite != null && persona != null)
                {
                    lblCodigoHR.Content = tramite.IdTramite;
                    lblInteresado.Content = persona.Nombres + " " + persona.ApellidoPaterno + " " + persona.ApellidoMaterno;
                    lblCelularInteresado.Content = persona.Celular;

                    DoubleAnimation showAnimation = new DoubleAnimation(500, new Duration(TimeSpan.FromSeconds(0.5)));
                    VistaDetallesTramite.BeginAnimation(WidthProperty, showAnimation);
                }
            }

            isDetailsVisible = !isDetailsVisible;
        }
    }


























    private void Observar_OnClick(object sender, RoutedEventArgs e)
    {
        ObservarRechazarTramite tra = new ObservarRechazarTramite(this);
        tra.ShowDialog();














    }


    private void AprobaryFinalizar(object sender, RoutedEventArgs e)
    {




        using (var connection = Coneccion.Instance.GetConnection())
        {
            int TramiteId = taskiid;

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








            int destinatarioId = EncontrarDestinatarioId(tipoTramiteId, RemitenteId);


            
            {



                string query4 = "UPDATE FLUJO_HOJA_DE_RUTA " +
               "SET EstadoTramite = 'Finalizado' " +
               "WHERE TramiteId = @TramiteId " +
               "AND NumeroInstancia = @NumeroInstancia " +
               "AND EstadoTramite NOT IN ('Devuelto', 'Rechazado', 'Mandado');";

                using (var command = new SqlCommand(query4, connection))
                {
                    command.Parameters.AddWithValue("@TramiteId", TramiteId);
                    command.Parameters.AddWithValue("@NumeroInstancia", numeroInstancia);
                    connection.Open();
                    command.ExecuteNonQuery();

                    connection.Close();
                }




                MessageBox.Show(RemitenteId + " " + destinatarioId + " " + numeroInstancia + " " + taskiid);


                int numerodelaInstancia = encontrarNumeroInstancia(tipoTramiteId, destinatarioId);









                string query7 = "UPDATE TRAMITES SET EstadoTramite = @estadito WHERE IdTramite = @IdTramite;";

                
                {
                    using (var command = new SqlCommand(query7, connection))
                    {
                        command.Parameters.AddWithValue("@IdTramite", taskiid);
                        command.Parameters.AddWithValue("@estadito", "Finalizado");

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }

                //InsertarHojaDeRuta(RemitenteId, destinatarioId, TramiteId, "Finalizado", numerodelaInstancia, "Normal", "Nose", "Noise", 1);
            }


        }

    }

    private void Rechazar_OnClick(object sender, RoutedEventArgs e)
    {
        //RechazarTramite tra = new RechazarTramite();
        //tra.ShowDialog();


        string query = "UPDATE TRAMITES SET EstadoRegistro = 0 WHERE IdTramite = @IdTramite;";

        using (var connection = Coneccion.Instance.GetConnection())
        {
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@IdTramite", taskiid);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }


    }






}


