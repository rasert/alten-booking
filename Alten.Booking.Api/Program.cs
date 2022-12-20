using Alten.Booking.Api.Middlewares;
using Alten.Booking.Application.Abstractions;
using Alten.Booking.Application.Services;
using Alten.Booking.Infrastructure.Persistence;
using System.Text.Json.Serialization;

namespace Alten.Booking.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            builder.Services.AddDbContext<ApplicationContext>();
            builder.Services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<ApplicationContext>());
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IReservationService, ReservationService>();
            builder.Services.AddScoped<IRoomService, RoomService>();
            builder.Services.AddSingleton<ExceptionHandlerMiddleware>();

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                //app.UseDeveloperExceptionPage();
                //app.UseMigrationsEndPoint();
            }

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationContext>();
                context.Database.EnsureCreated();
                DbSeeder.Seed(context);
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}