
using EasySave;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace View.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour ThirdView.xaml
    /// </summary>
    public partial class ThirdView : UserControl
    {
        public ThirdView()
        {
            InitializeComponent();
        }

        private void DifferentialButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(BackUpNameBox.Text))
            {
                ErrorNameLabel.Content = "Le champ BackUp Name est requis.";
                ErrorNameLabel.Visibility = Visibility.Visible;
                return;
            }

            if (string.IsNullOrEmpty(SoucePathBox.Text))
            {
                ErrorSourceLabel.Content = "Le champ Source Path est requis.";
                ErrorSourceLabel.Visibility = Visibility.Visible;
                return;
            }
            else if (!Directory.Exists(SoucePathBox.Text))
            {
                SoucePathBox.Text = "";
                ErrorSourceLabel.Content = "Le répertoire Source Path n'existe pas.";
                ErrorSourceLabel.Visibility = Visibility.Visible;
                return;
            }

            if (string.IsNullOrEmpty(TargetPathBox.Text))
            {
                ErrorTargetLabel.Content = "Le champ Target Path est requis.";
                ErrorTargetLabel.Visibility = Visibility.Visible;
                return;
            }
            else if (!Directory.Exists(TargetPathBox.Text))
            {
                TargetPathBox.Text = "";
                ErrorTargetLabel.Content = "Le répertoire Target Path n'existe pas.";
                ErrorTargetLabel.Visibility = Visibility.Visible;
                return;
            }
            else if (SoucePathBox.Text == TargetPathBox.Text)
            {
                TargetPathBox.Text = "";
                ErrorTargetLabel.Content = "Le répertoire Source ne peux pas etre egal au repertoire de Destination";
                ErrorTargetLabel.Visibility = Visibility.Visible;

                return;
            }
            else if (SoucePathBox.Text.StartsWith(TargetPathBox.Text) || TargetPathBox.Text.StartsWith(SoucePathBox.Text))
            {
                TargetPathBox.Text = "";
                ErrorTargetLabel.Content = "Le répertoire de Destination ne peux pas etre inclus dans le repertoire Source ";
                ErrorTargetLabel.Visibility = Visibility.Visible;
                return;
            }

            Jobs job = new Jobs
            {
                appellation = BackUpNameBox.Text,
                sourcePath = SoucePathBox.Text,
                targetPath = TargetPathBox.Text
            };

            BackUpNameBox.Text = "";
            SoucePathBox.Text = "";
            TargetPathBox.Text = "";

            SuccesLabel.Content = "Les données ont été sauvegardées avec succès.";
            SuccesLabel.Visibility = Visibility.Visible;

            HomeView.jobsProperties.Add(job);

            HomeView.Saves.Add(new DiffSave(job.appellation, job.sourcePath, job.targetPath));
            
            HomeView.GlobalSize +=  GeneralTools.DirectorySize(job.sourcePath, job.targetPath);
        }

        private void BackUpNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ErrorNameLabel.Visibility = Visibility.Collapsed;
            SuccesLabel.Visibility = Visibility.Collapsed;

        }

        private void SoucePathBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ErrorSourceLabel.Visibility = Visibility.Collapsed;
            SuccesLabel.Visibility = Visibility.Collapsed;


        }

        private void TargetPathBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ErrorTargetLabel.Visibility = Visibility.Collapsed;
            SuccesLabel.Visibility = Visibility.Collapsed;


        }

        private void source_Folder_Button_Click(object sender, RoutedEventArgs e)
        {
            var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();

            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SoucePathBox.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void target_Folder_Button_Click(object sender, RoutedEventArgs e)
        {
            var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();

            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TargetPathBox.Text = folderBrowserDialog.SelectedPath;
            }
        }
    }
}
