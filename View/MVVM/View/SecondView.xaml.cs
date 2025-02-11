using EasySave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using View.MVVM.View;
using static View.MVVM.View.HomeView;
using static View.MVVM.View.SecondView;

namespace View.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour SecondView.xaml
    /// </summary>
    public partial class SecondView : UserControl
    {
        

        public SecondView()
        {
            InitializeComponent();
        }

       

        private void CompleteButton_Click(object sender, RoutedEventArgs e)
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
                ErrorTargetLabel.Content = "Le répertoire Target Path n'existe pas.";
                ErrorTargetLabel.Visibility = Visibility.Visible;
                return;
            }
            else if (SoucePathBox.Text == TargetPathBox.Text)
            {
                TargetPathBox.Text = "";
                ErrorTargetLabel.Content = "Le répertoire de destination ne peux pas etre égal au repertoire Source";
                ErrorTargetLabel.Visibility = Visibility.Visible;
                
                return;
            }
            else if(SoucePathBox.Text.StartsWith(TargetPathBox.Text) || TargetPathBox.Text.StartsWith(SoucePathBox.Text))
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

            HomeView.Saves.Add(new CompleteSave(job.appellation, job.sourcePath, job.targetPath));
            
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

