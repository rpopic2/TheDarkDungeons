using System.Collections.Generic;
pr("The Dark Dungeon ver 0.1\nPress any key to start...");
Console.ReadKey();

Console.Clear();
//Console.BackgroundColor = ConsoleColor.White;
// Console.ForegroundColor = ConsoleColor.DarkRed;

Charactor charactor = new Charactor();

pr("Choose charactor`s name...");
charactor.charName = Console.ReadLine() ?? "Michael";

pr("Choose your class...");
charactor.charClass = (ClassName)sel(new string[] { "(W)arrior", "(A)ssassin", "(M)age" });

charactor.GainExp(1);
charactor.PrintStats();

pr("Your adventure begins...");
DoAction();

void DoAction()
{
    int action = sela(new string[] { "(R)est", "(A)ttack", "S(w)itch Stance..." }, new Action[] { () => Rest(), () => Attack(), () => SwitchStance() });
}

void Rest()
{
    pr("\nRested a turn.");
    charactor.Draw();
    pr(charactor.Hands);
}

void Attack()
{
    pr("Atk");
}

void SwitchStance()
{
    pr(charactor.Hands);
    string index = Console.ReadKey().KeyChar.ToString();
    if (int.TryParse(index, out int x))
    {
        charactor.FlipStanceAt(x - 1);
    }
    else
    {
        SwitchStance();
    }
    pr(charactor.Hands);
}

void pr(object x)
{
    Console.WriteLine(x);
}

int sel(string[] options)
{
    string printResult = "/";
    char[] keys = new char[options.Length];

    for (int i = 0; i < options.Length; i++)
    {
        int beforeIndex = options[i].IndexOf('(');
        keys[i] = Char.ToLower(options[i][beforeIndex + 1]);
    }

    foreach (string item in options)
    {
        string tempItem = item;
        printResult += $" {tempItem} /";
    }
    pr(printResult);
    char key = Console.ReadKey().KeyChar;
    int indexOf = (Array.IndexOf<char>(keys, Char.ToLower(key)));
    if (indexOf != -1)
    {
        return indexOf;
    }
    return sel(options);
}
int sela(string[] options, Action[] actions)
{
    int result = sel(options);
    actions[result]();
    DoAction();
    return result;
}