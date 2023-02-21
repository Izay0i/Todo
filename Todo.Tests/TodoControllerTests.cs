using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Moq;
using Xunit;

using Todo.Models;
using Todo.Repository;

namespace Todo.Tests
{
    public class TodoControllerTests
    {
        private readonly Mock<IRepository<TodoItem>> _repo;

        private void _SetUpMocks()
        {
            _repo.Setup(re => re.GetAll()).Returns(new List<TodoItem> 
            { 
                new TodoItem { Name = "Hello", Description = "World", },
                new TodoItem { Name = "Foo", Description = "Bar", },
            });
        }

        public TodoControllerTests()
        {
            _repo = new Mock<IRepository<TodoItem>>();
            _SetUpMocks();
        }

        [Fact]
        public void GetAll()
        {
            //Arrange

            //Act
            var result = _repo.Object.GetAll().ToList();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(1)]
        public void GetById(int id)
        {
            //Arrange
            _repo.Setup(re => re.GetById(It.IsAny<int>())).Returns(
                id == 1 ? new TodoItem() : null
                );

            //Act
            var result = _repo.Object.GetById(id);

            //Assert
            if (id == -1)
            {
                Assert.Null(result);
            }
            else
            {
                Assert.NotNull(result);
            }
        }
    }
}
