namespace OldPhonePad.Tests
{
    public class OldPhonePadTests
    {
        [Theory]
        [InlineData("33#", "E")]
        [InlineData("227*#", "B")]
        [InlineData("4433555 555666#", "HELLO")]
        [InlineData("8 88777444666*664#", "TURING")]
        public void Should_Convert_Examples_From_Spec(string input, string expected)
        {
            var result = OldPhonePadConverter.OldPhonePad(input);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("2 2#", "AA")]          // pause allows same-key letters
        [InlineData("***#", "")]            // backspaces at start shouldn't break
        [InlineData("22 222#", "BC")]       // two groups on same key
        [InlineData("7777#", "S")]          // boundary: last letter on '7'
        [InlineData("999999#", "X")]        // 6 taps on '9' => (6-1)%4=1 => 'X'
        public void Should_Handle_Common_Edge_Cases(string input, string expected)
        {
            var result = OldPhonePadConverter.OldPhonePad(input);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Unknown_Digits_Are_Ignored_By_Default()
        {
            var result = OldPhonePadConverter.OldPhonePad("101#");
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void Zero_As_Space_When_Enabled()
        {
            var opts = new OldPhonePadOptions { ZeroAsSpace = true };
            var result = OldPhonePadConverter.OldPhonePad("20 20#", opts);
            // '2' -> A, pause, '0' -> ' ' (space), pause, '2' -> A, '0' -> ' '
            Assert.Equal("A A ", result);
        }
    }
}