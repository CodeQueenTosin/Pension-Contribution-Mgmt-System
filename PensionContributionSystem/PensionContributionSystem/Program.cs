using Microsoft.EntityFrameworkCore;
using PensionContributionSystem.Application.Interfaces;
using PensionContributionSystem.Infrastructure.Data;
using Hangfire; 
using PensionContributionSystem.Application.Services;
using PensionContributionSystem.Infrastructure.Repositories;
using PensionContributionSystem.Application.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using PensionContributionSystem.Application.Mappings;
using PensionContributionSystem.API.Middleware;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAutoMapper(typeof(MemberProfile));

builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register DbContext
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register application services
builder.Services.AddScoped<IContributionService, ContributionService>();
builder.Services.AddScoped<IBenefitService, BenefitService>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IBenefitRepository, BenefitRepository>();
builder.Services.AddScoped<IContributionRepository, ContributionRepository>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IContributionService, ContributionService>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateMemberDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateMemberDtoValidator>();




// Configure Hangfire with SQL Server storage
builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();

var app = builder.Build();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
    await dbContext.SeedDataAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseAuthorization();

// Add Hangfire dashboard
app.UseHangfireDashboard();


RecurringJob.AddOrUpdate<IContributionService>("validate-monthly-contributions", s => s.ValidateMonthlyContributions(), Cron.Monthly) ;
RecurringJob.AddOrUpdate<IBenefitService>("update-benefits", s => s.RecalculateAndStoreBenefits(), Cron.Monthly);
RecurringJob.AddOrUpdate<IContributionService>("generate-statements", s => s.GenerateMemberStatements(), Cron.Monthly);
RecurringJob.AddOrUpdate<IBenefitService>("calculate-monthly-interest", s => s.ApplyMonthlyInterest(), Cron.Monthly);


app.MapControllers();

app.Run();