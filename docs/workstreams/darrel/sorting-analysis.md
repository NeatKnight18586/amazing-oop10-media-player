# Insertion Sort — Trace, Complexity and FR12 Compliance

**Author:** Darrel Cango  
**File:** `app/MediaPlayerPrj/LibrarySorter.cs` → `LibrarySorter.InsertionSort`  
**Requirement:** FR12 — hand-written insertion sort by Title or Artist; no `List.Sort` or `OrderBy`.

---

## 1. Step-by-Step Trace

### Input (sorted by Title, 5 items, initially scrambled)

| Index | Title |
|---|---|
| 0 | "Mango" |
| 1 | "Apple" |
| 2 | "Cherry" |
| 3 | "Banana" |
| 4 | "Date" |

### Algorithm reference (`LibrarySorter.cs`)

```csharp
for (int i = 1; i < items.Count; i++)
{
    MediaItem current = items[i];
    int j = i - 1;

    while (j >= 0 && Compare(items[j], current, key) > 0)
    {
        items[j + 1] = items[j];
        j--;
    }

    items[j + 1] = current;
}
```

### Pass-by-pass trace

**Pass i = 1** — `current = "Apple"`

| j | items[j] | Compare > 0? | Action |
|---|---|---|---|
| 0 | "Mango" | Yes | Shift "Mango" → index 1 |
| −1 | — | Loop ends | — |

Place "Apple" at index 0.

```
[ Apple, Mango, Cherry, Banana, Date ]
```

---

**Pass i = 2** — `current = "Cherry"`

| j | items[j] | Compare > 0? | Action |
|---|---|---|---|
| 1 | "Mango" | Yes | Shift "Mango" → index 2 |
| 0 | "Apple" | No | Loop ends |

Place "Cherry" at index 1.

```
[ Apple, Cherry, Mango, Banana, Date ]
```

---

**Pass i = 3** — `current = "Banana"`

| j | items[j] | Compare > 0? | Action |
|---|---|---|---|
| 2 | "Mango" | Yes | Shift "Mango" → index 3 |
| 1 | "Cherry" | Yes | Shift "Cherry" → index 2 |
| 0 | "Apple" | No | Loop ends |

Place "Banana" at index 1.

```
[ Apple, Banana, Cherry, Mango, Date ]
```

---

**Pass i = 4** — `current = "Date"`

| j | items[j] | Compare > 0? | Action |
|---|---|---|---|
| 3 | "Mango" | Yes ("Mango" > "Date") | Shift "Mango" → index 4 |
| 2 | "Cherry" | No ("Cherry" < "Date") | Loop ends |

Place "Date" at index 3.

### Final sorted list

| Index | Title |
|---|---|
| 0 | "Apple" |
| 1 | "Banana" |
| 2 | "Cherry" |
| 3 | "Date" |
| 4 | "Mango" |

The list is sorted in ascending order using the same case-insensitive comparison as the implementation.

---

## 2. Best-Case Complexity — O(n)

**Scenario:** The list is already sorted in ascending order.

```
[ Apple, Banana, Cherry, Date, Mango ]
```

On every pass `i`, `current = items[i]`. The first comparison checks `Compare(items[i-1], current, key)`. Because the list is already sorted, the previous item compares less than or equal to `current`, so the `while` condition is false immediately.

The inner `while` loop body never executes. The outer `for` loop runs exactly **n − 1** times.

- Comparisons: n − 1
- Shifts: 0
- **Time complexity: O(n)**

This is the best case: the input is already ordered.

---

## 3. Worst-Case Complexity — O(n²)

**Scenario:** The list is sorted in reverse (descending) order.

```
[ Mango, Date, Cherry, Banana, Apple ]
```

On pass `i`, the current item is smaller than every preceding item. The inner `while` loop therefore shifts all `i` preceding items one position to the right.

| Pass (i) | Shifts |
|---|---|
| 1 | 1 |
| 2 | 2 |
| 3 | 3 |
| 4 | 4 |
| … | … |
| n − 1 | n − 1 |

**Total shifts** = 1 + 2 + 3 + … + (n − 1) = **n(n − 1) / 2**

For n = 5, this is 1 + 2 + 3 + 4 = **10 shifts**. The sum grows proportionally to n², so the worst-case time complexity is **O(n²)**.

The algorithm sorts in place and uses only a fixed number of extra variables, so its auxiliary space complexity is **O(1)**.

---

## 4. Why the Implementation Is Stable

A stable sort preserves the original relative order of items with equal sort keys.

The important condition is:

```csharp
while (j >= 0 && Compare(items[j], current, key) > 0)
```

It uses strict greater-than (`> 0`), not greater-than-or-equal. When two items have equal keys, `Compare` returns 0, the loop stops, and the earlier equal item is not shifted past `current`.

For example, sorting these items by Artist:

```
[ ("Track 1", "Same Band"), ("Track 2", "Same Band"), ("Track 3", "Same Band") ]
```

Each comparison between adjacent equal artists returns 0, so their order remains unchanged. This is why the insertion sort is stable.

The behavior is covered by `InsertionSort_ByArtist_IsStable_PreservesRelativeOrderForEqualArtists` in `LibrarySorterComparisonTests.cs`.

---

## 5. FR12 Compliance

FR12 requires a hand-written insertion sort by title and artist, prohibits `List.Sort`/`OrderBy`, and includes refreshing the list while retaining the current item.

The `LibrarySorter.InsertionSort` method satisfies the algorithm and sorting clauses. Retaining the current item in the refreshed UI is a separate integration responsibility.

| FR12 clause | How it is satisfied |
|---|---|
| Hand-written insertion sort | The algorithm is implemented with a `for` loop and a `while` loop in `LibrarySorter.cs`. |
| No `List.Sort` or `OrderBy` | The insertion-sort method does not call framework sorting or LINQ ordering methods. |
| Sorts by Title | `SortKey.Title` selects Title comparisons. |
| Sorts by Artist | `SortKey.Artist` selects Artist comparisons. |
| Preserves media object identity during sorting | The method sorts the caller's list in place, moving existing object references rather than creating replacement media objects. |
| Retains the currently playing item in the refreshed UI | This is an integration responsibility. The UI layer must track the playing item by object reference and restore/select it after refreshing the displayed list. This document does not claim that UI integration has been tested here. |
| Stable | The strict `> 0` condition leaves equal-keyed items in their original relative order. |

---

## Summary

| Property | Value |
|---|---|
| Algorithm | Hand-written insertion sort |
| In-place | Yes |
| Stable | Yes |
| Best-case time | O(n) |
| Worst-case time | O(n²) |
| Auxiliary space | O(1) |
| FR12 algorithm clauses | Satisfied by `LibrarySorter.InsertionSort` |
| Current-item UI retention | Requires separate UI integration |
