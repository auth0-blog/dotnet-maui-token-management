using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using IdentityModel.Client;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IdentityModel.OidcClient.Results;

namespace MauiAuth0App.Auth0;

public class Auth0Client
{
  private readonly OidcClient oidcClient;
  private string audience;

  public Auth0Client(Auth0ClientOptions options)
  {
    oidcClient = new OidcClient(new OidcClientOptions
    {
      Authority = $"https://{options.Domain}",
      ClientId = options.ClientId,
      Scope = options.Scope,
      RedirectUri = options.RedirectUri,
      Browser = options.Browser
    });

    audience = options.Audience;
  }

  public IdentityModel.OidcClient.Browser.IBrowser Browser
  {
    get
    {
      return oidcClient.Options.Browser;
    }
    set
    {
      oidcClient.Options.Browser = value;
    }
  }

  public async Task<LoginResult> LoginAsync()
  {
    LoginRequest loginRequest = null;

    if (!string.IsNullOrEmpty(audience))
    {
      loginRequest = new LoginRequest
        {
          FrontChannelExtraParameters = new Parameters(new Dictionary<string, string>()
            {
              {"audience", audience}
            })
      };
    }

    var loginResult = await oidcClient.LoginAsync(loginRequest);

    if (!loginResult.IsError) 
    {
      await SecureStorage.Default.SetAsync("access_token", loginResult.AccessToken);
      await SecureStorage.Default.SetAsync("id_token", loginResult.IdentityToken);

      if (loginResult.RefreshToken != null)
      {
        await SecureStorage.Default.SetAsync("refresh_token", loginResult.RefreshToken);        
      }
    }

    return loginResult;
  }

  public async Task<BrowserResult> LogoutAsync()
  {
    var logoutParameters = new Dictionary<string, string>
    {
      {"client_id", oidcClient.Options.ClientId },
      {"returnTo", oidcClient.Options.RedirectUri }
    };

    var logoutRequest = new LogoutRequest();
    var endSessionUrl = new RequestUrl($"{oidcClient.Options.Authority}/v2/logout")
      .Create(new Parameters(logoutParameters));
    var browserOptions = new BrowserOptions(endSessionUrl, oidcClient.Options.RedirectUri)
    {
      Timeout = TimeSpan.FromSeconds(logoutRequest.BrowserTimeout),
      DisplayMode = logoutRequest.BrowserDisplayMode
    };

    var browserResult = await oidcClient.Options.Browser.InvokeAsync(browserOptions);

    SecureStorage.Default.RemoveAll();

    return browserResult;
  }

  public async Task<ClaimsPrincipal> GetAuthenticatedUser()
  {
    ClaimsPrincipal user = null;
    
    var idToken = await SecureStorage.Default.GetAsync("id_token");

    if (idToken != null)
    {
      var idTokenObject = new JwtSecurityToken(idToken);
      if (idTokenObject.ValidTo > DateTime.Now)
        user = new ClaimsPrincipal(new ClaimsIdentity(idTokenObject.Claims, "none", "name", "role"));
    }

    return user;
  }

  public async Task<RefreshTokenResult> RefreshTokenAsync(string refreshToken)
  {
    var refreshResult = await oidcClient.RefreshTokenAsync(refreshToken);

    if (!refreshResult.IsError)
    {
      await SecureStorage.Default.SetAsync("access_token", refreshResult.AccessToken);
      await SecureStorage.Default.SetAsync("id_token", refreshResult.IdentityToken);
      
      if (refreshResult.RefreshToken != null)
      {
        await SecureStorage.Default.SetAsync("refresh_token", refreshResult.RefreshToken);
      }
    }
    
    return refreshResult;
  }
}