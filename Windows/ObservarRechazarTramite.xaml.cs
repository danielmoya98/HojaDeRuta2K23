using System.Windows;

namespace HojadeRuta2K23.Windows;

public partial class ObservarRechazarTramite : Window
{
    public ObservarRechazarTramite()
    {
        InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}