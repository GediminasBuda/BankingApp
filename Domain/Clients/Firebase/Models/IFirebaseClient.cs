using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Clients.Firebase.Models
{
    public interface IFirebaseClient
    {
        Task<FirebaseSignUpResponse> SignUpAsync(string email, string password);
        Task<FirebaseSignInResponse> SignInAsync(string email, string password);

    }
}
