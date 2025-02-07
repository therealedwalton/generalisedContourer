using Blazored.LocalStorage;
using Contour.Client.Components;
using Contour.Core;
using Microsoft.AspNetCore.Components;

namespace Contour.Client.Views
{
    public partial class Home
    {
        [Inject]
        private ILocalStorageService localStorageService { get; set; }

        List<ValuePoint> points = new List<ValuePoint>();

        List<ContourLevel> contourLevels = new List<ContourLevel>();

        ContourView contourView;

        protected override async Task OnInitializedAsync()
        {
            await InitializePoints();
            await InitializeContours();
        }

        private async Task InitializePoints()
        {
            points = await localStorageService.GetItemAsync<List<ValuePoint>>("contourPoints");

            if (points == null || points.Count == 0)
            {
                var random = new Random();
                var minValue = 0;
                var maxValue = 100;

                var gaussian = new GaussianDistribution2D(new Point(GenerateRangedRandom(random, minValue, maxValue), GenerateRangedRandom(random, minValue, maxValue)), 100, 20, 20);

                List<Func<double, double, double>> funcs = new List<Func<double, double, double>>()
                {
                    (x, y) => gaussian.Value(x, y),
                };

                points = new List<ValuePoint>();

                for (int i = 0; i < 20; i++)
                {
                    double x = GenerateRangedRandom(random, minValue, maxValue);
                    double y = GenerateRangedRandom(random, minValue, maxValue);
                    double z = 0;

                    foreach (var func in funcs)
                    {
                        z += func(x, y);
                    }

                    points.Add(new ValuePoint(x, y, z));
                }
            }
        }

        private double GenerateRangedRandom(Random random, double minValue, double maxValue)
        {
            return minValue + (random.NextDouble() * (maxValue - minValue));
        }

        private async Task InitializeContours()
        {
            contourLevels = await localStorageService.GetItemAsync<List<ContourLevel>>("contourLevels");

            if (contourLevels == null || contourLevels.Count == 0)
            {
                contourLevels = new List<ContourLevel>();

                var colours = new string[] { "#9400d3", "#4b0082", "#0000ff", "#00ff00", "#ffff00", "#ff7f00", "#ff0000" };

                for (int i = 1; i < 8; i++)
                {
                    contourLevels.Add(new ContourLevel() { Value = i * 10, Colour = colours[i - 1] });
                }
            }
        }

        protected void UpdatePlot()
        {
            localStorageService?.SetItemAsync("contourPoints", points);
            localStorageService?.SetItemAsync("contourLevels", contourLevels);
            contourView?.UpdatePlot();
        }
    }
}
