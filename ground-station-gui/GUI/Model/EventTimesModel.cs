namespace ESFT.GroundStation.GUI.Model {
    internal class EventTimesModel : BaseModel {
        private uint _backDrogueTime;

        private uint _backMainTime;

        private uint _elapsedTime;

        private uint _launchTime;

        private uint _selfDrogueBackTime;

        private uint _selfDroguePrimTime;

        private uint _selfMainBackTime;

        private uint _selfMainPrimTime;

        public uint ElapsedTime {
            get { return _elapsedTime; }
            set { SetProperty(ref _elapsedTime, value); }
        }

        public uint LaunchTime {
            get { return _launchTime; }
            set { SetProperty(ref _launchTime, value); }
        }

        public uint BackDrogueTime {
            get { return _backDrogueTime; }
            set { SetProperty(ref _backDrogueTime, value); }
        }

        public uint BackMainTime {
            get { return _backMainTime; }
            set { SetProperty(ref _backMainTime, value); }
        }

        public uint SelfDroguePrimTime {
            get { return _selfDroguePrimTime; }
            set { SetProperty(ref _selfDroguePrimTime, value); }
        }

        public uint SelfDrogueBackTime {
            get { return _selfDrogueBackTime; }
            set { SetProperty(ref _selfDrogueBackTime, value); }
        }

        public uint SelfMainPrimTime {
            get { return _selfMainPrimTime; }
            set { SetProperty(ref _selfMainPrimTime, value); }
        }

        public uint SelfMainBackTime {
            get { return _selfMainBackTime; }
            set { SetProperty(ref _selfMainBackTime, value); }
        }
    }
}
