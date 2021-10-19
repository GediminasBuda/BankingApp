using Contracts.Models.RequestModels;
using Contracts.Models.ResponseModels;
using Domain.Clients.Firebase.Models;
using Persistence.Models.WriteModels;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IFirebaseClient _firebaseClient;

        public AuthService(IFirebaseClient firebaseClient, IUserRepository userRepository)
        {
            _firebaseClient = firebaseClient;
            _userRepository = userRepository;
        }
        public async Task<SignUpResponse> SignUpAsync(SignUpRequest request)
        {
            var user = await _firebaseClient.SignUpAsync(request.Email, request.Password);
            var userSql = new UserWriteModel

            {
                UserId = Guid.NewGuid(),
                Username = request.Username,
                FirebaseId = user.FirebaseId,
                Email = user.Email,
                DateCreated = DateTime.Now

            };
            await _userRepository.SaveAsync(userSql);

            return new SignUpResponse
            {
                UserId = userSql.UserId,
                IdToken = user.IdToken,
                Username = userSql.Username,
                Email = userSql.Email,
                DateCreated = userSql.DateCreated
            };

        }
        public async Task<SignInResponse> SignInAsync(SignInRequest request)
        {
            var firebaseSignInResponse = await _firebaseClient.SignInAsync(request.Email, request.Password);

            var user = await _userRepository.GetByIdAsync(firebaseSignInResponse.FirebaseId);

            return new SignInResponse
            {
                Username = user.Username,
                Email = user.Email,
                IdToken = firebaseSignInResponse.IdToken
            };

        }
    }
}
