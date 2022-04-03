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

        try { player.UseSkill(Item.bareHand, 0); } catch (System.InvalidOperationException) { }
        try { mob.UseSkill(Item.bareHand, 0); } catch (System.InvalidOperationException) { }
        //check if player skill is used properly : stance changed, token deleted
        Assert.Equal(Stance.Offence, player.Stance.stance);
        Assert.InRange(player.Stance.amount, Stat.MIN, Player.BASICSTAT);
        Assert.Equal(0, player.tokens.Count);
        //check if mob skill is used properly.
        Assert.Equal(Stance.Offence, mob.Stance.stance);
        Assert.InRange(mob.Stance.amount, Stat.MIN, lunData.stat.sol);
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
        Player player = _SetupPlayer();
        Monster mob = _SetupMonser(out _);

        //1. New Turn. update its targets
        _StartTurn(new Fightable[] { player, mob });
        Assert.Equal(mob, player.Target);
        Assert.Equal(player, mob.Target);

        Assert.Equal(0, player.Stance.amount);
        Assert.Equal(Stance.None, player.Stance.stance);
        Assert.Equal(0, mob.Stance.amount);
        Assert.Equal(Stance.None, mob.Stance.stance);
    }
    [Fact]
    public void TestSkillUse()
    {
        _SetupPlayer();
        //2. Use Skills
        Player player = Player.instance;
        Item bareHand = Item.bareHand;

        _SelectSkill(player, bareHand, 0);
        Assert.Equal(Stance.Offence, player.Stance.stance);
        Assert.InRange(player.Stance.amount, Stat.MIN, player.GetStat(bareHand.skills[0].statName));
    }
    [Fact]
    public void PlayerAtksMobDefs()
    {
        Player player = _SetupPlayer();
        Monster mob = _SetupMonser(out _);
        _StartTurn(new Fightable[] { player, mob });
        _SelectSkill(player, Item.bareHand, 0);
        _SelectSkill(mob, Item.bareHand, 1);
        _ElaspeTurn(new Fightable[] { player, mob });
        Assert.Equal(Stance.Defence, mob.Stance.stance);
        if (mob.Stance.amount >= player.Stance.amount)
        {
            Assert.Equal(mob.Hp.Max, mob.Hp.Cur);
        }
        else
        {
            Assert.NotEqual(mob.Hp.Max, mob.Hp.Cur);
        }
        Assert.Equal(player.Hp.Max, player.Hp.Cur);
    }
    ///<summary> Always setup player first</summary>
    private Player _SetupPlayer()
    {
        Player._instance = new("Tester", ClassName.Warrior);
        Player player = Player.instance;
        player.tokens.Add(TokenType.Offence);
        player.tokens.Add(TokenType.Defence);
        return player;
    }
    private void _SelectSkill(Fightable caster, Item item, int skill)
    {
        try { caster.UseSkill(item, skill); } catch (System.InvalidOperationException) { }
    }
    private void _TryAttack(Fightable caster)
    {
        try { caster.TryAttack(); } catch (System.InvalidOperationException) { }
    }
    ///<summary> Always setup player first</summary>
    private Monster _SetupMonser(out Map map)
    {
        map = new(5, false);
        MonsterData lunData = MonsterDb.lunatic;
        map._Spawn(lunData, new(1, 1, Facing.Back));
        return ((Monster)map.Moveables[1]!);
    }
    private void _StartTurn(Fightable[] fights)
    {
        foreach (Fightable item in fights)
        {
            item.OnTurnEnd();
        }
    }
    private void _ElaspeTurn(Fightable[] fights)
    {
        foreach (Fightable item in fights)
        {
            _TryAttack(item);
        }
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