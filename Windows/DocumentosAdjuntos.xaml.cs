﻿using System.Windows;

namespace HojadeRuta2K23.Windows;

public partial class DocumentosAdjuntos : Window
{
    public DocumentosAdjuntos()
    {
        InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}