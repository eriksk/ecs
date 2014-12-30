using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ecs.Core.Messages;

namespace Ecs.Core.Functions
{
    public interface IReceiveMessage
    {
        void ReceiveMessage(Message message);
    }
}
