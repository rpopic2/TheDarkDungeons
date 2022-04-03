using Entities;
using Xunit;

public class CombatTest
{
    [Fact]
    public void Movement()
    {
        Player._instance = new("Tester", ClassName.Warrior);
        Player player = Player.instance;
        Map map = new(5, false);
        Assert.Equal(new(0, 0), player.Pos);

        //move foward
        player.Move(1);
        Assert.Equal(new(1, 0), player.Pos);

        //move backward
        player.Move(-1);
        Assert.Equal(new(0, 1, Facing.Back), player.Pos);

        //cannot move further than 0
        player.Move(-1);
        Assert.Equal(new(0, 1, Facing.Back), player.Pos);

        //moving further than max moves you to next room
        player.Move(4);
        Assert.Equal(new(0, 0), player.Pos);
    }
    [Fact]
    public void Combat()
    {
        Player._instance = new("Tester", ClassName.Warrior);
        Player player = Player.instance;
        Map map = new(5, false);
        map._Spawn(MonsterDb.lunatic, new(1, 1, Facing.Back));
        Monster mob = ((Monster)map.Moveables[1]!);

        player.UpdateTarget();
        Assert.Equal(mob, player.Target);

        player.tokens.Add(TokenType.Offence);
        try
        {
            player.SelectSkillAndUse(Item.bardHand, 0);
        }
        catch (System.InvalidOperationException)
        {

        }
        Assert.Equal(Stance.Offence, player.CurStance.stance);
        Assert.Equal(0, player.tokens.Count);

        try
        {
            player.TryAttack();
        }
        catch (System.InvalidOperationException)
        {

        }
        Assert.NotEqual(mob.Hp.Max, mob.Hp.Cur);
    }
}