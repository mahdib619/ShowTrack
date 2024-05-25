namespace ShowTrack.Domain.Entities;

public sealed class ShowSchedule
{
    public string Id { get; init; } = Guid.NewGuid().ToString();

    public required string ShowId { get; init; }
    public Show? Show { get; init; }

    public DateOnly ReleaseDate { get; init; }

    public required string Season { get; init; }
}