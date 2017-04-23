using System.Windows.Media;

namespace ESFT.GroundStation.GUI.Model {
    internal class ConnectivityModel : BaseModel {
        private Brush _background;

        private bool _isConnected;

        private string _text = "Not Connected";

        public ConnectivityModel() {
            IsConnected = false;
        }

        public bool IsConnected {
            get { return _isConnected; }
            set {
                SetProperty(ref _isConnected, value);
                Background = IsConnected ? Brushes.GreenYellow : Brushes.Red;
                Text = IsConnected ? "Connected" : "Not Connected";
            }
        }

        public Brush Background {
            get { return _background; }
            private set { SetProperty(ref _background, value); }
        }

        public string Text {
            get { return _text; }
            private set { SetProperty(ref _text, value); }
        }
    }
}
