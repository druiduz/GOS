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

        DateTime debut, fin;
        private int typeExport; //0: csv, 1: mail
        private string pathDest;
        private string emailDest;

        public RapportPage()
        {
            InitializeComponent();
            typeExport = -1;
            pathDest = "";
            emailDest = "";
        }

        private void radioCsv_Checked(object sender, RoutedEventArgs e)
        {
            this.txtPath.IsEnabled = true;
            this.txtMail.IsEnabled = false;
            this.typeExport = 0;
        }
        
        private void radioMail_Checked(object sender, RoutedEventArgs e)
        {
            this.txtPath.IsEnabled = false;
            this.txtMail.IsEnabled = true;
            this.typeExport = 1;
        }

        private bool validateForm()
        {

            if (this.debut.ToString() == new DateTime().ToString())
            {
                MessageBox.Show("Une date de début doit être spécifié");
                return false;
            }
            if (this.fin.ToString() == new DateTime().ToString())
            {
                MessageBox.Show("Une date de fin doit être spécifié");
                return false;
            }

            if (this.debut >= this.fin)
            {
                MessageBox.Show("La date de début doit être antérieur à la date de fin");
                return false;
            }

            switch(typeExport)
            {
                case 0:
                    if (String.IsNullOrEmpty(this.pathDest))
                    {
                        MessageBox.Show("Le chemin de destination de l'export doit être spécifé");
                        return false;
                    }
                    break;
                case 1:
                    if (String.IsNullOrEmpty(this.emailDest))
                    {
                        MessageBox.Show("L'email du destinataire doit être spécifié");
                        return false;
                    }
                    break;
                case -1:
                default:
                    MessageBox.Show("Veuillez specifié le type d'export");
                    return false;
                    break;
            }

            return true;
        }

        private void btnCreer_Click(object sender, RoutedEventArgs e)
        {
            if (this.datePickerDebut.SelectedDate.HasValue)
            {
                this.debut = this.datePickerDebut.SelectedDate.Value;
            }
            if (this.datePickerFin.SelectedDate.HasValue)
            {
                this.fin = this.datePickerFin.SelectedDate.Value;
            }

            this.pathDest = this.txtPath.Text;
            this.emailDest = this.txtMail.Text;

            if(!this.validateForm())
            {
                return;
            }

            return;

            try
            {
                switch (typeExport)
                {
                    case 0:
                        Repporting.repportCsv(debut, fin, pathDest);
                        break;
                    case 1:
                        Repporting.repportMail(debut, fin, emailDest);
                        break;
                    case -1:
                    default:
                        MessageBox.Show("Veuillez specifié le type d'export");
                        break;
                }
            }
            catch (Exception any)
            {
                MessageBox.Show("Un problème est survenue durant l'export");
                return;
            }

            MainWindow main = (MainWindow)this.Parent;
            main.Content = new HomePage();
        }

        private void btnRetour_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)this.Parent;
            main.Content = new HomePage();
        }
    }
}
