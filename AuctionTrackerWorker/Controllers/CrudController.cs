using AuctionTrackerWorker.Models;
using Microsoft.AspNetCore.Mvc;
using AuctionTrackerWorker.Services;

namespace AuctionTrackerWorker.Controllers;

[ApiController]
[Route("bidservice/v1")]
public class CRUDController : ControllerBase
{


    private readonly ILogger<CRUDController> _logger;

    private readonly IMongoServiceFactory _mongoServiceFactory;

    public CRUDController(ILogger<CRUDController> logger, IMongoServiceFactory factory)
    {
        _logger = logger;
        _mongoServiceFactory = factory;
       
    }

    //getAllActiveBids
    [HttpGet("getallactivebids")]
     public async Task<IActionResult> GetAll()
    {
        
        try
        {
            var dbService = _mongoServiceFactory.CreateScoped();
            _logger.LogInformation("create scoped done");
            var response = await dbService.GetAllBids();
            _logger.LogInformation("er ude af serice igen");
            return Ok(response);
        }
        catch (ItemsNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
         catch (Exception ex)
        {
            
            return StatusCode(500, new { error = "An unexpected error occurred." + ex.Message });
        }
       
    }
    [HttpGet("getactivebidbycatalogid/{id}")]
    public async Task<IActionResult> GetById([FromRoute]string id)
    {
        try
        {
            var dbService = _mongoServiceFactory.CreateScoped();
            var item = await dbService.GetById(id);
            return Ok(item);
        }
        catch (ItemsNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            // Handle other exceptions or unexpected errors
            return StatusCode(500, new { error = "An unexpected error occurred."+ ex.Message });
        }
    }

    [HttpGet("getactivebidsbyemail/{email}")]
    public async Task<IActionResult> GetByCategory([FromRoute]string email)
    {
        try
        {
            var dbService = _mongoServiceFactory.CreateScoped();
            var items = await dbService.GetByEmail(email);
            return Ok(items);
        }
        catch (ItemsNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            // Handle other exceptions or unexpected errors
            return StatusCode(500, new { error = "An unexpected error occurred."+ ex.Message });
        }
    }

     [HttpGet("getalllogs")]
    public async Task<IActionResult> GetAllLogs()
    {
        
        try
        {
            var dbService = _mongoServiceFactory.CreateScoped();
            var response = await dbService.GetAllLogs();
            return Ok(response);
        }
        catch (ItemsNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
         catch (Exception ex)
        {
            
            return StatusCode(500, new { error = "An unexpected error occurred." + ex.Message });
        }
       
    }
        [HttpGet("getlogsbycatalogid/{id}")]
    public async Task<IActionResult> GetAllLogsById([FromRoute]string id)
    {
        
        try
        {
            var dbService = _mongoServiceFactory.CreateScoped();
            var response = await dbService.GetLogsById(id);
            return Ok(response);
        }
        catch (ItemsNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
         catch (Exception ex)
        {
            
            return StatusCode(500, new { error = "An unexpected error occurred." + ex.Message });
        }
       
    }

    [HttpGet("getlogsbycatalogid/{email}")]
    public async Task<IActionResult> GetAllLogsByEmail([FromRoute]string email)
    {
        
        try
        {
            var dbService = _mongoServiceFactory.CreateScoped();
            var response = await dbService.GetLogsByEmail(email);
            return Ok(response);
        }
        catch (ItemsNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
         catch (Exception ex)
        {
            
            return StatusCode(500, new { error = "An unexpected error occurred." + ex.Message });
        }
       
    }
    
}