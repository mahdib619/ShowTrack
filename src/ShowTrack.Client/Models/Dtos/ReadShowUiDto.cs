using ShowTrack.Contracts.Dtos;

namespace ShowTrack.Client.Models.Dtos;

public sealed class ReadShowUiDto : ReadShowDto
{
    public bool ShowDeletePrompt { get; set; }
}
