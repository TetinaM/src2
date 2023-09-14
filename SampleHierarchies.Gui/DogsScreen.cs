using Newtonsoft.Json;
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
public sealed class DogsScreen : Screen
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
    public DogsScreen(IDataService dataService, ISettings settings, ScreenDefinitionService screenDefinitionService)
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
        Console.ForegroundColor = ConvertColorNameToConsoleColor(_settings.DogsScreenColor);

        while (true)
        {
            Console.WriteLine();
            _screenDefinitionService.Display(jsonFileNameDogs, 0);
            _screenDefinitionService.Display(jsonFileNameDogs, 1);
            _screenDefinitionService.Display(jsonFileNameDogs, 2);
            _screenDefinitionService.Display(jsonFileNameDogs, 3);
            _screenDefinitionService.Display(jsonFileNameDogs, 4);
            _screenDefinitionService.Display(jsonFileNameDogs, 5);
            _screenDefinitionService.Display(jsonFileNameDogs, 6);

            string? choiceAsString = Console.ReadLine();

            // Validate choice
            try
            {
                if (choiceAsString is null)
                {
                    throw new ArgumentNullException(nameof(choiceAsString));
                }

                DogsScreenChoices choice = (DogsScreenChoices)Int32.Parse(choiceAsString);
                switch (choice)
                {
                    case DogsScreenChoices.List:
                        ListDogs();
                        break;

                    case DogsScreenChoices.Create:
                        AddDog(); break;

                    case DogsScreenChoices.Delete:
                        DeleteDog();
                        break;

                    case DogsScreenChoices.Modify:
                        EditDogMain();
                        break;

                    case DogsScreenChoices.Exit:
                        _screenDefinitionService.Display(jsonFileNameDogs, 7);
                        return;
                }
            }
            catch
            {
                _screenDefinitionService.Display(jsonFileNameDogs, 8);
            }
        }
    }

    #endregion // Public Methods

    #region Private Methods

    /// <summary>
    /// List all dogs.
    /// </summary>

    private void ListDogs()
    {
        Console.WriteLine();
        if (_dataService?.Animals?.Mammals?.Dogs is not null &&
            _dataService.Animals.Mammals.Dogs.Count > 0)
        {
            _screenDefinitionService.Display(jsonFileNameDogs, 9);
            int i = 1;
            foreach (Dog dog in _dataService.Animals.Mammals.Dogs)
            {
                Console.Write($"Dog number {i}, ");
                dog.Display();
                i++;
                if (_dataService != null)
                {
                   
                }
            }
        }
        else
        {
            _screenDefinitionService.Display(jsonFileNameDogs, 10);
        }
    }

    /// <summary>
    /// Add a dog.
    /// </summary>
    private void AddDog()
    {
        try
        {
            Dog dog = AddEditDog();
            _dataService?.Animals?.Mammals?.Dogs?.Add(dog);
            Console.WriteLine("Dog with name: {0} has been added to a list of dogs", dog.Name);

            if (_dataService != null)
            {
                _dataService.Write("animals.json"); // Zapis danych do pliku
                _dataService.Read("animals.json"); // Odczyt danych z pliku
            }
        }
        catch
        {
            _screenDefinitionService.Display(jsonFileNameDogs, 16);
        }
    }
        /// <summary>
        /// Deletes a dog.
        /// </summary>
        private void DeleteDog()
        {
            try
            {
                _screenDefinitionService.Display(jsonFileNameDogs, 12);
                string? name = Console.ReadLine();
                if (name is null)
                {
                    throw new ArgumentNullException(nameof(name));
                }
                Dog? dog = (Dog?)(_dataService?.Animals?.Mammals?.Dogs
                    ?.FirstOrDefault(d => d is not null && string.Equals(d.Name, name)));
                if (dog is not null)
                {
                    _dataService?.Animals?.Mammals?.Dogs?.Remove(dog);
                    Console.WriteLine("Dog with name: {0} has been deleted from a list of dogs", dog.Name);
                }
                else
                {
                    Console.WriteLine("Dog not found.");
                }
                if (_dataService != null)
                {

                    _dataService.Write("animals.json"); // Zapis danych do pliku
                    _dataService.Read("animals.json"); // Odczyt danych z pliku
            }

            }
            catch
            {
            _screenDefinitionService.Display(jsonFileNameDogs, 16);
        }
        }

        /// <summary>
        /// Edits an existing dog after choice made.
        /// </summary>
        private void EditDogMain()
        {
            try
            {
                _screenDefinitionService.Display(jsonFileNameDogs, 11);
                string? name = Console.ReadLine();
                if (name is null)
                {
                    throw new ArgumentNullException(nameof(name));
                }
                Dog? dog = (Dog?)(_dataService?.Animals?.Mammals?.Dogs
                    ?.FirstOrDefault(d => d is not null && string.Equals(d.Name, name)));
                if (dog is not null)
                {
                    Dog dogEdited = AddEditDog();
                    dog.Copy(dogEdited);
                    _screenDefinitionService.Display(jsonFileNameDogs, 19);
                dog.Display();
                }
                else
                {
                    _screenDefinitionService.Display(jsonFileNameDogs, 18);
            }
                if (_dataService != null)
                {
                _dataService.Write("animals.json"); // Zapis danych do pliku
                _dataService.Read("animals.json"); // Odczyt danych z pliku
            }

            }
            catch
            {
                _screenDefinitionService.Display(jsonFileNameDogs, 17);
            }
        }

        /// <summary>
        /// Adds/edit specific dog.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        private Dog AddEditDog()
        {
            _screenDefinitionService.Display(jsonFileNameDogs, 13);
            string? name = Console.ReadLine();
             _screenDefinitionService.Display(jsonFileNameDogs, 14);
            string? ageAsString = Console.ReadLine();
            _screenDefinitionService.Display(jsonFileNameDogs, 15);
             string? breed = Console.ReadLine();

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

            int age = Int32.Parse(ageAsString);
            Dog dog = new Dog(name, age, breed);

            return dog;

        }

    private readonly string jsonFileNameDogs = "DogsScreen.json";
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

