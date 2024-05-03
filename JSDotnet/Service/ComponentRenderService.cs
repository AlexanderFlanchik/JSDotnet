using Jint;
using JSDotnet.DTOs;

namespace JSDotnet.Service
{
    public interface IComponentRenderService
    {
        Task<RenderComponentResult> RenderAsync(RenderComponentParameters parameters, object? context = default);
    }

    public class ComponentRenderService : IComponentRenderService
    {
        public async Task<RenderComponentResult> RenderAsync(RenderComponentParameters parameters, object? context = default)
        {
            var scriptsFolder = Path.Combine("Components");
            var componentScript = $"{Path.Combine(scriptsFolder, parameters.ComponentName)}.js";

            if (!File.Exists(componentScript))
            {
                return new RenderComponentResult 
                { 
                    Exception = new InvalidOperationException($"No component file '{parameters.ComponentName}' found") 
                };
            }

            using var engine = new Engine();
            var componentResult = new RenderComponentResult();

            engine.SetValue("dataJson", parameters.DataJson);
            engine.SetValue("context", context);

            engine.SetValue("callback", 
                (string? content, string? redirectUrl, string? contentType) => {
                    componentResult.Content = content;
                    componentResult.RedirectUrl = redirectUrl;
                    componentResult.ContentType = contentType;
                });

            return await Task.Run(async () =>
            {
                try
                {
                    engine.Evaluate(await File.ReadAllTextAsync(componentScript));

                    return componentResult;
                }
                catch (Exception ex)
                {
                    return new RenderComponentResult { Exception = ex };
                }
            });
        }
    }
}
