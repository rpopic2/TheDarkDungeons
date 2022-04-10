// using Xunit;
// using Entities;
// using System.Linq;

// public class UnitTest
// {
//     [Fact]
//     public void TokenCreationTest()
//     {
//         Tokens tokens = new(3);
//         tokens.Add(TokenType.Offence);
//         Assert.Equal("토큰 : (", tokens.ToString());
//         tokens.Add(TokenType.Defence);
//         Assert.Equal("토큰 : ( [", tokens.ToString());
//         tokens.Add(TokenType.Charge);
//         Assert.Equal("토큰 : ( [ <", tokens.ToString());
//         tokens.Remove(TokenType.Defence);
//         tokens.Add(TokenType.Charge);
//         Assert.Equal("토큰 : ( < <", tokens.ToString());

//     }
//     [Fact]
//     public void TokenPickup()
//     {
//         Player player = new("TestPlayer");
//         player.tokens.Add(TokenType.Offence);
//         Assert.Equal((byte)TokenType.Offence, player.tokens[0]);
//         Assert.NotEqual((byte)TokenType.Defence, player.tokens[0]);
//         player.tokens.Remove(TokenType.Offence);
//         Assert.Equal(0, player.tokens.Count);

//         player.tokens.Add((byte)0);
//         Assert.Equal(1, player.tokens.Count);
//         player.tokens.RemoveAt(0);
//         Assert.Equal(0, player.tokens.Count);

//     }
//     [Fact]
//     public void TokenCapExceedTest()
//     {
//         Tokens token = new(2);
//         token.Add(TokenType.Offence);
//         token.Add(TokenType.Offence);
//         Assert.Throws<System.IndexOutOfRangeException>(() => token.Add(TokenType.Offence));
//     }
//     [Fact]
//     public void SkillStructTest()
//     {
//         //Skill skill = new("주먹질", TokenType.Offence, StatName.Sol, "Test!");
//         //Assert.Equal("(주먹질)", skill.ToString());
//         //Assert.NotEqual("[주먹질]", skill.ToString());
//     }
// }