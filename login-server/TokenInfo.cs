﻿// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using apiTokenInfo;
//
//    var apiTokenInfo = ApiTokenInfo.FromJson(jsonString);

namespace apiTokenInfo
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class ApiTokenInfo
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }

        [JsonProperty("refresh_expires_in")]
        public long RefreshExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("not-before-policy")]
        public long NotBeforePolicy { get; set; }

        [JsonProperty("session_state")]
        public Guid SessionState { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }
    }

    public partial class ApiTokenInfo
    {
        public static ApiTokenInfo FromJson(string json) => JsonConvert.DeserializeObject<ApiTokenInfo>(json, apiTokenInfo.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this ApiTokenInfo self) => JsonConvert.SerializeObject(self, apiTokenInfo.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
