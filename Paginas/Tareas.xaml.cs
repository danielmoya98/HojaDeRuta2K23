﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HojadeRuta2K23.Paginas;

public partial class Tareas : Page
{
    public List<member> usuarios = new List<member>();

    public Tareas()
    {
        InitializeComponent();
        usuarios.Add(new member
        {
            Id = "1",
            Foto =
                "C:\\Users\\Alienware\\RiderProjects\\HojadeRuta2K23\\Imagenes\\icons8-carpeta-48.png",
            NombreCompleto = "Control De Notas",
            Profesion = "Secretaria",
            Email = "En Curso",
            Celular = "Alta",
            Ubicacion = "11/12/2023",
            Operations = "Operación 1"
        });

        usuarios.Add(new member
        {
            Id = "1",
            Foto =
                "C:\\Users\\Alienware\\RiderProjects\\HojadeRuta2K23\\Imagenes\\icons8-carpeta-48.png",
            NombreCompleto = "Control De Notas",
            Profesion = "Secretaria",
            Email = "En Curso",
            Celular = "Alta",
            Ubicacion = "11/12/2023",
            Operations = "Operación 1"
        });
        usuarios.Add(new member
        {
            Id = "1",
            Foto =
                "C:\\Users\\Alienware\\RiderProjects\\HojadeRuta2K23\\Imagenes\\icons8-carpeta-48.png",
            NombreCompleto = "Control De Notas",
            Profesion = "Secretaria",
            Email = "En Curso",
            Celular = "Alta",
            Ubicacion = "11/12/2023",
            Operations = "Operación 1"
        });
        usuarios.Add(new member
        {
            Id = "1",
            Foto =
                "C:\\Users\\Alienware\\RiderProjects\\HojadeRuta2K23\\Imagenes\\icons8-carpeta-48.png",
            NombreCompleto = "Control De Notas",
            Profesion = "Secretaria",
            Email = "En Curso",
            Celular = "Alta",
            Ubicacion = "11/12/2023",
            Operations = "Operación 1"
        });
        usuarios.Add(new member
        {
            Id = "1",
            Foto =
                "C:\\Users\\Alienware\\RiderProjects\\HojadeRuta2K23\\Imagenes\\icons8-carpeta-48.png",
            NombreCompleto = "Control De Notas",
            Profesion = "Secretaria",
            Email = "En Curso",
            Celular = "Alta",
            Ubicacion = "11/12/2023",
            Operations = "Operación 1"
        });
        usuarios.Add(new member
        {
            Id = "1",
            Foto =
                "C:\\Users\\Alienware\\RiderProjects\\HojadeRuta2K23\\Imagenes\\icons8-carpeta-48.png",
            NombreCompleto = "Control De Notas",
            Profesion = "Secretaria",
            Email = "En Curso",
            Celular = "Alta",
            Ubicacion = "11/12/2023",
            Operations = "Operación 1"
        });
        usuarios.Add(new member
        {
            Id = "1",
            Foto =
                "C:\\Users\\Alienware\\RiderProjects\\HojadeRuta2K23\\Imagenes\\icons8-carpeta-48.png",
            NombreCompleto = "Control De Notas",
            Profesion = "Secretaria",
            Email = "En Curso",
            Celular = "Alta",
            Ubicacion = "11/12/2023",
            Operations = "Operación 1"
        });
        usuarios.Add(new member
        {
            Id = "1",
            Foto =
                "C:\\Users\\Alienware\\RiderProjects\\HojadeRuta2K23\\Imagenes\\icons8-carpeta-48.png",
            NombreCompleto = "Control De Notas",
            Profesion = "Secretaria",
            Email = "En Curso",
            Celular = "Alta",
            Ubicacion = "11/12/2023",
            Operations = "Operación 1"
        });
        usuarios.Add(new member
        {
            Id = "1",
            Foto =
                "C:\\Users\\Alienware\\RiderProjects\\HojadeRuta2K23\\Imagenes\\icons8-carpeta-48.png",
            NombreCompleto = "Control De Notas",
            Profesion = "Secretaria",
            Email = "En Curso",
            Celular = "Alta",
            Ubicacion = "11/12/2023",
            Operations = "Operación 1"
        });
        usuarios.Add(new member
        {
            Id = "1",
            Foto =
                "C:\\Users\\Alienware\\RiderProjects\\HojadeRuta2K23\\Imagenes\\icons8-carpeta-48.png",
            NombreCompleto = "Control De Notas",
            Profesion = "Secretaria",
            Email = "En Curso",
            Celular = "Alta",
            Ubicacion = "11/12/2023",
            Operations = "Operación 1"
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

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        if (NavigationService != null)
        {
            NavigationService.Navigate(new NuevaTarea());
        }
    }
}