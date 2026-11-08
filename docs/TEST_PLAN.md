# Test plan — real outcomes pending

**Rule:** Entries are proposed test cases only; no test has been executed. Record actual steps, expected output, observed result, date, environment, tester, and PASS/FAIL/NOT RUN. At least **15 executed** tests are required. Include failures and retests in BUG_LOG.md.

| ID | Requirement | Steps | Expected |
|---|---|---|---|
| TC01 | FR1 | Add valid MP3 | Added to list with metadata |
| TC02 | FR1 | Add valid MP4 | Added to list, video plays in panel |
| TC03 | FR1/7 | Select unsupported extension | Friendly rejection, no crash |
| TC04 | FR2 | Import audio with album | Title/artist/duration/path/type/album displayed |
| TC05 | FR2 | Import video with resolution | Type/path/resolution/duration displayed |
| TC06 | FR3 | Save and relaunch with media | Library reloaded |
| TC07 | FR3 | First run with no Library.txt | Empty library, no crash |
| TC08 | FR7 | Add corrupt six-field row to Library.txt, relaunch | Invalid row skipped/reported |
| TC09 | FR7 | Delete a referenced media path | User-visible error, player survives |
| TC10 | FR4 | Play/Pause/Resume/Stop MP3 | Correct player state and output |
| TC11 | FR4 | Next/Previous with >=3 tracks | Correct selection and playback |
| TC12 | FR4 | Adjust volume | Actual sound level changes |
| TC13 | FR5 | Select item in playlist | Now Playing matches selection |
| TC14 | FR9 | Start application | Splash shows all 5, title/group/section, auto closes ~3s |
| TC15 | FR9 | Skip splash by click/key | Splash closes early |
| TC16 | FR9 | Remove member photo asset | Placeholder; no crash |
| TC17 | FR10 | Test all menu shortcuts | Same result as matching buttons |
| TC18 | FR11 | Play >2 seconds | Elapsed/total visible, updates >=1/sec |
| TC19 | FR12 | Sort unsorted titles, compare to expected | Insertion-sorted by title |
| TC20 | FR12 | Sort artists while playing item | Correct artist ordering; current track continues |
| TC21 | FR13 | Repeat Off from last track | Playback stops at end |
| TC22 | FR13 | Repeat All from last track | Wraps and plays first |
| TC23 | FR13 | Repeat One | Same track restarts |
| TC24 | FR13 | Shuffle on with several items | Valid next track, obeys policy |
| TC25 | FR13 | One-item playlist with repeat/shuffle modes | No crash; behavior matches chosen policy |
| TC26 | FR8 | Heart favorite then relaunch | Favorite persists |
| TC27 | FR8 | Toggle favorite off | Removed from favorites view/persistence |
| TC28 | FR8 | Favorites with missing/corrupt storage | Recover without crash |
| TC29 | Skin | Run 100% scale | Controls readable and accessible |
| TC30 | Skin | Run 125% scale | UI readable; video unobstructed |
| TC31 | Deployment | Extract ZIP on different Windows PC | App launches, VLC native files found |
| TC32 | Deployment | Restart deployed app | Library and favorites persist |

## Test results (fill only after real execution)
| ID | Date | Tester / machine | Actual result | Status | Bug |
|---|---|---|---|---|---|
| TC01–TC32 | — | — | NOT RUN | NOT RUN | — |

For each completed case, replace placeholder with real evidence; do not label all tests PASS based on code inspection. Original filename examples and acceptance conditions are based on the instructor PDF.
