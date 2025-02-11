using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EasySave;
using View;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

namespace View.MVVM.View
{

    /// <summary>
    /// Logique d'interaction pour BlackListSoftware.xaml
    /// </summary>
    public partial class BlackListSoftware : UserControl
    {
        public ObservableCollection<string> SoftwareList { get; set; } = new ObservableCollection<string>();
        public BusinessSoftware Software = new BusinessSoftware();





        public BlackListSoftware()
        {
            InitializeComponent();

        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SoftwareBox.Text))
            {
                ErrorTargetLabel.Visibility = Visibility.Collapsed;
                SuccesLabel.Visibility = Visibility.Collapsed;

                ErrorTargetLabel.Content = "Veuillez entrer un logiciel Metier";
                ErrorTargetLabel.Visibility = Visibility.Visible;


            }
            else if (Software.Add(SoftwareBox.Text))
            {
                ErrorTargetLabel.Visibility = Visibility.Collapsed;
                SuccesLabel.Visibility = Visibility.Collapsed;

                SoftwareBox.Text = "";

                SuccesLabel.Content = "Le Logiciel Metier a bien été Ajouté";
                SuccesLabel.Visibility = Visibility.Visible;


            }
            else
            {
                ErrorTargetLabel.Visibility = Visibility.Collapsed;
                SuccesLabel.Visibility = Visibility.Collapsed;

                ErrorTargetLabel.Content = "Le Logiciel metier que vous avez rentré n'existe pas ou existe déja";
                ErrorTargetLabel.Visibility = Visibility.Visible;

            }

            BusinessGrid.Items.Clear();

            foreach (var software in Software.Name)
            {
                var softwareList = new { SoftwareList = software };
                BusinessGrid.Items.Add(softwareList);
            }
        }


        private void SoftwareBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ErrorTargetLabel.Visibility = Visibility.Collapsed;
            SuccesLabel.Visibility = Visibility.Collapsed;
        }

        public class ExtensionsPrio
        {
            public string extensionPrio { get; set; }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SoftwareBox.Text))
            {
                ErrorTargetLabel.Visibility = Visibility.Collapsed;
                SuccesLabel.Visibility = Visibility.Collapsed;

                ErrorTargetLabel.Content = "Veuillez entrer un logiciel Metier";
                ErrorTargetLabel.Visibility = Visibility.Visible;
            }
            else if (Software.Delete(SoftwareBox.Text))
            {
                ErrorTargetLabel.Visibility = Visibility.Collapsed;
                SuccesLabel.Visibility = Visibility.Collapsed;

                SoftwareBox.Text = "";

                SuccesLabel.Content = "Le Logiciel Metier a bien été Ajouté";
                SuccesLabel.Visibility = Visibility.Visible;

                // Vider la liste d'items du DataGrid
                BusinessGrid.Items.Clear();

                // Ajouter chaque logiciel métier à la liste d'items du DataGrid
                foreach (var software in Software.Name)
                {
                    var softwareList = new { SoftwareList = software };
                    BusinessGrid.Items.Add(softwareList);
                }
            }
            else
            {
                ErrorTargetLabel.Visibility = Visibility.Collapsed;
                SuccesLabel.Visibility = Visibility.Collapsed;

                ErrorTargetLabel.Content = "Le Logiciel metier que vous avez rentré n'existe pas ou existe déja";
                ErrorTargetLabel.Visibility = Visibility.Visible;
            }
        }
    }
}
