using System.Windows;

namespace HojadeRuta2K23.Windows;

public partial class RechazarTarea : Window
{
    public RechazarTarea()
    {
        InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}