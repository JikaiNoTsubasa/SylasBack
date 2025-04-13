using System;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace sylas_api.Global;

public class CustomMetadataProvider : IMetadataDetailsProvider, IDisplayMetadataProvider
{
    public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
    {
        if (context.Key.MetadataKind == ModelMetadataKind.Property){
            context.DisplayMetadata.ConvertEmptyStringToNull = false;
        }
    }
}