using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View;

namespace View.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand SecondViewCommand { get; set; }
        public RelayCommand ThirdViewCommand { get; set; }
        public RelayCommand SettingsViewCommand { get; set; }
        public RelayCommand BlackListViewCommand { get; set; }
        public RelayCommand AboutUsViewCommand { get; set; }
        public HomeViewModel HomeVM { get; set; }
        public SecondViewModel SecondVM { get; set; }
        public ThirdViewModel ThirdVM { get; set; }
        public SettingsViewModel SettingsVM { get; set; }
        public BlackListSoftwareViewModel BlackListVM { get; set; }
        public AboutUsViewModel AboutUsVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set 
            { 
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            HomeVM = new HomeViewModel();
            SecondVM = new SecondViewModel();
            ThirdVM = new ThirdViewModel();
            SettingsVM = new SettingsViewModel();
            BlackListVM = new BlackListSoftwareViewModel();
            AboutUsVM = new AboutUsViewModel();
            CurrentView = HomeVM;

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVM;
            });
            
            SecondViewCommand = new RelayCommand(o =>
            {
                CurrentView = SecondVM;
            });

            ThirdViewCommand = new RelayCommand(o =>
            {
                CurrentView = ThirdVM;
            });

            SettingsViewCommand = new RelayCommand(o =>
            {
                CurrentView = SettingsVM;
            });

            BlackListViewCommand = new RelayCommand(o =>
            {
                CurrentView = BlackListVM;
            });

            AboutUsViewCommand = new RelayCommand(o =>
            {
                CurrentView = AboutUsVM;
            });
        }
    }
}
