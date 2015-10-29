using System;
namespace MeshSimulator.Model
{
    public enum MessageType
    {
        Sync,
        InfoExpand
    }
    public interface IMessage
    {
        int FromId { get; set; }
        bool IsNoise { get; set; }
        int ToId { get; set; }
        MessageType Type { get; set; }
    }
}
