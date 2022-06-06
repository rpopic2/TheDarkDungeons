using Xunit;

public class IOTest : TestTemplate
{
    [Fact]
    public void IOPressInvalidKeyDoesNotThrow()
    {
        IO2.Press('x');
    }
}