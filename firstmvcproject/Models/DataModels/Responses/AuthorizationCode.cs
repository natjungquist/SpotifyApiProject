﻿using System.Diagnostics;

namespace firstmvcproject.Models.DataModels.Responses
{
    /// <summary>
    ///     The recipient of the response from RequestAuthorizationCodeTokens in SpotifyAccountService
    /// </summary>
    public class AuthorizationCode
    {
        public string access_token { get; set; }
        public string token_ype { get; set; }
        public string scope { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
    }
}
