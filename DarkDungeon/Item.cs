namespace Entities;
public record Item(string name, ItemType itemType, IBehaviour[] skills)
{
    public override string ToString()
    {
        string temp = itemType == ItemType.Equip ? "()" : "<>";
        return temp.Insert(1, name);
    }
    public WearEffect[]? GetWearEffects()
    {
        return (WearEffect[]?)(from p in skills where p is WearEffect select p);
    }
}
public partial class Fightable
{
    public static NonTokenSkill Stun = new("기절", StanceName.Charge, "은 기절 상태이다!", (f, x, y) => { }, (i) => { });
    public static readonly Item basicActions = new("기본)", ItemType.Equip, new NonTokenSkill[]{
        new("이동", StanceName.Charge, string.Empty, (f, x, y)=>
            f.Move(new(x, (Facing)y)), (f)=>{}),
        new("숨고르기",StanceName.Charge, "은 숨을 골랐다.", (f,x,y)=>{
            if(f is Player p) p.PickupToken(1);
            else f.PickupToken((TokenType)x, y);
        }, (f)=>{}),
        new("상호작용", StanceName.Charge, string.Empty, (f,x,y)=>{
            if(f is Player player) player.Interact();
        },(f)=>{})
    });
    public static readonly Item bareHand = new("맨손", ItemType.Equip, new Skill[] {
        new("주먹질", StanceName.Offence, TokenType.Offence, StatName.Sol, DamageType.Normal, "은 주먹을 휘둘렀다.", (i)=>i.Throw(1)),
        new("구르기", StanceName.Offence, TokenType.Defence, StatName.Lun, DamageType.Thrust, "은 옆으로 굴렀다.", (i)=>{})
        });
    public static readonly Item sword = new("검", ItemType.Equip, new Skill[] {
        new("베기", StanceName.Offence, TokenType.Offence, StatName.Sol, DamageType.Slash, "은 칼을 휘둘러 앞을 베었다.", (i)=>i.Throw(1)),
        new("칼로막기", StanceName.Defence, TokenType.Defence, StatName.Sol, DamageType.Slash, "은 칼로 막기 자세를 취했다.", (i)=>{})
        });
    public static readonly Item holySword = new("광란의 신성검", ItemType.Equip, new Skill[] {
        new("베기", StanceName.Offence, TokenType.Offence, StatName.Sol, DamageType.Slash, "은 칼을 휘둘러 앞을 베었다.", (i)=>i.Throw(1)),
        new("광란의기도", StanceName.Charge, TokenType.Charge, StatName.Con, DamageType.Magic, "은 미친 듯이 기도하였고 칼이 빛나기 시작했다.", (i)=>{})
        });
    public static readonly Item staff = new("지팡이", ItemType.Equip, new Skill[] {
        new("휘두르기", StanceName.Offence, TokenType.Offence, StatName.Sol, DamageType.Normal, "은 지팡이를 휘둘렀다.", (i)=>i.Throw(1)),
        new("별빛부름", StanceName.Charge, TokenType.Charge, StatName.Con, DamageType.Magic, "은 신비한 별빛을 불러내어 지팡이를 휘감았다.", (i)=>{})
        });
    public static readonly Item dagger = new("단검", ItemType.Equip, new Skill[] {
        new("휘두르기", StanceName.Offence,  TokenType.Offence, StatName.Sol, DamageType.Slash, "은 단검을 휘둘렀다.", (i)=>i.Throw(1)),
        new("투검", StanceName.Offence, TokenType.Offence, StatName.Lun, DamageType.Thrust, "은 적을 향해 단검을 던졌다.", (i)=>{i.Throw(3); i.lastHit?.Inven.Add(dagger!); i.Inven.Remove(dagger!);})
    });
    public static readonly Item bow = new("활", ItemType.Equip, new Skill[] {
        new("쏘기", StanceName.Offence, TokenType.Offence, StatName.Sol, DamageType.Thrust, "은 활시위를 당겼다가 놓았다.", (i)=>{
            if(i.Inven.Contains(arrow)) {i.Throw(3) ; i.Inven.Remove(arrow!);}
            else{ i.Stance.Reset(); IO.rk("화살이 없다!");}
            }),
    });
    public static readonly Item shield = new("방패", ItemType.Equip, new Skill[]{
        new("방패밀기", StanceName.Offence, TokenType.Offence, StatName.Sol, DamageType.Normal, "은 방패를 앞으로 세게 밀쳤다.", (i)=>i.Throw(1)),
        new("방패막기", StanceName.Defence, TokenType.Defence, StatName.Sol, DamageType.Slash, "은 방패로 공격을 막았다.", (i)=>i.Stance.AddAmount(2))
    });
    public static readonly Item arrow = new("화살", ItemType.Consume, new IBehaviour[] { });
    public static readonly Item batItem = new("박쥐", ItemType.Equip, new Skill[] {
        new("들이박기", StanceName.Offence, TokenType.Offence, StatName.Lun, DamageType.Normal, "는 갑자기 당신의 얼굴로 날아들어 부딪혔다!", (i)=>{i.Throw(1); i.Hp -= 1;}),
        new("구르기", StanceName.Defence, TokenType.Defence, StatName.Lun, DamageType.Thrust, "는 가벼운 날개짓으로 옆으로 피했다.", (i)=>{})
        });
    public static readonly Item tearOfLun = new("달의 눈물", ItemType.Consume, new Consume[]{
        new("사용한다", StanceName.Charge, "은 포션을 상처 부위에 떨어뜨렸고, 이윽고 상처가 씻은 듯이 아물었다.", (p)=>p.Hp += 3)
    });
    public static readonly Item boneOfTheDeceased = new("망자의 뼈", ItemType.Consume, new Passive[]{
        new("망자의 뼈", StanceName.None, "죽은 뼈지만 마치 살아 움직이는 듯한 기분이 든다.", (f)=>{
            if(!f.IsAlive) 
            {
                f.Inven.Consume(boneOfTheDeceased!); 
                f.IsAlive = true; 
                f.Hp += f.Hp.Max; 
                Map.level --; 
                Map.NewMap();
                IO.rk($"{f.Name}은 부활하였다.");
            }})
    });
    private const int TORCH_BRIGHTNESS = 2;
    public static readonly Item torch = new("횃불", ItemType.Equip, new IBehaviour[]{
        new Skill("휘두르기", StanceName.Offence, TokenType.Offence, StatName.Sol, DamageType.Normal, "횃불을 휘둘렀다.", (i)=>i.Throw(1)),
        new WearEffect("밝음", StanceName.None, "횃불이 활활 타올라 앞을 비추고 있다.", (p)=>{p.Sight+=TORCH_BRIGHTNESS;p.Inven.GetMeta(torch!).stack=15;}, (p)=>p.Sight-=TORCH_BRIGHTNESS),
        new Passive("꺼져가는 횃불", StanceName.None, "횃불은 언젠가는 꺼질 것이다.", (p)=>{p.Inven.Consume(torch!);})
    });
}