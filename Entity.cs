class Entity : IMass
{
    public int lv;
    public int cap = 3;
    public Hand hand;
    public int maxHp = 1;
    public int curHp = 1;
    public int sol {get; set;}
    public int lun {get; set;}
    public int con {get; set;}

    public Entity()
    {
        hand = new Hand(cap);
        curHp = maxHp;
    }
}