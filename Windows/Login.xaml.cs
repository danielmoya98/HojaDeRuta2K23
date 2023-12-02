using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using HandyControl.Controls;
using Window = System.Windows.Window;

namespace HojadeRuta2K23.Windows;

public partial class Login : Window
{
    public Login()
    {
        InitializeComponent();
    }


    private void Hyperlink_OnClick(object sender, RoutedEventArgs e)
    {
        DoubleAnimation animation = new DoubleAnimation();
        animation.From = 225;
        animation.To = 0;
        animation.Duration = new Duration(TimeSpan.FromSeconds(0.5));

        Email.BeginAnimation(StackPanel.WidthProperty, animation);
        
        DoubleAnimation animation1 = new DoubleAnimation();
        animation1.From = 225;
        animation1.To = 0;
        animation1.Duration = new Duration(TimeSpan.FromSeconds(0.5));

        Contraseña.BeginAnimation(StackPanel.WidthProperty, animation);
        Info.Visibility = Visibility.Visible;
        RecuperarContraseña.Visibility = Visibility.Visible;
        Email.Visibility = Visibility.Collapsed;
        iniciar.Visibility = Visibility.Collapsed;
        enviar.Visibility = Visibility.Visible;
        Contraseña.Visibility = Visibility.Collapsed;
        recuperar.Visibility = Visibility.Collapsed;
        tengo.Visibility = Visibility.Visible;

    }

    private void Hyperlink2_OnClick(object sender, RoutedEventArgs e)
    {
        DoubleAnimation animation = new DoubleAnimation();
        animation.From = 0;
        animation.To = 225;
        animation.Duration = new Duration(TimeSpan.FromSeconds(0.5));

        Email.BeginAnimation(StackPanel.WidthProperty, animation);
        
        DoubleAnimation animation1 = new DoubleAnimation();
        animation1.From = 0;
        animation1.To = 225;
        animation1.Duration = new Duration(TimeSpan.FromSeconds(0.5));

        Contraseña.BeginAnimation(StackPanel.WidthProperty, animation);
        Info.Visibility = Visibility.Collapsed;
        RecuperarContraseña.Visibility = Visibility.Collapsed;
        Email.Visibility = Visibility.Visible;
        iniciar.Visibility = Visibility.Visible;
        enviar.Visibility = Visibility.Collapsed;
        Contraseña.Visibility = Visibility.Visible;
        recuperar.Visibility = Visibility.Visible;
        tengo.Visibility = Visibility.Collapsed;
        
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}