using System.Windows;
using System.Windows.Controls;

namespace HojadeRuta2K23.Paginas;

public partial class NuevaTarea : Page
{
    public NuevaTarea()
    {
        InitializeComponent();
        Combo.Items.Add("Funcionario");
        Combo.Items.Add("cliente");
    }


    private void Combo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (Combo.SelectedIndex == 0)
        {
            funcionario.Visibility = Visibility.Visible;
            cliente.Visibility = Visibility.Collapsed;
        }
        if (Combo.SelectedIndex == 1)
        {
            cliente.Visibility = Visibility.Visible;
            funcionario.Visibility = Visibility.Collapsed;
        }
    }
}