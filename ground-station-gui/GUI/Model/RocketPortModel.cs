using System;
using System.Diagnostics.CodeAnalysis;
using System.IO.Ports;
using ESFT.GroundStation.Packet;
using ESFT.GroundStation.Packet.Data;

namespace ESFT.GroundStation.GUI.Model {
    internal class RocketPortModel : PortModel {
        public RocketPortModel() {
            RawData = new byte[RocketData.Size];
            COMPort.DataReceived += OnCOMPortDataReceived;
        }

        public RocketDataPacket RocketData { get; } = new RocketDataPacket();

        public PacketLogger RocketLogger { get; private set; }

        public RocketCommandPacket RocketCommand { get; } = new RocketCommandPacket();

        public event EventHandler<PacketUpdatedEventArgs> PacketReceived {
            add { RocketData.PacketUpdated += value; }
            remove { RocketData.PacketUpdated -= value; }
        }

        private void OnCOMPortDataReceived(object sender, SerialDataReceivedEventArgs e) {
            do {
                var rxChar = Convert.ToByte(COMPort.ReadByte());
                RawData[DataIndex++] = rxChar;
                if (DataIndex >= RawData.Length) {
                    DataIndex = 0;
                    DataValid = false;
                    try {
                        RocketData.Bytes = RawData;
                        DataLastUpdateTime = DateTime.UtcNow;
                        DataStreamValid++;
                    } catch (ArgumentException ex) {
                        Console.WriteLine($@"Rocket Data Error: {ex.Message}");
                        DataStreamInvalid++;
                    }
                } else if (rxChar == 0xFF) {
                    if (DataIndex >= 8) DataValid = true;
                } else if (!DataValid) {
                    DataIndex = 0;
                }
            } while (IsConnected && COMPort.BytesToRead > 0);
        }

        public void SendPacket(bool launchArmed) {
            RocketCommand.LaunchArmed = launchArmed;
            if (!IsConnected) return;
            var data = RocketCommand.Bytes;
            COMPort.Write(data, 0, data.Length);
        }

        protected override void ConnectToPort(object obj) {
            base.ConnectToPort(obj);
            RocketLogger = new PacketLogger(RocketData);
        }

        #region IDisposable Support

        private bool _disposedValue; // To detect redundant calls

        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "<RocketCommand>k__BackingField")]
        protected override void Dispose(bool disposing) {
            if (_disposedValue) return;
            if (disposing) {
                ((IDisposable) RocketData).Dispose();
                ((IDisposable) RocketCommand).Dispose();
            }
            base.Dispose(disposing);
            _disposedValue = true;
        }

        #endregion
    }
}
