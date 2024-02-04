using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace XboxPromotionCheckerBot.App.Infrastructure.Providers;

public sealed class SteamAppData
{
    [JsonProperty("success", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("data")]
    public Data Data { get; set; }
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

    [JsonProperty("required_age", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("required_age")]
    public int? RequiredAge { get; set; }

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

    [JsonProperty("packages", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("packages")]
    public List<int?> Packages { get; } = new List<int?>();

    [JsonProperty("package_groups", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("package_groups")]
    public List<PackageGroup> PackageGroups { get; } = new List<PackageGroup>();

    [JsonProperty("platforms", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("platforms")]
    public Platforms Platforms { get; set; }
    
    [JsonProperty("genres", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("genres")]
    public List<Genre> Genres { get; } = new List<Genre>();

    [JsonProperty("support_info", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("support_info")]
    public SupportInfo SupportInfo { get; set; }

    [JsonProperty("background", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("background")]
    public string Background { get; set; }

    [JsonProperty("background_raw", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("background_raw")]
    public string BackgroundRaw { get; set; }
}

public class Genre
{
    [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("description")]
    public string Description { get; set; }
}

public class Highlighted
{
    [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("path")]
    public string Path { get; set; }
}

public class PackageGroup
{
    [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonProperty("selection_text", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("selection_text")]
    public string SelectionText { get; set; }

    [JsonProperty("save_text", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("save_text")]
    public string SaveText { get; set; }

    [JsonProperty("display_type", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("display_type")]
    public int? DisplayType { get; set; }

    [JsonProperty("is_recurring_subscription", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("is_recurring_subscription")]
    public string IsRecurringSubscription { get; set; }

    [JsonProperty("subs", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("subs")]
    public List<Sub> Subs { get; } = new List<Sub>();
}

public class Platforms
{
    [JsonProperty("windows", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("windows")]
    public bool? Windows { get; set; }

    [JsonProperty("mac", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("mac")]
    public bool? Mac { get; set; }

    [JsonProperty("linux", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("linux")]
    public bool? Linux { get; set; }
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

public class Sub
{
    [JsonProperty("packageid", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("packageid")]
    public int? Packageid { get; set; }

    [JsonProperty("percent_savings_text", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("percent_savings_text")]
    public string PercentSavingsText { get; set; }

    [JsonProperty("percent_savings", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("percent_savings")]
    public int? PercentSavings { get; set; }

    [JsonProperty("option_text", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("option_text")]
    public string OptionText { get; set; }

    [JsonProperty("option_description", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("option_description")]
    public string OptionDescription { get; set; }

    [JsonProperty("can_get_free_license", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("can_get_free_license")]
    public string CanGetFreeLicense { get; set; }

    [JsonProperty("is_free_license", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("is_free_license")]
    public bool? IsFreeLicense { get; set; }

    [JsonProperty("price_in_cents_with_discount", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("price_in_cents_with_discount")]
    public int? PriceInCentsWithDiscount { get; set; }
}

public class SupportInfo
{
    [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("email")]
    public string Email { get; set; }
}