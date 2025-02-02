## Migration to .net core

The steps that will be attempted are:

1. convert projects to .net standard 2.1
2. Isolate as much logic as possible away from System.web
  1. pull any classes that do not rely on system.web from the proj and into a .net 8 csproj

3. Try creating a test proj
4. create a ascx control thAt acts as a reverse proxy that delegates the rendering to a .net 8 web service.

- anything that cannot be replicated, let the webforms handle it

5. Webforms effectively becomes a View engine/layer

### csprojs referencing system.web
1. Rock.Lava
2. Rock.Lava.Shared, .Liquid and .Fluid

![Rock RMS](https://raw.githubusercontent.com/SparkDevNetwork/Rock/develop/Images/github-banner.png)

Rock RMS is an open source Relationship Management System (RMS) and Application
Framework for 501c3 organizations[^1]. While Rock specializes in serving the unique needs of churches it's
useful in a wide range of service industries. Rock is an ASP.NET 4.5 C# web application
that uses Entity Framework 6.0, jQuery, Bootstrap 3, and many other open source libraries.

Our main developer starting point site is [the wiki](https://github.com/SparkDevNetwork/Rock/wiki).

## Learn More

Jump over to our [Rock website](https://www.rockrms.com/) to find out more. Keep up to date by:

* [Reading our blog](https://community.rockrms.com/connect)
* [Following us on Twitter](https://www.twitter.com/therockrms)
* [Liking us on Facebook](https://www.facebook.com/therockrms)
* [Reading the community Q & A](https://community.rockrms.com/ask)
* [Subscribing to our newsletter](https://www.rockrms.com/Rock/Subscribe)

## License

Rock released under the [Rock Community License](https://www.rockrms.com/license).

## Crafted By

A community of developers led by the [Spark Development Network](https://www.sparkdevnetwork.com/).

## Installer Note

Normally the [Rock installer](https://www.rockrms.com/Download) generates a unique `PasswordKey`
`DataEncryptionKey` and MachineKey's `validationKey` and `decryptionKey`. So if you decide
to clone the repo and run it directly, you will need to handle that aspect yourself.

[^1]: [See our FAQ for details on our license](https://www.rockrms.com/faq)
