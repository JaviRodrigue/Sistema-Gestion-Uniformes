using CommunityToolkit.Mvvm.ComponentModel;

namespace VentasApp.Desktop.ViewModels.Base
{
    /// <summary>
    /// Clase base para todos los ViewModels.
    /// Proporciona soporte para PropertyChanged y notificaciones.
    /// </summary>
    public class ViewModelBase : ObservableObject
    {
        // Aquí puedes agregar propiedades comunes a todos los ViewModels
        // Por ejemplo, para mostrar mensajes o loading indicators

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private string _statusMessage = string.Empty;
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        // Constructor opcional
        public ViewModelBase() { }

        // Métodos comunes
        protected void SetBusy(bool busy, string message = "")
        {
            IsBusy = busy;
            StatusMessage = message;
        }
    }
}
