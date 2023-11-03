using Flurl;
using Flurl.Http;
using Futures.ApiContracts;
using Futures.Services.ServiceInterfaces;
using Microsoft.Extensions.Logging;
using Polly;
using System.Net;

namespace Futures.Services
{
    public class ZoneService : IZoneService
    {

        public readonly ILogger<ZoneService>   _logger;

        public ZoneService(ILogger<ZoneService> logger)
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
       
        // Setting up variables for the APi call
        private const string _classPath = "zone";
        private readonly Random _random = new();

        
        public async void DeleteByIdAsync(int zoneId)
        {
            if (zoneId < 0)
            {
                Exception ex = new ArgumentOutOfRangeException(nameof(zoneId), "Zone id cannot be 0 or less");
                _logger.LogError("Zone id cannot be 0 or less", ex);
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
                        .AppendPathSegment(_classPath + "/DeleteById/" + zoneId)
                        .WithHeader("accept", "application/json")
                        .GetJsonAsync<GetZoneResponseModel>()
                    );


            }
            catch (FlurlHttpException ex)
            {

                throw(ex);
            }
        }

        public async Task<IEnumerable<GetZoneResponseModel>> GetAllAsync()
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
                        .GetJsonAsync<IEnumerable<GetZoneResponseModel>>()
                    );

                return apiResponse;
            }
            catch (FlurlHttpException ex)
            {
                _logger.LogError("You are getting a Flurl error, there is something wrong with your API call", ex);
                throw (ex);
            }
        }

        public async Task<GetZoneResponseModel> GetbyIdAsync(int zoneId)
        {
            if (zoneId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(zoneId), "Zone id cannot be 0 or less");
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
                        .AppendPathSegment(_classPath + "/" + zoneId)
                        .WithHeader("accept", "application/json")
                        .GetJsonAsync<GetZoneResponseModel>()
                    );

                return apiResponse;
            }
            catch (FlurlHttpException ex)
            {

                throw (ex);
            }
        }

        public async void InsertNewAsync()
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
                        .AppendPathSegment(_classPath + "/Insert/")
                        .WithHeader("accept", "application/json")
                        .GetJsonAsync<GetZoneResponseModel>()
                    );


            }
            catch (FlurlHttpException ex)
            {

                throw (ex);
            }
        }

        public async void UpdateAsync(GetZoneResponseModel zone)
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
                        .AppendPathSegment(_classPath + "/Upsert/")
                        .WithHeader("accept", "application/json")
                        .GetJsonAsync<GetZoneResponseModel>()
                    );


            }
            catch (FlurlHttpException ex)
            {

                throw (ex);
            }
        }
    }
}
