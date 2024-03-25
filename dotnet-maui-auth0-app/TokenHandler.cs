using System.Net.Http.Headers;

namespace MauiAuth0App;

public class TokenHandler : DelegatingHandler
{
  private readonly Auth0Client auth0Client;

  public TokenHandler(Auth0Client _auth0Client)
  {
    auth0Client= _auth0Client;
  }
  
  protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
  {
    var accessToken = await SecureStorage.Default.GetAsync("access_token");
    var refreshToken = await SecureStorage.Default.GetAsync("refresh_token");
    
    request.Headers.Authorization =
        new AuthenticationHeaderValue("Bearer", accessToken);

    var responseMessage = await base.SendAsync(request, cancellationToken);

    if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized 
        && refreshToken != null)
    {
      var refreshResult = await RefreshTokensAsync(refreshToken);
      
      if (!refreshResult.IsError)
      {
        request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", refreshResult.AccessToken);

        responseMessage = await base.SendAsync(request, cancellationToken);
      }
    }

    return responseMessage;
  }

  private async Task<RefreshTokenResult> RefreshTokensAsync(string refreshToken)
  {
    var refreshResult = await auth0Client.RefreshTokenAsync(refreshToken);

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