using System.Windows.Controls;
using System.Windows.Input;

namespace HojadeRuta2K23.Paginas;

public partial class Perfil : Page
{
    public Perfil()
    {
        InitializeComponent();
    }

    private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        EditarPerfil edit = new EditarPerfil();
        edit.Show();
    }
}