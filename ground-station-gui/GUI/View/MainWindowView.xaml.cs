using ESFT.GroundStation.GUI.ViewModel;

namespace ESFT.GroundStation.GUI.View {
    /// <summary>
    ///     Interaction logic for MainWindowView.xaml
    /// </summary>
    public partial class MainWindowView {
        public MainWindowView() {
            InitializeComponent();
            var model = new MainViewModel();
            DataContext = model;
            Closing += model.OnClosing;
        }
    }
}
