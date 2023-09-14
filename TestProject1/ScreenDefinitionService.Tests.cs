using SampleHierarchies.Services;
using Xunit;

namespace SampleHierarchies.Services.Tests.xUnit
{
    public class ScreenDefinitionServiceTests
    {
        [Fact]
        public void Load_ValidJsonFile_ReturnsScreenDefinition()
        {
            // Arrange
            var service = new ScreenDefinitionService();
            var validJsonFileName = "C:\\Users\\marti\\OneDrive\\Рабочий стол\\src — kopia\\SampleHierarchies.App\\valid.json"; 

            // Act
            var screenDefinition = ScreenDefinitionService.Load(validJsonFileName);

            // Assert
            Assert.NotNull(screenDefinition);
            
        }

        [Fact]
        public void Load_InvalidJsonFile_ReturnsNull()
        {
            // Arrange
            var service = new ScreenDefinitionService();
            var invalidJsonFileName = "C:\\Users\\marti\\OneDrive\\Рабочий стол\\src — kopia\\SampleHierarchies.App\\invalid.json";

            // Act
            var screenDefinition = ScreenDefinitionService.Load(invalidJsonFileName);

            // Assert
            Assert.Null(screenDefinition);
        }

        [Fact]
        public void Display_ValidJsonFileAndLineIndex_SuccessfullyDisplays()
        {
            // Arrange
            var service = new ScreenDefinitionService();
            var validJsonFileName = "valid.json"; 
            var lineIndex = 0; 

            // Act & Assert
            Assert.NotNull(() => service.Display(validJsonFileName, lineIndex));
            
        }
    }
}