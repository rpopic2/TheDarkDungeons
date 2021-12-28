pr("The Dark Dungeon ver 0.1\nPress any key to start...");
Console.ReadKey();

Console.Clear();
//Console.BackgroundColor = ConsoleColor.White;
Console.ForegroundColor = ConsoleColor.DarkRed;

Charactor charactor = new Charactor();

pr("Choose charactor`s name...");
charactor.charName = Console.ReadLine() ?? "Michael";

pr("Choose your class...");
sel(new string[] = ("Warrior", "Assassin", "Mage"));
char temp = Console.ReadKey().KeyChar;

charactor.GainExp(1);
charactor.PrintStats();

void pr(object x)
{
    Console.WriteLine(x);
}

void sel(string[] options)
{
    string printResult = "/";
    foreach (string item in options)
    {
        string tempItem = item;
        item.Insert(0, "(");
        item.Insert(2, ")");
        printResult += $" {tempItem} /";
    }
    pr(printResult);
}