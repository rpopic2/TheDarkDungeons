namespace Entities;
public record Item(string Name, ItemType itemType, IBehaviour[] skills)
{
    public override string ToString()
    {
        string temp = itemType == ItemType.Equip ? "()" : "<>";
        return temp.Insert(1, Name);
    }
    public void ForEach<T>(Action<T> action) where T : IBehaviour
    {
        var behav = skills.OfType<T>();
        foreach (T wear in behav) action(wear);
    }
}
public partial class Fightable
{
    public static readonly NonTokenSkill Stun = new("기절", StanceName.Charge, "은 기절 상태이다!", (f, x, y) => { }, (i) => { });
    public static readonly Item basicActions = new("기본", ItemType.Equip, new NonTokenSkill[]{
        new("이동", StanceName.Charge, string.Empty, (f, x, y)=>
            f.Move(new(x, (Facing)y)), (f)=>{}),
        new("숨고르기",StanceName.Charge, "은 숨을 골랐다.", (f,x,y)=>{
            f.GainEnergy(1);
        }, (f)=>{}),
        new("상호작용", StanceName.Charge, string.Empty, (f,x,y)=>{
            if(f is Player player) player.Interact();
        },(f)=>{})
    });
    public static readonly Item bareHand = new("맨손", ItemType.Equip, new Skill[] {
        new("주먹질", StanceName.Offence, StatName.Sol, DamageType.Normal, "은 주먹을 휘둘렀다.", (i)=>i.Attack(1)),
        new("구르기", StanceName.Defence, StatName.Lun, DamageType.Thrust, "은 옆으로 굴렀다.", (i)=>{})
        });
    public static readonly Skill sommersault = new("공중제비", StanceName.Charge, StatName.None, DamageType.None, "은 화려하게 뒤로 한 바퀴 공중제비 넘어 착지했다.", (f) => { f.Move(new(-2, f.Pos.facing)); });
    public static readonly Item assBareHand = bareHand with { Name = "암살자의 손", skills = new IBehaviour[] { bareHand.skills[0], bareHand.skills[1], sommersault } };
    public static readonly Item sword = new("검", ItemType.Equip, new Skill[] {
        new("베기", StanceName.Offence, StatName.Sol, DamageType.Slash, "은 칼을 휘둘러 앞을 베었다.", (i)=>i.Attack(1)),
        new("칼로막기", StanceName.Defence, StatName.Sol, DamageType.Slash, "은 칼로 막기 자세를 취했다.", (i)=>{})
        });
    public static readonly Item holySword = new("광란의 신성검", ItemType.Equip, new IBehaviour[] {
        new Skill("베기", StanceName.Offence, StatName.Sol, DamageType.Slash, "은 칼을 휘둘러 앞을 베었다.", (i)=>i.Attack(1)),
        new Charge("광란의기도", StatName.Con, DamageType.Magic, "은 미친 듯이 기도하였고 칼이 빛나기 시작했다.", (i)=>{i.Charge(holySword!);})
        });
    public static readonly Item staff = new("지팡이", ItemType.Equip, new IBehaviour[] {
        new Skill("때리기", StanceName.Offence, StatName.Sol, DamageType.Normal, "은 지팡이로 앞을 떄렸다.", (i)=>i.Attack(1)),
        new Charge("별빛부름", StatName.Con, DamageType.Magic, "은 신비한 별빛을 불러내어 지팡이를 휘감았다.", (i)=>{i.Charge(staff!);})
        });
    public static readonly Item spiritStaff = new("정령 깃든 지팡이", ItemType.Equip, new IBehaviour[] {
        new Skill("휘두르기", StanceName.Offence, StatName.Lun, DamageType.Normal, "은 지팡이를 휘둘렀다.", (i)=>i.Attack(4)),
        new Charge("정령부름", StatName.Con, DamageType.Magic, "은 정령을 불러내었고 그 힘이 지팡이에 깃들었다.", (i)=>{i.Charge(spiritStaff!);})
        });
    public static readonly Item magicBook = new("마법책", ItemType.Equip, new IBehaviour[]{
        new Charge("화염부름", StatName.Con, DamageType.Magic, "가 마법책에 쓰인 주문을 외우자 허공에 화염이 나타났다.", (f)=>{f.Charge();}),
    });
    public static readonly Item dagger = new("단검", ItemType.Equip, new Skill[] {
        new("휘두르기", StanceName.Offence,  StatName.Sol, DamageType.Slash, "은 단검을 휘둘렀다.", (i)=>i.Attack(1)),
        new("투검", StanceName.Offence, StatName.Lun, DamageType.Thrust, "은 적을 향해 단검을 던졌다.", (i)=>i.Throw(3, dagger!))
    });
    public static readonly Item bow = new("활", ItemType.Equip, new Skill[] {
        new("쏘기", StanceName.Offence, StatName.Lun, DamageType.Thrust, "은 활시위를 당겼다가 놓았다.", (i)=>i.Throw(3, arrow!))});
    public static readonly Item shield = new("방패", ItemType.Equip, new Skill[]{
        new("방패밀기", StanceName.Offence, StatName.Sol, DamageType.Normal, "은 방패를 앞으로 세게 밀쳤다.", (i)=>i.Attack(1)),
        new("방패막기", StanceName.Defence, StatName.Sol, DamageType.Slash, "은 방패로 공격을 막았다.", (i)=>i.Status.AddAmount(2))
    });
    public static readonly Item arrow = new("화살", ItemType.Consume, new IBehaviour[] { });
    public static readonly Item batItem = new("박쥐의 날개", ItemType.Equip, new Skill[] {
        new("들이박기", StanceName.Offence, StatName.Lun, DamageType.Normal, "는 갑자기 당신의 얼굴로 날아들어 부딪혔다!", (i)=>{i.Attack(1); i.Stat.Damage(1);}),
        new("구르기", StanceName.Defence, StatName.Lun, DamageType.Thrust, "는 가벼운 날개짓으로 옆으로 피했다.", (i)=>{})
        });
    public static readonly Item snakeItem = new("뱀의 이빨", ItemType.Equip, new IBehaviour[] {
        new Charge("독니", StatName.None, DamageType.Normal, "의 하악 소리가 울려퍼지며 위협적인 이빨을 드러냈다. 독이 흐르는 듯 하다.", (i)=>i.PoisonItem(snakeItem!)),
        new Skill("물기", StanceName.Offence, StatName.Sol, DamageType.Slash, "은 그 커다란 이빨로 적을 깨물었다!", (i)=>i.Attack(1))
    });
    public static Skill bite = new Skill("물기", StanceName.Offence, StatName.Sol, DamageType.Slash, "은 그 커다란 이빨로 적을 깨물었다!", (i) => i.Attack(1));

    public static readonly Item ratItem = new("쥐의 이빨", ItemType.Equip, new IBehaviour[]{
            bite, new Skill("돌진", StanceName.Offence, StatName.Sol, DamageType.Normal, "는 찍찍 소리를 내며 거대한 이빨을 드러내고 있다. 쥐가 달려든다!", (i)=>{i.Dash(new Position(2, i.Pos.facing));i.Attack(2);i._lastHit?.Status.SetStun(1);})
            });
    public static readonly Item poison = new("독", ItemType.Consume, new IBehaviour[] {
        new Charge("독 바르기", StatName.None, DamageType.Normal, "은 독을 무기에 바르기로 했다.", (f)=>{f.PoisonItem(); f.Inven.Consume(poison!);})
    });
    public static readonly Item tearOfLun = new("달의 눈물", ItemType.Consume, new Consume[]{
        new("사용한다", StanceName.Charge, "은 포션을 상처 부위에 떨어뜨렸고, 이윽고 상처가 씻은 듯이 아물었다.", (p)=>p.Stat.Heal(3))
    });
    public static readonly Item boneOfTheDeceased = new("망자의 뼈", ItemType.Consume, new Passive[]{
        new("망자의 뼈", StanceName.None, "죽은 뼈지만 마치 살아 움직이는 듯한 기분이 든다.", (f)=>{
            if(!f.IsAlive)
            {
                f.Inven.Consume(boneOfTheDeceased!);
                f.IsAlive = true;
                f.Stat.Heal(f.GetHp().Max);
                Map.Depth --;
                Map.NewMap();
                IO.rk($"{f.Name}은 망자의 뼈로 부활하였다.");
            }})
    });
    private const int TORCH_BRIGHTNESS = 2;
    private const int TORCH_DURATION = 30;
    public static readonly Item torch = new("횃불", ItemType.Consume, new IBehaviour[]{
        new Skill("휘두르기", StanceName.Offence, StatName.Sol, DamageType.Normal, "횃불을 휘둘렀다.", (i)=>i.Attack(1)),
        new WearEffect("밝음", StanceName.None, "횃불이 활활 타올라 앞을 비추고 있다.", (p)=>{p.Stat.AddSight(TORCH_BRIGHTNESS);p.Inven.GetMeta(torch!).stack=TORCH_DURATION;}, (p)=>p.Stat.ResetSight()),
        new Passive("꺼져가는 횃불", StanceName.None, "횃불은 언젠가는 꺼질 것이다.", (p)=>{p.Inven.Consume(torch!);})
    });
}