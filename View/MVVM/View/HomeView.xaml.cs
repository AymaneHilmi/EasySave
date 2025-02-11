using EasySave;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using View;

namespace View.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour HomeView.xaml
    /// </summary>
    public class Jobs
    {
        public string appellation { get; set; }
        public string sourcePath { get; set; }
        public string targetPath { get; set; }

    }

    public partial class HomeView : UserControl, INotifyPropertyChanged
    {
        private static Mutex Complete = new Mutex();
        private static Mutex Diff = new Mutex();

        public static List<Jobs> jobsProperties = new List<Jobs>();
        public static List<Jobs> JobsProperties { get => jobsProperties; set => jobsProperties = value; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public static List<ISave> Saves { get; set; } = new List<ISave>();
       
        List<string> businessList =
                           GeneralTools.conf.GetSection("Business_Software").Get<List<string>>();

        public static readonly DependencyProperty JobsNumberProperty = DependencyProperty.Register(
        "JobsNumber",
        typeof(int),
        typeof(HomeView),
        new PropertyMetadata(0)
        );

        public static readonly DependencyProperty CompleteSaveProperty = DependencyProperty.Register(
        "CompleteSaveNumber",
        typeof(int),
        typeof(HomeView),
        new PropertyMetadata(0)
        );

        public static readonly DependencyProperty DiffSaveProperty = DependencyProperty.Register(
        "DiffSaveNumber",
        typeof(int),
        typeof(HomeView),
        new PropertyMetadata(0)
        );

        private int _currentProgress;

        public int CurrentProgress
        {
            get { return _currentProgress; }
            set
            {
                _currentProgress = value;
                OnPropertyChanged(nameof(CurrentProgress)); // déclenche l'événement de notification de changement de propriété
            }
        }
        public int JobsNumber
        {
            get { return (int)GetValue(JobsNumberProperty); }
            set { SetValue(JobsNumberProperty, value); }
        }
        

        public int CompleteSaveNumber
        {
            get { return (int)GetValue(CompleteSaveProperty); }
            set { SetValue(CompleteSaveProperty, value); }
        }
        public int DiffSaveNumber
        {
            get { return (int)GetValue(DiffSaveProperty); }
            set { SetValue(DiffSaveProperty, value); }
        }
        private static bool isStopped = false;

        public HomeView()
        {
            InitializeComponent();
            DataContext = this;
            JobsGrid.ItemsSource = JobsProperties;
            JobsNumber = Saves.Count;
            CompleteSaveNumber = 0;
            DiffSaveNumber = 0;

        }


        public static long GlobalSize = 0;

        public void JobsButton_Click(object sender, RoutedEventArgs e)
        {
            Save._stopped = false;
            Thread[] threads = new Thread[Saves.Count];
            HomeView.isStopped = false;
            Task[] tasks = new Task[Saves.Count];

            // Nb de fichiers restants

            if (GeneralTools.VerifyBusinessSoftwareRunning(businessList))
                MessageBox.Show(
                    "Un logiciel Metier est en cours d'execution, Veuillez le fermer pour finaliser la Sauvegarde");

            Task prog = new Task(async () =>
            {
                while (Save.globalProgression != 100)
                {
                    Dispatcher.Invoke(() =>
                    {
                        ProgressBar.Value = Save.globalProgression;
                    });
                    await Task.Delay(100); // Pause de 100ms pour éviter de surcharger le processeur
                }
                Dispatcher.Invoke(() =>
                {
                    ProgressBar.Value = Save.globalProgression;
                });
            });
            prog.Start();

            for (int i = 0; i < JobsNumber; i++)
            {
                ISave save = Saves[i];
                if (save is CompleteSave)
                {
                    tasks[i] = Task.Run(() =>
                    {
                        (save as CompleteSave)?.RepositorySave();

                        Dispatcher.Invoke(() =>
                        {
                            CompleteSaveNumber = CompleteSaveNumber + 1;
                        });
                    });
                }
                else if (save is DiffSave)
                {
                    tasks[i] = Task.Run(() =>
                    {
                        (save as DiffSave)?.RepositorySave();

                        Dispatcher.Invoke(() =>
                        {
                            DiffSaveNumber = DiffSaveNumber + 1;
                        });
                    });
                }
            }
 
            // clear the List and set the jobs,Completesavenumber,diffsavenumber to 0

            Saves.Clear();
            CompleteSaveNumber = 0;
            DiffSaveNumber = 0;
            JobsNumber = 0;
        }

        public void StopButton_Click(object sender, RoutedEventArgs e)
        {
            Save._paused = true;
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            Save._paused = false;
        }

        private void BreakButton_Click(object sender, RoutedEventArgs e)
        {
            Save._stopped = true;
            return;
        }

        
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        
    }


}
