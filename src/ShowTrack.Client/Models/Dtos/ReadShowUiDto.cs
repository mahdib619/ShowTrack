using ShowTrack.Contracts.Dtos;
using System.Globalization;

namespace ShowTrack.Client.Models.Dtos;

public sealed class ReadShowUiDto : ReadShowDto
{
    public string State => IsEnded ? "ended" : "on-going";
    public string ReleaseDateString => Schedule?.ReleaseDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) ?? "-";
    public bool IsPinned => DatePinned.ToUniversalTime() > DateTime.MinValue.ToUniversalTime();
    public bool ShowDeletePrompt { get; set; }

    private bool _enableRating;
    public bool EnableRating
    {
        get => PersonalRating >= 0 || _enableRating;
        set => _enableRating = value;
    }

    public void UnPin()
    {
        DatePinned = DateTime.MinValue;
    }
}
