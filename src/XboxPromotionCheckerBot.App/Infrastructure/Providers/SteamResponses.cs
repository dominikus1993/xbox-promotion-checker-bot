using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace XboxPromotionCheckerBot.App.Infrastructure.Providers;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(Dictionary<string, SteamAppData>))]
[JsonSerializable(typeof(SteamAppResponse))]
internal partial class SteamSerializationConfig : JsonSerializerContext
{
}

public sealed class SteamAppData
{
    [JsonProperty("success", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("data")]
    public Data? Data { get; set; }
}

public sealed class Data
{
    [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonProperty("steam_appid", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("steam_appid")]
    public int? SteamAppid { get; set; }

    [JsonProperty("is_free", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("is_free")]
    public bool? IsFree { get; set; }

    [JsonProperty("detailed_description", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("detailed_description")]
    public string DetailedDescription { get; set; }

    [JsonProperty("about_the_game", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("about_the_game")]
    public string AboutTheGame { get; set; }

    [JsonProperty("short_description", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("short_description")]
    public string ShortDescription { get; set; }

    [JsonProperty("supported_languages", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("supported_languages")]
    public string SupportedLanguages { get; set; }

    [JsonProperty("header_image", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("header_image")]
    public string HeaderImage { get; set; }

    [JsonProperty("capsule_image", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("capsule_image")]
    public string CapsuleImage { get; set; }

    [JsonProperty("capsule_imagev5", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("capsule_imagev5")]
    public string CapsuleImagev5 { get; set; }

    [JsonProperty("price_overview", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("price_overview")]
    public PriceOverview PriceOverview { get; set; }
}


public class PriceOverview
{
    [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("currency")]
    public string Currency { get; set; }

    [JsonProperty("initial", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("initial")]
    public int? Initial { get; set; }

    [JsonProperty("final", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("final")]
    public int? Final { get; set; }

    [JsonProperty("discount_percent", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("discount_percent")]
    public int? DiscountPercent { get; set; }

    [JsonProperty("initial_formatted", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("initial_formatted")]
    public string? InitialFormatted { get; set; }

    [JsonProperty("final_formatted", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("final_formatted")]
    public string? FinalFormatted { get; set; }
}

