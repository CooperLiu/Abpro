using Xunit;
using Shouldly;

namespace Abpro.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            1.ShouldNotBe(2);
        }
    }
}
