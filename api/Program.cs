using api.Data;
using api.Interface;
using api.Models;
using api.Repositories;
using api.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDataContext>(options => {
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
builder.Services.AddIdentity<AppUser,IdentityRole>(options =>
{
    options.Password.RequireDigit=true;
    options.Password.RequiredLength=12; 
    options.Password.RequireLowercase=true;
    options.Password.RequireUppercase=true;
}).AddEntityFrameworkStores<ApplicationDataContext>();
builder.Services.AddAuthentication(options=> 
{
    options.DefaultAuthenticateScheme=
    options.DefaultChallengeScheme=
    options.DefaultForbidScheme=
    options.DefaultScheme=
    options.DefaultSignInScheme=
    options.DefaultSignOutScheme=JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer (options=>
{
    options.TokenValidationParameters= 
    new TokenValidationParameters
    {
        ValidateIssuer= true,
        ValidIssuer= builder.Configuration["JWT:Issuer"],
        ValidateAudience= true,
        ValidAudience= builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey =true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]))
    };
});
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();


