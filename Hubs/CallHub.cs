using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Minicad2Core.Models;

namespace Minicad2Core.Hubs
{

  public interface ICallHub
  {
    Task Calls(List<Call> calls);
  }

  public class CallHub : Hub<ICallHub>
  {
    [HubMethodName("Calls")]
    public async Task Calls(List<Call> calls)
    {
      await Clients.All.Calls(calls);
    }

  }
}
