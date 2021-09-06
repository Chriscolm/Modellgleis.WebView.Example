using Modellgleis.WebView.Example.Infrastructure;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Modellgleis.WebView.Example.ViewModels
{
    public class ViewModelHolder : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private IView _currentView;
        private ICommand _switchViewCommand;

        public IView CurrentView { 
            get => _currentView; 
            set { 
                _currentView = value; 
                Notify(nameof(CurrentView)); 
            } 
        }
        public ICommand SwitchViewCommand => _switchViewCommand ??= new RelayCommand(p => SwitchView(), q => CanSwitchView());

        public ViewModelHolder()
        {
            _currentView = new ViewModel2D();
        }

        private bool CanSwitchView()
        {
            return true;
        }

        private void SwitchView()
        {
            CurrentView = _currentView switch
            {
                ViewModel2D _ => new ViewModel3D(),
                ViewModel3D _ => new ViewModel2D(),
                _ => throw new NotImplementedException("Benutzer fand nicht definierte Ansicht"),
            };
        }

        protected void Notify(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
