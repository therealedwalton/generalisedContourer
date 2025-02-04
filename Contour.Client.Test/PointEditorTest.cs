using AngleSharp.Dom;
using Bunit;
using Contour.Client.Components;
using Contour.Core;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Services;

namespace Contour.Client.Test
{
    public class PointEditorTest : TestContext
    {
        public PointEditorTest() : base()
        {
            JSInterop.Mode = JSRuntimeMode.Loose;
            Services.AddMudServices();
        }

        [Fact]
        public void ShouldRender()
        {
            // Arrange
            var cut = RenderComponent<PointsEditor>(parameters => parameters.Add(x => x.Points, CreatePoints(2)));

            // Act
            var table = cut.Find(".mud-table-root");

            // Assert
            Assert.NotNull(table);
        }

        [Fact]
        public void ShouldDisplayPoints()
        {
            // Arrange
            var points = CreatePoints(2);
            var cut = RenderComponent<PointsEditor>(parameters => parameters.Add(x => x.Points, points));

            // Act
            var xInputs = cut.FindAll(".point-x input");
            var yInputs = cut.FindAll(".point-y input");
            var zInputs = cut.FindAll(".point-z input");

            // Assert
            Assert.Equal(points.Count, xInputs.Count);
            Assert.Equal(points.Count, yInputs.Count);
            Assert.Equal(points.Count, zInputs.Count);

            for (var i = 0; i < points.Count; i++)
            {
                Assert.Equal(points[i].x.ToString(), xInputs[i].GetAttribute("value"));
                Assert.Equal(points[i].y.ToString(), yInputs[i].GetAttribute("value"));
                Assert.Equal(points[i].z.ToString(), zInputs[i].GetAttribute("value"));
            }
        }

        [Fact]
        public void UserCanEditPoints()
        {
            // Arrange
            var points = CreatePoints(1);
            var cut = RenderComponent<PointsEditor>(parameters => parameters.Add(x => x.Points, points));

            // Act
            var xInput = cut.Find(".point-x input");
            var yInput = cut.Find(".point-y input");
            var zInput = cut.Find(".point-z input");

            var newXValue = points[0].x + 1;
            var newYValue = points[0].y + 1;
            var newZValue = points[0].z + 1;

            xInput.Change(newXValue);
            yInput.Change(newYValue);
            zInput.Change(newZValue);

            // Assert
            Assert.Equal(newXValue, points[0].x);
            Assert.Equal(newYValue, points[0].y);
            Assert.Equal(newZValue, points[0].z);
        }

        [Fact]
        public void UserCanAddAPoint()
        {
            // Arrange
            var initialPoints = 1;
            var points = CreatePoints(initialPoints);
            var cut = RenderComponent<PointsEditor>(parameters => parameters.Add(x => x.Points, points));

            // Act
            var addButton = cut.Find(".add-point-button");
            addButton.Click();

            // Assert
            Assert.Equal(initialPoints + 1, points.Count);
        }

        [Fact]
        public void UserCanDeleteAPoint()
        {
            // Arrange
            var initialPoints = 2;
            var points = CreatePoints(initialPoints);
            var cut = RenderComponent<PointsEditor>(parameters => parameters.Add(x => x.Points, points));

            // Act
            var addButton = cut.Find(".delete-point-button");
            addButton.Click();

            // Assert
            Assert.Equal(initialPoints - 1, points.Count);
        }

        [Fact]
        public void UserCanRequestAnUpdate()
        {
            // Arrange
            var updateCalled = false;
            var cut = RenderComponent<PointsEditor>(parameters => parameters.Add(x => x.UpdateRequest, x => updateCalled = true));

            // Act
            var addButton = cut.Find(".update-points-button");
            addButton.Click();

            // Assert
            Assert.True(updateCalled);
        }

        [Fact]
        public void UserCanClearPoints()
        {
            // Arrange
            var initialPoints = 2;
            var points = CreatePoints(initialPoints);
            var cut = RenderComponent<PointsEditor>(parameters => parameters.Add(x => x.Points, points));

            // Act
            var addButton = cut.Find(".clear-button");
            addButton.Click();

            // Assert
            Assert.Equal(0, points.Count);
        }

        private List<ValuePoint> CreatePoints(int numberOfPoints)
        {
            var points = new List<ValuePoint>();

            for (int i = 0; i < numberOfPoints; i++)
            {
                points.Add(new ValuePoint(i, i + 0.5, i + 2));
            }

            return points;
        }
    }
}
