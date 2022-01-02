public class Entity : IMass
{
    public int lv = 0;
    public int cap = 3;
    public Hand hand;
    public Hp hp;
    public int sol {get; set;}
    public int lun {get; set;}
    public int con {get; set;}

    public Entity()
    {
        hand = new Hand(cap);
        hp = new Hp(this, 5);
    }
    public void Death()
    {

    }
}