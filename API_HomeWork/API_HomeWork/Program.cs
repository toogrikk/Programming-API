using API_HomeWork.Business_Logic;

namespace API_HomeWork
    
{
    public class Program
    {
        public record CreateInstrumentDto(string Name, decimal Price);

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSingleton<InstrumentRepository>(); //   ƒодаЇмо DI

            // Add services to the container.
            builder.Services.AddAuthorization();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            // GET
            app.MapGet("/instruments", (InstrumentRepository repo) =>
            {
                return Results.Ok(repo.GetAll);
            });

            // POST
            app.MapPost("/instruments", (CreateInstrumentDto dto, InstrumentRepository repo) =>
            {
                try
                {
                    var instrument = new MusicalInstrument(dto.Name, dto.Price);
                    repo.Add(instrument);
                    return Results.Created($"/instruments/{instrument.Id}", instrument);
                }
                catch (ArgumentException ex)
                {
                    // якщо передали порожнЇ ≥м'€ або в≥д'Їмну ц≥ну
                    return Results.BadRequest(new { error = ex.Message});
                }
            });

            // DELETE by name
            app.MapDelete("/instruments/by-name/{name}", (string name, InstrumentRepository repo) =>
            {
                bool removed = repo.RemoveName(name);

                // «ручний запис, зам≥сть if el
                return removed
                    ? Results.Ok(new { message = $"≤нструмент '{name}' усп≥шно видалено." }) 
                    : Results.NotFound(new { message = $"≤нструмент '{name}' не знайдено." });
            });

            // DELETE by id
            app.MapDelete("/instruments/by-id-greater/{threshold:int}", (int threshold, InstrumentRepository repo) =>
            {
                int count = repo.RemoveWhereIdGreaterThan(threshold);
                return Results.Ok(new { message = $"¬идалено елемент≥в з Id > {threshold}: {count}" });
            });

            // DELETE all
            app.MapDelete("/instruments/clear", (InstrumentRepository repo) =>
            {
                repo.Clear();
                return Results.Ok(new { message = "—ховище повн≥стю очищено." });
            });

            app.Run();
        }
    }
}
