# MooGameV3 Folder Structure

This document describes the main folders and their purpose in the MooGameV3 project.

## Domain
Contains core business logic and domain models.
- `Domain/Game/` - Game rules, code evaluation, and related abstractions.
- `Domain/Players/` - Player statistics and related models.

## Application
Application services and interfaces.
- `Application/Abstractions/` - Service interfaces used throughout the app.
- `Application/Services/` - Implementations of application services.

## Infrastructure
Technical implementations for persistence and other infrastructure concerns.
- `Infrastructure/FileSystem/` - File-based score repository and related options.
- `Infrastructure/CodeGeneration/` - Random code generator for the game.

## Presentation
User interface and interaction logic.
- `Presentation/Console/` - Console UI components.
  - `Commands/` - Command routing and context for console commands.
  - `Configuration/` - Service registration and configuration for the console app.
  - `Game/` - Game runner and round logic for the console.
  - `IO/` - Input/output abstractions and formatters.
  - `Intro/` - Welcome and introduction messages.
  - `Scoring/` - Score presentation logic.

## UnitTests
Automated tests for the application.
- `UnitTests/` - MSTest-based unit and end-to-end tests.

## Solution Items
- `Directory.Build.props` - Solution-wide build configuration.

---

Feel free to update this document as your project evolves!