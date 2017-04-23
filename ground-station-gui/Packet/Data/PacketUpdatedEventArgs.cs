using System;

namespace ESFT.GroundStation.Packet.Data {
    internal class PacketUpdatedEventArgs : EventArgs {
        public DateTime UpdateTime { get; set; }
        public IPacket Packet { get; set; }
    }
}
