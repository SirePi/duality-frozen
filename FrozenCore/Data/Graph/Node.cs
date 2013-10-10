// This code is provided under the MIT license. Originally by Alessandro Pilati.

namespace FrozenCore.Data.Graph
{
    /// <summary>
    /// Interface that should be implemented by a class in order to be used as Node in a Graph
    /// </summary>
    public interface INode
    {
        float NodeX { get; set; }
        float NodeY { get; set; }
    }
}