using Microsoft.Extensions.Logging;
using MauiAuth0App.Auth0;
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

    builder.Services.AddSingleton(new Auth0Client(new()
    {
      Domain = "<YOUR_AUTH0_DOMAIN>",
      ClientId = "<YOUR_CLIENT_ID>",
      Scope = "openid profile",
			Audience = "<YOUR_API_IDENTIFIER>",
#if WINDOWS
			RedirectUri = "http://localhost/callback"
#else
      RedirectUri = "myapp://callback"
#endif
    }));

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
