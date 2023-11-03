using Futures.Api.Data.Interfaces;
using Futures.Api.Data.Models;
using Futures.ApiContracts;
using Microsoft.AspNetCore.Mvc;

namespace Futures_API.Controllers
{

    [ApiController]
    [Route("zone")]
    public class ZoneController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<ZoneController> _logger;
        private readonly IZoneRepository _repository;

        public ZoneController(ILogger<ZoneController> logger, IConfiguration configuration, IZoneRepository repository)
        {
            _logger = logger;
            _configuration = configuration;
            _repository = repository;
        }


        [HttpGet("{id?}")]
        [ProducesResponseType(typeof(GetZoneResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetZoneResponseModel>> Get(int id)
        {
            _logger.LogInformation("You have reached the Controller");

            if (id < 0)
            {
                return BadRequest("Please enter a valid zone ID");
            }

            var zone = await _repository.GetById(id);

            if (zone == null)
            {
                return NotFound();
            }

            var result = new GetZoneResponseModel
            {
                zone_id = zone.ZoneId,
                zone_description = zone.ZoneDescription
            };
            
            return new ActionResult<GetZoneResponseModel>(result);
        }


        [HttpGet("GetByZoneDescription/{zoneDescription}")]
        [ProducesResponseType(typeof(GetZoneResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetZoneResponseModel>> GetByZoneDescription(string zoneDescription)
        {

            _logger.LogInformation("You have reached the Controller");

            if (string.IsNullOrWhiteSpace(zoneDescription))
            {
                _logger.LogError($"Your string {zoneDescription} is empty, whitespaced or invalid");

                return BadRequest("Please enter a valid zone description");
            }

            var zone = await _repository.GetByZoneDescription(zoneDescription);

            if (zone == null)
            {
                _logger.LogError($"Your Zone {zoneDescription} doesn't exist in the database");

                return NotFound();
            }

            // For later: convert this to automapper
            var result = new GetZoneResponseModel
            {
                zone_id = zone.ZoneId,
                zone_description = zone.ZoneDescription
            };

            return new ActionResult<GetZoneResponseModel>(result);
        }


        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(IEnumerable<GetZoneResponseModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<GetZoneResponseModel>>> GetAll()
        {
            _logger.LogInformation("You have reached the Controller");

            var zones = await _repository.GetAll();

            var result = new List<GetZoneResponseModel>();

            foreach (var z in zones)
            {
                var zone = new GetZoneResponseModel
                {
                    zone_id = z.ZoneId,
                    zone_description = z.ZoneDescription
                };

                result.Add(zone);
            };

            if(zones.Count() <= 0)
            {
                _logger.LogError("The database doesn't contain any Zones yet! Please add Zones");
            }

            return new ActionResult<IEnumerable<GetZoneResponseModel>>(result);
        }

        [HttpPost("Insert")]
        [ProducesResponseType(typeof(GetZoneResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task InsertNewAsync(GetZoneResponseModel newZone)
        {
            _logger.LogInformation("You have reached the Controller");

            var itemToAdd = new Zone
            {   
                ZoneId = newZone.zone_id,
                ZoneDescription = newZone.zone_description
            };

            // verification values
            var check = await _repository.GetByZoneDescription(itemToAdd.ZoneDescription);

            if (check == null)
            {
                await _repository.Insert(itemToAdd);
                _logger.LogInformation("The database added your Zone");
            }
            if (check != null)
            {
                await _repository.Update(itemToAdd);
                _logger.LogInformation("The database updated an existing Zone with the information you provided");
            }
        }


        [HttpPatch("Update")]
        [ProducesResponseType(typeof(GetZoneResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task UpdateNewAsync(GetZoneResponseModel newZone)
        {
            _logger.LogInformation("You have reached the Controller");

            var updateZone = new Zone
            {
                ZoneDescription = newZone.zone_description,
                ZoneId = newZone.zone_id
            };

            await _repository.Update(updateZone);
            _logger.LogInformation("The database updated an existing Zone");
        }

        [HttpDelete("DeleteById")]
        [ProducesResponseType(typeof(GetZoneResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<string> DeleteById(int zoneId) 
        {
            _logger.LogInformation("You have reached the Controller");

            if (zoneId <= 0)
            {
                _logger.LogError("You entered a Zone Id that is 0 or less");
                return "Id must be higher than 0";
            }
            else
            {
                var resultFromId = await _repository.GetById(zoneId);

                if(resultFromId == null)
                {
                    _logger.LogError("You entered a Zone Id that doesn't exist in the database");
                    return $"There is no Zone with the id {zoneId}";
                }
                else
                {
                    await _repository.DeleteAsync(zoneId);
                    _logger.LogError($"The Zone with zone Id: {zoneId} has been deleted");
                    return $"Zone {zoneId} has been deleted";
                }             
            }
        }
    }
}
