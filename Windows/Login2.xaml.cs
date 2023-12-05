using Microsoft.Data.SqlClient;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace HojadeRuta2K23.Windows;

public partial class Login2 : Window
{
    private int cont;
    public Login2()
    {
        InitializeComponent();
    }

    private void Hyperlink_OnClick(object sender, RoutedEventArgs e)
    {
        DoubleAnimation animation1 = new DoubleAnimation();
        animation1.From = 300;
        animation1.To = 0;
        animation1.Duration = new Duration(TimeSpan.FromSeconds(0.5));
        Border1.BeginAnimation(StackPanel.WidthProperty, animation1);

        DoubleAnimation animation2 = new DoubleAnimation();
        animation2.From = 300;
        animation2.To = 0;
        animation2.Duration = new Duration(TimeSpan.FromSeconds(0.5));
        Border2.BeginAnimation(StackPanel.WidthProperty, animation2);

        Border1.Visibility = Visibility.Collapsed;
        Border2.Visibility = Visibility.Collapsed;
        Info.Visibility = Visibility.Visible;
        Border.Visibility = Visibility.Visible;
        iniciar.Visibility = Visibility.Collapsed;
        enviar.Visibility = Visibility.Visible;
        olvidar.Visibility = Visibility.Collapsed;
        tengor.Visibility = Visibility.Visible;

    }

    private void Hyperlink2_OnClick(object sender, RoutedEventArgs e)
    {
        DoubleAnimation animation1 = new DoubleAnimation();
        animation1.From = 0;
        animation1.To = 300;
        animation1.Duration = new Duration(TimeSpan.FromSeconds(0.5));
        Border1.BeginAnimation(StackPanel.WidthProperty, animation1);

        DoubleAnimation animation2 = new DoubleAnimation();
        animation2.From = 0;
        animation2.To = 300;
        animation2.Duration = new Duration(TimeSpan.FromSeconds(0.5));
        Border2.BeginAnimation(StackPanel.WidthProperty, animation2);

        Border1.Visibility = Visibility.Visible;
        Border2.Visibility = Visibility.Visible;
        Info.Visibility = Visibility.Collapsed;
        Border.Visibility = Visibility.Collapsed;
        iniciar.Visibility = Visibility.Visible;
        enviar.Visibility = Visibility.Collapsed;
        olvidar.Visibility = Visibility.Visible;
        tengor.Visibility = Visibility.Collapsed;
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
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

    private bool VerificarContraseña(string contraseña, string hash)
    {
        // Calcular el hash de la contraseña ingresada
        string contraseñaIngresadaHash = CalcularHash(contraseña);

        // Comparar los hashes
        StringComparer comparer = StringComparer.OrdinalIgnoreCase;
        return comparer.Compare(contraseñaIngresadaHash, hash) == 0;
    }

    private void iniciar_Click_1(object sender, RoutedEventArgs e)
    {
        SqlConnection conexion = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB; Initial Catalog=SIVALLE_DB; Integrated Security=True;");

        try
        {
            conexion.Open();
            string username = txtEmailUsuario.Text;
            string password = PassContra.Password;
            string pass = PassContra1.Text;
            string query = "SELECT Usuario, Contrasena FROM LOGIN WHERE Usuario = @Usuario";
            SqlCommand comando = new SqlCommand(query, conexion);
            comando.Parameters.AddWithValue("@Usuario", username); // Change parameter name to match your SQL query
            SqlDataReader registros = comando.ExecuteReader();
            if (registros.Read())
            {
                string contraseñaHash = registros["Contrasena"].ToString(); // Change field name to match your SQL query

                // ...

                if (VerificarContraseña(password, contraseñaHash) || VerificarContraseña(pass, contraseñaHash))
                {
                    // Aquí puedes realizar las acciones que deseas realizar al autenticar al usuario
                    // Por ejemplo, obtener datos del usuario y mostrar una ventana de bienvenida

                    //string nombreUsuario = registros["Nombre"].ToString();
                    //string cargoUsuario = registros["Cargo"].ToString();
                    //string apellidosUsuario = registros["Apellidos"].ToString();

                    //MessageBox.Show("Bienvenido, " + nombreUsuario + ". Cargo: " + cargoUsuario);

                    // Oculta la ventana de inicio de sesión
                    Hide();

                    // Abre el formulario de bienvenida
                    SplashScreen formBienvenida = new SplashScreen(); //(nombreUsuario, apellidosUsuario, cargoUsuario);
                    formBienvenida.Show();

                    // Configura un temporizador para cerrar la ventana de bienvenida después de 30 segundos
                    DispatcherTimer timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromMilliseconds(3000); // 30 segundos
                    timer.Tick += (s, args) =>
                    {
                        timer.Stop();
                        formBienvenida.Close(); // Cierra la ventana de bienvenida
                                                // Abre la ventana principal
                        MainWindow mainWindow = new MainWindow(); //(nombreUsuario, cargoUsuario);
                        mainWindow.Show();
                    };
                    timer.Start();
                }
                else
                {
                    cont++;
                    MessageBox.Show("Nombre de usuario o contraseña incorrectos.");
                    if (cont == 3)
                    {
                        Application.Current.Shutdown();
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(username))
                {
                    MessageBox.Show("No ingresó ningún dato.");
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error de conexión: " + ex.Message);
        }
        finally
        {
            conexion.Close();
        }
    }
    private void chkMostrarContrasena_Checked(object sender, RoutedEventArgs e)
    {
        PassContra.Visibility = Visibility.Collapsed;
        PassContra1.Text = PassContra.Password;
        PassContra1.Visibility = Visibility.Visible;

    }

    private void chkMostrarContrasena_Unchecked(object sender, RoutedEventArgs e)
    {
        PassContra.Visibility = Visibility.Visible;
        PassContra.Password = PassContra1.Text;
        PassContra1.Visibility = Visibility.Collapsed;
    }


}