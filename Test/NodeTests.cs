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
        public void TestIfIsColumnNodeReturnsTrue()
        {
            // arrange
            var node = Node.CreateColumnNameNode("Name");

            // act
            var result = node.IsColumnNode();

            // assert
            Assert.True(result);
        }

        [Fact]
        public void TestIfIsValueNodeReturnsTrue()
        {
            // arrange
            var node = Node.CreateValueNode("John");

            // act
            var result = node.IsValueNode();

            // assert
            Assert.True(result);
        }

        [Fact]
        public void TestIfIsOperatorNodeReturnsTrue()
        {
            // arrange
            var node = Node.CreateOperatorNode(OperatorEnum.EQ, Node.CreateColumnNameNode("Name"),
                Node.CreateValueNode("John"));

            // act
            var result = node.IsOperatorNode();

            // assert
            Assert.True(result);
        }
    }
}
