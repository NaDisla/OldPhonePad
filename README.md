# OldPhonePad (Console App, .NET 8)

Simulation of text input on old mobile phone keypads using the **multi-tap** mechanism.

> ⚠️ **Important:** This project is for a coding challenge. The repository includes the implementation, tests, and professional project structure. Please review carefully and adapt the logic or assumptions as needed.

---

## Requirements
- .NET SDK 8.0+
- Visual Studio 2022 17.8+ or VS Code with C# Dev Kit

---

## Run
```bash
dotnet build
dotnet run --project src/OldPhonePad
```
---

## Test

```dotnet test```

---

## Specification (summary)

```OldPhonePad(string input)``` returns the decoded text.

```input``` always ends with ```#``` (send).

- Keys:

	- ```2..9``` → multi-tap letters.
	- Whitespace in the input → pause (separates groups to allow two consecutive letters from the same key).
	- ```*``` → backspace (removes the last confirmed character).
	- ```#``` → end / send.

- Key map:
```2 → ABC
3 → DEF
4 → GHI
5 → JKL
6 → MNO
7 → PQRS
8 → TUV
9 → WXYZ
```

---

## Design
- ```OldPhonePadConverter``` encapsulates the logic.

- ```OldPhonePadOptions``` makes behavior explicit:

	- ```ZeroAsSpace``` → treat 0 as a space character.

	- ```ThrowOnUnknownDigit``` → throw if encountering unsupported digits/characters.

- Complexity:

	- Time: **O(n)** (one pass over input).

	- Space: **O(1)** extra (besides output buffer).

---

## Decisions & Assumptions
- ```1``` and unsupported characters are ignored by default (fail-soft).

- When ```ZeroAsSpace = true```, each ```0``` emits exactly one space.

- All output is uppercase.

- Input without ```#``` still flushes the last group (more user-friendly).

---

## Quality
- ```.editorconfig``` enforces consistent style.

- GitHub Actions workflow runs build + tests on push/PR.

- Nullable enabled.

- Unit tests (xUnit) cover:

	- Happy path examples from the spec (```33#``` → ```E```, etc.).

	- Edge cases (multiple spaces, long repeats, backspaces at start).

	- Option toggles (```ZeroAsSpace```, ```ThrowOnUnknownDigit```).

---

## Repository Structure
```
src/OldPhonePad/             # Console app (.NET 8, no top-level statements)
tests/OldPhonePad.Tests/     # Unit tests (xUnit)
.github/workflows/dotnet.yml # CI (build + test)
README.md
```

---

## Example Runs
```
Input: 33#
Output: E

Input: 4433555 555666#
Output: HELLO

Input: 8 88777444666*664#
Output: TURING

Input: 20 20#   (with ZeroAsSpace = true)
Output: A A
```