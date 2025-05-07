using Xunit;
using System.IO;
using System.Text;

namespace LoadTestTool.Tests
{
    public class ConfigTests
    {
        private string CreateTestIniFile()
        {
            var content = new StringBuilder();
            content.AppendLine("[Database]");
            content.AppendLine("Server=localhost");
            content.AppendLine("Port=5432");
            content.AppendLine("Username=admin");
            content.AppendLine("Password=secret");
            content.AppendLine("");
            content.AppendLine("[Logging]");
            content.AppendLine("Level=INFO");
            content.AppendLine("File=app.log");
            content.AppendLine("MaxSize=10MB");
            content.AppendLine("Debug=true");
            content.AppendLine("");
            content.AppendLine("DefaultKey=DefaultValue");

            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, content.ToString());
            return tempFile;
        }

        [Fact]
        public void Load_ValidIniFile_LoadsAllSettings()
        {
            // Arrange
            var tempFile = CreateTestIniFile();
            var config = new Config(tempFile);

            // Act
            config.Load(tempFile);

            // Assert
            Assert.Equal("localhost", config.Get("Server", "Database"));
            Assert.Equal(5432, config.GetInt("Port", "Database"));
            Assert.Equal("INFO", config.Get("Level", "Logging"));
            Assert.Equal("DefaultValue", config.Get("DefaultKey"));
            Assert.True(config.GetBool("Debug", "Logging"));

            // Cleanup
            File.Delete(tempFile);
        }

        [Fact]
        public void Get_NonExistentSection_ThrowsKeyNotFoundException()
        {
            // Arrange
            var tempFile = CreateTestIniFile();
            var config = new Config(tempFile);
            config.Load(tempFile);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => config.Get("Key", "NonExistentSection"));

            // Cleanup
            File.Delete(tempFile);
        }

        [Fact]
        public void Get_NonExistentKey_ThrowsKeyNotFoundException()
        {
            // Arrange
            var tempFile = CreateTestIniFile();
            var config = new Config(tempFile);
            config.Load(tempFile);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => config.Get("NonExistentKey", "Database"));

            // Cleanup
            File.Delete(tempFile);
        }

        [Fact]
        public void GetInt_InvalidInteger_ThrowsFormatException()
        {
            // Arrange
            var tempFile = CreateTestIniFile();
            var config = new Config(tempFile);
            config.Load(tempFile);

            // Act & Assert
            Assert.Throws<FormatException>(() => config.GetInt("Level", "Logging"));

            // Cleanup
            File.Delete(tempFile);
        }

        [Fact]
        public void HasKey_ExistingKey_ReturnsTrue()
        {
            // Arrange
            var tempFile = CreateTestIniFile();
            var config = new Config(tempFile);
            config.Load(tempFile);

            // Act & Assert
            Assert.True(config.HasKey("Server", "Database"));
            Assert.True(config.HasKey("DefaultKey"));

            // Cleanup
            File.Delete(tempFile);
        }

        [Fact]
        public void HasKey_NonExistentKey_ReturnsFalse()
        {
            // Arrange
            var tempFile = CreateTestIniFile();
            var config = new Config(tempFile);
            config.Load(tempFile);

            // Act & Assert
            Assert.False(config.HasKey("NonExistentKey", "Database"));
            Assert.False(config.HasKey("NonExistentKey"));

            // Cleanup
            File.Delete(tempFile);
        }

        [Fact]
        public void HasSection_ExistingSection_ReturnsTrue()
        {
            // Arrange
            var tempFile = CreateTestIniFile();
            var config = new Config(tempFile);
            config.Load(tempFile);

            // Act & Assert
            Assert.True(config.HasSection("Database"));
            Assert.True(config.HasSection("Logging"));

            // Cleanup
            File.Delete(tempFile);
        }

        [Fact]
        public void HasSection_NonExistentSection_ReturnsFalse()
        {
            // Arrange
            var tempFile = CreateTestIniFile();
            var config = new Config(tempFile);
            config.Load(tempFile);

            // Act & Assert
            Assert.False(config.HasSection("NonExistentSection"));

            // Cleanup
            File.Delete(tempFile);
        }

        [Fact]
        public void GetKeys_ExistingSection_ReturnsAllKeys()
        {
            // Arrange
            var tempFile = CreateTestIniFile();
            var config = new Config(tempFile);
            config.Load(tempFile);

            // Act
            var dbKeys = config.GetKeys("Database");

            // Assert
            Assert.Contains("Server", dbKeys);
            Assert.Contains("Port", dbKeys);
            Assert.Contains("Username", dbKeys);
            Assert.Contains("Password", dbKeys);

            // Cleanup
            File.Delete(tempFile);
        }

        [Fact]
        public void GetSections_ReturnsAllSections()
        {
            // Arrange
            var tempFile = CreateTestIniFile();
            var config = new Config(tempFile);
            config.Load(tempFile);

            // Act
            var sections = config.GetSections();

            // Assert
            Assert.Contains("Database", sections);
            Assert.Contains("Logging", sections);

            // Cleanup
            File.Delete(tempFile);
        }

        [Fact]
        public void Set_NewKey_AddsKeyToSection()
        {
            // Arrange
            var tempFile = CreateTestIniFile();
            var config = new Config(tempFile);
            config.Load(tempFile);

            // Act
            config.Set("NewKey", "NewValue", "Database");

            // Assert
            Assert.Equal("NewValue", config.Get("NewKey", "Database"));

            // Cleanup
            File.Delete(tempFile);
        }

        [Fact]
        public void Set_NewSection_CreatesSection()
        {
            // Arrange
            var tempFile = CreateTestIniFile();
            var config = new Config(tempFile);
            config.Load(tempFile);

            // Act
            config.Set("Key", "Value", "NewSection");

            // Assert
            Assert.True(config.HasSection("NewSection"));
            Assert.Equal("Value", config.Get("Key", "NewSection"));

            // Cleanup
            File.Delete(tempFile);
        }

        [Fact]
        public void Save_WritesAllSettingsToFile()
        {
            // Arrange
            var tempFile = CreateTestIniFile();
            var config = new Config(tempFile);
            config.Load(tempFile);

            // Act
            config.Set("NewKey", "NewValue", "Database");
            config.Save();

            // Assert
            var newConfig = new Config(tempFile);
            newConfig.Load(tempFile);
            Assert.Equal("NewValue", newConfig.Get("NewKey", "Database"));

            // Cleanup
            File.Delete(tempFile);
        }
    }
} 