using System.Collections.Generic;

namespace mediaPlayerPrj;

/// <summary>Information shown on the startup screen and in the project documentation.</summary>
public sealed record TeamMember(string Name, string Role, string PhotoFile);

/// <summary>Group roster for the splash screen.</summary>
public static class TeamInfo
{
    /// <summary>College section.</summary>
    public const string Section = "BSCS2A";

    /// <summary>Team designation.</summary>
    public const string Team = "Group 1 · Amazing";

    /// <summary>Members and agreed project responsibilities.</summary>
    public static IReadOnlyList<TeamMember> Members { get; } = new []
    {
        new TeamMember("Darrel Cango", "Library / Sorting", @"Team\darrel.png"),
        new TeamMember("Chris Garcia", "Playback Modes", @"Team\chris.png"),
        new TeamMember("Mark Guevarra", "Lead / Integration", @"Team\mark.png"),
        new TeamMember("Shanen Mirador", "UI / Skin", @"Team\shanen.png"),
        new TeamMember("Mitchel Susi", "Favorites / QA", @"Team\mitchel.png")
    };
}
