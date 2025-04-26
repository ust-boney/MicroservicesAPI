using Mango.Services.ApiGateway.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOcelot();
builder.AddAppAuthentication();
var app = builder.Build();

app.UseOcelot();
app.MapGet("/", () => "Hello World!");

app.Run();
