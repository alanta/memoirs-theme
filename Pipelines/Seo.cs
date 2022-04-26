using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.Urls.Delivery.QueryParameters;
using Kontent.Statiq;
using Statiq.Common;
using Statiq.Core;

namespace MemoirsTheme.Pipelines
{
    public class Seo : Pipeline
    {
        public Seo(IDeliveryClient deliveryClient)
        {
            InputModules = new ModuleList
            {
                new Kontent<Kentico.Kontent.Statiq.Memoirs.Models.Home>(deliveryClient)
                    .WithQuery(new LimitParameter(1), new DepthParameter(1))
            };
        }
    }
}