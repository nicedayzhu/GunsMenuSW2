using System.Text.Json.Serialization;

namespace GunsMenuSW2;

public class GunsMenuConfig
{
    [JsonPropertyName("PermissionForCommands")]
    public string FlagForCommands { get; set; } = "";

    [JsonPropertyName("Blacklist")]
    public List<string> WeaponBlacklist { get; set; } = new();
}
