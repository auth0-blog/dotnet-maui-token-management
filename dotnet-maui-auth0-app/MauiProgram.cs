using Microsoft.Extensions.Logging;
using Auth0.OidcClient;
using Microsoft.Extensions.Http;

namespace MauiAuth0App;

public static class MauiProgram
{
  public static MauiApp CreateMauiApp()
  {
    var builder = MauiApp.CreateBuilder();
    builder
      .UseMauiApp<App>()
      .ConfigureFonts(fonts =>
      {
        fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
        fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
      });

#if DEBUG
    builder.Logging.AddDebug();
#endif

    builder.Services.AddSingleton<MainPage>();

    const string domain = "<YOUR_AUTH0_DOMAIN>";
    const string clientId = "<YOUR_CLIENT_ID>";

    builder.Services.AddSingleton(new Auth0Client(new()
    {
      Domain = domain,
      ClientId = clientId,
      RedirectUri = "myapp://callback/",
      PostLogoutRedirectUri = "myapp://callback/",
      Scope = "openid profile email offline_access"
    }));

    builder.Services.AddSingleton(new UserManager(domain, clientId));

    builder.Services.AddSingleton<TokenHandler>();
    builder.Services.AddHttpClient("DemoAPI",
        client => client.BaseAddress = new Uri("https://localhost:6061")
      ).AddHttpMessageHandler<TokenHandler>();
        builder.Services.AddTransient(
        sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("DemoAPI")
    );

    return builder.Build();
  }
}
