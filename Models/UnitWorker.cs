using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.SignalR;
using Minicad2Core.Hubs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Minicad2Core.Models
{
  public class UnitWorker : BackgroundService
  {
    //private readonly ILogger<UnitWorker> _logger;
    private readonly ConnectionConfig _config;
    private readonly IHubContext<UnitHub, IUnitHub> _unitHub;


    public UnitWorker(IHubContext<UnitHub, IUnitHub> unitHub, IOptions<ConnectionConfig> connectionConfig)
    {
      //_logger = logger;    
      _config = connectionConfig.Value;
      _unitHub = unitHub;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        //_logger.LogInformation("Worker running at: {Time}", DateTime.Now);
        var units = Unit.GetAllCurrentUnits(_config.Tracking);
        await _unitHub.Clients.All.Units(units);
        await Task.Delay(10000);
      }
    }

  }
}
