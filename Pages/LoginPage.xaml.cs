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
    /// Logique d'interaction pour LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void btnConnexion_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)this.Parent;
            try
            {
                main.vendeur = User.connect(this.inputLogin.Text, this.inputPass.Password);
                if (main.vendeur != null)
                {
                    HomePage home = new HomePage();
                    main.Content = home;
                }
                else
                {
                    MessageBox.Show("Login / mdp incorrect");
                }
            }
            catch (Exception exect)
            {

            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.inputLogin.Focus();
        }
    }
}
