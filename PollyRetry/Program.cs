using Polly;
using Polly.Extensions.Http;
using PollyRetry.Clients;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ILogger logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                                                                    retryAttempt)));
}

builder.Services.AddHttpClient<PollyClient>(c =>
{
    c.BaseAddress = new Uri("https://localhost:7283");
}).AddPolicyHandler(GetRetryPolicy());
//.AddTransientHttpErrorPolicy(builder => builder.Or<TimeoutRejectedException>().WaitAndRetryAsync(
//    2,
//    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),

//    onRetry: (outcome, timespan, retryAttempt) =>
//    {

//        logger.LogWarning($"Delaying for {timespan.TotalSeconds} seconds, then making retry {retryAttempt}");
//    }
//));
//.AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(1));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
