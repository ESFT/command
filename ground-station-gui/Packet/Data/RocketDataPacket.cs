using System;
using System.Linq;
using System.Runtime.InteropServices;
using ESFT.GroundStation.Helpers;

namespace ESFT.GroundStation.Packet.Data {
    internal class RocketDataPacket : IPacket {
        private readonly Crc32 _crc32 = new Crc32();

        private string _csvHeader;

        private Packet _data;

        public ulong Magic => _data.magic;

        public bool LaunchedArmed => Convert.ToBoolean(_data.armedStatus & 0x01);

        public bool AltimeterArmed => Convert.ToBoolean(_data.armedStatus & 0x02);

        public bool IMUConnected => Convert.ToBoolean(_data.armedStatus & 0x04);

        public bool AltConnected => Convert.ToBoolean(_data.armedStatus & 0x08);

        public bool SDCardConnected => Convert.ToBoolean(_data.armedStatus & 0x10);

        public bool GPSConnected => Convert.ToBoolean(_data.armedStatus & 0x20);

        public uint ElapsedTime => _data.elapsedTime;

        public uint ArmHeartbeatTime => _data.armHeartbeatTime;

        public uint LaunchTime => _data.launchTime;

        public uint BackDrogueTime => _data.backDrogueTime;

        public uint BackMainTime => _data.backMainTime;

        public uint SelfDroguePrimTime => _data.selfDroguePrimTime;

        public uint SelfDrogueBackTime => _data.selfDrogueBackTime;

        public uint SelfMainPrimTime => _data.selfMainPrimTime;

        public uint SelfMainBackTime => _data.selfMainBackTime;

        public bool SelfPrimaryMainFired => Convert.ToBoolean(_data.parachuteDeployment & 0x01);

        public bool SelfBackupMainFired => Convert.ToBoolean(_data.parachuteDeployment & 0x02);

        public bool SelfPrimaryDrogueFired => Convert.ToBoolean(_data.parachuteDeployment & 0x04);

        public bool SelfBackupDrogueFired => Convert.ToBoolean(_data.parachuteDeployment & 0x08);

        public bool BackupMainFired => Convert.ToBoolean(_data.parachuteDeployment & 0x10);

        public bool BackupDrogueFired => Convert.ToBoolean(_data.parachuteDeployment & 0x20);

        public float AltAltitude => _data.altitude_Alt;

        public float AltAltitudeInit => _data.altitudeInit_Alt;

        public float AltAltitudeMax => _data.altitudeMax_Alt;

        public DateTime UTC {
            get {
                try {
                    return new DateTime(_data.year, _data.month, _data.day, _data.hour, _data.minute, _data.second,
                                        _data.hundredths, DateTimeKind.Utc);
                } catch (ArgumentOutOfRangeException) {
                    return DateTime.UtcNow;
                }
            }
        }

        public ulong FixAge => _data.fixAge;

        public bool GPSNavLocked => Convert.ToBoolean(_data.navLocked);

        public float Latitude => _data.latitude;

        public float Longitude => _data.longitude;

        public float GPSAltitude => _data.altitude_GPS;

        public float Course => _data.course;

        public float Speed => _data.speed;

        public float GPSAltitudeInit => _data.altitudeInit_GPS;

        public float GPSAltitudeMax => _data.altitudeMax_GPS;

        public float Temperature => _data.temp;

        public float Pressure => _data.pres;

        public float[] Acceleration => new[] {_data.accelX, _data.accelY, _data.accelZ};

        public float[] GyroScope => new[] {_data.gyroX, _data.gyroY, _data.gyroZ};

        public float[] Magnetometer => new[] {_data.magX * 1000000, _data.magY * 1000000, _data.magZ * 1000000};

        public float[] Quaternion => new[] {_data.quatA, _data.quatI, _data.quatJ, _data.quatK};

        public float Roll => _data.roll;

        public float Pitch => _data.pitch;

        public float Yaw => _data.yaw;

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
                PacketUpdated?.Invoke(this, new PacketUpdatedEventArgs {UpdateTime = DateTime.UtcNow, Packet = this});
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

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct Packet {
            public ulong magic;
            
            public int year;
            public byte month;
            public byte day;
            public byte hour;
            public byte minute;
            public byte second;
            public byte hundredths;
            public byte navLocked;

            public byte armedStatus;

            public byte parachuteDeployment;

            private readonly byte pad1;
            private readonly byte pad2;
            private readonly byte pad3;

            public uint elapsedTime;

            public uint armHeartbeatTime;
            public uint launchTime;
            public uint backDrogueTime;
            public uint backMainTime;
            public uint selfDroguePrimTime;
            public uint selfDrogueBackTime;
            public uint selfMainPrimTime;
            public uint selfMainBackTime;
            
            public float temp;
            public float pres;
            public float altitudeInit_Alt;
            public float altitudeMax_Alt;
            public float altitude_Alt;

            public ulong fixAge;
            public float latitude;
            public float longitude;
            public float course;
            public float speed;
            public float altitudeInit_GPS;
            public float altitudeMax_GPS;
            public float altitude_GPS;

            public float accelX;
            public float accelY;
            public float accelZ;
            public float gyroX;
            public float gyroY;
            public float gyroZ;
            public float magX;
            public float magY;
            public float magZ;

            public float quatA;
            public float quatI;
            public float quatJ;
            public float quatK;

            public float roll;
            public float pitch;
            public float yaw;

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
