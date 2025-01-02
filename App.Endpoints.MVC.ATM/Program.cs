using App.Domain.Core.ATM.Cards.AppService;
using App.Domain.Core.ATM.Cards.Data.Repository;
using App.Domain.Core.ATM.Cards.Service;
using App.Domain.Core.ATM.TransActions.AppService;
using App.Domain.Core.ATM.TransActions.Data.Repository;
using App.Domain.Core.ATM.TransActions.Service;
using App.Domain.Core.ATM.Users.AppService;
using App.Domain.Core.ATM.Users.Data.Repository;
using App.Domain.Core.ATM.Users.Service;
using App.Domain.Services.AppService.ATM.Cards;
using App.Domain.Services.AppService.ATM.TransActions;
using App.Domain.Services.AppService.ATM.Users;
using App.Domain.Services.Service.ATM.Cards;
using App.Domain.Services.Service.ATM.TransActions;
using App.Domain.Services.Service.ATM.Users;
using App.Infrastructure.DbAccess.Repository.Ef.ATM.Cards;
using App.Infrastructure.DbAccess.Repository.Ef.ATM.TransActions;
using App.Infrastructure.DbAccess.Repository.Ef.ATM.Users;
using Microsoft.AspNetCore.Cors.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICardRepository, CardEfRepository>();
builder.Services.AddScoped<IUserRepository, UserEfRepository>();
builder.Services.AddScoped<ITransActionRepository, TransActionEfRepository>();

builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITransActionService, TransActionService>();

builder.Services.AddScoped<ICardAppService, CardAppService>();
builder.Services.AddScoped<IUserAppService, UserAppService>();
builder.Services.AddScoped<ITransActionAppService, TransActionAppService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

app.UseSession();




// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Card}/{action=Index}/{id?}");

app.Run();
