using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;
using MessageBox = HandyControl.Controls.MessageBox;


namespace HojadeRuta2K23.Paginas;

public partial class qr : Page
{
    public qr()
    {
        InitializeComponent();
    }
    
    private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        try
        {
           
            // Obtener la URI del Hyperlink
            Uri uri = ((Hyperlink)e.Source).NavigateUri;

            // Abrir la página web en el navegador predeterminado
            Process.Start(new ProcessStartInfo
            {
                FileName = uri.AbsoluteUri,
                UseShellExecute = true
            });

            // Indicar que el evento ha sido manejado
            e.Handled = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al abrir la página web: {ex.Message}");
        }
    }
}