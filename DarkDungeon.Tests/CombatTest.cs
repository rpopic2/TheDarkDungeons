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
    public void PlayerAtksMobAtks()
    {
        //Setup
        Player._instance = new("Tester", ClassName.Warrior);
        Player player = Player.instance;
        Map map = new(5, false);
        MonsterData lunData = MonsterDb.lunatic;
        map._Spawn(lunData, new(1, 1, Facing.Back));
        Monster mob = ((Monster)map.Moveables[1]!);
        //update its targets
        player.UpdateTarget();
        mob.UpdateTarget();
        Assert.Equal(mob, player.Target);
        Assert.Equal(player, mob.Target);
        //Give player and monster a token
        player.tokens.Add(TokenType.Offence);
        Assert.Equal(1, player.tokens.Count);
        //Mobs gets full 2 tokens on spawn
        Assert.Equal(lunData.stat.cap, mob.tokens.Count);

        //use a skill : 맨손 - 주먹질
        try { player.SelectSkillAndUse(Item.bardHand, 0); } catch (System.InvalidOperationException) { }
        try { mob.SelectSkillAndUse(Item.bardHand, 0); } catch (System.InvalidOperationException) { }

        //check if skill is used properly : stance changed, token deleted
        Assert.Equal(Stance.Offence, player.CurStance.stance);
        Assert.InRange(player.CurStance.amount, Stat.MIN, Player.BASICSTAT);
        Assert.Equal(0, player.tokens.Count);

        Assert.Equal(Stance.Offence, mob.CurStance.stance);
        Assert.InRange(mob.CurStance.amount, Stat.MIN, lunData.stat.sol);
        Assert.Equal(lunData.stat.cap - 1, mob.tokens.Count);

        //Perform attack
        Assert.Equal(mob.Hp.Max, mob.Hp.Cur);
        try { player.TryAttack(); } catch (System.InvalidOperationException) { }

        Assert.Equal(player.Hp.Max, player.Hp.Cur);
        try { mob.TryAttack(); } catch (System.InvalidOperationException) { }

        Assert.NotEqual(mob.Hp.Max, mob.Hp.Cur);
        Assert.NotEqual(player.Hp.Max, player.Hp.Cur);
    }
}