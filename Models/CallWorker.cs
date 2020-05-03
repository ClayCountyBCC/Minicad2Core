using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.SignalR;
using Minicad2Core.Hubs;
using Microsoft.Extensions.Logging;

namespace Minicad2Core.Models
{
  public class CallWorker : BackgroundService
  {
    private readonly ILogger<CallWorker> _logger;
    private readonly IHubContext<CallHub, ICallHub> _callHub;

    public CallWorker(ILogger<CallWorker> logger, IHubContext<CallHub, ICallHub> callHub)
    {
      _logger = logger;
      _callHub = callHub;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        _logger.LogInformation("Worker running at: {Time}", DateTime.Now);
        await _callHub.Clients.All.SendMessage(DateTime.Now.ToString());
        await Task.Delay(1000);
      }
    }

  }
}
