# Use case diagram (M1)

```mermaid
flowchart LR
  User([User])
  subgraph Player[Amazing Player]
    U1([Add MP3 or MP4])
    U2([Save / Reload Library])
    U3([View playlist and metadata])
    U4([Play / Pause / Stop])
    U5([Next / Previous])
    U6([Change volume or seek])
    U7([Sort by title or artist])
    U8([Toggle Shuffle / Repeat])
    U9([Favorite / Unfavorite])
    U10([Show Favorites])
  end
  User --- U1
  User --- U2
  User --- U3
  User --- U4
  User --- U5
  User --- U6
  User --- U7
  User --- U8
  User --- U9
  User --- U10
```

**Main interactions:** Add Media imports local files and saves Library.txt; Play selects an existing file and updates Now Playing; Sort uses insertion sort on a copy; Favorites toggles a selected item and persists it separately; Repeat/Shuffle select what plays after a media-ended event.

**Exceptions:** empty playlist displays a status message, absent media does not start, and unreadable Library.txt rows are skipped while valid records are retained.