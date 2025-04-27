using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SharedModels.Models;
using TestMAUIAppApi.Data;
using TestMAUIAppApi.Overrides;

/**
 * The Program.cs file is where:

Services required by the app are configured.
The app's request handling pipeline is defined as a series of middleware components.
Dependency Injection - The framework takes on the responsibility of creating an instance of the dependency and disposing of it when it's no longer needed.
*/

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

//The AddEntityFrameworkStores() method configures the Entity Framework as the storage mechanism for Identity data
builder.Services.AddIdentityApiEndpoints<User>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
    options.User.RequireUniqueEmail = true;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<DataContext>();

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "TestBooksAPI", Description = "Test API for Books", Version = "v1" });

    //Describe c�mo mi API est� protegida
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        //An API key is a token that a client provides when making API calls
        Type = SecuritySchemeType.ApiKey
    });

    //Esta línea se utiliza al configurar Swagger en ASP.NET Core, espec�ficamente para habilitar autenticaci�n/autorization en la documentación generada.
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                Type = ReferenceType.SecurityScheme,
                 Id = "oauth2"
                }
            },

            //"Este esquema de seguridad no requiere scopes adicionales para ser usado". Por ejemplo, operaciones read, write, etc.
            Array.Empty<string>()
        }
    });
});

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5000); // HTTP para Android
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Books API V1");
    });
}

app.MapIdentityApiOverrided<User>();

//Middleware web: Permite la comunicaci�n entre servidores web y aplicaciones
//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


/* 
 * Se crea un scope para obtener instancias de servicios Scoped.
    Se obtienen instancias de:
    RoleManager para gestionar roles.
    UserManager para gestionar usuarios.
    DataContext para interactuar con la base de datos.
    Se llama al método Seeder.Seed() para inicializar la base de datos con roles y usuarios predefinidos.
*/
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
    var context = serviceProvider.GetRequiredService<DataContext>();

    await Seeder.Seed(roleManager, userManager, context);
}

app.Run();
