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
        public List<ValuePoint> Points { get; set; } = new List<ValuePoint>();

        [Parameter]
        public EventCallback UpdateRequest { get; set; }

        [Parameter]
        public ValuePoint SelectedPoint { get; set; }

        private void AddPoint()
        {
            var newPoint = new ValuePoint(0, 0, 0);
            Points.Add(newPoint);
            SelectedPoint = newPoint;
        }

        private void DeletePoint(ValuePoint point)
        {
            Points.Remove(point);
            if (SelectedPoint == point)
            {
                SelectedPoint = null;
            }
        }

        private void SelectPoint(ValuePoint point)
        {
            SelectedPoint = point;
        }

        private async Task Update()
        {
            await UpdateRequest.InvokeAsync();
        }
    }
}
