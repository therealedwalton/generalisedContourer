using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contour.Core;

namespace Contour.Client.Components
{
    public partial class ContourLevelEditor
    {
        [Parameter]
        public List<ContourLevel> ContourLevels { get; set; } = new();

        private void AddLevel()
        {
            ContourLevels.Add(new ContourLevel
            {
                Value = ContourLevels.Any()
                    ? ContourLevels.Max(l => l.Value) + 10
                    : 0,
                Colour = "#1976D2" // MudBlazor primary color
            });
            StateHasChanged();
            _ = NotifyLevelsChanged();
        }

        private void RemoveLevel(ContourLevel level)
        {
            if (level != null)
            {
                ContourLevels.Remove(level);
                StateHasChanged();
                _ = NotifyLevelsChanged();
            }
        }

        private void UpdateLevel(ContourLevel level)
        {
            StateHasChanged();
            _ = NotifyLevelsChanged();
        }

        private void UpdateColor(ContourLevel level)
        {
            StateHasChanged();
            _ = NotifyLevelsChanged();
        }

        [Parameter]
        public EventCallback<List<ContourLevel>> OnLevelsChanged { get; set; }

        private async Task NotifyLevelsChanged()
        {
            if (OnLevelsChanged.HasDelegate)
            {
                await OnLevelsChanged.InvokeAsync(ContourLevels);
            }
        }
    }
}
