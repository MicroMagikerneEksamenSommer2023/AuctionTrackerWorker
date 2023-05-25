using AuctionTrackerWorker.Models;
using Microsoft.AspNetCore.Mvc;
using AuctionTrackerWorker.Services;

namespace AuctionTrackerWorker.Controllers;

[ApiController]
[Route("bidworker/v1")]
public class CRUDController : ControllerBase
{


    private readonly ILogger<CRUDController> _logger;

    private readonly IMongoServiceFactory _mongoServiceFactory;

    public CRUDController(ILogger<CRUDController> logger, IMongoServiceFactory factory)
    {
        _logger = logger;
        _mongoServiceFactory = factory;
       
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