using System;

namespace ESFT.GroundStation.Packet.Data {
    internal class GSCommandPacket : IPacket {
        public byte[] Bytes {
            get { throw new NotImplementedException(); }

            set { throw new NotImplementedException(); }
        }

        public string CSVData {
            get { throw new NotImplementedException(); }
        }

        public string CSVHeader {
            get { throw new NotImplementedException(); }
        }

        public int Size {
            get { throw new NotImplementedException(); }
        }

        public uint CRC {
            get { throw new NotImplementedException(); }
        }

        public event EventHandler<PacketUpdatedEventArgs> PacketUpdated;

        #region IDisposable Support

        private bool _disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (_disposedValue) return;
            if (disposing) {
                // TODO: dispose managed state (managed objects).
            }

            _disposedValue = true;
        }

        public void Dispose() {
            Dispose(true);
        }

        #endregion
    }
}
