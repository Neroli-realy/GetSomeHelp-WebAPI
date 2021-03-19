# Description 
Simple API for helping Old People to ask for help in dialy tasks Wrote in ASP.NET core 3.1 Using MVC Design Pattern.

# API-Diagram

![GetSomeHelp](https://user-images.githubusercontent.com/25514920/89131959-00096f00-d511-11ea-8030-2ecb7afc64ff.png)

# Functionalities
* Login
* Register
* AddTask
* get Tasks
* Accept task
* Mark Task As Finished
* Filter Tasks

# Used libraries
* [JWTBearer AspNetCore](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer)
* [EntityFramework](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/5.0.0-preview.7.20365.15)
* [AspNetCoreRateLimit](https://www.nuget.org/packages/AspNetCoreRateLimit/)

# Security
* Implemented Rate Limit to Limit User requests which can lead to DDos or Spam Tasks read more about it [here](https://en.wikipedia.org/wiki/Rate_limiting).

* Implemented JWT Token to Confirm Identity of the user and it's role Which prevents also [csrf](https://portswigger.net/web-security/csrf) attacks and [IDOR](https://portswigger.net/web-security/access-control/idor) Vulnerabilities.

# TO-DO
* Add Asynchronous Tasks to prevend Dead Lock
* Create Cross-Platform Application to use the API 


# Refferences
* [authentication-and-authorization-in-aspnet-web-api](https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/authentication-and-authorization-in-aspnet-web-api)
* [preventing-cross-site-request-forgery-csrf-attacks](https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/preventing-cross-site-request-forgery-csrf-attacks)
* [MVC](https://docs.microsoft.com/en-us/aspnet/mvc/)
* [ASPNet core Web-API](https://docs.microsoft.com/en-us/aspnet/web-api/)
* 
