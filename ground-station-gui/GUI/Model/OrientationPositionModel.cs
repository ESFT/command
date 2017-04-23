using System;
using System.Collections.Generic;
using System.Linq;

namespace ESFT.GroundStation.GUI.Model {
    internal class OrientationPositionModel : BaseModel {
        private float[] _acceleration = new float[3];

        private float _altAGL;

        private float _altAGLMax;

        private float _altASL;

        private float _altASLMax;

        private float _altInit;

        private float _course;

        private float[] _euler = new float[3];

        private float _gpsAGL;

        private float _gpsAGLMax;

        private float _gpsAltInit;

        private float _gpsASL;

        private float _gpsASLMax;

        private float[] _gyroscope = new float[3];

        private float _latitude;

        private float _longitude;

        private float[] _magnetometer = new float[3];

        private float _pressure;

        private float _speed;

        private float _temperature;

        private DateTime _utc = DateTime.UtcNow;

        public float[] Acceleration {
            get { return _acceleration; }
            set {
                SetProperty(ref _acceleration, value);
                var gx = AccelerationGraphX;
                var gy = AccelerationGraphY;
                var gz = AccelerationGraphZ;
                //gx.Add(new KeyValuePair<DateTime, float>(DateTime.UtcNow, value[0]));
                AccelerationGraphX = gx.Where(x => (x.Key - DateTime.UtcNow).TotalMinutes < 1)
                                       .OrderBy(x => x.Key)
                                       .ToList();
                //gy.Add(new KeyValuePair<DateTime, float>(DateTime.UtcNow, value[1]));
                AccelerationGraphY = gy.Where(x => (x.Key - DateTime.UtcNow).TotalMinutes < 1)
                                       .OrderBy(x => x.Key)
                                       .ToList();
                //gz.Add(new KeyValuePair<DateTime, float>(DateTime.UtcNow, value[2]));
                AccelerationGraphZ = gz.Where(x => (x.Key - DateTime.UtcNow).TotalMinutes < 1)
                                       .OrderBy(x => x.Key)
                                       .ToList();
            }
        }

        public float[] Gyroscope {
            get { return _gyroscope; }
            set {
                SetProperty(ref _gyroscope, value);
                var gx = GyroscopeGraphX;
                var gy = GyroscopeGraphY;
                var gz = GyroscopeGraphZ;
                //gx.Add(new KeyValuePair<DateTime, float>(DateTime.UtcNow, value[0]));
                GyroscopeGraphX = gx.Where(x => (x.Key - DateTime.UtcNow).TotalMinutes < 1)
                                    .OrderBy(x => x.Key)
                                    .ToList();
                //gy.Add(new KeyValuePair<DateTime, float>(DateTime.UtcNow, value[1]));
                GyroscopeGraphY = gy.Where(x => (x.Key - DateTime.UtcNow).TotalMinutes < 1)
                                    .OrderBy(x => x.Key)
                                    .ToList();
                //gz.Add(new KeyValuePair<DateTime, float>(DateTime.UtcNow, value[2]));
                GyroscopeGraphZ = gz.Where(x => (x.Key - DateTime.UtcNow).TotalMinutes < 1)
                                    .OrderBy(x => x.Key)
                                    .ToList();
            }
        }

        public float[] Magnetometer {
            get { return _magnetometer; }
            set {
                SetProperty(ref _magnetometer, value);
                var gx = MagnetometerGraphX;
                var gy = MagnetometerGraphY;
                var gz = MagnetometerGraphZ;
                //gx.Add(new KeyValuePair<DateTime, float>(DateTime.UtcNow, value[0]));
                MagnetometerGraphX = gx.Where(x => (x.Key - DateTime.UtcNow).TotalMinutes < 1)
                                       .OrderBy(x => x.Key)
                                       .ToList();
                //gy.Add(new KeyValuePair<DateTime, float>(DateTime.UtcNow, value[1]));
                MagnetometerGraphY = gy.Where(x => (x.Key - DateTime.UtcNow).TotalMinutes < 1)
                                       .OrderBy(x => x.Key)
                                       .ToList();
                //gz.Add(new KeyValuePair<DateTime, float>(DateTime.UtcNow, value[2]));
                MagnetometerGraphZ = gz.Where(x => (x.Key - DateTime.UtcNow).TotalMinutes < 1)
                                       .OrderBy(x => x.Key)
                                       .ToList();
            }
        }

        public float[] Euler {
            get { return _euler; }
            set {
                SetProperty(ref _euler, value);
                var gx = EulerGraphR;
                var gy = EulerGraphP;
                var gz = EulerGraphY;
                //gx.Add(new KeyValuePair<DateTime, float>(DateTime.UtcNow, value[0]));
                EulerGraphR = gx.Where(x => (x.Key - DateTime.UtcNow).TotalMinutes < 1).OrderBy(x => x.Key).ToList();
                //gy.Add(new KeyValuePair<DateTime, float>(DateTime.UtcNow, value[1]));
                EulerGraphP = gy.Where(x => (x.Key - DateTime.UtcNow).TotalMinutes < 1).OrderBy(x => x.Key).ToList();
                //gz.Add(new KeyValuePair<DateTime, float>(DateTime.UtcNow, value[2]));
                EulerGraphY = gz.Where(x => (x.Key - DateTime.UtcNow).TotalMinutes < 1).OrderBy(x => x.Key).ToList();
            }
        }

        public float Temperature {
            get { return _temperature; }
            set {
                SetProperty(ref _temperature, value);
                var gt = TemperatureGraph;
                gt.Add(new KeyValuePair<DateTime, float>(DateTime.UtcNow, value));
                TemperatureGraph = gt.Where(x => (x.Key - DateTime.UtcNow).TotalMinutes < 1)
                                     .OrderBy(x => x.Key)
                                     .ToList();
            }
        }

        public float Pressure {
            get { return _pressure; }
            set {
                SetProperty(ref _pressure, value);
                var gp = PressureGraph;
                //gp.Add(new KeyValuePair<DateTime, float>(DateTime.UtcNow, value));
                PressureGraph = gp.Where(x => (x.Key - DateTime.UtcNow).TotalMinutes < 1).OrderBy(x => x.Key).ToList();
            }
        }

        public float AltInit {
            get { return _altInit; }
            set {
                SetProperty(ref _altInit, value);
                AltAGL = AltASL - AltInit;
            }
        }

        public float AltASL {
            get { return _altASL; }
            set {
                SetProperty(ref _altASL, value);
                AltAGL = AltASL - AltInit;
                var ga = AltitudeGraphA;
                //ga.Add(new KeyValuePair<DateTime, float>(DateTime.UtcNow, value));
                AltitudeGraphA = ga.Where(x => (x.Key - DateTime.UtcNow).TotalMinutes < 1).OrderBy(x => x.Key).ToList();
            }
        }

        public float AltASLMax {
            get { return _altASLMax; }
            set {
                SetProperty(ref _altASLMax, value);
                AltAGLMax = AltASLMax - AltInit;
            }
        }

        public float AltAGL {
            get { return _altAGL; }
            private set { SetProperty(ref _altAGL, value); }
        }

        public float AltAGLMax {
            get { return _altAGLMax; }
            private set { SetProperty(ref _altAGLMax, value); }
        }

        public DateTime UTC {
            get { return _utc; }
            set { SetProperty(ref _utc, value); }
        }

        public float Course {
            get { return _course; }
            set { SetProperty(ref _course, value); }
        }

        public float Speed {
            get { return _speed; }
            set { SetProperty(ref _speed, value); }
        }

        public float Latitude {
            get { return _latitude; }
            set { SetProperty(ref _latitude, value); }
        }

        public float Longitude {
            get { return _longitude; }
            set { SetProperty(ref _longitude, value); }
        }

        public float GPSAltInit {
            get { return _gpsAltInit; }
            set {
                SetProperty(ref _gpsAltInit, value);
                GPSAGL = GPSASL - GPSAltInit;
            }
        }

        public float GPSASL {
            get { return _gpsASL; }
            set {
                SetProperty(ref _gpsASL, value);
                GPSAGL = GPSASL - GPSAltInit;
                var gg = AltitudeGraphG;
                //gg.Add(new KeyValuePair<DateTime, float>(DateTime.UtcNow, value));
                AltitudeGraphG = gg.Where(x => (x.Key - DateTime.UtcNow).TotalMinutes < 1).OrderBy(x => x.Key).ToList();
            }
        }

        public float GPSASLMax {
            get { return _gpsASLMax; }
            set {
                SetProperty(ref _gpsASLMax, value);
                GPSAGLMax = GPSASLMax - GPSAltInit;
            }
        }

        public float GPSAGL {
            get { return _gpsAGL; }
            private set { SetProperty(ref _gpsAGL, value); }
        }

        public float GPSAGLMax {
            get { return _gpsAGLMax; }
            private set { SetProperty(ref _gpsAGLMax, value); }
        }

        private List<KeyValuePair<DateTime, float>> _accelerationGraphX = new List<KeyValuePair<DateTime, float>>();

        public List<KeyValuePair<DateTime, float>> AccelerationGraphX {
            get { return _accelerationGraphX; }
            private set { SetProperty(ref _accelerationGraphX, value); }
        }

        private List<KeyValuePair<DateTime, float>> _accelerationGraphY = new List<KeyValuePair<DateTime, float>>();

        public List<KeyValuePair<DateTime, float>> AccelerationGraphY {
            get { return _accelerationGraphY; }
            private set { SetProperty(ref _accelerationGraphY, value); }
        }

        private List<KeyValuePair<DateTime, float>> _accelerationGraphZ = new List<KeyValuePair<DateTime, float>>();

        public List<KeyValuePair<DateTime, float>> AccelerationGraphZ {
            get { return _accelerationGraphZ; }
            private set { SetProperty(ref _accelerationGraphZ, value); }
        }

        private List<KeyValuePair<DateTime, float>> _gyroscopeGraphX = new List<KeyValuePair<DateTime, float>>();

        public List<KeyValuePair<DateTime, float>> GyroscopeGraphX {
            get { return _gyroscopeGraphX; }
            private set { SetProperty(ref _gyroscopeGraphX, value); }
        }

        private List<KeyValuePair<DateTime, float>> _gyroscopeGraphY = new List<KeyValuePair<DateTime, float>>();

        public List<KeyValuePair<DateTime, float>> GyroscopeGraphY {
            get { return _gyroscopeGraphY; }
            private set { SetProperty(ref _gyroscopeGraphY, value); }
        }

        private List<KeyValuePair<DateTime, float>> _gyroscopeGraphZ = new List<KeyValuePair<DateTime, float>>();

        public List<KeyValuePair<DateTime, float>> GyroscopeGraphZ {
            get { return _gyroscopeGraphZ; }
            private set { SetProperty(ref _gyroscopeGraphZ, value); }
        }

        private List<KeyValuePair<DateTime, float>> _magnetometerGraphX = new List<KeyValuePair<DateTime, float>>();

        public List<KeyValuePair<DateTime, float>> MagnetometerGraphX {
            get { return _magnetometerGraphX; }
            private set { SetProperty(ref _magnetometerGraphX, value); }
        }

        private List<KeyValuePair<DateTime, float>> _magnetometerGraphY = new List<KeyValuePair<DateTime, float>>();

        public List<KeyValuePair<DateTime, float>> MagnetometerGraphY {
            get { return _magnetometerGraphY; }
            private set { SetProperty(ref _magnetometerGraphY, value); }
        }

        private List<KeyValuePair<DateTime, float>> _magnetometerGraphZ = new List<KeyValuePair<DateTime, float>>();

        public List<KeyValuePair<DateTime, float>> MagnetometerGraphZ {
            get { return _magnetometerGraphZ; }
            private set { SetProperty(ref _magnetometerGraphZ, value); }
        }

        private List<KeyValuePair<DateTime, float>> _eulerGraphR = new List<KeyValuePair<DateTime, float>>();

        public List<KeyValuePair<DateTime, float>> EulerGraphR {
            get { return _eulerGraphR; }
            private set { SetProperty(ref _eulerGraphR, value); }
        }

        private List<KeyValuePair<DateTime, float>> _eulerGraphP = new List<KeyValuePair<DateTime, float>>();

        public List<KeyValuePair<DateTime, float>> EulerGraphP {
            get { return _eulerGraphP; }
            private set { SetProperty(ref _eulerGraphP, value); }
        }

        private List<KeyValuePair<DateTime, float>> _eulerGraphY = new List<KeyValuePair<DateTime, float>>();

        public List<KeyValuePair<DateTime, float>> EulerGraphY {
            get { return _eulerGraphY; }
            private set { SetProperty(ref _eulerGraphY, value); }
        }

        private List<KeyValuePair<DateTime, float>> _altitudeGraphA = new List<KeyValuePair<DateTime, float>>();

        public List<KeyValuePair<DateTime, float>> AltitudeGraphA {
            get { return _altitudeGraphA; }
            private set { SetProperty(ref _altitudeGraphA, value); }
        }

        private List<KeyValuePair<DateTime, float>> _altitudeGraphG = new List<KeyValuePair<DateTime, float>>();

        public List<KeyValuePair<DateTime, float>> AltitudeGraphG {
            get { return _altitudeGraphG; }
            private set { SetProperty(ref _altitudeGraphG, value); }
        }

        private List<KeyValuePair<DateTime, float>> _temperatureGraph = new List<KeyValuePair<DateTime, float>>();

        public List<KeyValuePair<DateTime, float>> TemperatureGraph {
            get { return _temperatureGraph; }
            private set { SetProperty(ref _temperatureGraph, value); }
        }

        private List<KeyValuePair<DateTime, float>> _pressureGraph = new List<KeyValuePair<DateTime, float>>();

        public List<KeyValuePair<DateTime, float>> PressureGraph {
            get { return _pressureGraph; }
            private set { SetProperty(ref _pressureGraph, value); }
        }
    }
}
