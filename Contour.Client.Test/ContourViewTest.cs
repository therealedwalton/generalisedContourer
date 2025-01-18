using Bunit;
using Contour.Client;

namespace Contour.Client.Test
{
    public class IconComponentTests : TestContext
    {
        [Fact]
        public void ShouldRenderSvg()
        {
            // Arrange
            var cut = RenderComponent<ContourView>();

            // Act
            var svg = cut.Find("svg");

            // Assert
            Assert.NotNull(svg);
        }
    }
}