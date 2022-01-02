public class Entity : IMass
{
    private int lv = 0;
    protected int cap = 3;
    private Hand hand;
    public Hand Hand { get; }
    private Hp hp;
    public Hp Hp { get; }
    public int sol { get; set; }
    public int lun { get; set; }
    public int con { get; set; }
    public int Lv { get => lv; protected set => lv = value; }
    protected Random rnd = new Random();

    public Entity()
    {
        hand = new Hand(cap);
        hp = new Hp(this, 5);
    }
    public virtual Card Draw()
        => new Card(rnd.Next(sol), rnd.Next(lun));
    public void OnDeath()
    {

    }
}