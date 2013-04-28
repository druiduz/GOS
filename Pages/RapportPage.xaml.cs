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
using GOS.Classes;

namespace GOS.Pages
{
    /// <summary>
    /// Logique d'interaction pour RapportPage.xaml
    /// </summary>
    public partial class RapportPage : Page
    {
        private int typeExport; //0: csv, 1: mail

        public RapportPage()
        {
            InitializeComponent();
            typeExport = -1;
        }

        private void radioCsv_Checked(object sender, RoutedEventArgs e)
        {
            this.txtPath.IsEnabled = true;
            this.txtMail.IsEnabled = false;
        }
        
        private void radioMail_Checked(object sender, RoutedEventArgs e)
        {
            this.txtPath.IsEnabled = false;
            this.txtMail.IsEnabled = true;
        }

        private void btnCreer_Click(object sender, RoutedEventArgs e)
        {
            DateTime debut = this.datePickerDebut.SelectedDate.Value, fin = this.datePickerFin.SelectedDate.Value;

            if (debut >= fin)
            {
                MessageBox.Show("Mauvais interval de temps");
                return;
            }


            //return;

            switch(typeExport)
            {
                case 0:
                    string pathExport = this.txtPath.Text;
                    Repporting.repportCsv(debut, fin, pathExport);
                    break;
                case 1:
                    string mailExport = this.txtMail.Text;
                    Repporting.repportMail(debut, fin, mailExport);
                    break;
                case -1:
                default:
                    MessageBox.Show("Veuillez specifié le type d'export");
                    break;
            }

            return;
        }

        private void btnRetour_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)this.Parent;
            main.Content = new HomePage();
        }
    }
}
