﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Open.OAuthManager.Azure.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Open.AzureRestAPI.Core
{
    ///this is the response class
    ///this class can be used to return the response received from api call, by casting object received to ResultType and Errors


    public class RestResponse<R, E>
    {
        public R Result { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public OAuthErrors OAuthError { get; set; }

        public E Error { get; set; }

        public bool IsSucceeded { get; set; }

    }
}
