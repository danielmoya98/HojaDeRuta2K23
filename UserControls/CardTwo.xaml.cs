using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HojadeRuta2K23.UserControls;

public partial class CardTwo : UserControl
{
    public CardTwo()
    {
        InitializeComponent();
        // Agregar un manejador de eventos para el clic
        this.MouseLeftButtonDown += CardTwo_Click;
    }
    
    public event RoutedEventHandler Click;

    private void CardTwo_Click(object sender, MouseButtonEventArgs e)
    {
        // Disparar el evento Click si está suscrito
        Click?.Invoke(this, new RoutedEventArgs());
    }
    
    
    
    //propiedad Color1//  
    public String Color1
    {
        get { return (String)GetValue(Color1Property); }
        set { SetValue(Color1Property, value); }
    }
    
    public static readonly DependencyProperty Color1Property =
        DependencyProperty.Register("Color1", typeof(String), typeof(CardTwo));

    //propiedad Color2//  
    public String Color2
    {
        get { return (String)GetValue(Color2Property); }
        set { SetValue(Color2Property, value); }
    }
    
    public static readonly DependencyProperty Color2Property =
        DependencyProperty.Register("Color2", typeof(String), typeof(CardTwo));
    
    //propiedad Titulo largo//  
    public String TituloLargo
    {
        get { return (String)GetValue(TituloLargoProperty); }
        set { SetValue(TituloLargoProperty, value); }
    }
    
    public static readonly DependencyProperty TituloLargoProperty =
        DependencyProperty.Register("TituloLargo", typeof(String), typeof(CardTwo));

    //propiedad Titulo//  
    public String Titulo
    {
        get { return (String)GetValue(TituloProperty); }
        set { SetValue(TituloProperty, value); }
    }
    
    public static readonly DependencyProperty TituloProperty =
        DependencyProperty.Register("Titulo", typeof(String), typeof(CardTwo));

    //propiedad Icono//  
    public String Icono
    {
        get { return (String)GetValue(IconoProperty); }
        set { SetValue(IconoProperty, value); }
    }
    
    public static readonly DependencyProperty IconoProperty =
        DependencyProperty.Register("Icono", typeof(String), typeof(CardTwo));

    
}