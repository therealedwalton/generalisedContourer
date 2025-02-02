using Contour.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contour.Client.Components
{
    public partial class ContourView
    {
        [Parameter]
        public List<ValuePoint> Points { get; set; } = new List<ValuePoint>();

        [Parameter]
        public List<ContourLevel> ContourLevels { get; set; } = new List<ContourLevel>();

        public List<ContourLevelData> ContourLines { get; set; } = new List<ContourLevelData>();

        public Func<double, double> MappingY { get; private set; } = y => -y;

        IEnumerable<Triangle<Point>> Triangles { get; set; } = new List<Triangle<Point>>();

        Point ViewboxOrigin { get; set; } = new Point(0, -100);

        Point ViewboxSize { get; set; } = new Point(100, 100);

        double paddingPercentage = 0.1;
        double typicalElementSize = 0.2;

        protected override async Task OnParametersSetAsync()
        {
            await UpdatePlot();
        }

        public async Task UpdatePlot()
        {
            if (Points?.Count > 0)
            {
                var triangulation = new DelaunayTriangulation(Points.ConvertAll(x => x as Point));

                Triangles = await triangulation.Triangulate();

                var generator = new ContourGenerator();

                foreach (var level in ContourLevels)
                {
                    var levelLines = await generator.GenerateContour(Triangles.Select(x => new Triangle<ValuePoint>(x.Vertices.Select(x => x as ValuePoint).ToList())).ToList(), level.Value);

                    if (levelLines.Count > 0)
                    {
                        ContourLines.Add(new ContourLevelData { Edges = levelLines, Level = level });
                    }
                }

                CalculateViewBox();
            }
        }

        protected void CalculateViewBox()
        {
            var minX = Points.Min(p => p.x);
            var minY = MappingY(Points.Max(p => p.y));

            var width = Points.Max(p => p.x) - minX;
            var height = MappingY(Points.Min(p => p.y)) - minY;

            typicalElementSize = Math.Max(width, height) * 0.005;

            ViewboxOrigin = new Point(minX - paddingPercentage * width, minY - paddingPercentage * height);
            ViewboxSize = new Point(width + 2 * paddingPercentage * width, height + 2 * paddingPercentage * width);
        }

        //https://www.petercollingridge.co.uk/tutorials/svg/interactive/dragging/
        //https://github.com/KristofferStrube/Blazor.SVGEditor
        //protected void HandleMouseDown(MouseEventArgs args, object obj)
        //{

        //}

        //protected void HandleMouseMove(MouseEventArgs args, object obj)
        //{

        //}

        //protected void HandleMouseLeave(MouseEventArgs args, object obj)
        //{

        //}
    }
}
