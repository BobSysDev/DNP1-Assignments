using BlazorApp.Components;
using BlazorApp.Components.Auth;
using BlazorApp.Services;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IUserService, HttpUserService>();
builder.Services.AddScoped<ICommentService, HttpCommentService>();
builder.Services.AddScoped<IPostService, HttpPostService>();
builder.Services.AddScoped<AuthenticationStateProvider, SimpleAuthProvider>();

builder.Services.AddScoped(sp => new HttpClient
{
    // BaseAddress = new Uri("https://quickshift.electimore.xyz")
    BaseAddress = new Uri("http://localhost:5070")
});

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
