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
        //Give player a token
        player.tokens.Add(TokenType.Offence);
        Assert.Equal(1, player.tokens.Count);
        //Mobs gets full 2 tokens on spawn
        Assert.Equal(lunData.stat.cap, mob.tokens.Count);

        //1. New Turn. update its targets
        player.OnTurnEnd();
        mob.OnTurnEnd();
        Assert.Equal(mob, player.Target);
        Assert.Equal(player, mob.Target);

        //2. Select Behaviour use a skill : 맨손 - 주먹질

        try { player.SelectSkillAndUse(Item.bardHand, 0); } catch (System.InvalidOperationException) { }
        try { mob.SelectSkillAndUse(Item.bardHand, 0); } catch (System.InvalidOperationException) { }
        //check if player skill is used properly : stance changed, token deleted
        Assert.Equal(Stance.Offence, player.CurStance.stance);
        Assert.InRange(player.CurStance.amount, Stat.MIN, Player.BASICSTAT);
        Assert.Equal(0, player.tokens.Count);
        //check if mob skill is used properly.
        Assert.Equal(Stance.Offence, mob.CurStance.stance);
        Assert.InRange(mob.CurStance.amount, Stat.MIN, lunData.stat.sol);
        Assert.Equal(lunData.stat.cap - 1, mob.tokens.Count);

        //3. Perform selected behaviour : attack
        Assert.Equal(player.Hp.Max, player.Hp.Cur);
        Assert.Equal(mob.Hp.Max, mob.Hp.Cur);
        try { player.TryAttack(); } catch (System.InvalidOperationException) { }
        try { mob.TryAttack(); } catch (System.InvalidOperationException) { }
        //check damage taken
        Assert.NotEqual(mob.Hp.Max, mob.Hp.Cur);
        Assert.NotEqual(player.Hp.Max, player.Hp.Cur);
    }
    [Fact]
    public void TestOnNewTurn()
    {
        //Setup
        _SetupPlayer();
        Player player = Player.instance;
        Map map = new(5, false);
        MonsterData lunData = MonsterDb.lunatic;
        map._Spawn(lunData, new(1, 1, Facing.Back));
        Monster mob = ((Monster)map.Moveables[1]!);
        //Give player 2 tokens. Mob gets 3 tokens on spawn

        //1. New Turn. update its targets
        player.OnTurnEnd();
        mob.OnTurnEnd();
        Assert.Equal(mob, player.Target);
        Assert.Equal(player, mob.Target);

        Assert.Equal(0, player.CurStance.amount);
        Assert.Equal(Stance.None, player.CurStance.stance);
        Assert.Equal(0, mob.CurStance.amount);
        Assert.Equal(Stance.None, mob.CurStance.stance);
    }
    [Fact]
    public void TestSelectSkillAndUse()
    {
        _SetupPlayer();
        Player player = Player.instance;
        Skill fist = Item.bardHand.skills[0];
        try { player.SelectSkillAndUse(Item.bardHand, 0); } catch (System.InvalidOperationException) { }
        Assert.Equal(Stance.Offence, player.CurStance.stance);
        Assert.InRange(player.CurStance.amount, Stat.MIN, player.GetStat(fist.statName));
    }
    private void _SetupPlayer()
    {
        Player._instance = new("Tester", ClassName.Warrior);
        Player player = Player.instance;
        player.tokens.Add(TokenType.Offence);
        player.tokens.Add(TokenType.Defence);
    }
    // [Fact]
    // public void PlayerAtksMobDefs()
    // {
    //     //Setup
    //     Player._instance = new("Tester", ClassName.Warrior);
    //     Player player = Player.instance;
    //     Map map = new(5, false);
    //     MonsterData lunData = MonsterDb.lunatic;
    //     map._Spawn(lunData, new(1, 1, Facing.Back));
    //     Monster mob = ((Monster)map.Moveables[1]!);
    //     //Give player 2 tokens. Mob gets 3 tokens on spawn
    //     player.tokens.Add(TokenType.Offence);
    //     player.tokens.Add(TokenType.Defence);

    //     //1. New Turn. update its targets
    //     player.OnTurnEnd();
    //     mob.OnTurnEnd();
    //     Assert.Equal(mob, player.Target);
    //     Assert.Equal(player, mob.Target);
    //     Assert.Equal(default, player.CurStance);
    //     Assert.Equal(default, mob.CurStance);

    //     //2. Select Behaviour use a skill : 맨손 - 주먹질

    //     try { player.SelectSkillAndUse(Item.bardHand, 0); } catch (System.InvalidOperationException) { }
    //     try { mob.SelectSkillAndUse(Item.bardHand, 0); } catch (System.InvalidOperationException) { }
    //     //check if player skill is used properly : stance changed, token deleted
    //     Assert.Equal(Stance.Offence, player.CurStance.stance);
    //     Assert.InRange(player.CurStance.amount, Stat.MIN, Player.BASICSTAT);
    //     //check if mob skill is used properly.
    //     Assert.Equal(Stance.Offence, mob.CurStance.stance);
    //     Assert.InRange(mob.CurStance.amount, Stat.MIN, lunData.stat.sol);

    //     //3. Perform selected behaviour : attack
    //     Assert.Equal(player.Hp.Max, player.Hp.Cur);
    //     Assert.Equal(mob.Hp.Max, mob.Hp.Cur);
    //     try { player.TryAttack(); } catch (System.InvalidOperationException) { }
    //     try { mob.TryAttack(); } catch (System.InvalidOperationException) { }
    //     //check damage taken
    //     Assert.NotEqual(mob.Hp.Max, mob.Hp.Cur);
    //     Assert.NotEqual(player.Hp.Max, player.Hp.Cur);
    // }
}