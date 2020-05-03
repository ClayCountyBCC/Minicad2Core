using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Minicad2Core.Models;
using Microsoft.Extensions.Options;

namespace Minicad2Core.Hubs
{

  public interface IUnitHub
  {
    Task Units(List<Unit> units);
  }

  public class UnitHub : Hub<IUnitHub>
  {

    [HubMethodName("Units")]
    public async Task SendClientMessages(List<Unit> units)
    {
      await Clients.All.Units(units);
    }

  }
}
