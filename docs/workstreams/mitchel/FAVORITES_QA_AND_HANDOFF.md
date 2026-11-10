# Favorites / Liked Songs (FR8) — QA Report & Integration Handoff

**Author:** Mitchel Susi (`susimitchel@gmail.com`)  
**Feature:** FR8 — Favorites / Liked Songs  
**Branch:** `feat/mitchel-favorites`  
**Target Milestone:** M3/M4 Integration  
**Date:** 2026-11-10  

---

## 1. Executive Summary & Design Scope

The Favorites enhancement (FR8) allows users to:
1. Favorite / unfavorite any media track (MP3 or MP4) via a heart toggle button.
2. Visibly distinguish liked tracks in the playlist interface.
3. Filter the playlist display to show only favorited items.
4. Persist liked items across application restarts using a dedicated `Favorites.txt` file.

### Critical Architecture Boundaries
- **Strict Separation of Storage:** Favorites are persisted solely in `Favorites.txt`. The required 6-field `Library.txt` format (`Type|Title|Artist|HH:MM:SS|FilePath|AlbumOrResolution`) remains 100% untouched and unaltered.
- **Robust Path Keying:** Tracks are tracked by normalized, absolute, case-insensitive local file paths via `FavoritesStore.NormalizePath`.
- **Fault-Tolerant File I/O:** Atomic file writes (using `.tmp` swap) avoid partial file corruption. Corrupt rows, illegal characters, null bytes, blank lines, and duplicates in `Favorites.txt` are safely ignored on load and counted in `LastSkippedCount`.
- **Zero UI / Core Coupling:** Neither `BasePlayerForm.cs`, `Form1.cs`, `Program.cs`, nor `LibraryStore.cs` were modified. All functionality is encapsulated within `FavoritesStore.cs` and `FavoritesService.cs`.

---

## 2. Integration Contract & Handoff for Shanen Mirador (UI Owner)

Shanen, here are the exact C# signatures and ready-to-use snippets to wire the Favorites UI into `Form1.cs` without having to change any service logic.

### 2.1 Service Instance
Declare and instantiate the service at the top of `Form1`:

```csharp
// In Form1.cs private fields:
private readonly FavoritesService _favoritesService = new();
```

### 2.2 Heart Button / Toggle Action
When the user clicks the Heart button or presses a favorite shortcut:

```csharp
private void OnHeartButtonClicked()
{
    if (CurrentIndex < 0 || CurrentIndex >= Playlist.Count)
    {
        SetStatus("Select a track first to favorite/unfavorite.");
        return;
    }

    MediaItem item = Playlist[CurrentIndex];
    try
    {
        // ToggleFavorite returns true if now favorited, false if removed
        bool isFavorited = _favoritesService.ToggleFavorite(item.FilePath);

        // Update UI (e.g., heart button text/icon and playlist view)
        btnHeart.Text = isFavorited ? "♥ Liked" : "♡ Like";
        SetStatus(isFavorited 
            ? $"Added '{item.Title}' to Favorites." 
            : $"Removed '{item.Title}' from Favorites.");

        RebuildList(); // or update selected row tag/icon
    }
    catch (FavoritesIOException ex)
    {
        SetStatus("Favorites storage warning: " + ex.Message);
    }
}
```

### 2.3 Checking Favorite Status for Playlist Rows
When populating or refreshing the ListView / Playlist grid:

```csharp
// Check if track is a favorite:
bool isFav = _favoritesService.IsFavorite(item.FilePath);

// Display visual indicator (e.g., heart prefix in title or dedicated column):
string displayTitle = (isFav ? "♥ " : "") + item.Title;
```

### 2.4 Favorites-Only Filter
When the user toggles a "Favorites Only" checkbox, tab, or menu item:

```csharp
private bool _showFavoritesOnly = false;

private void OnToggleFavoritesFilter()
{
    _showFavoritesOnly = !_showFavoritesOnly;

    if (_showFavoritesOnly)
    {
        List<MediaItem> favoriteTracks = _favoritesService.GetFavorites(Playlist);
        // Bind favoriteTracks to your playlist view
        SetStatus($"Showing {favoriteTracks.Count} favorite track(s).");
    }
    else
    {
        // Restore full playlist view
        SetStatus("Showing full library.");
    }
}
```

### 2.5 Quick Reference API Signatures
| Method | Signature | Description |
|---|---|---|
| `ToggleFavorite` | `bool ToggleFavorite(string? path)` | Toggles status; returns new favorite state |
| `IsFavorite` | `bool IsFavorite(string? path)` | Checks if path is favorited (case-insensitive) |
| `IsFavorite` | `bool IsFavorite(MediaItem? item)` | Overload inspecting `item.FilePath` |
| `GetFavorites` | `List<MediaItem> GetFavorites(IEnumerable<MediaItem>? items)` | Filters media collection preserving order |
| `FilterFavorites` | `List<MediaItem> FilterFavorites(IEnumerable<MediaItem>? items)` | Alias for `GetFavorites` |
| `Add` | `bool Add(string? path)` | Adds track; returns false if duplicate/invalid |
| `Remove` | `bool Remove(string? path)` | Removes track; returns false if not present |
| `Count` | `int Count { get; }` | Number of currently favorited items |
| `LastSkippedCount` | `int LastSkippedCount { get; }` | Corrupted/duplicate lines skipped on startup |

---

## 3. Integration Handoff for Mark Guevarra (Lead Integrator)

1. **Storage Location:** Backing file defaults to `Path.Combine(AppContext.BaseDirectory, "Favorites.txt")`.
2. **Error Recovery:** In case of disk write permission failure or locked file, `FavoritesIOException` is thrown with clear error details so the UI can report it in `SetStatus` without crashing.
3. **First Run:** If `Favorites.txt` does not exist, `Load` returns an empty set with 0 skipped rows without creating unnecessary error logs.
4. **Deduplication:** Repeated imports or multiple entries pointing to the same file (e.g. `song.mp3` vs `./song.mp3` vs `"C:\music\song.mp3"`) are deduplicated through path normalization.

---

## 4. Test Plan & QA Execution Results

All unit tests are structured in MSTest targeting `.NET 8.0` under `tests/MitchelFavoritesTests/`. Per course requirements and AGENTS.md rules, all tests unexecuted in this non-Windows / non-.NET agent environment are recorded truthfully as **NOT RUN**.

### 4.1 Automated Test Suite Inventory (69 Tests)

#### A. FavoritesStoreTests (33 tests)
| ID | Test Name | Tested Behavior | Status |
|---|---|---|---|
| ST01 | `NormalizePath_NullInput_ReturnsNull` | Rejects null path input | NOT RUN |
| ST02 | `NormalizePath_EmptyString_ReturnsNull` | Rejects empty string | NOT RUN |
| ST03 | `NormalizePath_WhitespaceOnly_ReturnsNull` | Rejects whitespace string | NOT RUN |
| ST04 | `NormalizePath_ValidAbsolutePath_ReturnsFullPath` | Returns full normalized path | NOT RUN |
| ST05 | `NormalizePath_LeadingTrailingWhitespace_IsTrimmed` | Trims leading/trailing whitespace | NOT RUN |
| ST06 | `NormalizePath_RelativePath_ResolvesToAbsolute` | Resolves relative path to absolute | NOT RUN |
| ST07 | `NormalizePath_PathWithNullChar_ReturnsNull` | Rejects null character in path | NOT RUN |
| ST08 | `NormalizePath_TwoEquivalentPaths_NormalizeIdentically` | Resolves `..` traversal identically | NOT RUN |
| ST09 | `NormalizePath_QuotedPath_StripsQuotesAndNormalizes` | Removes quotes before resolution | NOT RUN |
| ST10 | `Constructor_DefaultPath_EndsWithFavoritesTxt` | Defaults to Favorites.txt | NOT RUN |
| ST11 | `Constructor_CustomPath_UsesProvidedPath` | Respects custom file path | NOT RUN |
| ST12 | `Constructor_NullPath_FallsBackToDefault` | Null falls back to default | NOT RUN |
| ST13 | `Constructor_BlankPath_FallsBackToDefault` | Blank falls back to default | NOT RUN |
| ST14 | `Load_NonExistentFile_ReturnsEmptySetAndZeroSkipped` | Missing file returns empty set | NOT RUN |
| ST15 | `Load_ReturnedSet_IsCaseInsensitive` | HashSet uses OrdinalIgnoreCase | NOT RUN |
| ST16 | `Load_FileWithValidPaths_ReturnsAllPaths` | Reads valid lines from file | NOT RUN |
| ST17 | `Load_EmptyFile_ReturnsEmptySetZeroSkipped` | Empty file returns 0 items | NOT RUN |
| ST18 | `Load_BlankLines_AreSkippedAndCounted` | Blank lines skipped and counted | NOT RUN |
| ST19 | `Load_DuplicatePaths_DedupedAndCounted` | Duplicate lines deduplicated | NOT RUN |
| ST20 | `Load_PathsWithLeadingTrailingSpaces_AreTrimmed` | Trims lines loaded from file | NOT RUN |
| ST21 | `Load_LineWithNullChar_IsSkipped` | Skips lines with invalid chars | NOT RUN |
| ST22 | `Load_CaseDuplicatesOnSamePath_DedupedCaseInsensitively` | Case-insensitive deduplication | NOT RUN |
| ST23 | `Save_ValidPaths_WritesOnePerLine` | Writes lines one per line | NOT RUN |
| ST24 | `Save_EmptyCollection_WritesEmptyFile` | Writes empty file safely | NOT RUN |
| ST25 | `Save_NullEntriesInCollection_AreSkipped` | Skips null entries in collection | NOT RUN |
| ST26 | `Save_NullArgument_Throws` | Throws ArgumentNullException | NOT RUN |
| ST27 | `Save_ThenLoad_RoundTrip` | Round-trip save and load parity | NOT RUN |
| ST28 | `Save_OverwritesExistingFile` | Atomic overwrite of existing file | NOT RUN |
| ST29 | `Save_CreatesParentDirectoryIfNotExists` | Auto-creates missing directory | NOT RUN |
| ST30 | `Save_UnicodePaths_PreservesEncodingInUtf8` | UTF-8 without BOM preserves Unicode | NOT RUN |
| ST31 | `Load_CorruptFileWithBinaryAndGarbage_RecoversValidLinesSafely` | Recovers valid lines from corrupt file | NOT RUN |
| ST32 | `Load_FileWithUtf8Bom_LoadsCorrectly` | Handles UTF-8 BOM cleanly | NOT RUN |
| ST33 | `SaveAndLoad_LargeBatch_MaintainsIntegrity` | Round-trips 200 items cleanly | NOT RUN |

#### B. FavoritesServiceTests (28 tests)
| ID | Test Name | Tested Behavior | Status |
|---|---|---|---|
| SV01 | `Add_ValidPath_ReturnsTrueAndIncreasesCount` | Adds track and increments count | NOT RUN |
| SV02 | `Add_DuplicatePath_ReturnsFalseAndCountUnchanged` | Rejects duplicate track add | NOT RUN |
| SV03 | `Add_NullOrWhitespace_ReturnsFalse` | Rejects null or whitespace add | NOT RUN |
| SV04 | `Remove_ExistingFavorite_ReturnsTrueAndDecreasesCount` | Removes existing favorite | NOT RUN |
| SV05 | `Remove_NonExistingFavorite_ReturnsFalse` | Returns false when not found | NOT RUN |
| SV06 | `Remove_NullOrWhitespace_ReturnsFalse` | Returns false on invalid remove | NOT RUN |
| SV07 | `Toggle_WhenNotFavorite_AddsAndReturnsTrue` | Toggles on and returns true | NOT RUN |
| SV08 | `Toggle_WhenAlreadyFavorite_RemovesAndReturnsFalse` | Toggles off and returns false | NOT RUN |
| SV09 | `Toggle_InvalidPath_ReturnsFalseAndCountUnchanged` | Rejects toggle on invalid path | NOT RUN |
| SV10 | `ToggleFavorite_Alias_BehavesSameAsToggle` | Alias matches Toggle behavior | NOT RUN |
| SV11 | `IsFavorite_WhenFavorited_ReturnsTrue` | Returns true for liked track | NOT RUN |
| SV12 | `IsFavorite_WhenNotFavorited_ReturnsFalse` | Returns false for unliked track | NOT RUN |
| SV13 | `IsFavorite_NullOrEmpty_ReturnsFalse` | Null-safe false on invalid query | NOT RUN |
| SV14 | `IsFavorite_MediaItemOverload_ReturnsExpected` | Checks MediaItem overload | NOT RUN |
| SV15 | `GetAllFavorites_ReturnsAllTrackedPaths` | Snapshot returns all favorites | NOT RUN |
| SV16 | `FilterFavorites_PreservesOrderAndFiltersOnlyFavorites` | Filters playlist in original order | NOT RUN |
| SV17 | `FilterFavorites_NullInput_ReturnsEmptyList` | Null playlist returns empty list | NOT RUN |
| SV18 | `GetFavorites_Alias_ReturnsSameAsFilterFavorites` | Alias matches FilterFavorites | NOT RUN |
| SV19 | `Clear_EmptiesCollectionAndSaves` | Empties list and updates file | NOT RUN |
| SV20 | `AddRange_AddsUniqueOnlyAndReportsCount` | Batch adds unique items only | NOT RUN |
| SV21 | `Persistence_SecondServiceInstance_LoadsSavedFavoritesAcrossRestart` | Simulates app restart persistence | NOT RUN |
| SV22 | `Toggle_Repeatedly_AlternatesStateAndPersists` | Multiple toggle consistency | NOT RUN |
| SV23 | `Reload_ReflectsExternalModifications` | Reloads modified file on disk | NOT RUN |
| SV24 | `LastSkippedCount_ReflectsStoreSkippedLinesOnStartup` | Tracks skipped corrupt entries | NOT RUN |
| SV25 | `FilterFavorites_HandlesMixedMediaAndInvalidItemPaths` | Skips items with empty/bad paths | NOT RUN |
| SV26 | `PathMatching_RelativeAndAbsoluteVariants_MatchConsistently` | Cross-matches relative & absolute | NOT RUN |
| SV27 | `Remove_ThenAddAgain_ReEnablesFavorite` | Full lifecycle verification | NOT RUN |
| SV28 | `FavoritesIOException_PreservesMessageAndInnerException` | Exception message and inner cause | NOT RUN |

#### C. BoundaryAndEdgeCaseTests (8 tests)
| ID | Test Name | Tested Behavior | Status |
|---|---|---|---|
| ED01 | `PathWithSpaces_PreservesInternalSpacesWhileTrimmingPadding` | Internal path spaces preserved | NOT RUN |
| ED02 | `SlashConsistency_ForwardAndBackslashes_ResolveToSameFavorite` | Slash variants resolve identically | NOT RUN |
| ED03 | `Toggle_MixedCaseInput_CorrectlyRemovesExistingItem` | Case-insensitive toggle removal | NOT RUN |
| ED04 | `Remove_MixedCaseInput_Succeeds` | Case-insensitive item removal | NOT RUN |
| ED05 | `InvalidPathWithPipe_IsSafelyRejected` | Rejects pipe characters in paths | NOT RUN |
| ED06 | `WhiteSpaceVariants_AllRejectedConsistently` | Tabs, spaces, newlines rejected | NOT RUN |
| ED07 | `FilterFavorites_PreservesPolymorphicSubclassProperties` | Preserves AudioItem & VideoItem data | NOT RUN |
| ED08 | `SequenceOperations_AddRemoveToggle_StateRemainsConsistent` | Multi-step lifecycle consistency | NOT RUN |

---

### 4.2 PRD Acceptance Test Cases (`docs/TEST_PLAN.md` Mapping)

The following test scenarios from `docs/TEST_PLAN.md` correspond to the Favorites feature (FR8):

| Test Plan ID | Requirement | Scenario Description | Automated Test Equivalent | Windows GUI Status |
|---|---|---|---|---|
| **TC26** | FR8 | Heart favorite track then relaunch app | `SV21`, `ST27` | **NOT RUN** (pending Windows GUI) |
| **TC27** | FR8 | Toggle favorite off | `SV08`, `SV22`, `ED03` | **NOT RUN** (pending Windows GUI) |
| **TC28** | FR8 | Favorites with missing or corrupted storage | `ST14`, `ST31`, `SV24` | **NOT RUN** (pending Windows GUI) |
| **TC32** | Deployment | Restart deployed app; library & favorites persist | `SV21`, `ST27` | **NOT RUN** (pending final release publish) |

---

## 5. Instructions for Running Tests on Windows

To run all unit tests on a developer or testing machine with the .NET 8 SDK installed:

```bash
# Navigate to the test project directory:
cd tests/MitchelFavoritesTests

# Execute test suite:
dotnet test --logger "console;verbosity=detailed"
```
