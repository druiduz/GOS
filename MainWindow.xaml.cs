using System;
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
using GOS.Pages;
using GOS.Classes;

namespace GOS
{
    
    public partial class MainWindow : Window
    {
        public User vendeur;

        public MainWindow()
        {
            InitializeComponent();
            LoginPage login = new LoginPage();
            this.Content = login;
        }
    }
}
