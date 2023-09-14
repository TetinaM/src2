using SampleHierarchies.Data.Mammals;
using SampleHierarchies.Enums;
using SampleHierarchies.Interfaces.Data;
using SampleHierarchies.Interfaces.Services;
using SampleHierarchies.Services;
using System.IO;

namespace SampleHierarchies.Gui;

/// <summary>
/// Mammals main screen.
/// </summary>
public sealed class CatsScreen : Screen
{
    #region Properties And Ctor

    private readonly ScreenDefinitionService _screenDefinitionService;
    /// <summary>
    /// Settings
    /// </summary>
    private readonly ISettings _settings;


    /// <summary>
    /// Data service.
    /// </summary>
    private IDataService _dataService;

    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="dataService">Data service reference</param>
    public CatsScreen(IDataService dataService, ISettings settings, ScreenDefinitionService screenDefinitionService)
    {
        _screenDefinitionService = screenDefinitionService;
        _settings = settings;
        _dataService = dataService;
    }

    #endregion Properties And Ctor

    #region Public Methods

    /// <inheritdoc/>
    public override void Show()
    {
        //konwersja koloru z ciągu znaków na ConsoleColor
        Console.ForegroundColor = ConvertColorNameToConsoleColor(_settings.CatsScreenColor);

        while (true)
        {
            Console.WriteLine();
            _screenDefinitionService.Display(jsonFileNameCats, 0);
            _screenDefinitionService.Display(jsonFileNameCats, 1);
            _screenDefinitionService.Display(jsonFileNameCats, 2);
            _screenDefinitionService.Display(jsonFileNameCats, 3);
            _screenDefinitionService.Display(jsonFileNameCats, 4);
            _screenDefinitionService.Display(jsonFileNameCats, 5);
            _screenDefinitionService.Display(jsonFileNameCats, 6);

            string? choiceAsString = Console.ReadLine();

            // Validate choice
            try
            {
                if (choiceAsString is null)
                {
                    throw new ArgumentNullException(nameof(choiceAsString));
                }

                CatsScreenChoices choice = (CatsScreenChoices)Int32.Parse(choiceAsString);
                switch (choice)
                {
                    case CatsScreenChoices.List:
                        ListCats();
                        break;

                    case CatsScreenChoices.Create:
                        AddCat(); break;

                    case CatsScreenChoices.Delete:
                        DeleteCat();
                        break;

                    case CatsScreenChoices.Modify:
                        EditCatMain();
                        break;

                    case CatsScreenChoices.Exit:
                        _screenDefinitionService.Display(jsonFileNameCats, 7);
                        return;
                }
            }
            catch
            {
                _screenDefinitionService.Display(jsonFileNameCats, 8);
            }
        }
    }
    #endregion // Public Methods

    #region Private Methods

    /// <summary>
    /// List all dogs.
    /// </summary>
    private void ListCats()
    {
        Console.WriteLine();
        if (_dataService?.Animals?.Mammals?.Cats is not null &&
            _dataService.Animals.Mammals.Cats.Count > 0)
        {
            _screenDefinitionService.Display(jsonFileNameCats, 9);
            int i = 1;
            foreach (Cat cat in _dataService.Animals.Mammals.Cats)
            {
                Console.Write($"Cat number {i}, ");
                cat.Display();
                i++;
            }
        }
        else
        {
            _screenDefinitionService.Display(jsonFileNameCats, 10);
        }
    }

    /// <summary>
    /// Add a cat.
    /// </summary>
    private void AddCat()
    {
        try
        {
            Cat cat = AddEditCat();
            _dataService?.Animals?.Mammals?.Cats?.Add(cat);
            Console.WriteLine("Cat with name: {0} has been added to a list of cats", cat.Name);
            if (_dataService != null)
            {
                _dataService.Write("animals.json"); // Zapis danych do pliku
                _dataService.Read("animals.json"); // Odczyt danych z pliku
            }
        }
        catch
        {
            _screenDefinitionService.Display(jsonFileNameCats, 17);
        }
    }

    /// <summary>
    /// Deletes a cat.
    /// </summary>
    private void DeleteCat()
    {
        try
        {
            _screenDefinitionService.Display(jsonFileNameCats, 12);
            string? name = Console.ReadLine();
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            Cat? cat = (Cat?)(_dataService?.Animals?.Mammals?.Cats
                ?.FirstOrDefault(d => d is not null && string.Equals(d.Name, name)));
            if (cat is not null)
            {
                _dataService?.Animals?.Mammals?.Cats?.Remove(cat);
                Console.WriteLine("Cat with name: {0} has been deleted from a list of cats", cat.Name);
            }
            else
            {
                _screenDefinitionService.Display(jsonFileNameCats, 19);
            }
            if (_dataService != null)
            {
                _dataService.Write("animals.json"); // Zapis danych do pliku
                _dataService.Read("animals.json"); // Odczyt danych z pliku
            }
        }
        catch
        {
            _screenDefinitionService.Display(jsonFileNameCats, 17);
        }
    }

    /// <summary>
    /// Edits an existing cat after choice made.
    /// </summary>
    private void EditCatMain()
    {
        try
        {
            _screenDefinitionService.Display(jsonFileNameCats, 11);
            string? name = Console.ReadLine();
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            Cat? cat = (Cat?)(_dataService?.Animals?.Mammals?.Cats
                ?.FirstOrDefault(d => d is not null && string.Equals(d.Name, name)));
            if (cat is not null)
            {
                Cat catEdited = AddEditCat();
                cat.Copy(catEdited);
                _screenDefinitionService.Display(jsonFileNameCats, 20);
                cat.Display();
            }
            else
            {
                _screenDefinitionService.Display(jsonFileNameCats, 19);
            }
            if (_dataService != null)
            {
                _dataService.Write("animals.json"); // Zapis danych do pliku
                _dataService.Read("animals.json"); // Odczyt danych z pliku
            }
        }
        catch
        {
            _screenDefinitionService.Display(jsonFileNameCats, 18);
        }
    }

    /// <summary>
    /// Adds/edit specific cat.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    private Cat AddEditCat()
    {
        _screenDefinitionService.Display(jsonFileNameCats, 13);
        string? name = Console.ReadLine();
        _screenDefinitionService.Display(jsonFileNameCats, 14);
        string? ageAsString = Console.ReadLine();
        _screenDefinitionService.Display(jsonFileNameCats, 15);
        string? breed = Console.ReadLine();
        _screenDefinitionService.Display(jsonFileNameCats, 16);
        string? color = Console.ReadLine();
        if (name is null)
        {
            throw new ArgumentNullException(nameof(name));
        }
        if (ageAsString is null)
        {
            throw new ArgumentNullException(nameof(ageAsString));
        }
        if (breed is null)
        {
            throw new ArgumentNullException(nameof(breed));
        }
        if (color is null)
        {
            throw new ArgumentNullException(nameof(color));
        }

        int age = Int32.Parse(ageAsString);
        Cat cat = new Cat(name, age, breed, color);

        return cat;

    }

    private readonly string jsonFileNameCats = "CatsScreen.json";
    //implementacja mechanizmu konwersji ciągów znaków w ConsoleColor
    private ConsoleColor ConvertColorNameToConsoleColor(string colorName)
    {
        ConsoleColor color;
        if (Enum.TryParse(colorName, out color))
        {
            return color;
        }
        else
        {

            return ConsoleColor.White;
        }


    }
    #endregion // Private Methods
}

