using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using ESFT.GroundStation.GUI.Model;
using ESFT.GroundStation.Packet.Data;

namespace ESFT.GroundStation.GUI.ViewModel {
    internal class MainViewModel : BaseViewModel {
        public MainViewModel() {
            ((RocketPortModel) Ports["RT"]).PacketReceived += OnRocketDataPacketReceived;
            ((GSPortModel) Ports["GS"]).PacketReceived += OnGSDataPacketReceived;

            RTCommandPacketUpdateTimer.Interval = TimeSpan.FromSeconds(2);
            RTCommandPacketUpdateTimer.Tick += OnRTCommandPacketUpdateTimerTick;
            RTCommandPacketUpdateTimer.Start();

            GSTimeoutTimer.Interval = TimeSpan.FromSeconds(5);
            GSTimeoutTimer.Tick += OnGSTimeoutTimerTick;
            GSTimeoutTimer.Start();
        }

        public Dictionary<string, ConnectivityModel> IsConnected { get; } = new Dictionary<string, ConnectivityModel> {
                                                                                                                          {
                                                                                                                              "Radio",
                                                                                                                              new
                                                                                                                                  ConnectivityModel()
                                                                                                                          }, {
                                                                                                                              "IMU",
                                                                                                                              new
                                                                                                                                  ConnectivityModel()
                                                                                                                          }, {
                                                                                                                              "Alt",
                                                                                                                              new
                                                                                                                                  ConnectivityModel()
                                                                                                                          }, {
                                                                                                                              "SDCard",
                                                                                                                              new
                                                                                                                                  ConnectivityModel()
                                                                                                                          }, {
                                                                                                                              "GPS",
                                                                                                                              new
                                                                                                                                  ConnectivityModel()
                                                                                                                          }
                                                                                                                      };

        public LaunchArmedModel LaunchArmed { get; } = new LaunchArmedModel(4);

        public EventTimesModel EventTimes { get; } = new EventTimesModel();

        public Dictionary<string, PortModel> Ports { get; } =
            new Dictionary<string, PortModel> {{"RT", new RocketPortModel()}, {"GS", new GSPortModel()}};

        public OrientationPositionModel OrientationPosition { get; } = new OrientationPositionModel();

        protected DispatcherTimer RTCommandPacketUpdateTimer { get; } = new DispatcherTimer();

        protected DispatcherTimer GSTimeoutTimer { get; } = new DispatcherTimer();
        
        protected void OnRocketDataPacketReceived(object sender, PacketUpdatedEventArgs data) {
            Application.Current.Dispatcher.Invoke(() => {
                var packet = (RocketDataPacket) data.Packet;
                IsConnected["Radio"].IsConnected = true;
                IsConnected["SDCard"].IsConnected = packet.SDCardConnected;

                LaunchArmed.IsRocketArmed = packet.LaunchedArmed;
                EventTimes.BackDrogueTime = packet.BackDrogueTime;
                EventTimes.BackMainTime = packet.BackMainTime;
                EventTimes.ElapsedTime = packet.ElapsedTime;
                EventTimes.LaunchTime = packet.LaunchTime;
                EventTimes.SelfDrogueBackTime = packet.SelfDrogueBackTime;
                EventTimes.SelfDroguePrimTime = packet.SelfDroguePrimTime;
                EventTimes.SelfMainBackTime = packet.SelfMainBackTime;
                EventTimes.SelfMainPrimTime = packet.SelfMainPrimTime;

                IsConnected["IMU"].IsConnected = packet.IMUConnected;
                if (packet.IMUConnected) {
                    OrientationPosition.Acceleration = packet.Acceleration;
                    OrientationPosition.Gyroscope = packet.GyroScope;
                    OrientationPosition.Magnetometer = packet.Magnetometer;
                    OrientationPosition.Euler = new[] {packet.Roll, packet.Pitch, packet.Yaw};
                }

                IsConnected["Alt"].IsConnected = packet.AltConnected;
                if (packet.AltConnected) {
                    OrientationPosition.Temperature = packet.Temperature;
                    OrientationPosition.Pressure = packet.Pressure;
                    OrientationPosition.AltASL = packet.AltAltitude;
                    OrientationPosition.AltASLMax = packet.AltAltitudeMax;
                }

                IsConnected["GPS"].IsConnected = packet.GPSConnected;
                if (packet.GPSConnected) {
                    OrientationPosition.GPSAltInit = packet.GPSAltitudeInit;
                    OrientationPosition.Course = packet.Course;
                    OrientationPosition.Latitude = packet.Latitude;
                    OrientationPosition.Longitude = packet.Longitude;
                    OrientationPosition.UTC = packet.UTC;
                }
            });
        }

        protected void OnGSDataPacketReceived(object sender, PacketUpdatedEventArgs data) {
            Dispatcher.CurrentDispatcher.Invoke(() => {
                foreach (var key in LaunchArmed.KeyList) key.IsEngaged = true;
            });
        }
        
        protected void OnGSTimeoutTimerTick(object sender, EventArgs e) {
            if (!(DateTime.UtcNow.Subtract(Ports["GS"].DataLastUpdateTime).TotalSeconds >= 2)) return;
            foreach (var key in LaunchArmed.KeyList) key.IsEngaged = false;
        }

        protected void OnRTCommandPacketUpdateTimerTick(object sender, EventArgs e) {
            ((RocketPortModel) Ports["RT"]).SendPacket(LaunchArmed.IsGSArmed);
            if ((DateTime.UtcNow - Ports["RT"].DataLastUpdateTime).TotalSeconds > 5)
                IsConnected["Radio"].IsConnected = false;
        }

        public override void OnClosing(object sender, CancelEventArgs e) {
            Dispose();
        }

        #region IDisposable Support

        private bool _disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (_disposedValue) return;
            if (disposing) {
            }

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
