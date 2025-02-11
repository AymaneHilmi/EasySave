using EasySaveApp.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace View.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public static bool option = false;

        public Brush Red { get; private set; }

        public SettingsView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EasySaveApp.Properties.Settings.Default.languageCode = "fr-FR";
            EasySaveApp.Properties.Settings.Default.Save();
            MessageBox.Show("Reload application for changes to take effect");

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            EasySaveApp.Properties.Settings.Default.languageCode = "en-US";
            EasySaveApp.Properties.Settings.Default.Save();
            MessageBox.Show("Redémarrer l'application pour que les changements prennent effet");
        }

        private bool IsNumber(string text)
        {
            return int.TryParse(text, out int result);
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsNumber(e.Text);
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SizeSaveTextBox.Text))
            {
                SuccesLabel6.Visibility = Visibility.Collapsed;
                SuccesLabel8.Visibility = Visibility.Collapsed;
                SuccesLabel3.Visibility = Visibility.Collapsed;
                SuccesLabel6bis.Visibility = Visibility.Collapsed;
                SuccesLabel8bis.Visibility = Visibility.Collapsed;
                SuccesLabel9.Visibility = Visibility.Collapsed;
                SuccesLabel9bis.Visibility = Visibility.Collapsed;

                SuccesLabel6bis.Content = "Veuillez entrer une Valeur";
                SuccesLabel6bis.Visibility = Visibility.Visible;


                return;
            }
            else
            {
                MaxParallelSize maxsizeparallel = new MaxParallelSize();
                maxsizeparallel.Modify(long.Parse(SizeSaveTextBox.Text));

                SuccesLabel6.Visibility = Visibility.Collapsed;
                SuccesLabel8.Visibility = Visibility.Collapsed;
                SuccesLabel3.Visibility = Visibility.Collapsed;
                SuccesLabel6bis.Visibility = Visibility.Collapsed;
                SuccesLabel8bis.Visibility = Visibility.Collapsed;
                SuccesLabel9.Visibility = Visibility.Collapsed;
                SuccesLabel9bis.Visibility = Visibility.Collapsed;

                SuccesLabel6.Content = "La taille maximale de sauvegarde a bien été modifiée";
                SuccesLabel6.Visibility = Visibility.Visible;

                SizeSaveTextBox.Text = "";
            }

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(NetworkLoadBox.Text))
            {
                SuccesLabel6.Visibility = Visibility.Collapsed;
                SuccesLabel8.Visibility = Visibility.Collapsed;
                SuccesLabel3.Visibility = Visibility.Collapsed;
                SuccesLabel6bis.Visibility = Visibility.Collapsed;
                SuccesLabel8bis.Visibility = Visibility.Collapsed;
                SuccesLabel9.Visibility = Visibility.Collapsed;
                SuccesLabel9bis.Visibility = Visibility.Collapsed;


                SuccesLabel8bis.Content = "Veuillez entrer une Valeur";
                SuccesLabel8bis.Visibility = Visibility.Visible;


                return;
            }
            else
            {
                NetworkLoad networkload = new NetworkLoad();
                networkload.Modify(long.Parse(NetworkLoadBox.Text));

                SuccesLabel6.Visibility = Visibility.Collapsed;
                SuccesLabel8.Visibility = Visibility.Collapsed;
                SuccesLabel3.Visibility = Visibility.Collapsed;
                SuccesLabel6bis.Visibility = Visibility.Collapsed;
                SuccesLabel8bis.Visibility = Visibility.Collapsed;
                SuccesLabel9bis.Visibility = Visibility.Collapsed;
                SuccesLabel9.Visibility = Visibility.Collapsed;

                SuccesLabel8.Content = "Les Modification de surcharge Reseau on bien été pris en compte";
                SuccesLabel8.Visibility = Visibility.Visible;

                NetworkLoadBox.Text = "";
            }



        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            LogJsonOrXml xml = new LogJsonOrXml();
            xml.ToXml();

            SuccesLabel6.Visibility = Visibility.Collapsed;
            SuccesLabel8.Visibility = Visibility.Collapsed;
            SuccesLabel3.Visibility = Visibility.Collapsed;
            SuccesLabel6bis.Visibility = Visibility.Collapsed;
            SuccesLabel8bis.Visibility = Visibility.Collapsed;
            SuccesLabel9bis.Visibility = Visibility.Collapsed;
            SuccesLabel9.Visibility = Visibility.Collapsed;

            SuccesLabel3.Content = "Les log files ont bien ete changer en type XML";
            SuccesLabel3.Visibility = Visibility.Visible;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            LogJsonOrXml json = new LogJsonOrXml();
            json.ToJson();

            SuccesLabel6.Visibility = Visibility.Collapsed;
            SuccesLabel8.Visibility = Visibility.Collapsed;
            SuccesLabel3.Visibility = Visibility.Collapsed;
            SuccesLabel6bis.Visibility = Visibility.Collapsed;
            SuccesLabel8bis.Visibility = Visibility.Collapsed;
            SuccesLabel9.Visibility = Visibility.Collapsed;
            SuccesLabel9bis.Visibility = Visibility.Collapsed;

            SuccesLabel3.Content = "Les log files ont bien ete changer en type JSON";
            SuccesLabel3.Visibility = Visibility.Visible;


        }

        public ObservableCollection<string> ExtensionList { get; set; } = new ObservableCollection<string>();
        public PriorityExt Extension = new PriorityExt();
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            string extensionprio = PrioritaryFilesExtension.Text;
            PriorityExt prioritytext = new PriorityExt();

            if (string.IsNullOrEmpty(extensionprio))
            {
                SuccesLabel6.Visibility = Visibility.Collapsed;
                SuccesLabel8.Visibility = Visibility.Collapsed;
                SuccesLabel3.Visibility = Visibility.Collapsed;
                SuccesLabel6bis.Visibility = Visibility.Collapsed;
                SuccesLabel8bis.Visibility = Visibility.Collapsed;
                SuccesLabel9.Visibility = Visibility.Collapsed;
                SuccesLabel9bis.Visibility = Visibility.Collapsed;


                SuccesLabel9.Content = "Veuillez entrer une Valeur";
                SuccesLabel9.Visibility = Visibility.Visible;


                return;
            }
            else if (!extensionprio.StartsWith("."))
            {
                extensionprio = "." + extensionprio;
            }



            if (!prioritytext.Add(extensionprio))
            {
                SuccesLabel6.Visibility = Visibility.Collapsed;
                SuccesLabel8.Visibility = Visibility.Collapsed;
                SuccesLabel3.Visibility = Visibility.Collapsed;
                SuccesLabel6bis.Visibility = Visibility.Collapsed;
                SuccesLabel8bis.Visibility = Visibility.Collapsed;
                SuccesLabel9.Visibility = Visibility.Collapsed;
                SuccesLabel9bis.Visibility = Visibility.Collapsed;

                PrioritaryFilesExtension.Text = "";

                SuccesLabel9.Content = "Fichier Prioritaire déja Existant";
                SuccesLabel9.Visibility = Visibility.Visible;
                return;
            }

            prioritytext.Add(extensionprio);

            SuccesLabel6.Visibility = Visibility.Collapsed;
            SuccesLabel8.Visibility = Visibility.Collapsed;
            SuccesLabel3.Visibility = Visibility.Collapsed;
            SuccesLabel6bis.Visibility = Visibility.Collapsed;
            SuccesLabel8bis.Visibility = Visibility.Collapsed;
            SuccesLabel9.Visibility = Visibility.Collapsed;
            SuccesLabel9bis.Visibility = Visibility.Collapsed;

            PrioritaryFilesExtension.Text = "";

            SuccesLabel9bis.Content = "Le fichier prioritaire a bien été ajouté";
            SuccesLabel9bis.Visibility = Visibility.Visible;




            ExtensionGrid.Items.Clear();

            foreach (var extension in Extension.Name)
            {
                var extensionList = new { ExtensionList = extension };
                ExtensionGrid.Items.Add(extensionList);
            }

        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            string extensionprio = PrioritaryFilesExtension.Text;
            PriorityExt prioritytext = new PriorityExt();

            if (string.IsNullOrEmpty(extensionprio))
            {
                SuccesLabel6.Visibility = Visibility.Collapsed;
                SuccesLabel8.Visibility = Visibility.Collapsed;
                SuccesLabel3.Visibility = Visibility.Collapsed;
                SuccesLabel6bis.Visibility = Visibility.Collapsed;
                SuccesLabel8bis.Visibility = Visibility.Collapsed;
                SuccesLabel9.Visibility = Visibility.Collapsed;
                SuccesLabel9bis.Visibility = Visibility.Collapsed;


                SuccesLabel9.Content = "Veuillez entrer une Valeur";
                SuccesLabel9.Visibility = Visibility.Visible;


                return;
            }
            else if (!extensionprio.StartsWith("."))
            {
                extensionprio = "." + extensionprio;
            }



            if (!prioritytext.Delete(extensionprio))
            {
                SuccesLabel6.Visibility = Visibility.Collapsed;
                SuccesLabel8.Visibility = Visibility.Collapsed;
                SuccesLabel3.Visibility = Visibility.Collapsed;
                SuccesLabel6bis.Visibility = Visibility.Collapsed;
                SuccesLabel8bis.Visibility = Visibility.Collapsed;
                SuccesLabel9.Visibility = Visibility.Collapsed;
                SuccesLabel9bis.Visibility = Visibility.Collapsed;

                PrioritaryFilesExtension.Text = "";

                SuccesLabel9.Content = "Fichier Prioritaire Inexistant";
                SuccesLabel9.Visibility = Visibility.Visible;
                return;
            }

            prioritytext.Delete(PrioritaryFilesExtension.Text);

            SuccesLabel6.Visibility = Visibility.Collapsed;
            SuccesLabel8.Visibility = Visibility.Collapsed;
            SuccesLabel3.Visibility = Visibility.Collapsed;
            SuccesLabel6bis.Visibility = Visibility.Collapsed;
            SuccesLabel8bis.Visibility = Visibility.Collapsed;
            SuccesLabel9.Visibility = Visibility.Collapsed;
            SuccesLabel9bis.Visibility = Visibility.Collapsed;

            PrioritaryFilesExtension.Text = "";

            SuccesLabel9bis.Content = "Le fichier prioritaire a bien été supprimé";
            SuccesLabel9bis.Visibility = Visibility.Visible;

            ExtensionGrid.Items.Clear();

            foreach (var Extension in Extension.Name)
            {
                var ExtensionList = new { ExtensionList = Extension };
                ExtensionGrid.Items.Add(ExtensionList);
            }

        }
    }
}
