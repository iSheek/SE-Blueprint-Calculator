# SE-Blueprint-Calculator

SE-Blueprint-Calculator is a tool designed to calculate the total number of components required to build blueprints in the game **Space Engineers**.

## Description
The application allows the user to load a blueprint file (`.sbc`) and a folder containing block definitions (`CubeBlocks`), then automatically calculates and summarizes all the materials necessary for the construction.

## Features
- **Blueprint Loading:** Reads `.sbc` files to identify the blocks used.
- **Source Analysis:** Loads block definition files from a selected folder.
- **Total Calculation:** Aggregates all components needed for the full construction of the project.
- **Clear Interface:** Displays results in an organized list, sorted by the most required components.

## Requirements
- .NET 10.0 (or newer)
- Avalonia UI (for the graphical user interface)

## How to use the application?
1. Run the application.
2. Click **"Select CubeBlocks folder"** to indicate the folder containing the block definition files (e.g., from the game's `Content/Data/CubeBlocks` directory).
3. Click **"Select Blueprint file (.sbc)"** to choose the blueprint file you want to calculate.
4. Click the **"Calculate"** button to see the list of necessary components.

## Project Structure
- `Calculator.cs`: Core logic for calculating the sum of components based on block dictionaries.
- `BlueprintLoader.cs`: Responsible for parsing Space Engineers XML blueprint files.
- `SourceBlocksLoader.cs`: Loads block definitions and their assigned components.
- `MainWindow.axaml.cs`: UI logic handling file selection and calculation triggering.
