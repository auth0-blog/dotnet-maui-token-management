﻿using Auth0.OidcClient;
using System.Net.Http.Headers;

namespace MauiAuth0App;

public partial class MainPage : ContentPage
{
	int count = 0;
	private readonly Auth0Client auth0Client;
  private HttpClient _httpClient;

  public MainPage(Auth0Client client, HttpClient httpClient)
	{
		InitializeComponent();
    auth0Client = client;
    _httpClient = httpClient;
  }

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}

  private async void OnLoginClicked(object sender, EventArgs e)
  {
    var loginResult = await auth0Client.LoginAsync(new { audience = "https://myapi.com"});

    if (!loginResult.IsError)
    {
      UsernameLbl.Text = loginResult.User.Identity.Name;
      UserPictureImg.Source = loginResult.User
        .Claims.FirstOrDefault(c => c.Type == "picture")?.Value;

      LoginView.IsVisible = false;
      HomeView.IsVisible = true;

      TokenHolder.AccessToken = loginResult.AccessToken;
    }
    else
    {
      await DisplayAlert("Error", loginResult.ErrorDescription, "OK");
    }
  }

  private async void OnLogoutClicked(object sender, EventArgs e)
  {
    var logoutResult = await auth0Client.LogoutAsync();

    HomeView.IsVisible = false;
    LoginView.IsVisible = true;
  }

  private async void OnApiCallClicked(object sender, EventArgs e)
  {
    try
    {
      HttpResponseMessage response = await _httpClient.GetAsync("api/messages/protected");
      {
        string content = await response.Content.ReadAsStringAsync();
        await DisplayAlert("Info", content, "OK");
      }
    }
    catch (Exception ex)
    {
      await DisplayAlert("Error", ex.Message, "OK");
    }
  }
}
