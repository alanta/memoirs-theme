using Statiq.Common;
using Statiq.Core;

namespace MemoirsTheme.Pipelines
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