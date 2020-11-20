using NUglify;
using NUglify.Html;
using NUglify.JavaScript;
using Statiq.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemoirsTheme.Modules
{
    public class OptimizeHtml : Module
    {
        protected override async Task<IEnumerable<IDocument>> ExecuteInputAsync(IDocument input, IExecutionContext context)
        {
            var settings = new HtmlSettings
            {
                MinifyJs = false
            };
            var original = await input.GetContentStringAsync();
            var minifiedContent = Uglify.Html(original, settings);

            if (minifiedContent.HasErrors)
            {
                context.LogError(input, $"Minification failed");
                foreach (var error in minifiedContent.Errors)
                {
                    context.LogError(input, $"{error.Message} {error.File} line ({error.StartLine},{error.StartColumn}) through ({error.EndLine},{error.EndColumn})");
                }
                return (input).Yield(); // return original document
            }

            context.LogInformation(input, $"Content minified by {minifiedContent.Code.Length*100M/original.Length:N2}%");
            return (await input.CloneAsync(content: minifiedContent.Code)).Yield();


        }
    }
}
