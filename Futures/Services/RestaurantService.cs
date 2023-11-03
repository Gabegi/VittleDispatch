using System.Net;
using Flurl;
using Flurl.Http;
using Futures.ApiContracts;
using Futures.Services.ServiceInterfaces;
using Polly;

namespace Futures.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly ILogger<RestaurantService> _logger;
        public RestaurantService(ILogger<RestaurantService> logger)
        {
            _logger = logger;
        }

        private static bool IsTransientErrors(FlurlHttpException exception)
        {
            int[] httpStatusCodeWorthRetrying =
            {
                (int)HttpStatusCode.RequestTimeout,
                (int)HttpStatusCode.BadGateway,
                (int)HttpStatusCode.ServiceUnavailable,
                (int)HttpStatusCode.GatewayTimeout
            };

            return exception.StatusCode.HasValue && httpStatusCodeWorthRetrying.Contains(exception.StatusCode.Value);
        }

        // Fetching the endpoint URL for Future API which is stored in app setting
        private readonly string _apiEndpoint = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["EndPointURL"];
        private const string _classPath = "restaurant";
        private readonly Random _random = new();


        public async Task<GetRestaurantResponseModel> GetbyIdAsync(int restaurantId)
        {
            if (restaurantId <= 0)
            {

                var ex =  new ArgumentOutOfRangeException(nameof(restaurantId), "Restaurant id cannot be 0 or less");
                _logger.LogError("You are passing a Restaurant id that is 0 or less, please provide a valid id", ex);
                throw ex;
                

            }


            try
            {

                   var apiResponse = await Policy
                    .Handle<FlurlHttpException>(IsTransientErrors)
                    .WaitAndRetryAsync(2, retryAttempt =>
                    {
                        var nextAttemptTime = TimeSpan.FromSeconds(_random.Next(1, retryAttempt));

                        Console.WriteLine($"Retry attempt {retryAttempt} to make request. Next try on {nextAttemptTime.TotalSeconds} seconds.");

                        return nextAttemptTime;
                    })
                    .ExecuteAsync(() => _apiEndpoint
                        .AppendPathSegment(_classPath + "/" + restaurantId)
                        .WithHeader("accept", "application/json")
                        .GetJsonAsync<GetRestaurantResponseModel>()
                    );
                if(apiResponse != null)
                {
                    return apiResponse;
                }

                else
                {
                    var ex = new NullReferenceException($"No restaurant with id {restaurantId} has been found");
                    _logger.LogError("The restaurant Id you provided doesn't exist in the database", ex);
                    throw ex;
                }
             }

            catch (FlurlHttpException ex)
            {
                _logger.LogError("You are getting a Flurl error, there is something wrong with your API call", ex);
                throw (ex);

            }


        }

        public async Task<GetRestaurantResponseModel> GetbyNameAsync(string restaurantName)
        {
            if (string.IsNullOrWhiteSpace(restaurantName))
            {
                var ex = new ArgumentNullException(nameof(restaurantName), "Restaurant name cannot be empty");
                _logger.LogError("You are passing a Restaurant Name that is empty or is a white space, please provide a valid name", ex);
                throw ex;
            }

            try
            {
                var apiResponse = await Policy
                    .Handle<FlurlHttpException>(IsTransientErrors)
                    .WaitAndRetryAsync(2, retryAttempt =>
                    {
                        var nextAttemptTime = TimeSpan.FromSeconds(_random.Next(1, retryAttempt));

                        Console.WriteLine($"Retry attempt {retryAttempt} to make request. Next try on {nextAttemptTime.TotalSeconds} seconds.");

                        return nextAttemptTime;
                    })
                    .ExecuteAsync(() => _apiEndpoint
                        .AppendPathSegment(_classPath + "/GetByName/" + restaurantName)
                        .WithHeader("accept", "application/json")
                        .GetJsonAsync<GetRestaurantResponseModel>()
                    );

                if (apiResponse != null)
                {
                    return apiResponse;
                }

                else
                {
                    var ex = new NullReferenceException($"No restaurant with the name {restaurantName} has been found");
                    _logger.LogError("You are passing a Restaurant Name that doesn't exist in the database", ex);
                    throw ex;
                }
            }
            catch (FlurlHttpException ex)
            {
                _logger.LogError("You are getting a Flurl error, there is something wrong with your API call", ex);
                throw (ex);
            }
        }

        public async Task<IEnumerable<GetRestaurantResponseModel>> GetAllAsync()
        {

            try
            {
                var apiResponse = await Policy
                    .Handle<FlurlHttpException>(IsTransientErrors)
                    .WaitAndRetryAsync(2, retryAttempt =>
                    {
                        var nextAttemptTime = TimeSpan.FromSeconds(_random.Next(1, retryAttempt));

                        Console.WriteLine($"Retry attempt {retryAttempt} to make request. Next try on {nextAttemptTime.TotalSeconds} seconds.");

                        return nextAttemptTime;
                    })
                    .ExecuteAsync(() => _apiEndpoint
                        .AppendPathSegment(_classPath + "/GetAll/")
                        .WithHeader("accept", "application/json")
                        .GetJsonAsync<IEnumerable<GetRestaurantResponseModel>>()
                    );

                return apiResponse;
            }
            catch (FlurlHttpException ex)
            {
                _logger.LogError("You are getting a Flurl error, there is something wrong with your API call", ex);
                throw (ex);
            }
        }

        public async Task DeleteByIdAsync(int restaurantId)
        {
            if (restaurantId <= 0)
            {
                var ex = new ArgumentOutOfRangeException(nameof(restaurantId), "Restaurant id cannot be 0 or less");

                _logger.LogError("You are passing a Restaurant Id that is 0 or less", ex);
                throw ex;
            }

            var result = await GetbyIdAsync(restaurantId);

            if (result == null)
            {
                var ex = new NullReferenceException($"You need an id to delete a restaurant, you entered a null: {restaurantId}");
                _logger.LogError("You are passing a Restaurant Id that doesn't exist in the database", ex);
                throw ex;
            }

            try
            {
                var apiResponse = await Policy
                    .Handle<FlurlHttpException>(IsTransientErrors)
                    .WaitAndRetryAsync(2, retryAttempt =>
                    {
                        var nextAttemptTime = TimeSpan.FromSeconds(_random.Next(1, retryAttempt));

                        Console.WriteLine($"Retry attempt {retryAttempt} to make request. Next try on {nextAttemptTime.TotalSeconds} seconds.");

                        return nextAttemptTime;
                    })
                    .ExecuteAsync(() => _apiEndpoint
                        .AppendPathSegment(_classPath + "/Delete/" + restaurantId)
                        .WithHeader("accept", "application/json")
                        .PostJsonAsync(restaurantId)
                    );


            }
            catch (FlurlHttpException ex)
            {
                _logger.LogError("You are getting a Flurl error, there is something wrong with your API call", ex);
                throw (ex);
            }
        }


        public async Task InsertNewAsync(GetRestaurantResponseModel newRestaurant)
        {

            if (string.IsNullOrEmpty(newRestaurant.RestaurantName))
            {

                var ex = new ArgumentNullException("Restaurant Name cannot be empty");
                _logger.LogError("You are passing a Restaurant Name that doesn't exist in the database", ex);
                throw ex;

            }

            var result = await GetbyNameAsync(newRestaurant.RestaurantName);

            if (result != null)
            {
                var ex = new Exception($"Restaurant with name {newRestaurant.RestaurantName} already exist");
                _logger.LogError("You are passing a Restaurant Name that already exists in the database", ex);
                throw ex;
            }

            try
            {
                var apiResponse = await Policy
                    .Handle<FlurlHttpException>(IsTransientErrors)
                    .WaitAndRetryAsync(2, retryAttempt =>
                    {
                        var nextAttemptTime = TimeSpan.FromSeconds(_random.Next(1, retryAttempt));

                        Console.WriteLine($"Retry attempt {retryAttempt} to make request. Next try on {nextAttemptTime.TotalSeconds} seconds.");

                        return nextAttemptTime;
                    })
                    .ExecuteAsync(() => _apiEndpoint
                        .AppendPathSegment(_classPath + "/Insert/")
                        .WithHeader("accept", "application/json")
                        .PostJsonAsync(newRestaurant)
                    );


            }
            catch (FlurlHttpException ex)
            {
                _logger.LogError("You are getting a Flurl error, there is something wrong with your API call", ex);
                throw (ex);
            }
        }

        public async Task UpdateAsync(GetRestaurantResponseModel newRestaurant)
        {


            if (string.IsNullOrWhiteSpace(newRestaurant.RestaurantName))
            {
                var ex =  new ArgumentNullException("Restaurant cannot be empty");
                _logger.LogError("You are passing a Restaurant Name that is empty", ex);
                throw ex;
            }

            var result = await GetbyNameAsync(newRestaurant.RestaurantName);

            if (result == null)
            {
                var ex =  new Exception($"Restaurant with name {newRestaurant.RestaurantName} doesn't exist in the database, please create a new restaurant instead");
                _logger.LogError("You are trying to update a Restaurant that doesn't exist in the database", ex);
                throw ex;
            }

            try
            {
                var apiResponse = await Policy
                    .Handle<FlurlHttpException>(IsTransientErrors)
                    .WaitAndRetryAsync(2, retryAttempt =>
                    {
                        var nextAttemptTime = TimeSpan.FromSeconds(_random.Next(1, retryAttempt));

                        Console.WriteLine($"Retry attempt {retryAttempt} to make request. Next try on {nextAttemptTime.TotalSeconds} seconds.");

                        return nextAttemptTime;
                    })
                    .ExecuteAsync(() => _apiEndpoint
                        .AppendPathSegment(_classPath + "/Upsert/")
                        .WithHeader("accept", "application/json")
                        .PutJsonAsync(newRestaurant)
                    );


            }
            catch (FlurlHttpException ex)
            {
                _logger.LogError("You are getting a Flurl error, there is something wrong with your API call", ex);
                throw (ex);
            }
        }
    }
}
