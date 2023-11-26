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

app.MapGet("/helloWorld/{id}", (string id) =>
{
    var output = 0;
    var result = int.TryParse(id, out output) ? Results.Ok($"Hello World! {id}") : Results.BadRequest($"Failed to bind parameter {id}");
    return result;
});

app.UseHttpsRedirection();

app.Run();