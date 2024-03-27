using KeyCloak.WebAPI.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<TokenExchange>();
// Add services to the container.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Authority = "https://localhost:8180/auth/realms/your-realm";
    options.Audience = "your-api-audience";
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = "https://localhost:8180/auth/realms/your-realm",
        ValidateAudience = true,
        ValidAudience = "your-api-audience",
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
})
.AddOpenIdConnect(options =>
{
    //Use default signin scheme
    options.SignInScheme = JwtBearerDefaults.AuthenticationScheme;
    //Keycloak server
    options.Authority = builder.Configuration.GetSection("Keycloak")["ServerRealm"];
    //Keycloak client ID
    options.ClientId = builder.Configuration.GetSection("Keycloak")["ClientId"];
    //Keycloak client secret
    options.ClientSecret = builder.Configuration.GetSection("Keycloak")["ClientSecret"];
    //Keycloak .wellknown config origin to fetch config
    options.MetadataAddress = builder.Configuration.GetSection("Keycloak")["Metadata"];
    //Require keycloak to use SSL
    options.RequireHttpsMetadata = true;
    options.GetClaimsFromUserInfoEndpoint = true;
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    //Save the token
    options.SaveTokens = true;
    //Token response type, will sometimes need to be changed to IdToken, depending on config.
    options.ResponseType = OpenIdConnectResponseType.Code;
    //SameSite is needed for Chrome/Firefox, as they will give http error 500 back, if not set to unspecified.
    options.NonceCookie.SameSite = SameSiteMode.Unspecified;
    options.CorrelationCookie.SameSite = SameSiteMode.Unspecified;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = "name",
        RoleClaimType = ClaimTypes.Role,

        ValidateIssuer = true,
        ValidateAudience = true
    };

});

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.Authority = "https://localhost:8180/auth/realms/your-realm";
//        options.Audience = "your-api-audience";
//        options.RequireHttpsMetadata = false;
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidIssuer = "https://localhost:8180/auth/realms/your-realm",
//            ValidateAudience = true,
//            ValidAudience = "your-api-audience",
//            ValidateLifetime = true,
//            ClockSkew = TimeSpan.Zero
//        };
//    });

builder.Services.AddAuthorization(options =>
{
    // Kullanýcýlarýn ve yöneticilerin eriþimini kontrol etmek için politikalarý oluþtur
    options.AddPolicy("users", policy =>
    {
        policy.RequireAssertion(context =>
            context.User.HasClaim(c => (c.Value == "user") || c.Value == "admin")
        );
    });

    options.AddPolicy("admins", policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, "admin");
    });

    options.AddPolicy("noaccess", policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, "noaccess");
    });
});



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
