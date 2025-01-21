using Contour.Core;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contour.Client.Components
{
    public partial class PointsEditor
    {
        [Parameter]
        public List<Point> Points { get; set; } = new List<Point>();

        [Parameter]
        public EventCallback UpdateRequest { get; set; }

        [Parameter]
        public Point SelectedPoint { get; set; }

        private void AddPoint()
        {
            var newPoint = new Point(0, 0);
            Points.Add(newPoint);
            SelectedPoint = newPoint;
        }

        private void DeletePoint(Point point)
        {
            Points.Remove(point);
            if (SelectedPoint == point)
            {
                SelectedPoint = null;
            }
        }

        private void SelectPoint(Point point)
        {
            SelectedPoint = point;
        }

        private async Task Update()
        {
            await UpdateRequest.InvokeAsync();
        }
    }
}
