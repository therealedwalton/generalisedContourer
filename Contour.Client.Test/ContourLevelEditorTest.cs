using AngleSharp.Dom;
using Bunit;
using Contour.Client.Components;
using Contour.Core;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contour.Client.Test
{
    public class ContourLevelEditorTest : TestContext
    {
        public ContourLevelEditorTest() : base()
        {
            JSInterop.Mode = JSRuntimeMode.Loose;
            Services.AddMudServices(options => { options.PopoverOptions.CheckForPopoverProvider = false; });
        }

        [Fact]
        public void ShouldRender()
        {
            var cut = RenderComponent<ContourLevelEditor>(parameters => parameters.Add(x => x.ContourLevels, CreateContours(2)));

            // Act
            var table = cut.Find(".mud-table-root");

            // Assert
            Assert.NotNull(table);
        }

        [Fact]
        public void ShouldDisplayContours()
        {
            // Arrange
            var contourLevels = CreateContours(2);
            var cut = RenderComponent<ContourLevelEditor>(parameters => parameters.Add(x => x.ContourLevels, contourLevels));

            // Act
            var valueInputs = cut.FindAll(".contour-value input");
            var colourInputs = cut.FindAll(".contour-colour input");

            //// Assert
            Assert.Equal(contourLevels.Count, valueInputs.Count);
            Assert.Equal(contourLevels.Count, colourInputs.Count);

            for (var i = 0; i < contourLevels.Count; i++)
            {
                Assert.Equal(contourLevels[i].Value.ToString(), valueInputs[i].GetAttribute("value"));
                Assert.Equal(contourLevels[i].Colour, colourInputs[i].GetAttribute("value"));
            }
        }

        [Fact]
        public void UserCanEditContours()
        {
            // Arrange
            var contourLevels = CreateContours(1);
            var cut = RenderComponent<ContourLevelEditor>(parameters => parameters.Add(x => x.ContourLevels, contourLevels));

            // Act
            var valueInput = cut.Find(".contour-value input");
            var colourInput = cut.Find(".contour-colour input");

            var newValue = contourLevels[0].Value + 10;
            var newColour = "#ff000000";

            valueInput.Change(newValue);
            colourInput.Change(newColour);

            // Assert
            Assert.Equal(newValue, contourLevels[0].Value);
            Assert.Equal(newColour, contourLevels[0].Colour);
        }

        [Fact]
        public void UserCanAddAContour()
        {
            // Arrange
            var initialPoints = 1;
            var contourLevels = CreateContours(initialPoints);
            var cut = RenderComponent<ContourLevelEditor>(parameters => parameters.Add(x => x.ContourLevels, contourLevels));

            // Act
            var addButton = cut.Find(".add-contour-button");
            addButton.Click();

            // Assert
            Assert.Equal(initialPoints + 1, contourLevels.Count);
        }

        [Fact]
        public void UserCanDeleteAContour()
        {
            // Arrange
            var initialPoints = 2;
            var contourLevels = CreateContours(initialPoints);
            var cut = RenderComponent<ContourLevelEditor>(parameters => parameters.Add(x => x.ContourLevels, contourLevels));

            // Act
            var addButton = cut.Find(".delete-contour-button");
            addButton.Click();

            // Assert
            Assert.Equal(initialPoints - 1, contourLevels.Count);
        }

        private List<ContourLevel> CreateContours(int numberOfContours)
        {
            var contourLevels = new List<ContourLevel>();

            for (int i = 0; i < numberOfContours; i++)
            {
                contourLevels.Add(new ContourLevel() { Value = i * 10, Colour = $"#ff000{i}ff"});
            }

            return contourLevels;
        }
    }
}
