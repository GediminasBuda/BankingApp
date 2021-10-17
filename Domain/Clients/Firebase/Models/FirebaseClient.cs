using Domain.Clients.Firebase.Models.RequestModels;
using Domain.Clients.Firebase.Models.ResponseModels;
using Domain.Clients.Firebase.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Clients.Firebase.Models
{
    public class FirebaseClient : IFirebaseClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress;
        private readonly FirebaseOptions _firebaseOptions;
        public FirebaseClient(HttpClient httpClient, IConfiguration configuration, IOptions<FirebaseOptions> firebaseOptions)
        {
            _baseAddress = configuration.GetSection("FirebaseOptions:BaseAddress").Value;
            _httpClient = httpClient;
            _firebaseOptions = firebaseOptions.Value;
        }
        public async Task<FirebaseSignInResponse> SignInAsync(string email, string password)
        {
            var url = $"{_firebaseOptions.BaseAddress}/accounts:signInWithPassword?key={_firebaseOptions.ApiKey}";

            var request = new FirebaseSignInRequest
            {
                Email = email,
                Password = password,

            };
            var response = await _httpClient.PostAsJsonAsync(url, request);

            if (!response.IsSuccessStatusCode)
            {
                var newError = await response.Content.ReadFromJsonAsync<ErrorResponseModel>();
                throw new BadHttpRequestException($"{newError.Error.Message}", newError.Error.Code);
            }

            return await response.Content.ReadFromJsonAsync<FirebaseSignInResponse>();
        }

        public async Task<FirebaseSignUpResponse> SignUpAsync(string email, string password)
        {
            var url = $"{_firebaseOptions.BaseAddress}/accounts:signUp?key={_firebaseOptions.ApiKey}";

            var request = new FirebaseSignUpRequest
            {
                Email = email,
                Password = password
            };

            var response = await _httpClient.PostAsJsonAsync(url, request);

            if (!response.IsSuccessStatusCode)
            {
                var newError = await response.Content.ReadFromJsonAsync<ErrorResponseModel>();
                throw new BadHttpRequestException($"{newError.Error.Message}", newError.Error.Code);
            }
            return await response.Content.ReadFromJsonAsync<FirebaseSignUpResponse>();
        }
    }
}
