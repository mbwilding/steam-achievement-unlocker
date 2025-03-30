# Steam Achievement Unlocker

## Description
Unlocks every achievement for all owned Steam titles.
Tested on Windows, Linux, and Mac (x64).
Make sure Steam is logged in.

## Requirements
- [.NET 9 Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- To build from source: .NET 9 SDK is required.

## General Instructions
1. Download `SteamAchievementUnlocker-{os}-{arch}.zip` from the release section: [GitHub Releases](https://github.com/mbwilding/steam-achievement-unlocker/releases).
2. Extract the downloaded files.
3. Run the executable to unlock achievements for all Steam titles.
4. To unlock achievements for specific app IDs, add them as command-line arguments separated by spaces.
   - Example for specific app IDs: `730 813780`
5. To clear achievements, add the `--clear` flag to the arguments.

## Platform-Specific Instructions

### Windows
- After extracting the files, run `SteamAchievementUnlocker.exe` while Steam is running.
- To target specific app IDs:
  ```powershell
  .\SteamAchievementUnlocker.exe 730 813780
  ```

### Linux / Mac
- Open a terminal and navigate to the extracted directory.
- Run the following commands:
  1. Set execution permissions:
     ```bash
     sudo chmod +x SteamAchievementUnlocker SteamAchievementUnlockerAgent
     ```
  2. Execute the application:
     ```bash
     ./SteamAchievementUnlocker
     ```
