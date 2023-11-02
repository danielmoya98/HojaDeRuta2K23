using System.Collections.Generic;
using System.Data.Common;
using System.Windows;
using System.Windows.Controls;
using HojadeRuta2K23.Windows;
using MaterialDesignThemes.Wpf;

namespace HojadeRuta2K23.Paginas;

public partial class AdministracionUsuarios : Page
{
    public List<member> usuarios = new List<member>();

    public AdministracionUsuarios()
    {
        InitializeComponent();
        usuarios.Add(new member
        {
            Id = "1",
            Foto =
                "C:\\Users\\Alienware\\RiderProjects\\HojadeRuta2K23\\Imagenes\\44110f877fb9796236f3d9c9cc3677da.gif",
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
                "C:\\Users\\Alienware\\RiderProjects\\HojadeRuta2K23\\Imagenes\\44110f877fb9796236f3d9c9cc3677da.gif",
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
                "C:\\Users\\Alienware\\RiderProjects\\HojadeRuta2K23\\Imagenes\\44110f877fb9796236f3d9c9cc3677da.gif",
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
                "C:\\Users\\Alienware\\RiderProjects\\HojadeRuta2K23\\Imagenes\\44110f877fb9796236f3d9c9cc3677da.gif",
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
                "C:\\Users\\Alienware\\RiderProjects\\HojadeRuta2K23\\Imagenes\\44110f877fb9796236f3d9c9cc3677da.gif",
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
                "C:\\Users\\Alienware\\RiderProjects\\HojadeRuta2K23\\Imagenes\\44110f877fb9796236f3d9c9cc3677da.gif",
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
                "C:\\Users\\Alienware\\RiderProjects\\HojadeRuta2K23\\Imagenes\\44110f877fb9796236f3d9c9cc3677da.gif",
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
                "C:\\Users\\Alienware\\RiderProjects\\HojadeRuta2K23\\Imagenes\\44110f877fb9796236f3d9c9cc3677da.gif",
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
                "C:\\Users\\Alienware\\RiderProjects\\HojadeRuta2K23\\Imagenes\\44110f877fb9796236f3d9c9cc3677da.gif",
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
                "C:\\Users\\Alienware\\RiderProjects\\HojadeRuta2K23\\Imagenes\\44110f877fb9796236f3d9c9cc3677da.gif",
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
                "C:\\Users\\Alienware\\RiderProjects\\HojadeRuta2K23\\Imagenes\\44110f877fb9796236f3d9c9cc3677da.gif",
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
                "C:\\Users\\Alienware\\RiderProjects\\HojadeRuta2K23\\Imagenes\\44110f877fb9796236f3d9c9cc3677da.gif",
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
                "C:\\Users\\Alienware\\RiderProjects\\HojadeRuta2K23\\Imagenes\\44110f877fb9796236f3d9c9cc3677da.gif",
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
                "C:\\Users\\Alienware\\RiderProjects\\HojadeRuta2K23\\Imagenes\\44110f877fb9796236f3d9c9cc3677da.gif",
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
                "C:\\Users\\Alienware\\RiderProjects\\HojadeRuta2K23\\Imagenes\\44110f877fb9796236f3d9c9cc3677da.gif",
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
                "C:\\Users\\Alienware\\RiderProjects\\HojadeRuta2K23\\Imagenes\\44110f877fb9796236f3d9c9cc3677da.gif",
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

    private void AgregarUsuario_OnClick(object sender, RoutedEventArgs e)
    {
        AgregarUsuario hola = new AgregarUsuario();
        hola.ShowDialog();
    }
    
}