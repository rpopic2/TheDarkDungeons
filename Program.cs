IO.pr("The Dark Dungeon ver 0.1\nPress any key to start...");
Console.ReadKey();

Console.Clear();

Charactor charactor = new Charactor();

IO.pr("Choose charactor`s name...");
charactor.charName = Console.ReadLine() ?? "Michael";

IO.pr("Choose your class...");
charactor.charClass = (ClassName)IO.sel(new string[] { "(W)arrior", "(A)ssassin", "(M)age" });

charactor.GainExp(1);
charactor.PrintStats();

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
    charactor.hand.Pickup(charactor.Draw());

    sela(new string[] { "(C)ontinue", "Change Card (S)tance" }, new Action[] { () => PromptAction(), () => charactor.hand.StanceShift() });
}

///<summary>select to actions
///selas(new string[] {}, new Action[] {});</summary>
int sela(string[] options, Action[] actions)
{
    int result = IO.sel(options);
    actions[result]();
    PromptAction();
    return result;
}