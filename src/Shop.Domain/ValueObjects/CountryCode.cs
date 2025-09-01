namespace Shop.Domain.ValueObjects;

public record CountryCode(string Value)
{
    public static readonly CountryCode GB = new("GB");
    public static readonly CountryCode EU = new("EU");
    public static readonly CountryCode Rest = new("REST");
    
    public static CountryCode FromString(string value) => value?.ToUpperInvariant() switch
    {
        "GB" => GB,
        "AT" or "BE" or "BG" or "HR" or "CY" or "CZ" or "DK" or "EE" or "FI" or "FR" or "DE" or "GR" or "HU" or "IE" or "IT" or "LV" or "LT" or "LU" or "MT" or "NL" or "PL" or "PT" or "RO" or "SK" or "SI" or "ES" or "SE" => EU,
        _ => Rest
    };
}
