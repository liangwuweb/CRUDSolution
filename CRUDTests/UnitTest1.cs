namespace CRUDTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            //Arrange
            MyMath m = new MyMath();

            //Act
            Assert.Equal(15, m.Add(10, 5));

            //Assert
        }
    }
}