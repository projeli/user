﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace Projeli.UserService.Api.Extensions;

public static class JsonExtension
{
    public static void AddUserServiceJson(this IMvcBuilder services)
    {
        services.AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });
    }
}