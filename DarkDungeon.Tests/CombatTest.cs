using System.Collections.Generic;
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
    public void TestOnNewTurn()
    {
        //Setup
        Player player = _SetupPlayer();
        Monster mob = _SetupMonser(out Map map);

        //1. New Turn. update its targets
        _StartTurn(map);
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
    public void PlayerAtksMobAtks()
    {
        (Player player, Monster mob) = _SetupCombat(0, 0);
        Assert.NotEqual(mob.Hp.Max, mob.Hp.Cur);
        Assert.NotEqual(player.Hp.Max, player.Hp.Cur);
    }
    [Fact]
    public void PlayerAtksMobDefs()
    {
        (Player player, Monster mob) = _SetupCombat(0, 1);
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
    [Fact]
    public void PlayerDefsMobAtks()
    {
        (Player player, Monster mob) = _SetupCombat(1, 0);
        if (player.Stance.amount >= mob.Stance.amount)
        {
            Assert.Equal(player.Hp.Max, player.Hp.Cur);
        }
        else
        {
            Assert.NotEqual(player.Hp.Max, player.Hp.Cur);
        }
        Assert.Equal(mob.Hp.Max, mob.Hp.Cur);
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
        try { caster.SelectSkill(item, skill); } catch (System.InvalidOperationException) { }
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
        return ((Monster)map.MoveablePositions[1]!);
    }
    private void _StartTurn(Map map)
    {
        foreach (Fightable item in map.Fightables)
        {
            item.OnTurnEnd();
        }
    }
    private void _ElaspeTurn(Map map)
    {
        foreach (Fightable item in map.Fightables)
        {
            _TryAttack(item);
        }
    }
    private (Player, Monster) _SetupCombat(int playerSkill, int mobSkill)
    {
        Player player = _SetupPlayer();
        Monster mob = _SetupMonser(out Map map);
        _StartTurn(map);
        _SelectSkill(player, Item.bareHand, playerSkill);
        _SelectSkill(mob, Item.bareHand, mobSkill);
        _ElaspeTurn(map);
        return(player, mob);
    }
}