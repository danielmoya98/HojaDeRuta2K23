using System.Windows;

namespace HojadeRuta2K23.Windows;

public partial class BtnAdmitir : Window
{
    public BtnAdmitir()
    {
        InitializeComponent();
        instancia.Visibility = Visibility.Visible;
    }

    private void Flujo_OnChecked(object sender, RoutedEventArgs e)
    {
        if (flujo.IsChecked == true)
        {
            instancia.Visibility = Visibility.Collapsed;
            btn.Visibility = Visibility.Visible;
            combo.Visibility = Visibility.Visible;
        }
       
    }

    private void Flujo_OnUnchecked(object sender, RoutedEventArgs e)
    {
        if(flujo.IsChecked == false)
        {
            instancia.Visibility = Visibility.Visible;
            btn.Visibility = Visibility.Collapsed;
            combo.Visibility = Visibility.Collapsed;
        }   
    }
}