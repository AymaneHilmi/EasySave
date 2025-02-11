using System;
using System.Windows;
using System.Windows.Input;
using View.MVVM.View;
using View;
using System.Net.Sockets;
using System.Net;
using System.Text;
using EasySaveApp;
using ServeurClient;



namespace View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Server _serveur;
        public Client _client;
        public MainWindow()
        {
            InitializeComponent();

            // Instancier le serveur avec le port 8000
            _serveur = new Server(12345);

            // Démarrer le serveur
            _serveur.Start();
        }

        //public object CompleteSaveObject { get; set; }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }

        }

        private void CloseApp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
