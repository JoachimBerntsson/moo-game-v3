# MooGameV3

## Project Overview

**MooGameV3** is a console-based implementation of the classic "Bulls & Cows" game. It features a modular architecture with clear separation between domain logic, application services, infrastructure, and presentation layers. The project is written in C# and uses MSTest for unit and end-to-end testing.

## Features

- Play Bulls & Cows in the console
- Scoreboard and player statistics
- Configurable game rules (code length, duplicates, etc.)
- Command routing for console commands (e.g., `/scores`)
- Dependency injection via Microsoft.Extensions.DependencyInjection
- Automated tests with MSTest

## Folder Structure

See [`FOLDER_STRUCTURE.md`](FOLDER_STRUCTURE.md) for a detailed description of the folder organization.

## Getting Started

1. **Clone the repository**
2. **Open in Visual Studio or VS Code**
3. **Build the solution**
4. **Run the console application**  
   The main entry point is in the `Presentation/Console/Game` folder.

## Testing

- Unit and end-to-end tests are located in the `MooGameV3.UnitTests` project.
- Tests use MSTest and can be run from Visual Studio's Test Explorer or via command line.

## Configuration

- Solution-wide build settings are in [`Directory.Build.props`](Directory.Build.props).
- Code analysis and editorconfig files are included for code quality.