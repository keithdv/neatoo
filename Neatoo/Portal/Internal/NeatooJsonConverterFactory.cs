﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;
using Neatoo.Core;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using Neatoo.Portal.Internal;

namespace Neatoo.Portal.Internal;

public class NeatooJsonConverterFactory : JsonConverterFactory
{
    private IServiceProvider scope;

    public NeatooJsonConverterFactory(IServiceProvider scope)
    {
        this.scope = scope;
    }

    public override bool CanConvert(Type typeToConvert)
    {
        if (typeToConvert.IsAssignableTo(typeof(IBase))
                || typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(Base<>))
        {
            return true;
        }
        else if (typeToConvert.IsAssignableTo(typeof(IListBase)))
        {
            return true;
        }
        return false;
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        if (typeToConvert.IsAssignableTo(typeof(IBase)))
        {
            return (JsonConverter)scope.GetRequiredService(typeof(NeatooBaseJsonTypeConverter<>).MakeGenericType(typeToConvert));
        }
        else if (typeToConvert.IsAssignableTo(typeof(IListBase)))
        {
            return (JsonConverter)scope.GetRequiredService(typeof(NeatooListBaseJsonTypeConverter<>).MakeGenericType(typeToConvert));
        }

        return null;
    }
}
