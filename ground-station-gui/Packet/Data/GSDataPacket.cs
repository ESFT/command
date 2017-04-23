using System;

namespace ESFT.GroundStation.Packet.Data {
    internal class GSDataPacket : IPacket {
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

        private bool disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~GSDataPacket() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose() {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}
