# Electric Field Visualizer

**Interactive 2D simulation of electric fields with real-time intensity measurement.**

![C#](https://img.shields.io/badge/C%23-239120?style=flat&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET_8-512BD4?style=flat&logo=dotnet&logoColor=white)
![Windows Forms](https://img.shields.io/badge/WinForms-0078D4?style=flat&logo=windows&logoColor=white)
![SkiaSharp](https://img.shields.io/badge/SkiaSharp-003366?style=flat)
![LiveCharts](https://img.shields.io/badge/LiveCharts2-FF6600?style=flat)
![SVG.NET](https://img.shields.io/badge/SVG.NET-green?style=flat)

---

## Description

A Windows desktop app that renders electric field vectors and heat maps for user-defined point charges. You can drag charges around, change their strength with the scroll wheel, place a measurement probe anywhere on the canvas, and watch the field intensity chart update live. Built as a semester project for the KIV/UPG course at the University of West Bohemia.

The physics follows Coulomb's law — each charge pushes or pulls the field, vectors at every grid cell show direction, and the colour gradient (red = positive, blue = negative) gives a quick read on field strength.

---

## Screenshots

**Scenario 0 — Single positive charge**
![Scenario 0: single charge at origin with orbiting probe](screenshots/scenario0_single_charge.png)

**Scenario 1 — Two equal positive charges (repulsion)**
![Scenario 1: two positive charges repelling each other](screenshots/scenario1_two_positive.png)

**Scenario 2 — Opposite charges (attraction)**
![Scenario 2: negative charge left, stronger positive right](screenshots/scenario2_opposite_charges.png)

**Scenario 3 — Quadrupole**
![Scenario 3: four charges at corners with varying strength](screenshots/scenario3_quadrupole.png)

**Scenario 4 — Dynamic oscillating charges**
![Scenario 4: dynamic charges with sine-wave oscillation](screenshots/scenario4_dynamic.png)

**Intensity over time chart (static probe)**
![Scenario 4 with live Intensity vs Time chart in separate window](screenshots/scenario4_intensity_chart.png)

---

## Demo

> GitHub renders these inline — click the play button.

**Overview — adding and moving charges**

<video src="screenshots/magnets.mp4" controls width="100%"></video>

**Scenario 1 — Repulsion (two equal positive charges)**

<video src="videos/demo_scenario1_repulsion.mp4" controls width="100%"></video>

**Scenario 2 — Dipole (opposite charges)**

<video src="videos/demo_scenario2_dipole.mp4" controls width="100%"></video>

**Scenario 3 — Quadrupole**

<video src="videos/demo_scenario3_quadrupole.mp4" controls width="100%"></video>

**Scenario 4 — Dynamic oscillating charges**

<video src="videos/demo_scenario4_dynamic.mp4" controls width="100%"></video>

**Switching between scenarios**

<video src="videos/demo_scenario_switching.mp4" controls width="100%"></video>

**Adding a charge at runtime**

<video src="videos/demo_add_magnet.mp4" controls width="100%"></video>

**Intensity over time chart (static probe)**

<video src="videos/demo_intensity_chart.mp4" controls width="100%"></video>

---

## Features

- **5 built-in scenarios** — single charge, dipole, asymmetric pair, quadrupole, dynamic oscillating charges
- **Field vector grid** — arrows at every grid cell pointing in the direction of the net electric field
- **Heat map** — radial colour gradients centred on each charge (red/blue for polarity)
- **Circular probe** — orbits the field automatically at configurable speed, shows the current field vector and intensity in MN/C
- **Static probe** — click anywhere to drop a measurement point; a live `Intensity vs Time` chart opens in a separate window
- **Interactive charges** — drag to reposition, scroll wheel to increase/decrease charge force (polarity flips when force hits minimum)
- **Add / remove charges** — toolbar buttons; the last clicked charge is the one that gets removed
- **Dynamic mode** (scenario 4) — charge forces oscillate with a sine function over time
- **Configurable grid spacing** — pass `--WxH` on the command line (e.g. `--50x50`)
- **SVG export** — saves the current canvas (and chart if open) as a vector image
- **Speed controls** — cycle probe speed and dynamic charge speed between 0.5×, 1×, 2×
- **Reset button** — returns the current scenario to its original state

---

## Tech Stack

| Library | Used for |
|---|---|
| `LiveChartsCore.SkiaSharpView.WinForms` | Real-time intensity chart |
| `SkiaSharp` | Rendering backend for LiveCharts |
| `SVG.NET` | SVG export |
| `System.Drawing` (GDI+) | All custom canvas rendering |

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Windows (WinForms is Windows-only)

### Build & Run

```bash
# Clone the repo
git clone https://github.com/MatejSulic/electric-field-visualizer-csharp.git
cd electric-field-visualizer-csharp

# Build (Windows)
Build.cmd

# Run with default scenario (scenario 0)
Run.cmd

# Run a specific scenario (0–4)
.\bin\ElectricFieldVis.exe 2

# Run with custom grid spacing (pixels per cell), e.g. 50×50
.\bin\ElectricFieldVis.exe 1 --50x50
```

### Scenarios

| ID | Description |
|---|---|
| 0 | Single positive charge at origin |
| 1 | Two equal positive charges (repulsion) |
| 2 | Opposite charges — negative left, stronger positive right |
| 3 | Quadrupole — four charges at corners with varying strength |
| 4 | Dynamic — two charges whose force oscillates with a sine wave |

---

## Project Structure

```
.
├── src/
│   ├── Program.cs               # Entry point, CLI argument parsing
│   ├── MainForm.cs              # Main window, event handlers, animation loop
│   ├── DrawingFunctions.cs      # All GDI+ rendering (magnets, vectors, heatmap, probes, legend)
│   ├── CalculationsFunctions.cs # Physics — Coulomb-based field intensity and vector math
│   ├── MagnetLogic.cs           # Scenario definitions, dynamic charge oscillation
│   ├── Magnet.cs                # Data model: position, polarity, force
│   ├── ChartForm.cs             # Separate window with live intensity chart (LiveCharts2)
│   ├── DrawingPanel.cs          # Custom panel subclass
│   └── ElectricFieldVis.csproj  # Project file, NuGet dependencies
├── bin/                         # Compiled output (populated by Build.cmd)
├── doc/                         # Project documentation (PDF)
├── screenshots/                 # PNG screenshots of all scenarios
├── videos/                      # MP4 demo recordings
├── Build.cmd                    # Windows build script (dotnet msbuild)
├── Run.cmd                      # Windows run script
├── Build.sh                     # Linux build script (mono/mcs — legacy)
└── Run.sh                       # Linux run script (mono)
```

---

## What I Learned

- **GDI+ coordinate transforms** — centering the origin, flipping Y, and keeping transforms consistent across nested draw calls is easy to get wrong; saving and restoring `g.Transform` everywhere was the fix.
- **Separating rendering from logic** — pulling all drawing code into `DrawingFunctions.cs` and all physics into `CalculationsFunctions.cs` made the codebase much easier to navigate than having everything in `MainForm`.
- **WinForms double-buffering** — enabling it via reflection (`DoubleBuffered = true` on the panel) eliminated flicker almost entirely; without it the animation was unusable.
- **LiveCharts2 in WinForms** — the library is designed with MVVM in mind, wiring it into a plain WinForms window required some workarounds (e.g. setting `AnimationsSpeed = TimeSpan.Zero` to avoid lag when updating data rapidly).
- **SVG export by pixel** — the naive approach of iterating every pixel and emitting an `SvgRectangle` per pixel works but produces enormous files. Good enough for a semester project, but a proper vector export would need to emit actual geometric primitives.

---

## Author

**Matěj Šulič**

[![GitHub](https://img.shields.io/badge/GitHub-MatejSulic-181717?style=flat&logo=github)](https://github.com/MatejSulic)
[![LinkedIn](https://img.shields.io/badge/LinkedIn-matej--sulic-0A66C2?style=flat&logo=linkedin)](https://linkedin.com/in/matej-sulic)
[![Email](https://img.shields.io/badge/Email-sul.matej%40gmail.com-D14836?style=flat&logo=gmail)](mailto:sul.matej@gmail.com)

---

## License

MIT — see [LICENSE](LICENSE)
