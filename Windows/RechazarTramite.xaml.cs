using System.Windows;

namespace HojadeRuta2K23.Windows;

public partial class RechazarTramite : Window
{
    public RechazarTramite()
    {
        InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}