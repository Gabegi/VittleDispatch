using Futures.Api.Data.Interfaces;
using Futures.Api.Data.Models;
using Futures.ApiContracts;
using Microsoft.AspNetCore.Mvc;


namespace Futures_API.Controllers
{
    /// <summary>
    /// A controller for interacting with Restaurants
    /// </summary>
    [ApiController]
    [Route("restaurant")]
    public class RestaurantController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<RestaurantController> _logger;
        private readonly IRestaurantRepository _repository;

        public RestaurantController(ILogger<RestaurantController> logger, IConfiguration configuration, IRestaurantRepository repository)
        {
            _logger = logger;
            _configuration = configuration;
            _repository = repository;
        }

        /// <summary>
        /// Get a specific restaurant by its ID. The id should be 0 or higher.
        /// Returns notfound if there is no restaurant with this ID
        /// </summary>
        /// <param name="id">A restaurant ID</param>
        /// <returns></returns>
        [HttpGet("{id?}")]
        [ProducesResponseType(typeof(GetRestaurantResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetRestaurantResponseModel>> Get(int id)
        {
            if (id < 0)
            {
                return BadRequest("Please enter a valid restaurant ID");
            }

            var restaurant = await _repository.GetById(id);

            if (restaurant == null)
            {
                return NotFound();
            }

            var result = new GetRestaurantResponseModel
            {
                CategoryName = restaurant.CategoryName,
                RestaurantName = restaurant.RestaurantName,
                RestaurantAddress = restaurant.RestaurantAddress,
                RestaurantId = restaurant.RestaurantId,
                ZoneId = restaurant.ZoneId
            };

            return new ActionResult<GetRestaurantResponseModel>(result);
        }

        /// Gets a restaurant by its name.
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("GetByName/{name}")]
        [ProducesResponseType(typeof(GetRestaurantResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetRestaurantResponseModel>> GetByName(string name) // check if no inputs/ check what's in the string (e.g. 1) // test if valid name // name for whcih no restaurants exist// check category of data
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Please enter a valid restaurant name");
            }

            var restaurant = await _repository.GetByName(name);

            if (restaurant == null)
            {
                return NotFound();
            }

            // For later: convert this to automapper
            var result = new GetRestaurantResponseModel
            {
                CategoryName = restaurant.CategoryName,
                RestaurantName = restaurant.RestaurantName,
                RestaurantAddress = restaurant.RestaurantAddress,
                RestaurantId = restaurant.RestaurantId,
                ZoneId = restaurant.ZoneId,
            };
            if(restaurant.CategoryNameNavigation != null)
            {
                result.CategoryNameNavigation = new FoodCategoryResponseModelWithoutRestaurant
                {
                    CategoryDescription = restaurant.CategoryNameNavigation.CategoryDescription,
                    CategoryName = restaurant.CategoryNameNavigation.CategoryName ?? string.Empty
                };
            }

            return new ActionResult<GetRestaurantResponseModel>(result);
        }

        /// Gets all restaurants
        /// <returns></returns>
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(IEnumerable<GetRestaurantResponseModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<GetRestaurantResponseModel>>> GetAll()
        {

            var restaurants = await _repository.GetAll();

            var result = new List<GetRestaurantResponseModel>();

            foreach (var r in restaurants)
            {
                var restaurant = new GetRestaurantResponseModel
                {
                    CategoryName = r.CategoryName,
                    RestaurantName = r.RestaurantName,
                    RestaurantAddress = r.RestaurantAddress,
                    RestaurantId = r.RestaurantId,
                    ZoneId = r.ZoneId
                };

                result.Add(restaurant);
            };

            return new ActionResult<IEnumerable<GetRestaurantResponseModel>>(result);
        }

        /// Inserts or Upserts a restaurant
        /// <returns></returns>
        [HttpPost("Insert")]
        [ProducesResponseType(typeof(GetRestaurantResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task InsertNewAsync(GetRestaurantResponseModel newRestaurant)
        {
            var ItemToAdd = new Restaurant
            {
                CategoryName = newRestaurant.CategoryName,
                RestaurantName = newRestaurant.RestaurantName,
                RestaurantAddress = newRestaurant.RestaurantAddress,
                ZoneId = newRestaurant.ZoneId,
            };

            // verification values
            var check = await _repository.GetByName(ItemToAdd.RestaurantName);

            if (check == null)
            {
                await _repository.Insert(ItemToAdd);
            }

            if (check != null)
            {
                await _repository.Update(ItemToAdd);
            }
        }


        /// Inserts or Upserts a restaurant
        /// <returns></returns>
        [HttpPut("Upsert")]
        [ProducesResponseType(typeof(GetRestaurantResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task UpsertNewAsync(GetRestaurantResponseModel newRestaurant)
        {
            var upserRestaurant = new Restaurant
            {
                RestaurantName = newRestaurant.RestaurantName,
                CategoryName = newRestaurant.CategoryName,
                RestaurantAddress = newRestaurant.RestaurantAddress,
                ZoneId = newRestaurant.ZoneId
            };

            await _repository.Update(upserRestaurant);
        }

        /// Deletes a restaurant
        [HttpDelete("Delete")]
        [ProducesResponseType(typeof(GetRestaurantResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task DeleteById(int restaurantId)
        {
            await _repository.DeleteAsync(restaurantId);
        }
    }
}
