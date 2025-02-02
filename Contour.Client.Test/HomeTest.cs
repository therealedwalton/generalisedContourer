using Blazored.LocalStorage;
using Bunit;
using Contour.Client.Components;
using Contour.Client.Views;
using Contour.Core;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MudBlazor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contour.Client.Test
{
    public class HomeTest : TestContext
    {
        private Mock<ILocalStorageService> mockLocalStorage { get; set; }

        public HomeTest() : base()
        {
            mockLocalStorage = new Mock<ILocalStorageService>();

            JSInterop.Mode = JSRuntimeMode.Loose;
            Services.AddMudServices(options => { options.PopoverOptions.CheckForPopoverProvider = false; });
            Services.AddScoped((x) => mockLocalStorage.Object);
        }

        [Fact]
        public void ShouldRenderContourAndPointEditor()
        {
            // Arrange
            var cut = RenderComponent<Home>();

            // Act
            var contourView = cut.FindComponent<ContourView>();
            var pointsEditor = cut.FindComponent<PointsEditor>();

            // Assert
            Assert.NotNull(contourView);
            Assert.NotNull(pointsEditor);
        }

        [Fact]
        public void ShouldGenerateRandomPointsIfNoLocalStorage()
        {
            // Arrange
            mockLocalStorage.Setup(x => x.GetItemAsync<List<Point>>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<List<Point>?>(new List<Point>()));

            var cut = RenderComponent<Home>();

            // Act
            var contourView = cut.FindComponent<ContourView>();
            var pointsEditor = cut.FindComponent<PointsEditor>();

            // Assert
            Assert.True(contourView.Instance.Points.Count > 0);
            Assert.True(pointsEditor.Instance.Points.Count > 0);
        }

        [Fact]
        public void ShouldGenerateContourLevelsIfNoLocalStorage()
        {
            // Arrange
            mockLocalStorage.Setup(x => x.GetItemAsync<List<ContourLevel>>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<List<ContourLevel>?>(new List<ContourLevel>()));

            var cut = RenderComponent<Home>();

            // Act
            ClickTab(cut, "Contours");

            var contourLevelView = cut.FindComponent<ContourLevelEditor>();

            // Assert
            Assert.True(contourLevelView.Instance.ContourLevels.Count > 0);
        }

        [Fact]
        public void ShouldReturnStoredPointsIfAvailable()
        {
            // Arrange
            var points = new List<ValuePoint>() { new ValuePoint(1,2,1), new ValuePoint(3, 4, 2), new ValuePoint(5, 6, 10) };

            mockLocalStorage.Setup(x => x.GetItemAsync<List<ValuePoint>>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<List<ValuePoint>?>(points));

            var cut = RenderComponent<Home>();

            // Act
            var contourView = cut.FindComponent<ContourView>();
            var pointsEditor = cut.FindComponent<PointsEditor>();

            // Assert
            Assert.Equal(points, contourView.Instance.Points);
            Assert.Equal(points, pointsEditor.Instance.Points);
        }

        [Fact]
        public void ShouldReturnStoredContourLevelsIfAvailable()
        {
            // Arrange
            var contourLevels = new List<ContourLevel>() { new ContourLevel(), new ContourLevel() };

            mockLocalStorage.Setup(x => x.GetItemAsync<List<ContourLevel>>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<List<ContourLevel>?>(contourLevels));

            var cut = RenderComponent<Home>();

            // Act
            ClickTab(cut, "Contours");

            var contourLevelView = cut.FindComponent<ContourLevelEditor>();

            // Assert
            Assert.Equal(contourLevels, contourLevelView.Instance.ContourLevels);
        }

        private void ClickTab(IRenderedComponent<Home> cut, string tabName)
        {
            var contourTab = cut.FindAll(".mud-tab").FirstOrDefault(x => x.InnerHtml == tabName);
            contourTab?.Click();
        }
    }
}
