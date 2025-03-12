using JSDotnet.DTOs;
using JSDotnet.Service;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IComponentRenderService, ComponentRenderService>();
builder.Services.AddAntiforgery(cfg => cfg.Cookie.Name = "XSRF_TOKEN");

var app = builder.Build();

app.UseAntiforgery();
app.UseStaticFiles();

app.MapGet("/", async (HttpContext context, IComponentRenderService componentService, IAntiforgery antiforgery) =>
{
    var tokens = antiforgery.GetTokens(context);
    if (!string.IsNullOrEmpty(tokens.CookieToken))
    {
        context.Response.Cookies.Append("XSRF_TOKEN", tokens.CookieToken);
    }

    var renderResult = await componentService.RenderAsync(
        new RenderComponentParameters
        { 
            ComponentName = "home-page@0.0.1",
            DataJson = JsonSerializer.Serialize(
                new 
                {
                    antiforgeryTokenFieldName = tokens.FormFieldName,
                    antiforgeryTokenValue = tokens.RequestToken
                }
            )
        }
    );

    return renderResult.IsSuccess ? Results.Content(renderResult.Content, renderResult.ContentType)
        : Results.BadRequest(renderResult.Exception);
});

app.MapPost("/submit-info", 
    async Task<IResult>(IComponentRenderService componentService, ILoggerFactory loggerFactory, [FromForm] HomePageForm form) => {
        var logger = loggerFactory.CreateLogger<HomePageForm>();
        
        logger.LogInformation("Received details: first name: '{0}', last name: '{1}', phone: '{2}'",
            form.FirstName,
            form.LastName,
            form.PhoneNumber);
        
        var context = new SubmitInfoContext((entry) => logger.LogInformation(entry));

        var componentResult = await componentService.RenderAsync(
            new RenderComponentParameters 
            { 
                ComponentName = "submitInfoService",
                DataJson = JsonSerializer.Serialize(form)
            },
            context);

        if (!componentResult.IsSuccess || string.IsNullOrEmpty(componentResult.RedirectUrl))
        {
            logger.LogWarning("Unable to run submitInfoService script.");
            return Results.StatusCode(500);
        }

        return Results.Redirect(componentResult.RedirectUrl); 
});

app.MapGet("/info-submitted", async (IComponentRenderService componentService) => {
    var componentResult = await componentService.RenderAsync(
        new RenderComponentParameters
        { 
            ComponentName = "info-submitted-page@0.0.1"
        });

    return componentResult.IsSuccess ? Results.Content(componentResult.Content, componentResult.ContentType) : Results.StatusCode(500);
});

app.Run();

class SubmitInfoContext(Action<string> logMethod)
{
    // This method is called from JS script
    public void Log(string message) => logMethod(message);
}