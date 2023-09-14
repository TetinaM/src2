using SampleHierarchies.Data;
using SampleHierarchies.Data.Mammals;
using SampleHierarchies.Enums;
using System.IO;
using SampleHierarchies.Interfaces.Data;
using SampleHierarchies.Interfaces.Services;
using SampleHierarchies.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHierarchies.Gui
{
    
    public sealed class ElephantsScreen : Screen
    {
        #region Properties And Ctor

        private readonly ScreenDefinitionService _screenDefinitionService;
        /// <summary>
        /// Settings
        /// </summary>
        private readonly ISettings _settings;

        /// <summary>
        /// Date Service
        /// </summary>
        private IDataService _dataService;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="dataService"></param>
        /// <param name="settings"></param>
        public ElephantsScreen(IDataService dataService, ISettings settings, ScreenDefinitionService screenDefinitionService)
        {
            _screenDefinitionService = screenDefinitionService;
            _settings = settings;
            _dataService = dataService;

        }
        #endregion //Properties And Ctor

        #region Public Methods
        public override void Show()
        {
            /// <summury>
            /// Konwersja koloru z ciągu znaków na ConsoleColor
            /// </summury>

            Console.ForegroundColor = ConvertColorNameToConsoleColor(_settings.ElephantsScreenColor);
            while (true)
            {
                Console.WriteLine();
                _screenDefinitionService.Display(jsonFileNameElephants, 0);
                _screenDefinitionService.Display(jsonFileNameElephants, 1);
                _screenDefinitionService.Display(jsonFileNameElephants, 2);
                _screenDefinitionService.Display(jsonFileNameElephants, 3);
                _screenDefinitionService.Display(jsonFileNameElephants, 4);
                _screenDefinitionService.Display(jsonFileNameElephants, 5);
                _screenDefinitionService.Display(jsonFileNameElephants, 6);

                string? choiceAsString = Console.ReadLine();

                // Validate choice
                try
                {
                    if (choiceAsString is null)
                    {
                        throw new ArgumentNullException(nameof(choiceAsString));
                    }

                    ElephantsScreenChoices choice = (ElephantsScreenChoices)Int32.Parse(choiceAsString);
                    switch (choice)
                    {
                        case ElephantsScreenChoices.List:
                            ListElephants();
                            break;

                        case ElephantsScreenChoices.Create:
                            AddElephant(); break;

                        case ElephantsScreenChoices.Delete:
                            DeleteElephant();
                            break;

                        case ElephantsScreenChoices.Modify:
                            EditElephantMain();
                            break;

                        case ElephantsScreenChoices.Exit:
                            _screenDefinitionService.Display(jsonFileNameElephants, 7);
                            return;
                    }

                }
                catch
                {
                    _screenDefinitionService.Display(jsonFileNameElephants, 8);
                }
            }

        }
        #endregion // Public Methods

        #region Private Methods
        private void ListElephants()
        {
            Console.WriteLine();
            if (_dataService?.Animals?.Mammals?.Elephants is not null &&
                _dataService.Animals.Mammals.Elephants.Count > 0)
            {
                _screenDefinitionService.Display(jsonFileNameElephants, 9);
                int i = 1;
                foreach (Elephant elephant in _dataService.Animals.Mammals.Elephants)
                {
                    Console.Write($"Elephant number {i}, ");
                    elephant.Display();
                    i++;
                }
            }
            else
            {
                _screenDefinitionService.Display(jsonFileNameElephants, 10);
            }
        }

        /// <summary>
        /// Add a elephant.
        /// </summary>
        private void AddElephant()
        {
            try
            {
                Elephant elephant = AddEditElephant();
                _dataService?.Animals?.Mammals?.Elephants?.Add(elephant);
                Console.WriteLine("Elephant with name: {0} has been added to a list of elephants", elephant.Name);

                if (_dataService != null)
                {
                    _dataService.Write("animals.json"); // Zapis danych do pliku
                    _dataService.Read("animals.json"); // Odczyt danych z pliku
                }
            }
            catch
            {
                _screenDefinitionService.Display(jsonFileNameElephants, 20);
            }
        }

        /// <summary>
        /// Deletes a elephant.
        /// </summary>
        private void DeleteElephant()
        {
            try
            {
                _screenDefinitionService.Display(jsonFileNameElephants, 12);
                string? name = Console.ReadLine();
                if (name is null)
                {
                    throw new ArgumentNullException(nameof(name));

                }
                Elephant? elephant = (Elephant?)(_dataService?.Animals?.Mammals?.Elephants
                    ?.FirstOrDefault(d => d is not null && string.Equals(d.Name, name)));
                if (elephant is not null)
                {
                    _dataService?.Animals?.Mammals?.Elephants?.Remove(elephant);
                    Console.WriteLine("Elephant with name: {0} has been deleted from a list elephants", elephant.Name);

                }
                else
                {
                    Console.WriteLine("Elephant not found.");
                }
                if (_dataService != null)
                {
                    _dataService.Write("animals.json"); // Zapis danych do pliku
                    _dataService.Read("animals.json"); // Odczyt danych z pliku
                }
            }
            catch
            {
                _screenDefinitionService.Display(jsonFileNameElephants, 20);
            }
        }

        /// <summary>
        /// Edits an existing elephant after choice made.
        /// </summary>
        private void EditElephantMain()
        {
            try
            {
                _screenDefinitionService.Display(jsonFileNameElephants, 11);
                string? name = Console.ReadLine();
                if (name is null)
                {
                    throw new ArgumentNullException(nameof(name));
                }
                Elephant? elephant = (Elephant?)(_dataService?.Animals?.Mammals?.Elephants
                    ?.FirstOrDefault(d => d is not null && string.Equals(d.Name, name)));
                if (elephant is not null)
                {
                    Elephant elephantEdited = AddEditElephant();
                    elephant.Copy(elephantEdited);
                    _screenDefinitionService.Display(jsonFileNameElephants, 23);
                    elephant.Display();
                }
                else
                {
                    _screenDefinitionService.Display(jsonFileNameElephants, 22);
                }
                if (_dataService != null)
                {
                    _dataService.Write("animals.json"); // Zapis danych do pliku
                    _dataService.Read("animals.json"); // Odczyt danych z pliku
                }
            }
            catch
            {
                _screenDefinitionService.Display(jsonFileNameElephants, 21);
            }
        }

        /// <summary>
        /// Adds/edit specific elephant.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        private Elephant AddEditElephant()
        {
            _screenDefinitionService.Display(jsonFileNameElephants, 13);
            string? name = Console.ReadLine();
            _screenDefinitionService.Display(jsonFileNameElephants, 14);
            string? ageAsString = Console.ReadLine();
            _screenDefinitionService.Display(jsonFileNameElephants, 15);
            string? heightAsString = Console.ReadLine();
            _screenDefinitionService.Display(jsonFileNameElephants, 16);
            string? weightAsString = Console.ReadLine();
            _screenDefinitionService.Display(jsonFileNameElephants, 17);
            string? tuskLengthAsString = Console.ReadLine();
            _screenDefinitionService.Display(jsonFileNameElephants, 18);
            string? longLifespanAsString = Console.ReadLine();
            _screenDefinitionService.Display(jsonFileNameElephants, 19);
            string? socialBehavior = Console.ReadLine();





            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (ageAsString is null)
            {
                throw new ArgumentNullException(nameof(ageAsString));
            }
            if (heightAsString is null)
            {
                throw new ArgumentNullException(nameof(heightAsString));
            }
            if (weightAsString is null)
            {
                throw new ArgumentNullException(nameof(weightAsString));
            }
            if (tuskLengthAsString is null)
            {
                throw new ArgumentNullException(nameof(tuskLengthAsString));
            }
            if (longLifespanAsString is null)
            {
                throw new ArgumentNullException(nameof(longLifespanAsString));
            }
            if (socialBehavior is null)
            {
                throw new ArgumentNullException(nameof(socialBehavior));
            }

            int age = Int32.Parse(ageAsString);
            float height = Single.Parse(heightAsString);
            float weight = Single.Parse(weightAsString);
            float tuskLength = Single.Parse(tuskLengthAsString);
            int longLifespan = Int32.Parse(longLifespanAsString);





            Elephant elephant = new Elephant(name, age, height, weight, tuskLength, longLifespan, socialBehavior);

            return elephant;
        }

        private readonly string jsonFileNameElephants = "ElephantsScreen.json";
        /// <summary>
        /// <param name="colorName"></param>
        /// implementacja mechanizmu konwersji ciągów znaków w ConsoleColor
        /// </summary>
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
        #endregion Private Methods

    }

}
