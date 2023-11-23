using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace HojadeRuta2K23.Paginas;

public partial class Dashboard : Page
{
    public Dashboard()
    {
        InitializeComponent();
    }

    private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
    }

    private void Tareas_OnClick(object sender, RoutedEventArgs e)
    {
        if (NavigationService != null)
        {
            NavigationService.Navigate(new Tareas());
        }
    }

    private void Tramites_OnClick(object sender, RoutedEventArgs e)
    {
        if (NavigationService != null)
        {
            NavigationService.Navigate(new Tramites());
        }
    }

    private void Admin_OnClick(object sender, RoutedEventArgs e)
    {
        if (NavigationService != null)
        {
            NavigationService.Navigate(new AdministracionUsuarios());
        }
    }

    private void Perfil_OnClick(object sender, RoutedEventArgs e)
    {
        
        if (NavigationService != null)
        {
            NavigationService.Navigate(new Perfil());
        }
    }
}