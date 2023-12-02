using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using HojadeRuta2K23.Windows;
using MaterialDesignThemes.Wpf;

namespace HojadeRuta2K23.Paginas;

public partial class AdministracionUsuarios : Page
{
    public List<member> usuarios = new List<member>();

    public AdministracionUsuarios()
    {
        InitializeComponent();
        // ComboBoxVistas.Items.Add("Vista Cuadriculada");
        // ComboBoxVistas.Items.Add("Vista Lista");
        //
        Combo.Items.Add("Funcionario");
        Combo.Items.Add("cliente");
   
            
        usuarios.Add(new member
        {
            Id = "1",
            Foto =
                "",
            NombreCompleto = "Pablo Chambi",
            Profesion = "Ingeniero",
            Email = "pablo@example.com",
            Celular = "123-456-7890",
            Ubicacion = "Ciudad",
            Operations = "Operación 1"
        });

        usuarios.Add(new member
        {
            Id = "2",
            Foto =
                "",
            NombreCompleto = "María López",
            Profesion = "Diseñadora",
            Email = "maria@example.com",
            Celular = "987-654-3210",
            Ubicacion = "Otra Ciudad",
            Operations = "Operación 2"
        });
        usuarios.Add(new member
        {
            Id = "1",
            Foto =
                "",
            NombreCompleto = "Pablo Chambi",
            Profesion = "Ingeniero",
            Email = "pablo@example.com",
            Celular = "123-456-7890",
            Ubicacion = "Ciudad",
            Operations = "Operación 1"
        });

        usuarios.Add(new member
        {
            Id = "2",
            Foto =
                "",
            NombreCompleto = "María López",
            Profesion = "Diseñadora",
            Email = "maria@example.com",
            Celular = "987-654-3210",
            Ubicacion = "Otra Ciudad",
            Operations = "Operación 2"
        });
        usuarios.Add(new member
        {
            Id = "1",
            Foto =
                "",
            NombreCompleto = "Pablo Chambi",
            Profesion = "Ingeniero",
            Email = "pablo@example.com",
            Celular = "123-456-7890",
            Ubicacion = "Ciudad",
            Operations = "Operación 1"
        });

        usuarios.Add(new member
        {
            Id = "2",
            Foto =
                "",
            NombreCompleto = "María López",
            Profesion = "Diseñadora",
            Email = "maria@example.com",
            Celular = "987-654-3210",
            Ubicacion = "Otra Ciudad",
            Operations = "Operación 2"
        });

        usuarios.Add(new member
        {
            Id = "1",
            Foto =
                "",
            NombreCompleto = "Pablo Chambi",
            Profesion = "Ingeniero",
            Email = "pablo@example.com",
            Celular = "123-456-7890",
            Ubicacion = "Ciudad",
            Operations = "Operación 1"
        });

        usuarios.Add(new member
        {
            Id = "2",
            Foto =
                "",
            NombreCompleto = "María López",
            Profesion = "Diseñadora",
            Email = "maria@example.com",
            Celular = "987-654-3210",
            Ubicacion = "Otra Ciudad",
            Operations = "Operación 2"
        });
        usuarios.Add(new member
        {
            Id = "1",
            Foto =
                "",
            NombreCompleto = "Pablo Chambi",
            Profesion = "Ingeniero",
            Email = "pablo@example.com",
            Celular = "123-456-7890",
            Ubicacion = "Ciudad",
            Operations = "Operación 1"
        });

        usuarios.Add(new member
        {
            Id = "2",
            Foto =
                "",
            NombreCompleto = "María López",
            Profesion = "Diseñadora",
            Email = "maria@example.com",
            Celular = "987-654-3210",
            Ubicacion = "Otra Ciudad",
            Operations = "Operación 2"
        });
        usuarios.Add(new member
        {
            Id = "1",
            Foto =
                "",
            NombreCompleto = "Pablo Chambi",
            Profesion = "Ingeniero",
            Email = "pablo@example.com",
            Celular = "123-456-7890",
            Ubicacion = "Ciudad",
            Operations = "Operación 1"
        });

        usuarios.Add(new member
        {
            Id = "2",
            Foto =
                "",
            NombreCompleto = "María López",
            Profesion = "Diseñadora",
            Email = "maria@example.com",
            Celular = "987-654-3210",
            Ubicacion = "Otra Ciudad",
            Operations = "Operación 2"
        });
        usuarios.Add(new member
        {
            Id = "1",
            Foto =
                "",
            NombreCompleto = "Pablo Chambi",
            Profesion = "Ingeniero",
            Email = "pablo@example.com",
            Celular = "123-456-7890",
            Ubicacion = "Ciudad",
            Operations = "Operación 1"
        });

        usuarios.Add(new member
        {
            Id = "2",
            Foto =
                "",
            NombreCompleto = "María López",
            Profesion = "Diseñadora",
            Email = "maria@example.com",
            Celular = "987-654-3210",
            Ubicacion = "Otra Ciudad",
            Operations = "Operación 2"
        });
        usuarios.Add(new member
        {
            Id = "1",
            Foto =
                "",
            NombreCompleto = "Pablo Chambi",
            Profesion = "Ingeniero",
            Email = "pablo@example.com",
            Celular = "123-456-7890",
            Ubicacion = "Ciudad",
            Operations = "Operación 1"
        });

        usuarios.Add(new member
        {
            Id = "2",
            Foto =
                "",
            NombreCompleto = "María López",
            Profesion = "Diseñadora",
            Email = "maria@example.com",
            Celular = "987-654-3210",
            Ubicacion = "Otra Ciudad",
            Operations = "Operación 2"
        });
        membersDataGrid.ItemsSource = usuarios;
    }


    public class member
    {
        public string Id { get; set; }
        public string Foto { get; set; }
        public string NombreCompleto { get; set; }
        public string Profesion { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public string Ubicacion { get; set; }
        public string Operations { get; set; }
    }

    private void Combo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (Combo.SelectedIndex == 0)
        {
            funcionario.Visibility = Visibility.Visible;
            cliente.Visibility = Visibility.Collapsed;
        }
        if (Combo.SelectedIndex == 1)
        {
            cliente.Visibility = Visibility.Visible;
            funcionario.Visibility = Visibility.Collapsed;
        }
    }
    
    private void AgregarUsuario_OnClick(object sender, RoutedEventArgs e)
    {
        DoubleAnimation animation = new DoubleAnimation();
        animation.From = 0;
        animation.To = 250;
        animation.Duration = new Duration(TimeSpan.FromSeconds(0.5));

        hola3.BeginAnimation(StackPanel.HeightProperty, animation);

        miBoton.Visibility = Visibility.Collapsed;
        miBoton1.Visibility = Visibility.Visible;
    }

    /*
    private void ComboBoxVistas_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        
        if (ComboBoxVistas.SelectedIndex == 0)
        {
            VistaCuadriculada.Visibility = Visibility.Visible;
            VistaLista.Visibility = Visibility.Hidden;
        }
        else if (ComboBoxVistas.SelectedIndex == 1)
        {
            VistaCuadriculada.Visibility = Visibility.Hidden;
            VistaLista.Visibility = Visibility.Visible;
        }

    }
    */

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        DoubleAnimation animation = new DoubleAnimation();
        animation.From = 250;
        animation.To = 0;
        animation.Duration = new Duration(TimeSpan.FromSeconds(0.5));

        hola3.BeginAnimation(StackPanel.HeightProperty, animation);
        miBoton.Visibility = Visibility.Visible;
        miBoton1.Visibility = Visibility.Collapsed;
    }

    private void MiBotonV_OnClick(object sender, RoutedEventArgs e)
    {
        DoubleAnimation animation = new DoubleAnimation();
        animation.From = 0;
        animation.To = 250;
        animation.Duration = new Duration(TimeSpan.FromSeconds(0.5));

        hola.BeginAnimation(StackPanel.WidthProperty, animation);
        miBotonV1.Visibility = Visibility.Visible;
        miBotonV.Visibility = Visibility.Collapsed;
    }

    private void MiBotonV1_OnClick(object sender, RoutedEventArgs e)
    {
        DoubleAnimation animation = new DoubleAnimation();
        animation.From = 250;
        animation.To = 0;
        animation.Duration = new Duration(TimeSpan.FromSeconds(0.5));

        hola.BeginAnimation(StackPanel.WidthProperty, animation);
        miBotonV.Visibility = Visibility.Visible;
        miBotonV1.Visibility = Visibility.Collapsed;
    }
}