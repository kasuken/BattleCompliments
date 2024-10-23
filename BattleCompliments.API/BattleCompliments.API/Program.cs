var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var compliments = new List<string>
{
    "You're amazing just the way you are!",
    "You light up the room!",
    "You're like a ray of sunshine on a cloudy day!",
    "You're an awesome friend!",
    "You have the best laugh!",
    "You're a gift to those around you!",
    "You're a smart cookie!",
    "You are awesome!"
};

// GET /compliment/random - Returns a random compliment
app.MapGet("/compliment/random", () =>
{
    var random = new Random();
    var compliment = compliments[random.Next(compliments.Count)];
    return Results.Ok(new { compliment });
})
    .WithName("ComplimentRandom")
    .WithOpenApi();

// POST /compliment - Submit a new compliment
app.MapPost("/compliment", (string newCompliment) =>
{
    compliments.Add(newCompliment);
    return Results.Ok(new { message = "Thanks for spreading the positivity!" });
})
    .WithName("AddCompliment")
    .WithOpenApi();

// GET /compliment/{name} - Get a personalized compliment
app.MapGet("/compliment/{name}", (string name) =>
{
    var random = new Random();
    var compliment = compliments[random.Next(compliments.Count)];
    return Results.Ok(new { compliment = $"{name}, {compliment}" });
})
    .WithName("GetCompliment")
    .WithOpenApi();

// POST /compliment/battle - Compliment battle with names in request body
app.MapPost("/compliment/battle", (ComplimentBattleRequest battleRequest) =>
{
    var random = new Random();
    var compliment1 = compliments[random.Next(compliments.Count)];
    var compliment2 = compliments[random.Next(compliments.Count)];

    var winner = random.Next(2) == 0 ? battleRequest.Name1 : battleRequest.Name2;

    return Results.Ok(new
    {
        name1 = $"{battleRequest.Name1}, {compliment1}",
        name2 = $"{battleRequest.Name2}, {compliment2}",
        winner = $"The winner is {winner} for being extra awesome today!"
    });
})
    .WithName("Battle")
    .WithOpenApi();

app.Run();

public class ComplimentBattleRequest
{
    public string Name1 { get; set; }
    public string Name2 { get; set; }
}