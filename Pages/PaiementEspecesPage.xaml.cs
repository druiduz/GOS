﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GOS.Classes;

namespace GOS.Pages
{
    /// <summary>
    /// Logique d'interaction pour PaiementEspecesPage.xaml
    /// </summary>
    public partial class PaiementEspecesPage : Page
    {
        Vente panier;

        public PaiementEspecesPage(Vente panier)
        {
            InitializeComponent();
            this.panier = panier;
        }
    }
}
