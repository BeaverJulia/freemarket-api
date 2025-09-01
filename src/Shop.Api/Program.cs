using Shop.Api.Middleware;
using Shop.Application.Interfaces;
using Shop.Application.Services;
using Shop.Infrastructure.Interfaces;
using Shop.Infrastructure.Stores;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Shop Cart API", Version = "v1" });
});

builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IPricingService, PricingService>();
builder.Services.AddScoped<IDiscountService,DiscountService>();
builder.Services.AddScoped<IShippingService, ShippingService>();
builder.Services.AddScoped<IVatService, VatService>();

builder.Services.AddSingleton<ICatalogStore, InMemoryCatalogStore>();
builder.Services.AddSingleton<ICartStore, InMemoryCartStore>();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shop Cart API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();