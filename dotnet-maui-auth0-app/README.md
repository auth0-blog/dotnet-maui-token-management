This repository contains a basic .NET MAUI application integrated with Auth0 authentication. The application uses refresh tokens and `SecureStorage` to store the tokens.

Check out the article [Managing Tokens in .NET MAUI](#) for the implementation details.

# Requirements

- [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0) installed on your machine
- The required assets needed for your target(s) platform(s) as described [here](https://docs.microsoft.com/en-us/dotnet/maui/get-started/first-app)
- For Mac, iOS, and Android platforms you need additional settings to enable `SecureStorage`. Please, check [this document](https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/storage/secure-storage#get-started) to learn more.
- Visual Studio 2022 for Windows 17.3  or Visual Studio 2022 for Mac 17.4 (optional)

# To run this application

1. Open the `MauiProgram.cs` file and replace the `<YOUR_AUTH0_DOMAIN>` and `<YOUR_CLIENT_ID>` placeholders with your Auth0 domain and client id respectively (see [Register with Auth0](https://auth0.com/blog/add-authentication-to-dotnet-maui-apps-with-auth0/#Register-with-Auth0) for more details).

2. Run the application with Visual Studio 2022 or use one of the following commands based on your target platform:

   ```bash
   # macOS target platform
   dotnet build -t:Run -f net7.0-maccatalyst
   
   # Android target platform
   dotnet build -t:Run -f net7.0-android
   
   # iOS target platform
   dotnet build -t:Run -f net7.0-ios
   
   # Windows target platform (⚠️ Currently not working! ⚠️)
   dotnet build -t:Run -f net7.0-windows10.0.19041.0
   ```


