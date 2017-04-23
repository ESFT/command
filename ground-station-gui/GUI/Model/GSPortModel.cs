using System;
using System.Diagnostics.CodeAnalysis;
using System.IO.Ports;
using ESFT.GroundStation.Packet.Data;

namespace ESFT.GroundStation.GUI.Model {
    internal class GSPortModel : PortModel {
        public GSPortModel() {
            COMPort.DataReceived += OnCOMPortDataReceived;
        }

        public GSDataPacket GSData { get; } = new GSDataPacket();

        public GSCommandPacket GSCommand { get; } = new GSCommandPacket();
        
        public event EventHandler<PacketUpdatedEventArgs> PacketReceived {
            add { GSData.PacketUpdated += value; }
            remove { GSData.PacketUpdated -= value; }
        }

        private void OnCOMPortDataReceived(object sender, SerialDataReceivedEventArgs e) {
            do {
                var rxChar = Convert.ToByte(COMPort.ReadByte());
                RawData[DataIndex++] = rxChar;
                if (DataIndex >= RawData.Length) DataIndex = 0;
                if (DataValid) {
                    DataValid = false;
                    try {
                        //GSData.Bytes = _portDataRaw;
                        DataLastUpdateTime = DateTime.UtcNow;
                        DataStreamValid++;
                    } catch (ArgumentException) {
                        DataStreamInvalid++;
                    }
                } else if (rxChar == 0xFF) {
                    if (DataIndex >= 8) DataValid = true;
                } else {
                    DataIndex = 0;
                }
            } while (COMPort.BytesToRead > 0);
        }

        public void SendPacket() {
            if (!IsConnected) return;
            var data = GSCommand.Bytes;
            COMPort.Write(data, 0, data.Length);
        }

        #region IDisposable Support

        private bool _disposedValue; // To detect redundant calls

        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId =
            "<GSCommand>k__BackingField")]
        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId =
            "<GSData>k__BackingField")]
        protected override void Dispose(bool disposing) {
            if (_disposedValue) return;
            if (disposing) {
                ((IDisposable) GSData).Dispose();
                ((IDisposable) GSCommand).Dispose();
            }
            base.Dispose(disposing);
            _disposedValue = true;
        }

        #endregion
    }
}
