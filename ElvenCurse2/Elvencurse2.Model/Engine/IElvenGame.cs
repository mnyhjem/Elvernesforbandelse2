using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Elvencurse2.Model.Engine
{
    public interface IElvenGame
    {
        ConcurrentQueue<Payload> GameChanges { get; set; }
    }
}
