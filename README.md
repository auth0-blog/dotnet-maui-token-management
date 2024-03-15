This repository contains a basic .NET MAUI application and an ASP.NET Core Web API. Both applications are integrated with Auth0 and the .NET MAUI application calls a protected API endpoint.

Check out the article [Call a Protected API from a .NET MAUI Application](https://auth0.com/blog/call-protected-api-from-dotnet-maui-application/) for the implementation details.

# Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) installed on your machine
- The required assets needed for your target(s) platform(s) as described [here](https://docs.microsoft.com/en-us/dotnet/maui/get-started/first-app)
- Visual Studio 2022 for Windows or Visual Studio 2022 for Mac (optional)

# To run this application

1. Clone the repo with the following command:

   ```bash
   git clone https://github.com/auth0-blog/dotnet-maui-auth0-call-api.git
   ```

2. Move to the `api_aspnet-core_csharp_hello-world` folder.

3. Follow the instructions in the [README](api_aspnet-core_csharp_hello-world/README.md) document to set up the ASP.NET Core Web API.

4. Move to the `dotnet-maui-auth0-app` folder.

5. Follow the instructions in the [README](dotnet-maui-auth0-app/README.md) document to set up the .NET MAUI application.
