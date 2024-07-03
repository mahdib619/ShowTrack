using ShowTrack.Contracts.Dtos;

namespace ShowTrack.Client.Models.Dtos;

public sealed class ReadShowUiDto : ReadShowDto
{
    public bool ShowDeletePrompt { get; set; }

    private bool _enableRating;
    public bool EnableRating
    {
        get => PersonalRating >= 0 || _enableRating;
        set => _enableRating = value;
    }
}
