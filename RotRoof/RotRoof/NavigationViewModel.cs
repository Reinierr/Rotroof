﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;

namespace RotRoof
{
    class NavigationViewModel : INotifyPropertyChanged
    {
        //Interfaces for Navigation windows
        public ICommand MapCommand { get; set; }
        public ICommand ChartCommand { get; set; }
        public ICommand Chart2Command { get; set; }
        public ICommand PieChartCommand { get; set; }

        //HomePage
        private object selectedViewModel = new HomeViewModel();

        //Select object
        public object SelectedViewModel
        {
            get { return selectedViewModel; }
            set { selectedViewModel = value; OnPropertyChanged("SelectedViewModel"); }
        }

        //Add Command to your interfaces so it can be called
        public NavigationViewModel()
        {
            MapCommand = new BaseCommand(OpenMap);
            ChartCommand = new BaseCommand(OpenChart);
            Chart2Command = new BaseCommand(OpenChart2);
            PieChartCommand = new BaseCommand(OpenPieChart);
        }

        private void OpenMap(object obj)
        {
            SelectedViewModel = new MapViewModel();
        }
        private void OpenChart(object obj)
        {
            SelectedViewModel = new ChartViewModel();
        }
        private void OpenPieChart(object obj) 
        {
            SelectedViewModel = new PieChartViewModel();
        }
        private void OpenChart2(object obj)
        {
            SelectedViewModel = new ChartView2Model();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }

    public class BaseCommand : ICommand
    {
        private Predicate<object> _canExecute;
        private Action<object> _method;
        public event EventHandler CanExecuteChanged;

        public BaseCommand(Action<object> method)
            : this(method, null)
        {
        }

        public BaseCommand(Action<object> method, Predicate<object> canExecute)
        {
            _method = method;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }

            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _method.Invoke(parameter);
        }
    }
}
