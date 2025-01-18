using Contour.Core;
using Microsoft.AspNetCore.Components;
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
    }
}
