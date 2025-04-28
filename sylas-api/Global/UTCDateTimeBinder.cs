using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace sylas_api.Global;

public class UTCDateTimeBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if(bindingContext.ModelType == typeof(DateTime) || bindingContext.ModelType == typeof(DateTime?))
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            var value = valueProviderResult.FirstValue;

            if (value == null)
            {
                return Task.CompletedTask;
            }

            DateTime datetime;
            
            if(!DateTime.TryParse(value, out datetime))
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Datetime format not valid");
                bindingContext.Result = ModelBindingResult.Failed();
            }
            else
            {
                // Si le type de timezone c'est pas spécifié, on le considère comme de l'UTC
                if(datetime.Kind == DateTimeKind.Unspecified)
                {
                    datetime = DateTime.SpecifyKind(datetime, DateTimeKind.Utc);
                }
                // Si le timezone est spécifié et pas en UTC, on le converti en UTC
                else if(datetime.Kind == DateTimeKind.Local)
                {
                    datetime = datetime.ToUniversalTime();
                }

                bindingContext.Result = ModelBindingResult.Success(datetime);
            }
            
        }
        else
        {
            bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Model type not supported");
            bindingContext.Result = ModelBindingResult.Failed();
        }

        return Task.CompletedTask;
    }
}
