using System.Windows;
using System.Windows.Controls;

namespace HojadeRuta2K23.Paginas;

public partial class Buscador : Page
{
    public Buscador()
    {
        InitializeComponent();
        buscador.Items.Add("Tareas");
        buscador.Items.Add("Tramites");
    }

    private void Buscador_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (buscador.SelectedIndex == 0)
        {
            tareas.Visibility = Visibility.Visible;
            tramites.Visibility = Visibility.Hidden;

        }
        
        if (buscador.SelectedIndex == 1)
        {
            tramites.Visibility = Visibility.Visible;
            tareas.Visibility = Visibility.Hidden;

        }
    }
}