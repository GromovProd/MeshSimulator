using System;
namespace MeshSimulator.Model
{
    interface IMessage
    {
        int FromId { get; set; }
        bool IsNoise { get; set; }
        int ToId { get; set; }
    }
}
