﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace HojadeRuta2K23.Paginas;

public partial class Tramites : Page
{
    public List<member> usuarios = new List<member>();

    public Tramites()
    {
        InitializeComponent();
        ComboBoxVistas.Items.Add("Vista Cuadriculada");
        ComboBoxVistas.Items.Add("Vista Lista");

        usuarios.Add(new member
        {
            Id = "1",
            Foto =
                "",
            NombreCompleto = "Control De Notas",
            Profesion = "Tramite",
            Email = "En Curso",
            Celular = "Lic. Jorge Padilla",
            Ubicacion = "Daniel Moya",
            Operations = "Operación 1"
        });

        usuarios.Add(new member
        {
            Id = "1",
            Foto =
                "",
            NombreCompleto = "Control De Notas",
            Profesion = "Tramite",
            Email = "En Curso",
            Celular = "Lic. Jorge Padilla",
            Ubicacion = "Daniel Moya",
            Operations = "Operación 1"
        });
        usuarios.Add(new member
        {
            Id = "1",
            Foto =
                "",
            NombreCompleto = "Control De Notas",
            Profesion = "Tramite",
            Email = "En Curso",
            Celular = "Lic. Jorge Padilla",
            Ubicacion = "Daniel Moya",
            Operations = "Operación 1"
        });
        usuarios.Add(new member
        {
            Id = "1",
            Foto =
                "",
            NombreCompleto = "Control De Notas",
            Profesion = "Tramite",
            Email = "En Curso",
            Celular = "Lic. Jorge Padilla",
            Ubicacion = "Daniel Moya",
            Operations = "Operación 1"
        });
        usuarios.Add(new member
        {
            Id = "1",
            Foto =
                "",
            NombreCompleto = "Control De Notas",
            Profesion = "Tramite",
            Email = "En Curso",
            Celular = "Lic. Jorge Padilla",
            Ubicacion = "Daniel Moya",
            Operations = "Operación 1"
        });
        usuarios.Add(new member
        {
            Id = "1",
            Foto =
                "",
            NombreCompleto = "Control De Notas",
            Profesion = "Tramite",
            Email = "En Curso",
            Celular = "Lic. Jorge Padilla",
            Ubicacion = "Daniel Moya",
            Operations = "Operación 1"
        });
        usuarios.Add(new member
        {
            Id = "1",
            Foto =
                "",
            NombreCompleto = "Control De Notas",
            Profesion = "Tramite",
            Email = "En Curso",
            Celular = "Lic. Jorge Padilla",
            Ubicacion = "Daniel Moya",
            Operations = "Operación 1"
        });
        usuarios.Add(new member
        {
            Id = "1",
            Foto =
                "",
            NombreCompleto = "Control De Notas",
            Profesion = "Tramite",
            Email = "En Curso",
            Celular = "Lic. Jorge Padilla",
            Ubicacion = "Daniel Moya",
            Operations = "Operación 1"
        });
        usuarios.Add(new member
        {
            Id = "1",
            Foto =
                "",
            NombreCompleto = "Control De Notas",
            Profesion = "Tramite",
            Email = "En Curso",
            Celular = "Lic. Jorge Padilla",
            Ubicacion = "Daniel Moya",
            Operations = "Operación 1"
        });
        usuarios.Add(new member
        {
            
            Id = "1",
            Foto =
                "",
            NombreCompleto = "Control De Notas",
            Profesion = "Tramite",
            Email = "En Curso",
            Celular = "Lic. Jorge Padilla",
            Ubicacion = "Daniel Moya",
            Operations = "Operación 1"
        });

        membersDataGrid.ItemsSource = usuarios;
    }
    
    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        if (NavigationService != null)
        {
            NavigationService.Navigate(new NuevaTarea());
        }
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
}