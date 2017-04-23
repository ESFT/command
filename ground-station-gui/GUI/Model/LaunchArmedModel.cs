using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;
using ESFT.GroundStation.Helpers;

namespace ESFT.GroundStation.GUI.Model {
    internal class LaunchArmedModel : BaseModel {
        private RelayCommand _forceArmCommand;

        private Brush _gsBackground;

        private string _gsText;

        private bool _isGSArmed;

        private bool _isGSForceArmed;

        private bool _isRocketArmed;

        private Brush _rrBackground;

        private string _rrText;

        public LaunchArmedModel(int keyCount) {
            for (var keyIndex = 0; keyIndex < keyCount; keyIndex++) {
                var key = new KeyEngagedModel(keyIndex);
                key.PropertyChanged += OnPropertyChanged;
                KeyList.Add(key);
            }
            IsGSForceArmed = false;
            IsRocketArmed = false;

        }

        public List<KeyEngagedModel> KeyList { get; set; } = new List<KeyEngagedModel>();

        public bool IsRocketArmed {
            get { return _isRocketArmed; }
            set {
                SetProperty(ref _isRocketArmed, value);
                RRText = IsRocketArmed ? "Rocket Armed" : "Rocket Disarmed";
                RRBackground = IsRocketArmed ? Brushes.GreenYellow : Brushes.Red;
            }
        }

        public bool IsGSArmed {
            get { return _isGSArmed; }
            private set {
                SetProperty(ref _isGSArmed, value);
                GSBackground = IsGSArmed ? Brushes.GreenYellow : Brushes.Red;
                GSText = IsGSForceArmed ? "Force Armed" : (IsGSArmed ? "Armed" : "Disarmed");
            }
        }

        public bool IsGSForceArmed {
            get { return _isGSForceArmed; }
            set {
                SetProperty(ref _isGSForceArmed, value);
                IsGSArmed = IsGSForceArmed || !KeyList.Exists(key => !key.IsEngaged);
            }
        }

        public Brush GSBackground {
            get { return _gsBackground; }
            private set { SetProperty(ref _gsBackground, value); }
        }

        public string GSText {
            get { return _gsText; }
            private set { SetProperty(ref _gsText, value); }
        }

        public Brush RRBackground {
            get { return _rrBackground; }
            private set { SetProperty(ref _rrBackground, value); }
        }

        public string RRText {
            get { return _rrText; }
            private set { SetProperty(ref _rrText, value); }
        }

        public RelayCommand ForceArmCommand => _forceArmCommand ??
                                               (_forceArmCommand = new RelayCommand(execute => {
                                                   IsGSForceArmed = !IsGSForceArmed;
                                               }));

        protected void OnPropertyChanged(object sender, PropertyChangedEventArgs e) {
            IsGSArmed = IsGSForceArmed || !KeyList.Exists(key => !key.IsEngaged);
        }
    }
}
