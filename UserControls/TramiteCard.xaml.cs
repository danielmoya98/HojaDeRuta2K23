using System;
using System.Windows;
using System.Windows.Controls;

namespace HojadeRuta2K23.UserControls;

public partial class TramiteCard : UserControl
{
    public TramiteCard()
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
        DependencyProperty.Register("Foto", typeof(String), typeof(TramiteCard));

    //propiedad Tramite//  
    public String Tarea
    {
        get { return (String)GetValue(TareaProperty); }
        set { SetValue(TareaProperty, value); }
    }
    
    public static readonly DependencyProperty TareaProperty =
        DependencyProperty.Register("Tramite", typeof(String), typeof(TramiteCard));
    
    //propiedad EstadoTramite//  
    public String EstadoTramite
    {
        get { return (String)GetValue(EstadoTramiteProperty); }
        set { SetValue(EstadoTramiteProperty, value); }
    }
    
    public static readonly DependencyProperty EstadoTramiteProperty =
        DependencyProperty.Register("EstadoTramite", typeof(String), typeof(TramiteCard));

    //propiedad NumeroVistas//  
    public String NumeroVistas
    {
        get { return (String)GetValue(NumeroVistasProperty); }
        set { SetValue(NumeroVistasProperty, value); }
    }
    
    public static readonly DependencyProperty NumeroVistasProperty =
        DependencyProperty.Register("NumeroVistas", typeof(String), typeof(TramiteCard));


}