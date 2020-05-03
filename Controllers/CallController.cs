using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minicad2Core.Models;
using Microsoft.Extensions.Options;

namespace Minicad2Core.Controllers
{
  [Route("api/Call")]
  [ApiController]
  public class CallController : ControllerBase
  {
    private ConnectionConfig _config;

    public CallController(IOptions<ConnectionConfig> connectionConfig)
    {
      _config = connectionConfig.Value;
    }

    [HttpGet("Get")]
    public ActionResult<List<Call>> GetActiveCalls()
    {
      return Call.GetActiveCalls(_config.CAD);
    }

  }

}