class Entity
{
    public int lv;
    public int cap = 3;
    public Card[] hand;
    public int maxHp = 1;
    public int curHp = 1;
    public int sol = 2;
    public int lun = 2;
    public int con = 2;

    public Entity()
    {
        hand = new Card[cap];
        curHp = maxHp;
    }
}