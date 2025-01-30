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

        ContourView contourView;

        protected override async Task OnInitializedAsync()
        {
            points = await localStorageService.GetItemAsync<List<ValuePoint>>("contourPoints");

            if (points == null || points.Count == 0)
            {
                var random = new Random();
                var minValue = 0;
                var maxValue = 100;

                points = new List<ValuePoint>();

                for (int i = 0; i < 20; i++)
                {
                    double x = minValue + (random.NextDouble() * (maxValue - minValue));
                    double y = minValue + (random.NextDouble() * (maxValue - minValue));
                    double z = minValue + (random.NextDouble() * (maxValue - minValue));
                    points.Add(new ValuePoint(x, y, z));
                }
            }
        }

        protected void UpdatePlot()
        {
            localStorageService?.SetItemAsync("contourPoints", points);
            contourView?.UpdatePlot();
        }
    }
}
