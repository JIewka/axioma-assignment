using AxiomaAssignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Test
{
    public class NodeTests
    {
        [Fact]
        public void IsColumnNode_ReturnsTrue_WhenOnlyColumnNameIsSet()
        {
            // Arrange
            var node = new Node
            {
                ColumnName = "Name"
            };

            // Act
            var result = node.IsColumnNode();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsColumnNode_ReturnsFalse_WhenColumnNameAndOperatorAreSet()
        {
            // Arrange
            var node = new Node
            {
                ColumnName = "Name",
                Operator = OperatorEnum.EQ
            };

            // Act
            var result = node.IsColumnNode();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValueNode_ReturnsTrue_WhenOnlyValueIsSet()
        {
            // Arrange
            var node = new Node
            {
                Value = "John"
            };

            // Act
            var result = node.IsValueNode();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValueNode_ReturnsFalse_WhenValueAndOperatorAreSet()
        {
            // Arrange
            var node = new Node
            {
                Value = "John",
                Operator = OperatorEnum.EQ
            };

            // Act
            var result = node.IsValueNode();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsOperatorNode_ReturnsTrue_WhenOnlyOperatorIsSet()
        {
            // Arrange
            var node = new Node
            {
                Operator = OperatorEnum.EQ
            };

            // Act
            var result = node.IsOperatorNode();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsOperatorNode_ReturnsFalse_WhenOperatorAndValueAreSet()
        {
            // Arrange
            var node = new Node
            {
                Operator = OperatorEnum.EQ,
                Value = "John"
            };

            // Act
            var result = node.IsOperatorNode();

            // Assert
            Assert.False(result);
        }
    }
}
