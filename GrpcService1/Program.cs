using GrpcService1;

public class Program
{

    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        _ = ActivatorUtilities.GetServiceOrCreateInstance<Service>(host.Services);

        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
        .UseDefaultServiceProvider((context, options) =>
        {
            options.ValidateScopes = context.HostingEnvironment.IsDevelopment(); ;
            options.ValidateScopes = true;
        });
}