var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddCors(options => {
    options.AddPolicy("AllowCors", builder =>
    {
        builder.WithOrigins("https://localhost:44300")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseHttpsRedirection();
app.UseCors("AllowCors");
app.MapControllers();

app.Run();