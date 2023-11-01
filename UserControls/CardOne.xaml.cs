using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace HojadeRuta2K23.UserControls;

public partial class CardOne : UserControl
{
    public CardOne()
    {
        InitializeComponent();
        this.IsHitTestVisible = true;
        
        // Agregar un manejador de eventos para el clic
        this.MouseLeftButtonDown += CardOne_Click;
    }
    
    
    public event RoutedEventHandler Click;

    private void CardOne_Click(object sender, MouseButtonEventArgs e)
    {
        // Disparar el evento Click si está suscrito
        Click?.Invoke(this, new RoutedEventArgs());
    }

    
    public bool IsActive
    {
        get { return (bool)GetValue(IsActiveProperty); }
        set { SetValue(IsActiveProperty, value); }
    }

    public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(CardOne));

    
    //titulo uno
    public string TituloUno
    {
        get { return (string)GetValue(TituloUnoProperty); }
        set { SetValue(TituloUnoProperty, value); }
    }

    public static readonly DependencyProperty TituloUnoProperty = DependencyProperty.Register("TituloUno", typeof(string), typeof(CardOne));
    
    //titulo 2.1
    public string TituloDos
    {
        get { return (string)GetValue(TituloDosProperty); }
        set { SetValue(TituloDosProperty, value); }
    }

    public static readonly DependencyProperty TituloDosProperty = DependencyProperty.Register("TituloDos", typeof(string), typeof(CardOne));

    //titulo 2.1
    public string TituloDosUno
    {
        get { return (string)GetValue(TituloDosUnoProperty); }
        set { SetValue(TituloDosUnoProperty, value); }
    }

    public static readonly DependencyProperty TituloDosUnoProperty = DependencyProperty.Register("TituloDosUno", typeof(string), typeof(CardOne));

    //titulo  2.2
    public string TituloDosDos
    {
        get { return (string)GetValue(TituloDosDosProperty); }
        set { SetValue(TituloDosDosProperty, value); }
    }

    public static readonly DependencyProperty TituloDosDosProperty = DependencyProperty.Register("TituloDosDos", typeof(string), typeof(CardOne));

    //titulo 2.3
    public string TituloDosTres
    {
        get { return (string)GetValue(TituloDosTresProperty); }
        set { SetValue(TituloDosTresProperty, value); }
    }

    public static readonly DependencyProperty TituloDosTresProperty = DependencyProperty.Register("TituloDosTres", typeof(string), typeof(CardOne));


    
    //titulo tres
    public string TituloTres
    {
        get { return (string)GetValue(TituloTresProperty); }
        set { SetValue(TituloTresProperty, value); }
    }

    public static readonly DependencyProperty TituloTresProperty = DependencyProperty.Register("TituloTres", typeof(string), typeof(CardOne));

    
    //propiedad imagen//  
    public ImageSource Image
    {
        get { return (ImageSource)GetValue(ImageProperty); }
        set { SetValue(ImageProperty, value); }
    }
    
    public static readonly DependencyProperty ImageProperty =
        DependencyProperty.Register("Image", typeof(ImageSource), typeof(CardOne));

}