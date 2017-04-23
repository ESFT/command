using System;
using System.IO;
using ESFT.GroundStation.Packet.Data;

namespace ESFT.GroundStation.Packet {
    internal class PacketLogger {
        private readonly IPacket _packet;
        private readonly string _fileName;
        private readonly string _header;

        public PacketLogger(IPacket packet, string header = "") :
            this(packet, header,
                 Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + packet.GetType().Name + "-" +
                 DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv") {
        }

        public PacketLogger(IPacket packet, string header, string filename) {
            _packet = packet;
            _fileName = filename;
            _header = header;
            Start();
        }

        public void Stop() {
            _packet.PacketUpdated -= Log;
        }

        public void Start() {
            _packet.PacketUpdated += Log;
        }

        private async void Log(object sender, PacketUpdatedEventArgs args) {
            try {
                // This text is added only once to the file.
                if (!File.Exists(_fileName))
                    using (var sw = File.CreateText(_fileName)) {
                        await sw.WriteLineAsync(_header);
                        await sw.WriteLineAsync(_packet.CSVHeader);
                    }
                using (var sw = File.AppendText(_fileName)) {
                    await sw.WriteLineAsync(_packet.CSVData);
                }
            } catch (Exception ex) {
                Console.WriteLine($@"File IO Error: {ex.Message}");
            }
        }
    }
}
