using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HojadeRuta2K23.Paginas;

public partial class EditarPerfil : Window
{
    public EditarPerfil()
    {
        InitializeComponent();
    }

    private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        Close();
    }
}