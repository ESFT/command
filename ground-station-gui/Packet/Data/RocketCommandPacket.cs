using System;
using System.Linq;
using System.Runtime.InteropServices;
using ESFT.GroundStation.Helpers;

namespace ESFT.GroundStation.Packet.Data {
    internal class RocketCommandPacket : IPacket {
        private readonly Crc32 _crc32 = new Crc32();

        private string _csvHeader;

        private Packet _data = new Packet {
                                              magic = 0xFFFFFFFFFFFFFFFF,
                                              launchArmed = 0,
                                              crc = 0
                                          };

        public RocketCommandPacket() {
            _data.crc = BitConverter.ToUInt32(_crc32.ComputeHash(Bytes), 0);
        }

        public bool LaunchArmed {
            get { return Convert.ToBoolean(_data.launchArmed); }
            set {
                _data.launchArmed = Convert.ToByte(value);
                _data.crc = 0;
                _data.crc = BitConverter.ToUInt32(_crc32.ComputeHash(Bytes), 0);
            }
        }

        public event EventHandler<PacketUpdatedEventArgs> PacketUpdated;

        public int Size => Marshal.SizeOf(_data);

        public byte[] Bytes {
            get {
                _data.crc = CRC;
                return _data.GetBytes();
            }
            set {
                if (value.Length != Size)
                    throw new
                        ArgumentException($@"Array is invalid size. Expected {Size} but got {value.Length}.",
                                          nameof(Bytes));
                _data = value.FromBytes<Packet>();
                value[value.Length - 1] = 0;
                value[value.Length - 2] = 0;
                value[value.Length - 3] = 0;
                value[value.Length - 4] = 0;
                var crcBuffer = _crc32.ComputeHash(value);
                Array.Reverse(crcBuffer);
                var crcCalc = BitConverter.ToUInt32(crcBuffer, 0);
                if (_data.crc != crcCalc)
                    throw new
                        ArgumentException($@"CRC Mismatch. Expected {_data.crc:X4} but got {crcCalc:X4}.",
                                          nameof(Bytes));
                PacketUpdated?.Invoke(this, new PacketUpdatedEventArgs { UpdateTime = DateTime.UtcNow, Packet = this });
            }
        }

        public string CSVData {
            get {
                return _data.ToCSV<Packet>(fields: typeof(Packet).GetFields()
                                                                 .Where(f => !f.Name.Contains("magic") &&
                                                                             !f.Name.Contains("pad"))
                                                                 .ToArray());
            }
        }

        public string CSVHeader {
            get {
                return _csvHeader ?? (_csvHeader = CSVHelpers.ToCSVHeader<Packet>(fields: typeof(Packet)
                                                                                      .GetFields()
                                                                                      .Where(f => !f.Name
                                                                                                    .Contains("magic") &&
                                                                                                  !f
                                                                                                      .Name
                                                                                                      .Contains("pad"))
                                                                                      .ToArray()));
            }
        }

        public uint CRC {
            get {
                _data.crc = 0;
                var crcBuffer = _crc32.ComputeHash(_data.GetBytes());
                Array.Reverse(crcBuffer);
                _data.crc = BitConverter.ToUInt32(crcBuffer, 0);
                return _data.crc;
            }
        }

        //
        // Output data struct for autonomous control.
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct Packet {
            public ulong magic;
            public uint launchArmed;
            public uint crc;
        }

        #region IDisposable Support

        private bool _disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (_disposedValue) return;
            if (disposing) _crc32.Dispose();
            _disposedValue = true;
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose() {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        #endregion
    }
}
