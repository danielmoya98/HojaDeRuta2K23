using System.Windows;

namespace HojadeRuta2K23.Windows;

public partial class EditarTarea : Window
{
    public EditarTarea()
    {
        InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
       
        Close();
    }
}