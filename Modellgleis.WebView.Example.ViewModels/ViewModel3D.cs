using Modellgleis.WebView.Example.Infrastructure;
using System.ComponentModel;
using System.Windows.Input;

namespace Modellgleis.WebView.Example.ViewModels
{
    public class ViewModel3D : IView, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isViewReady;
        private string _viewSource;        
        private ICommand _gotoLocalSourceCommand;
        private ICommand _gotoWwwSourceCommand;

        public bool IsViewReady
        {
            get => _isViewReady;
            set
            {
                _isViewReady = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsViewReady)));
            }
        }        
        public string ViewSource
        {
            get => _viewSource;
            set
            {
                _viewSource = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ViewSource)));
            }
        }
        public ICommand GotoLocalSourceCommand => _gotoLocalSourceCommand ??= new RelayCommand(p => GotoLocalSource(), q => CanGotoLocalSource());
        public ICommand GotoWwwSourceCommand => _gotoWwwSourceCommand ??= new RelayCommand(p => GotoWwwSource(), q => CanGotoWwwSource());

        private bool CanGotoLocalSource()
        {
            // Wenn Source noch nicht gesetzt wurde, muss Command zum Setzen verfügbar sein, um Source zu setzen
            return IsViewReady || ViewSource == null;
        }

        private void GotoLocalSource()
        {
            ViewSource = "http://local.modellgleis.de";            
        }

        private bool CanGotoWwwSource()
        {
            // Wenn Source noch nicht gesetzt wurde, muss Command zum Setzen verfügbar sein, um Source zu setzen
            return IsViewReady || ViewSource == null;
        }

        private void GotoWwwSource()
        {
            //ViewSource = "http://www.s21-modellgleis.de";
            //ViewSource = "https://www.stern.de/digital/computer/scheibes-kolumne-mal-wieder-die-festplatte-auswuchten-3746438.html";
            ViewSource = "http://pages.cs.wisc.edu/~ballard/bofh/bofhserver.pl";
        }
    }
}
