using System.Text.RegularExpressions;

namespace NewScotApp.Getway.Helpers;

public class SlugifyParameterTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        return value == null
            ? null
            : Regex.Replace(value.ToString() ?? string.Empty, "([a-z])([A-Z])", "$1-$2")
                .ToLower();
    }
}