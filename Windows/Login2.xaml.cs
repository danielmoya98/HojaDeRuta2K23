using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace HojadeRuta2K23.Windows;

public partial class Login2 : Window
{
    public Login2()
    {
        InitializeComponent();
    }

    private void Hyperlink_OnClick(object sender, RoutedEventArgs e)
    {
        DoubleAnimation animation1 = new DoubleAnimation();
        animation1.From = 300;
        animation1.To = 0;
        animation1.Duration = new Duration(TimeSpan.FromSeconds(0.5));
        Border1.BeginAnimation(StackPanel.WidthProperty, animation1);
        
        DoubleAnimation animation2 = new DoubleAnimation();
        animation2.From = 300;
        animation2.To = 0;
        animation2.Duration = new Duration(TimeSpan.FromSeconds(0.5));
        Border2.BeginAnimation(StackPanel.WidthProperty, animation2);

        Border1.Visibility = Visibility.Collapsed;
        Border2.Visibility = Visibility.Collapsed;
        Info.Visibility = Visibility.Visible;
        Border.Visibility = Visibility.Visible;
        ingresar.Visibility = Visibility.Collapsed;
        enviar.Visibility = Visibility.Visible;
        olvidar.Visibility = Visibility.Collapsed;
        tengor.Visibility = Visibility.Visible;

    }

    private void Hyperlink2_OnClick(object sender, RoutedEventArgs e)
    {
        DoubleAnimation animation1 = new DoubleAnimation();
        animation1.From = 0;
        animation1.To = 300;
        animation1.Duration = new Duration(TimeSpan.FromSeconds(0.5));
        Border1.BeginAnimation(StackPanel.WidthProperty, animation1);
        
        DoubleAnimation animation2 = new DoubleAnimation();
        animation2.From = 0;
        animation2.To = 300;
        animation2.Duration = new Duration(TimeSpan.FromSeconds(0.5));
        Border2.BeginAnimation(StackPanel.WidthProperty, animation2);
        
        Border1.Visibility = Visibility.Visible;
        Border2.Visibility = Visibility.Visible;
        Info.Visibility = Visibility.Collapsed;
        Border.Visibility = Visibility.Collapsed;
        ingresar.Visibility = Visibility.Visible;
        enviar.Visibility = Visibility.Collapsed;
        olvidar.Visibility = Visibility.Visible;
        tengor.Visibility = Visibility.Collapsed;
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}