using System;
using System.Diagnostics.CodeAnalysis;
using System.IO.Ports;
using System.Linq;
using ESFT.GroundStation.Helpers;

namespace ESFT.GroundStation.GUI.Model {
    internal class PortModel : BaseModel, IDisposable {
        private string[] _availablePorts;

        private DateTime _dataLastUpdateTime = DateTime.UtcNow;

        private long _dataStreamInvalid;

        private long _dataStreamValid;

        private RelayCommand _portConnectCommand;

        private string _selectedPort;

        public PortModel() {
            AvailablePorts = SerialPortWatcherService.GetAvailableSerialPorts();
            SerialPortWatcherService.PortsChanged += (sender, eventArgs) => {
                var ports = SerialPortWatcherService.GetAvailableSerialPorts();
                if (COMPort.IsOpen && !ports.Contains(_selectedPort)) ConnectToPort(this);
                AvailablePorts = ports;
            };
        }

        protected virtual void ConnectToPort(object obj) {
            if (IsConnected || SerialPort.GetPortNames().All(x => x != SelectedPort)) {
                COMPort.Close();
            } else {
                DataIndex = 0;
                DataValid = false;
                COMPort.PortName = SelectedPort;
                COMPort.BaudRate = 115200;
                COMPort.DataBits = 8;
                COMPort.StopBits = StopBits.One;
                COMPort.Handshake = Handshake.None;
                COMPort.Parity = Parity.None;
                try {
                    COMPort.Open();
                } catch (Exception ex) {
                    Console.WriteLine($@"Error opening port: {ex.Message}");
                }
                DataLastUpdateTime = DateTime.UtcNow;
            }
            OnPropertyChanged(nameof(ComboIsEnabled));
            OnPropertyChanged(nameof(IsConnected));
            OnPropertyChanged(nameof(ConnectText));
            OnPropertyChanged(nameof(ConnectIsEnabled));
        }

        public RelayCommand PortConnectCommand => _portConnectCommand ??
                                                  (_portConnectCommand = new RelayCommand(ConnectToPort));

        protected byte[] RawData { get; set; }

        protected int DataIndex { get; set; }

        protected bool DataValid { get; set; }

        protected SerialPort COMPort { get; set; } = new SerialPort();

        public long DataStreamValid {
            get { return _dataStreamValid; }
            protected set { SetProperty(ref _dataStreamValid, value); }
        }

        public long DataStreamInvalid {
            get { return _dataStreamInvalid; }
            protected set { SetProperty(ref _dataStreamInvalid, value); }
        }

        public DateTime DataLastUpdateTime {
            get { return _dataLastUpdateTime; }
            protected set { SetProperty(ref _dataLastUpdateTime, value); }
        }

        public string[] AvailablePorts {
            get { return _availablePorts; }
            set {
                SetProperty(ref _availablePorts, value);
                if (string.IsNullOrWhiteSpace(SelectedPort) && _availablePorts.Length > 0)
                    SelectedPort = _availablePorts[0];
                OnPropertyChanged(nameof(ComboIsEnabled));
                OnPropertyChanged(nameof(ConnectIsEnabled));
            }
        }

        public string SelectedPort {
            get { return _selectedPort; }
            set {
                if (AvailablePorts.Contains(value)) SetProperty(ref _selectedPort, value);
                else SetProperty(ref _selectedPort, AvailablePorts.FirstOrDefault());
                OnPropertyChanged(nameof(ComboIsEnabled));
                OnPropertyChanged(nameof(ConnectIsEnabled));
            }
        }

        public bool IsConnected => COMPort.IsOpen;

        public bool ComboIsEnabled => !IsConnected && _availablePorts.Length > 0;

        public bool ConnectIsEnabled => IsConnected || ComboIsEnabled;

        public string ConnectText => ConnectIsEnabled ? (IsConnected ? "Disconnect" : "Connect") : "No Ports";

        #region IDisposable Support

        private bool _disposedValue; // To detect redundant calls

        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId =
            "<COMPort>k__BackingField")]
        protected virtual void Dispose(bool disposing) {
            if (_disposedValue) return;
            if (disposing) ((IDisposable) COMPort).Dispose();
            _disposedValue = true;
        }

        public void Dispose() {
            Dispose(true);
        }

        #endregion
    }
}
