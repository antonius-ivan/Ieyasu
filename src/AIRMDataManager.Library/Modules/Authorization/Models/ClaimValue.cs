﻿namespace AIRMDataManager.Library.Modules.Authorization.Models;

// Original source: https://github.com/berhir/ReactAspNetcoreWebAssemblyCookieAuth.
public class ClaimValue
{
    public ClaimValue()
    {
    }

    public ClaimValue(string type, string value)
    {
        Type = type;
        Value = value;
    }

    public string Type { get; set; }

    public string Value { get; set; }
}
