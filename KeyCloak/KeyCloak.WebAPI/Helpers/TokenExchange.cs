using Newtonsoft.Json;

namespace KeyCloak.WebAPI.Helpers;

public class TokenExchange(IConfiguration configuration)
{
    static readonly HttpClient client = new HttpClient();


    public async Task<string> GetRefreshTokenAsync(string refreshToken)
    {
        /*
            Get refresh token
            Uses the settings injected from startup to read the configuration
        */
        try
        {
            var url = configuration["Keycloak:TokenExchange"];
            //Important, the grant types for token exchange, must be set to this!
            var grant_type = "urn:ietf:params:oauth:grant-type:token-exchange" ?? "";
            var client_id = configuration["Keycloak:ClientId"] ?? "";
            var client_secret = configuration["Keycloak:ClientSecret"] ?? "";
            var audience = configuration["Keycloak:Audience"] ?? "";
            var token = refreshToken;

            var form = new Dictionary<string, string>
            {
                {"grant_type", grant_type},
                {"client_id", client_id},
                {"client_secret", client_secret},
                {"audience", audience},
                {"subject_token", token }
            };
            var tokenResponse = await client.PostAsync(url, new FormUrlEncodedContent(form));
            var jsonContent = await tokenResponse.Content.ReadAsStringAsync();
            var tok = JsonConvert.DeserializeObject<Token>(jsonContent);

            return tok!.AccessToken;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public async Task<string> GetTokenExchangeAsync(string accessToken)
    {
        /*
         * Get exchange token
         * ses the settings injected from startup to read the configuration
         */
        try
        {
            string url = configuration["Keycloak:TokenExchange"] ?? "";
            //Important, the grant types for token exchange, must be set to this!
            string grant_type = "urn:ietf:params:oauth:grant-type:token-exchange" ?? "";
            string client_id = configuration["Keycloak:ClientId"] ?? "";
            string client_secret = configuration["Keycloak:ClientSecret"] ?? "";
            string audience = configuration["Keycloak:Audience"] ?? "";
            string token = accessToken;

            var form = new Dictionary<string, string>
                {
                    {"grant_type", grant_type},
                    {"client_id", client_id},
                    {"client_secret", client_secret},
                    {"audience", audience},
                    {"subject_token", token }
                };

            HttpResponseMessage tokenResponse = await client.PostAsync(url, new FormUrlEncodedContent(form));
            var jsonContent = await tokenResponse.Content.ReadAsStringAsync();
            var tok = JsonConvert.DeserializeObject<Token>(jsonContent);
            return tok!.AccessToken;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}

internal class Token
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; } = string.Empty;

    [JsonProperty("token_type")]
    public string TokenType { get; set; } = string.Empty;

    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; } = 0;

    [JsonProperty("refresh_token")]
    public string RefreshToken { get; set; } = string.Empty;
}
