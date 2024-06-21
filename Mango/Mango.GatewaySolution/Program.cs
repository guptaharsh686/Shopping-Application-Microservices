using Mango.GatewaySolution.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.AddAppAuthentication();
if (builder.Environment.ToString().ToLower().Equals("Production"))
{
    builder.Configuration.AddJsonFile("ocelot.Production.json", optional: false, reloadOnChange: true);

}
else
{
    //to use seperate ocelot json file
    builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
}

builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();
app.MapGet("/", () => "Hello World!");


app.UseOcelot().GetAwaiter().GetResult();

app.Run();
