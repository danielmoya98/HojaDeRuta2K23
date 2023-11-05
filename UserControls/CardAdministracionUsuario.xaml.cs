using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HojadeRuta2K23.UserControls;

public partial class CardAdministracionUsuario : UserControl
{
    
    public static readonly RoutedUICommand EliminarCommand = new RoutedUICommand("Eliminar", "Eliminar", typeof(CardAdministracionUsuario));
    public static readonly RoutedUICommand EditarCommand = new RoutedUICommand("Editar", "Editar", typeof(CardAdministracionUsuario));

    public CardAdministracionUsuario()
    {
        InitializeComponent();
    }
    
    private void LockButton_Click(object sender, RoutedEventArgs e)
    {
        EliminarCommand.Execute(null, this);
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        EditarCommand.Execute(null, this);
    }
    
    //propiedad Foto//  
    public String Foto
    {
        get { return (String)GetValue(FotoProperty); }
        set { SetValue(FotoProperty, value); }
    }
    
    public static readonly DependencyProperty FotoProperty =
        DependencyProperty.Register("Foto", typeof(String), typeof(CardAdministracionUsuario));

    //propiedad Nombre//  
    public String Nombre
    {
        get { return (String)GetValue(NombreProperty); }
        set { SetValue(NombreProperty, value); }
    }
    
    public static readonly DependencyProperty NombreProperty =
        DependencyProperty.Register("Nombre", typeof(String), typeof(CardAdministracionUsuario));
    
    //propiedad Estado//  
    public String Estado
    {
        get { return (String)GetValue(EstadoProperty); }
        set { SetValue(EstadoProperty, value); }
    }
    
    public static readonly DependencyProperty EstadoProperty =
        DependencyProperty.Register("Estado", typeof(String), typeof(CardAdministracionUsuario));

}