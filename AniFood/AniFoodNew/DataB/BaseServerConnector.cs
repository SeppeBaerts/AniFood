using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AniFoodNew.DataB
{
    public class BaseServerConnector
    {
        internal const string LoginTokenLocation = "LoginKey";
        /// <summary>
        /// THIS KEY CONTAINS A "/" AT THE END 
        /// MAKE SURE TO NOT ADD A "/" AT THE BEGINNING OF SPECIFIC LINK
        /// </summary>
        internal const string BaseApiLink = "https://82z9wcw5-7027.euw.devtunnels.ms/";
        internal protected readonly HttpClient _client;
        internal static bool HasUser => Preferences.Get(LoginTokenLocation, null) != null;
        public BaseServerConnector()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Preferences.Get(LoginTokenLocation, ""));
        }
        public void ChangeAuthorisation(AuthenticationHeaderValue value)
        {
            _client.DefaultRequestHeaders.Authorization = value;
        }
        public void ClearClient()
        {
            _client.DefaultRequestHeaders.Authorization = null;
        }
    }
}
