using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace sylas_api.Global;

public class UTCDateTimeBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if(context.Metadata.ModelType == typeof(DateTime) || context.Metadata.ModelType == typeof(DateTime?))
        {
            return new UTCDateTimeBinder();
        }

        return null;
    }
}
