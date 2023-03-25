
using Management_System.Services;



    var menu = new MenuService();

    #region Menu

while (true)
    {
        Console.Clear();
        Console.WriteLine("1. Skapa ett nytt ärende");
        Console.WriteLine("2. Visa alla ärenden & kommentarer");
        Console.WriteLine("3. Visa ett specifikt ärende");
        Console.WriteLine("4. Uppdatera status på ett ärende");
        Console.WriteLine("5. Kommentera ett ärende.");
        Console.Write("Välj ett av följande alternativ (1-5): ");

    switch (Console.ReadLine())
    {
        case "1":
            Console.Clear();
            await menu.CreateNewCaseAsync();
            break;

        case "2":
            Console.Clear();
            await menu.GetAllCases();
            break;

        case "3":
            Console.Clear();
            await menu.GetSpecificCaseAsync();
            break;

        case "4":
            Console.Clear();
            await menu.UpdateCaseAsync();
            break;

        case "5":
            Console.Clear();
            await menu.CommentCaseAsync();
            break;

    }

    Console.WriteLine("\nTryck på valfri knapp för att avsluta...");
    Console.ReadKey();
}
    #endregion


