using System;
using System.Windows;
using System.Windows.Controls;

namespace HojadeRuta2K23.UserControls;

public partial class TareaCard : UserControl
{
    public TareaCard()
    {
        InitializeComponent();
    }
    
    //propiedad Foto//  
    public String Foto
    {
        get { return (String)GetValue(FotoProperty); }
        set { SetValue(FotoProperty, value); }
    }
    
    public static readonly DependencyProperty FotoProperty =
        DependencyProperty.Register("Foto", typeof(String), typeof(TareaCard));

    //propiedad Tarea//  
    public String Tarea
    {
        get { return (String)GetValue(TareaProperty); }
        set { SetValue(TareaProperty, value); }
    }
    
    public static readonly DependencyProperty TareaProperty =
        DependencyProperty.Register("Tarea", typeof(String), typeof(TareaCard));
    
    //propiedad EstadoTarea//  
    public String EstadoTarea
    {
        get { return (String)GetValue(EstadoTareaProperty); }
        set { SetValue(EstadoTareaProperty, value); }
    }
    
    public static readonly DependencyProperty EstadoTareaProperty =
        DependencyProperty.Register("EstadoTarea", typeof(String), typeof(TareaCard));

    //propiedad NumeroVistas//  
    public String NumeroVistas
    {
        get { return (String)GetValue(NumeroVistasProperty); }
        set { SetValue(NumeroVistasProperty, value); }
    }
    
    public static readonly DependencyProperty NumeroVistasProperty =
        DependencyProperty.Register("NumeroVistas", typeof(String), typeof(TareaCard));
}