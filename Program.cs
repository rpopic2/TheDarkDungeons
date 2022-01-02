IO.pr("The Dark Dungeon ver 0.1\nPress any key to start...");
Console.ReadKey();

Console.Clear();


IO.pr("Choose charactor`s name...");
string name = Console.ReadLine() ?? "";

IO.pr("Choose your class...");
ClassName className = (ClassName)IO.sel(new string[] { "(W)arrior", "(A)ssassin", "(M)age" });

Charactor player = new Charactor(name, className);

player.exp.Gain(1);
IO.pr(player.Stats);

IO.pr("\nYour adventure begins...\n");
PromptAction();

///
void PromptAction()
{
    sela(new string[] { "(R)est" }, new Action[] { () => Rest() });
}

void Rest()
{
    IO.pr("Resting a turn.");
    player.Hand.Pickup(player.Draw());
    StanceShift();
}

void StanceShift()
{
    int result = sela(new string[] { "(C)ontinue", "(S)tanceshift" }, new Action[] { () => {PromptAction(); return;}, ()=>{player.Hand.StanceShift();
    StanceShift();} });

}

///<summary>select to actions
///selas(new string[] {}, new Action[] {});</summary>
int sela(string[] options, Action[] actions, bool retryOnFalldown = true)
{
    int result = IO.sel(options, retryOnFalldown);
    if (!retryOnFalldown) return result;
    actions[result]();
    PromptAction();
    return result;
}