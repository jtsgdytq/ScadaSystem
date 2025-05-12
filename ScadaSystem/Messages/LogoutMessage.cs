using CommunityToolkit.Mvvm.Messaging.Messages;
using ScadaSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaSystem.Messages
{
    class LogoutMessage : ValueChangedMessage<User>
    {
        public LogoutMessage(User value) : base(value)
        {
        }
    }
}
