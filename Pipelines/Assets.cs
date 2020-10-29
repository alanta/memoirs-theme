using Statiq.Common;
using Statiq.Core;

namespace Kentico.Kontent.Statiq.Lumen.Pipelines
{
    public class Assets : Pipeline
    {
        public Assets()
        {
            InputModules = new ModuleList
            {
                new ReadFiles(pattern: "assets/**/*.{*,!scss}"),
                new WriteFiles()
            };
        }
    }
}