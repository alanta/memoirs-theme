using NUglify;
using NUglify.Html;
using Statiq.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemoirsTheme.Modules
{
    /// <summary>
    /// Optimize HTML using NUglify.
    /// </summary>
    public class OptimizeHtml : Module
    {
        private readonly bool _enabled;
        private readonly HtmlSettings _settings = new HtmlSettings();

        /// <summary>
        /// Optimize HTML in the content of the Document.
        /// </summary>
        public OptimizeHtml() : this(true)
        {
            
        }

        /// <summary>
        /// Optimize HTML in the content of the Document.
        /// </summary>
        /// <param name="enabled">Set to false to skip optimization. For example for debug builds.</param>
        public OptimizeHtml(bool enabled)
        {
            _enabled = enabled;
        }

        /// <summary>
        /// Configure the NUglify settings
        /// </summary>
        /// <param name="settings">An action to tweak the settings.</param>
        /// <returns></returns>
        public OptimizeHtml WithSettings(Action<HtmlSettings> settings)
        {
            settings?.Invoke(_settings);
            return this;
        }

        protected override async Task<IEnumerable<IDocument>> ExecuteInputAsync(IDocument input, IExecutionContext context)
        {
            if (!_enabled)
            {
                return (input).Yield(); // nothing to do, return original document
            }

            var original = await input.GetContentStringAsync();
            if (string.IsNullOrWhiteSpace(original))
            {
                return (input).Yield(); // nothing to do, return original document
            }

            var minifiedContent = Uglify.Html(original, _settings);

            if (minifiedContent.HasErrors)
            {
                foreach (var error in minifiedContent.Errors)
                {
                    if (error.IsError)
                    {
                        context.LogError(input, error.ToString());
                    }
                    else
                    {
                        context.LogWarning(input, error.ToString());
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(minifiedContent.Code))
            {
                context.LogWarning(input, $"Content minification skipped.");
                return (input).Yield(); // return original document
            }

            context.LogInformation(input, $"Content minified by {minifiedContent.Code.Length*100M/original.Length:N2}%");
            return (input.Clone(content: minifiedContent.Code)).Yield();
        }
    }
}
