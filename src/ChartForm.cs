using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.WinForms;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ElectricFieldVis
{
    public partial class ChartForm : Form
    {
        private Panel drawingPanel;
        private CartesianChart cartesianChart;
        public CartesianChart CartesianChart => cartesianChart;

        public ChartForm(List<float> values, List<int> time)
        {
            InitializeComponent();

            // Vytvoříme drawing panel
            drawingPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };
            this.Controls.Add(drawingPanel);

            // Inicializace grafu
            cartesianChart = new CartesianChart
            {
                Dock = DockStyle.Fill,
                ZoomMode = LiveChartsCore.Measure.ZoomAndPanMode.X,
            };
            drawingPanel.Controls.Add(cartesianChart);

            this.Text = "Intensity Over Time";
            this.Size = new Size(600, 400);

            UpdateChart(values, time);
        }

        public void UpdateChart(List<float> intensityValues, List<int> timeValues)
        {
            List<float> scaledIntensityValues = intensityValues.ConvertAll(value => value / 1_000_000);

            var lineSeries = new LineSeries<float>
            {
                Values = scaledIntensityValues,
                Fill = null,
                Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 2 },
                GeometryFill = new SolidColorPaint(SKColors.Blue),
                GeometryStroke = new SolidColorPaint(SKColors.Blue),
                Name = "Intensity",
                IsVisible = true,
                AnimationsSpeed = TimeSpan.Zero,
                GeometrySize = 0,
            };

            cartesianChart.Series = new ISeries[] { lineSeries };
            cartesianChart.XAxes = new Axis[]
            {
                new Axis
                {
                    Name = "Time (seconds)",
                    Labels = timeValues.ConvertAll(t => t.ToString()),
                    AnimationsSpeed = TimeSpan.Zero
                }
            };
            cartesianChart.YAxes = new Axis[]
            {
                new Axis
                {
                    Name = "Intensity (MN/C)",
                    Labeler = value => $"{value} MN/C",
                    AnimationsSpeed = TimeSpan.Zero
                }
            };
        }
    }
}