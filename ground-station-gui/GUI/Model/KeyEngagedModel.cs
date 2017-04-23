using System.Windows.Media;

namespace ESFT.GroundStation.GUI.Model {
    internal class KeyEngagedModel : BaseModel {
        private Brush _background;

        private bool _isEngaged;

        private string _text = "";

        public KeyEngagedModel(int keyIndex) {
            Index = keyIndex;
            Text = $"Key {keyIndex}";
            IsEngaged = false;
        }

        public bool IsEngaged {
            get { return _isEngaged; }
            set {
                SetProperty(ref _isEngaged, value);
                Background = IsEngaged ? Brushes.GreenYellow : Brushes.Red;
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

        public int Index { get; }
    }
}
