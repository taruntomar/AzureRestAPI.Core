using Newtonsoft.Json;
using RestSharp;
using Open.OAuthManager.Azure.Entities;
using Open.OAuthManager.AzureAD;
using Open.OAuthManager;
using Newtonsoft.Json.Linq;
using System;

namespace Open.AzureRestAPI.Core
{
  
    public class AzureRestAPI
    {

        #region propeties
        // this is the base address of rest api, for example: https://www.management.azure.com/
        public string BaseURL { get; set; }
        // this is the authenticator which will do all token receiving and storing work.
        public AuthenticatorV2 Authenticator { get; set; }
        public GrantType AuthenticationMode { get; set; }
        public DatabaseDealer.DatabaseManager DBDealer { get; set; }
        #endregion
        public AzureRestAPI()
        {
            Authenticator = new AuthenticatorV2();
        }
        #region protected methods

        protected RestResponse<R, E> RestRequest<R, E>(string endppint, string scope)
        {
            var resp = new RestResponse<R, E>();
            var response = RestRequest(endppint, scope);
            if (response.IsSucceeded)
            {
                resp.OAuthError = OAuthErrors.None;
                if (response.Result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    E error = JsonConvert.DeserializeObject<E>(response.Result.Content, Converter.Settings);
                    resp.Error = error;
                    resp.IsSucceeded = false;

                }
                else
                {
                    R result = JsonConvert.DeserializeObject<R>(response.Result.Content, Open.OAuthManager.Azure.Entities.Converter.Settings);
                    resp.Result = result;
                    resp.IsSucceeded = true;
                }
            }
            else
            {
                resp.OAuthError = response.OAuthError;
                resp.IsSucceeded = false;
            }

            return resp;
        }

        #endregion


        #region private methods
        private RestResponse<IRestResponse, OAuthError> RestRequest(string endpoint, string scope)
        {

            var resp = new RestResponse<IRestResponse, OAuthError>();
            var url = Authenticator.Config.Resource + endpoint;
            RestClient client = new RestClient(url);
            RestRequest request = new RestRequest(Method.GET);
            Authenticator.Config.Scope = scope;
            AzureADAuthRestResponse<AccessTokenClass, OAuthError> access_token;
            if (AuthenticationMode == GrantType.UserCredential)
            {
                access_token = Authenticator.GetAccessToken_UserCredential(DBDealer);
            }
            else
            {
                access_token = Authenticator.GetAccessToken_FromClientCredential();
            }
            if (access_token.OAuthError == OAuthErrors.None)
            {
                string token = access_token.Result.AccessToken;
                request.AddHeader("Authorization", "bearer " + token);
                IRestResponse response = client.Execute(request);
                resp.IsSucceeded = true;
                resp.OAuthError = OAuthErrors.None;
                resp.Result = response;
            }
            else
            {
                resp.IsSucceeded = false;
                resp.Error = access_token.Error;
                resp.OAuthError = access_token.OAuthError;
            }
            return resp;
        }
        private string CreateUrl(string resource, string api, bool addApiVersion = true)
        {
            return BaseURL + resource + (addApiVersion ? ("?api-version=" + api) : "");
        }
        private K Parse<K>(IRestResponse response)
        {
            K data_array;

            try
            {
                JObject jobj = JObject.Parse(response.Content);
                data_array = JsonConvert.DeserializeObject<K>(jobj.First.First.ToString());


            }
            catch (Exception ex)
            {
                return default(K);
            }

            return data_array;
        }
        #endregion
    }
}
