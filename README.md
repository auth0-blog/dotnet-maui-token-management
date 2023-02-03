This repository contains a basic .NET MAUI application and an ASP.NET Core Web API. Both applications are integrated with Auth0 and the .NET MAUI application calls a protected API endpoint.

The main goal of this repository is to shown how to use refresh tokens and `SecureStorage` to store tokens in a .NET MAUI application.

Check out the article [Managing Tokens in .NET MAUI](#) for the implementation details.

# Requirements

- [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0) installed on your machine
- The required assets needed for your target(s) platform(s) as described [here](https://docs.microsoft.com/en-us/dotnet/maui/get-started/first-app)
- For Mac, iOS, and Android platforms you need additional settings to enable `SecureStorage`. Please, check [this document](https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/storage/secure-storage#get-started) to learn more.
- Visual Studio 2022 for Windows 17.3 or Visual Studio 2022 for Mac 17.4 (optional)

# To run this application

1. Clone the repo with the following command:

   ```bash
   git clone https://github.com/auth0-blog/dotnet-maui-token-management.git
   ```

2. Move to the `api_aspnet-core_csharp_hello-world` folder.

3. Follow the instructions in the [README](api_aspnet-core_csharp_hello-world/README.md) document to set up the ASP.NET Core Web API.

4. Move to the `dotnet-maui-auth0-app` folder.

5. Follow the instructions in the [README](dotnet-maui-auth0-app/README.md) document to set up the .NET MAUI application.
