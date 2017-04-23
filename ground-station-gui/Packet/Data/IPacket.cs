using System;

namespace ESFT.GroundStation.Packet.Data {
    internal interface IPacket : IDisposable {
        int Size { get; }

        byte[] Bytes { get; set; }

        string CSVData { get; }

        string CSVHeader { get; }

        uint CRC { get; }
        event EventHandler<PacketUpdatedEventArgs> PacketUpdated;
    }
}
