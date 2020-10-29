using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.Statiq.Memoirs.Models;
using Kontent.Statiq;
using Statiq.Common;
using Statiq.Core;

namespace Kentico.Kontent.Statiq.Lumen.Pipelines
{
    public class Authors : Pipeline
    {
        public Authors(IDeliveryClient deliveryClient)
        {
            InputModules = new ModuleList{
                new Kontent<Author>(deliveryClient)
            };
        }
    }
}