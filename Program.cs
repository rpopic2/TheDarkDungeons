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

IO.pr("Your adventure begins...");
PromptAction();

///

void PromptAction()
{
    int action = selas(new string[] { "(R)est" }, new Action[] { () => Rest()});
}

void Rest()
{
    IO.pr("Resting a turn.");
    IO.pr("You've found an card.");
    charactor.Draw();
    selas(new string[] {"(C)ontinue", "Change Card (S)tance"}, new Action[] {()=>PromptAction(), ()=>SwitchStance()});
}

void SwitchStance()
{
    charactor.StanceShift();
    // IO.pr(charactor.Hands);
    // string index = Console.ReadKey().KeyChar.ToString();
    // if (int.TryParse(index, out int x))
    // {
    //     charactor.FlipStanceAt(x - 1);
    // }
    // else
    // {
    //     SwitchStance();
    // }
    // IO.pr(charactor.Hands);
}

//selas(new string[] {}, new Action[] {});
int selas(string[] options, Action[] actions)
{
    int result = IO.sel(options);
    actions[result]();
    PromptAction();
    return result;
}