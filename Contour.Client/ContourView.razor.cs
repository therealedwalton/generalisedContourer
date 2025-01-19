using Contour.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contour.Client
{
    public partial class ContourView
    {
        [Parameter]
        public List<Point> Points { get; set; } = new List<Point>();

        IEnumerable<Triangle> Triangles { get; set; } = new List<Triangle>();

        protected override async Task OnParametersSetAsync()
        {
            if (Points?.Count > 0)
            {
                var triangulation = new DelaunayTriangulation(Points);

                Triangles = await triangulation.Triangulate();
            }
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
