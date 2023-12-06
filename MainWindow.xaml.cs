using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HojadeRuta2K23.Paginas;

namespace HojadeRuta2K23
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int clickCount2 = 0;

        private string foto = "C:\\Users\\Alienware\\RiderProjects\\SIVVALLE\\Images\\delivery-man.png";

        public MainWindow()
        {
            InitializeComponent();
            frame.NavigationService.Navigate(new Dashboard());
            noticaciones.Click += noticaciones_Click;
        }

        private CustomPopupPlacement[] CustomPopupPlacementMethod(Size popupSize, Size targetSize, Point offset)
        {
            double left = (targetSize.Width - popupSize.Width) / 2;
            double top = targetSize.Height;

            CustomPopupPlacement placement = new CustomPopupPlacement(new Point(left, top), PopupPrimaryAxis.Vertical);

            return new CustomPopupPlacement[] { placement };
        }

        private void noticaciones_Click(object sender, RoutedEventArgs e)
        {
            if (!NotificacionesPopup.IsOpen)
            {
                // Calcular la posición del Popup en relación con el botón de notificaciones
                CustomPopupPlacementCallback placementCallback =
                    new CustomPopupPlacementCallback(CustomPopupPlacementMethod);
                NotificacionesPopup.CustomPopupPlacementCallback = placementCallback;

                NotificacionesPopup.PlacementTarget = noticaciones;
                NotificacionesPopup.IsOpen = true;
            }
            else
            {
                NotificacionesPopup.IsOpen = false; // Cerrar el Popup si ya está abierto
            }
        }


        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private bool IsMaximize = false;

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsMaximize)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1280;
                    this.Height = 780;

                    IsMaximize = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;

                    IsMaximize = true;
                }
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void Salir_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CerrarSesion_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void Inventario_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void kardex_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonBase3_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonBase4_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonBase5_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonBase6_OnClick(object sender, RoutedEventArgs e)
        {
        }


        private void ButtonBase7_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonBase8_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonBase9_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonBase10_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonBase80_OnClick(object sender, RoutedEventArgs e)
        {
            if (clickCount2 % 2 == 0)
            {
                string colorHex1 = "#1b42db";
                string colorHex2 = "#24228f";


                Color color1 = (Color)ColorConverter.ConvertFromString(colorHex1);
                Color color2 = (Color)ColorConverter.ConvertFromString(colorHex2);


                LinearGradientBrush gradientBrush = new LinearGradientBrush();
                gradientBrush.StartPoint = new Point(0, 0);
                gradientBrush.EndPoint = new Point(1, 1);
                gradientBrush.GradientStops.Add(new GradientStop(color1, 0));
                gradientBrush.GradientStops.Add(new GradientStop(color2, 1));

                NavBar_uno.Background = gradientBrush;


                string mainColor = "#1a1a1a";

                Color color3 = (Color)ColorConverter.ConvertFromString(mainColor);

                Main_uno.Background = new SolidColorBrush(color3);

                InventarioButton.Click += InventarioDark_OnClick;

                CategoriasButton.Click += CategoriasDark_OnClick;
                ClientesButton.Click += ClientesDark_OnClick;
            }
            else
            {
                NavBar_uno.Background = Brushes.DarkRed;

                string mainColor = "#F7F6F4";

                Color color3 = (Color)ColorConverter.ConvertFromString(mainColor);

                Main_uno.Background = new SolidColorBrush(color3);

                InventarioButton.Click += Inventario_OnClick;

                CategoriasButton.Click += ButtonBase7_OnClick;
                ClientesButton.Click += ButtonBase8_OnClick;
            }

            clickCount2++;
        }

        private void UsuariosDark_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void ProductosDark_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void SalirDark_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void CerrarSesionDark_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void InventarioDark_OnClick(object sender, RoutedEventArgs e)
        {
        }


        private void ProveedoresDark_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void VentasDark_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void ReportesDark_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void CategoriasDark_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void ClientesDark_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void DevolucionesDark_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void ComprasDark_OnClick(object sender, RoutedEventArgs e)
        {
        }


        // Lma a esta función cuando desees navegar a la nueva página con la animación de deslizamient
        private void PerfilClikck(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new Perfil());
        }

        private void SalirClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ADministracionUsuariosClick(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new AdministracionUsuarios());
        }

        private void Tramites_OnClick(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new Tramites());
        }

        private void Tareas_OnClick(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new Tareas());
        }

        private void Notificaciones_OnClick(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new Notificaciones());
        }

        private void Univalle_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            frame.NavigationService.Navigate(new Dashboard());
        }

        private void Buscar_OnClick(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new Buscador());
        }
    }
}