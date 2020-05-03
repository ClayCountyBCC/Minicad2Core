using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Minicad2Core.Hubs
{

  public interface ICallHub
  {
    Task SendMessage(string message);
  }

  public class CallHub : Hub<ICallHub>
  {
    [HubMethodName("SendTime")]
    public async Task SendClientMessages(string message)
    {
      await Clients.All.SendMessage(message);
    }

  }
}
