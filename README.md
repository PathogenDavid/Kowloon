[//kow.loon](https://pixelalchemy.dev/portfolio/kowloon/)
=======================================================================================================================

Kowloon was an interactive art installation created in collaboration between [Tucker Baumgartner](https://www.baumxdesign.com/) and myself. It is named after the [Kowloon Walled City](https://en.wikipedia.org/wiki/Kowloon_Walled_City), which was the main inspiration behind its disjoint boxy appearance.

See [the relevant page on my portfolio](https://pixelalchemy.dev/portfolio/kowloon/) for further details on this project.

## Building

In order to build the project, you will need to install the [.NET Core 3.1 SDK](https://dotnet.microsoft.com/en-us/download/dotnet) (scroll down and expand "Out of support versions".)

Otherwise building is like any other .NET app. Open in Visual Studio and press F5 to run, or execute `dotnet run --project Kowloon` in the root of the repo.

The simulator is Windows-only as it uses WinForms. You can disable the simulator by commenting out the relevant `ProjectReference` in `Kowloon.csproj`.

## Project Overview

* `CyberTest` -- This is a smoke test app used for testing the basic functionality of the hardware and the `rpi_ws281x` bindings.
* `Kowloon` -- This is the Blazor Server UI for Kowloon.
* `Kowloon.Core` -- This is the meat and potatoes of Kowloon. All logic for handling LED strings and animations and such lives here.
    * `KowloonManager` is the root class here. It provides the interface used by the Blazor app to interact with `KowloonController`
    * `KowloonController` manages the rendering thread as well as the actual display.
        * 60 times per second it asks to current `IRenderer` to render out the display and then instructs the current `LedString` backend to update the display.
    * `ApartmentManager` this is the primary `IRenderer` implementation used when the display is not showing a test pattern.
        * It's responsible for keeping track of the colors of each apartment, applying animations, and the current palette.
    * `KowloonConfig` describes the physical topology of the display as well as the colors in each palette.
* `Kowloon.LedControl` -- This project provides the `LedString` backend abstraction as well as the primary implementation used on the Raspberry Pi for physical LEDs.
* `Kowloon.LedControl.Simulator` -- A quick and dirty `LedString` backend implementation that displays LED colors using WinForms.
* `RpiWs281x` -- Basic C# bindings around [rpi_ws281x](https://github.com/jgarff/rpi_ws281x)

## License

This project is being published as part of my portfolio and for educational purposes. It is **not** licensed as open-source software.

You may download and build this software for your own personal reference and learning purposes. You may not redistribute it in source or binary form. You may not use any files within this repo for commercial purposes.

I'd be open to properly open-sourcing portions of it if someone sees value in it, so feel free to open an issue with your use-case to express interest. (Most likely though everything in here is probably too old to be of any use except to wholesale clone Kowloon. You should make your own cool LED thing instead!)

Additionally, this project has some third-party dependencies. [See the third-party notice listing for details.](THIRD-PARTY-NOTICES.md)
