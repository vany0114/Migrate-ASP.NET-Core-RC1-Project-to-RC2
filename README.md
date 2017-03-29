# Migrate ASP.NET Core RC1 Project to RC2

About one year and a half ago I was exploring the new [Asp.net Core](https://www.asp.net/core) features, it had very cool and amazing stuff, but it was unstable as well, off course, it was a beta version. When you downloaded packages through different dotnet versions or even package versions, the changes were big ones, I mean, renamed namespaces, classes or methods didn't exist anymore, the methods sign were different, anyway, was very annoying deal with these stuff, because it was a framework in evolution process. So I just decided to leave the framework get mature. Today, a little late (after two release versions) comparing RC1 and RC2 versions I realize there are a lot of changes, so is why I decided migrate my old Asp.net Core project to the new one and I wanna show you the things what I faced doing that.

## Prerequisites and Installation Requirements
+ If you got Visual Studio 2015 you must install [.Net Core](https://www.microsoft.com/net/core) (not required for Visual Studio 2017, is already included it)


## Instructions
1. Clone this repository.
2. Compile it.
3. Execute the ***ParkingLot.Services*** project. You can use `dotnet run` command.
4. Execute the ***ParkingLot.Client*** project.

## Understanding the Code
### Project.json
#### Multiple framework versions and TFM (Target Framework Monikers)

The *frameworks* section's structure is slightly different:
* RC1
```javascript
 "frameworks": {
    "dnx451": {
    },
    "dnxcore50": {
    }
  }
```
* RC2
```javascript
  "frameworks": {
    "netcoreapp1.0": {
      "imports": [
        "dotnet5.6",
        "portable-net45+win8"
      ]
    }
  }
```
This means my application runs over .Net Core 1.0 but it uses libraries/packages from another framework versions with respect to target Core platform version (netcoreapp1.0)
You can read more about this topic on this [Microsoft documentation](https://blogs.msdn.microsoft.com/cesardelatorre/2016/06/28/running-net-core-apps-on-multiple-frameworks-and-what-the-target-framework-monikers-tfms-are-about/).

>If using “imports” to reference the traditional .NET Framework, there are many risks when targeting two frameworks at the same time from the same app, so that should be avoided.
>
>At the end of the day, “imports” is smoothening the transition from other preview frameworks to netstandard1.x and .NET Core.

Another important difference in *project.json* is the ***command*** section, it's no longer available, in its place is ***tools*** section. The way commands are registered has changed in RC2, due to DNX being replaced by .NET CLI. Commands are now registered in a ***tools*** section.

* RC1:
```javascript
"commands": {
    "web": "Microsoft.AspNet.Hosting --config hosting.ini",
    "ef": "EntityFramework.Commands"
  }
```
* RC2
```javascript
"tools": {
    "Microsoft.EntityFrameworkCore.Tools": "1.0.0-preview2-final"
  }
```
In the other hand if you want to use the Entity Fraework commands into the Package Manager Console in Visual Studio, you must install PowerShell 5. (This is a temporary requirement that will be removed in the next release)

By the way, the Entity Framework migrations are also different, mostly in .Net Core libraries, now you can't execute migrations commands directly on this ones, instead you need the next workaround: 
>You need indicate an startup project that will be executable, a console or web project, for example. You can [check this out](https://github.com/aspnet/EntityFramework/issues/5320) about this issue.

Add migration example:

`dotnet ef --project ../ParkingLot.Data --startup-project . migrations add Initial`

Update database example:

`dotnet ef --project ../ParkingLot.Data --startup-project . database update`

I executed these commands from *ParkingLot.Services* (an Asp.Net Web API project) as it shows in the image bellow:

[Ef-command-example]:https://github.com/vany0114/Migrate-ASP.NET-Core-RC1-Project-to-RC2/blob/master/images/ef_migrations.png "Ef-command-example"
![Ef-command-example][Ef-command-example]

### Package Names and Versions

There was a lot of changes about packages and namespaces, let's take a look some of this ones:

| RC1 Package   | RC2 Equivalent|
| ------------- |:-------------:|
| EntityFramework.MicrosoftSqlServer 7.0.0-rc1-final      | Microsoft.EntityFrameworkCore.SqlServer 1.0.0-rc2-final       |
| EntityFramework.InMemory 7.0.0-rc1-final                | Microsoft.EntityFrameworkCore.InMemory 1.0.0-rc2-final        |
| EntityFramework.Commands 7.0.0-rc1-final                | Microsoft.EntityFrameworkCore.Tools 1.0.0-preview1-final      |
|EntityFramework.MicrosoftSqlServer.Design 7.0.0-rc1-final| Microsoft.EntityFrameworkCore.SqlServer.Design 1.0.0-rc2-final|

As you can see the change is about naming convention (in EF case), the namespaces it before was ***Microsoft.Data.Entity***, now is ***Microsoft.EntityFrameworkCore***

Let's take a look the changes into Asp.Net Web projects:
* RC1:
```javascript
"dependencies": {
        "Microsoft.AspNet.Server.IIS": "1.0.0-beta6",
        "Microsoft.AspNet.Server.WebListener": "1.0.0-beta6",
        "Microsoft.AspNet.Mvc": "6.0.0-beta6"
    }
```  
* RC2:
```javascript
  "dependencies": {
    "Microsoft.NETCore.App": {
      "version": "1.0.1",
      "type": "platform"
    },    
    "Microsoft.AspNetCore.Mvc": "1.0.1",    
    "Microsoft.AspNetCore.Server.IISIntegration": "1.0.0",
    "Microsoft.AspNetCore.Server.Kestrel": "1.0.1",
    "Microsoft.AspNetCore.StaticFiles": "1.0.0",
    "Microsoft.Extensions.Configuration.EnvironmentVariables": "1.0.0",
    "Microsoft.Extensions.Configuration.Json": "1.0.0",
    "Microsoft.Extensions.Logging": "1.0.0",
    "Microsoft.Extensions.Logging.Console": "1.0.0",
    "Microsoft.Extensions.Logging.Debug": "1.0.0",
    "Microsoft.Extensions.Options.ConfigurationExtensions": "1.0.0",
    "Microsoft.VisualStudio.Web.BrowserLink.Loader": "14.0.0"
  }
``` 
As you can see RC2 is even more modular than RC1. That's so good!
>Notice there is a naming convention as well, ***AspNetCore*** instead ***AspNet***

### Code changes
These are some changes what I faced when I was migrating the project:

#### Controllers

* RC1:
```cs
  return HttpNotFound();
  return HttpBadRequest();
  Context.Response.StatusCode = 400;
  return new HttpStatusCodeResult(204);
``` 

* RC2:
```cs
  return NotFound();
  return BadRequest();
  Response.StatusCode = 400;
  return new StatusCodeResult(204);
``` 

#### Entity framework context

* RC1:
```cs
  public class ParkingLotContext : DbContext
    {
        private string _connectionString;

        public ParkingLotContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public virtual DbSet<ParkingLot> ParkingLot { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
``` 

* RC2:
```cs
  public class ParkingLotContext : DbContext
    {
        public ParkingLotContext(DbContextOptions<ParkingLotContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ParkingLot> ParkingLot { get; set; }
    }
``` 
>You need to add a constructor, to your derived context, that takes context options and passes them to the base constructor. This is needed because Microsoft removed some of the scary magic that snuck them in behind the scenes.

#### Startup

##### Constructor

* RC1:
```cs
  public Startup(IApplicationEnvironment env)
  {
      // adds json file to environment.
      IConfigurationBuilder configurationBuilder = new ConfigurationBuilder(env.ApplicationBasePath)
	     .AddJsonFile("config.json")
	     .AddEnvironmentVariables();

      configuration = configurationBuilder.Build();
  }
``` 

* RC2:
```cs
  public Startup(IHostingEnvironment env)
  {
      // adds json file to environment.
      IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
         .SetBasePath(env.ContentRootPath)
         .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
         .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
         .AddEnvironmentVariables();

      Configuration = configurationBuilder.Build();
  }
``` 
You can see some significant changes, for instance the interface name, the *SetBasePath* method and a very useful and cool property ***EnvironmentName***, that allows you have different settings between environments. (Like web.config transformations in Asp.Net)

##### ConfigureServices method

* RC1:
```cs
  public void ConfigureServices(IServiceCollection services)
  {
    // get connection string from configuration json file.
    var connectionString = configuration.Get("Data:DefaultConnection:ConnectionString");

    // inject context.
    services.AddEntityFramework()
      .AddSqlServer()
      .AddDbContext<ParkingLotContext>();

    // dependency injection
    services.AddInstance(typeof(string), connectionString);
    services.AddScoped<IRepository<Entities.ParkingLot>, Repository<Entities.ParkingLot>>();
    services.AddScoped<IParkingLotFacade, ParkingLotFacade>();

    // adds all of the dependencies that MVC 6 requires
    services.AddMvc();

    // Enabled cors.
    services.AddCors();
    var policy = new CorsPolicy();
    policy.Headers.Add("*");
    policy.Methods.Add("*");
    policy.Origins.Add("*");
    policy.SupportsCredentials = true;
    services.ConfigureCors(x => x.AddPolicy("defaultPolicy", policy));
  }
``` 
* RC2:
```cs
  public void ConfigureServices(IServiceCollection services)
  {
    // get connection string from configuration json file.
    var connectionString = Configuration.GetConnectionString("DefaultConnection");

    // inject context.
    services.AddDbContext<ParkingLotContext>(options =>
    options.UseSqlServer(connectionString));

    // dependency injection
    services.AddScoped<IRepository<Entities.ParkingLot>, Repository<Entities.ParkingLot>>();
    services.AddScoped<IParkingLotFacade, ParkingLotFacade>();

    // adds all of the dependencies that MVC 6 requires
    services.AddMvc();

    // Enabled cors. (don't do that in production environment, specify only trust origins)
    var policy = new CorsPolicy();
    policy.Headers.Add("*");
    policy.Methods.Add("*");
    policy.Origins.Add("*");
    policy.SupportsCredentials = true;

    services.AddCors(x => x.AddPolicy("defaultPolicy", policy));
  }
``` 
The first visible change is the way to get the connection string, RC2 has a method to get this one called ***GetConnectionString*** (also there is a change into appsettings.json that it will show bellow).

Another important change is the way to inject the Entity framework context, in RC1, you had to add Entity Framework services to the application service provider. In RC1 you passe an IServiceProvider to the context, this has now moved to DbContextOptions.

Finally, the ***ConfigurCors*** method name was changed by ***AddCors***.

As I said earlier, this the change about connection string into *appsettings.son* file:

* RC1:
```javascript
"Data": {
    "DefaultConnection": {
      "ConnectionString": "[your connection string];App=EntityFramework"
    }
  }
```

* RC2:
```javascript
 "ConnectionStrings": {
    "DefaultConnection": "[your connection string];App=EntityFramework"
  }
```

##### Configure method

* RC1:
```cs
  public void Configure(IApplicationBuilder app, IApplicationEnvironment env)
        {
            //Use the new policy globally
            app.UseCors("defaultPolicy");

            // adds MVC 6 to the pipeline
            app.UseMvc();
        }
``` 
* RC2:
```cs
  public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //Use the new policy globally
            app.UseCors("defaultPolicy");

            // adds MVC 6 to the pipeline
            app.UseMvc();
        }
``` 
The *Configure* method only has a signature change.

> I had troubles serving the static files in the Asp.Net Mvc project with html and js files in order to works correctly AngularJS implementation, so it was necessary the next configuration into the Configure Method:
```cs
	app.UseDefaultFiles();
	app.UseStaticFiles();
``` 

##### Sel-fhosting
* RC2:
```cs
  public class Program
  {
      public static void Main(string[] args)
      {
          var host = new WebHostBuilder()
              .UseKestrel()
              .UseContentRoot(Directory.GetCurrentDirectory())
              .UseIISIntegration()
              .UseStartup<Startup>()
              .Build();

          host.Run();
      }
  }
``` 
This is a very basic configurations to host the application, but you will be able to manage more advanced settings, check out [this documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/hosting).

## Bonus code!
Because Visual Studio has an integration with NPM I took advantage of  Task Runner Explorer in order to run NPM Scripts Tasks. Visual Studio manage the dependencies from ***package.json*** file. (You can learn more about this topic on my [Automation-with-Grunt-BrowserSync](https://github.com/vany0114/Automation-with-Grunt-BrowserSync) repository)

```javascript
{
  "version": "1.0.0",
  "private": true,
  "devDependencies": {
    "grunt": "0.4.5",  
    "grunt-contrib-uglify": "0.9.1",
    "grunt-contrib-watch": "0.6.1",
    "grunt-contrib-concat": "0.5.1",
    "grunt-contrib-cssmin": "0.13.0",
    "grunt-contrib-less": "1.0.1"
  }
}
```
So I had some task configured into the ***gruntfile***

```javascript
module.exports = function (grunt) {
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-contrib-cssmin');
    grunt.loadNpmTasks('grunt-contrib-less');

    grunt.initConfig({
        concat: {
            dist: {
                files: {
                    'wwwroot/js/libs.js': ['Scripts/Libs/*.js']
                }
            }
        },
        uglify: {
            my_target: {
                files: {
                    'wwwroot/js/app.js': ['Scripts/ParkingLot/module.js', 'Scripts/ParkingLot/**/*.js'],
                    'wwwroot/js/libs.js': ['wwwroot/js/libs.js']
                }
            },
            options: {
                sourceMap: true,
                sourceMapIncludeSources: true
            }
        },
        cssmin: {
            target: {
                files: [{
                    expand: true,
                    src: ['css/*.css', '!css/*.min.css'],
                    dest: 'wwwroot',
                    ext: '.min.css'
                }]
            }
        },
        less: {
            development: {
                options: {
                    paths: ["css"]
                },
                files: {
                    "wwwroot/css/site.css": "css/site.less"
                }
            }
        },
        watch: {
            scripts: {
                files: ['Scripts/**/*.js'],
                tasks: ['uglify']
            }
        }
    });
    grunt.registerTask('default', ['concat', 'uglify', 'less', 'cssmin', 'watch']);
};
```
The good news is with RC2 those tasks are easier thanks to ***"Bundling and minification"*** that comes built-in in Visual Studio. You can [check this out](https://docs.microsoft.com/en-us/aspnet/core/client-side/bundling-and-minification) to learn more about this awesome option.

So that's all, this was a brief resume about some important changes between Asp.Net core RC1 and RC2, at least the ones I faced up.
