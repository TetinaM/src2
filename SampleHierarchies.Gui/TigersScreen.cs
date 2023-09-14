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
    public sealed class TigersScreen : Screen
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
        /// Ctor.
        /// </summary>
        /// <param name="dataService"></param>
        /// <param name="settings"></param>
        public TigersScreen(IDataService dataService, ISettings settings, ScreenDefinitionService screenDefinitionService)
        {
            _screenDefinitionService = screenDefinitionService;
            _settings = settings;
            _dataService = dataService;

        }
        #endregion Properties And Ctor

        #region Public Methods
        public override void Show()
        {
            /// <summury>
            /// Konwersja koloru z ciągu znaków na ConsoleColor
            /// </summury>

            Console.ForegroundColor = ConvertColorNameToConsoleColor(_settings.TigersScreenColor);
            while (true)
            {
                Console.WriteLine();
                _screenDefinitionService.Display(jsonFileNameTigers, 0);
                _screenDefinitionService.Display(jsonFileNameTigers, 1);
                _screenDefinitionService.Display(jsonFileNameTigers, 2);
                _screenDefinitionService.Display(jsonFileNameTigers, 3);
                _screenDefinitionService.Display(jsonFileNameTigers, 4);
                _screenDefinitionService.Display(jsonFileNameTigers, 5);
                _screenDefinitionService.Display(jsonFileNameTigers, 6);

                string? choiceAsString = Console.ReadLine();

                // Validate choice
                try
                {
                    if (choiceAsString is null)
                    {
                        throw new ArgumentNullException(nameof(choiceAsString));
                    }

                    TigersScreenChoices choice = (TigersScreenChoices)Int32.Parse(choiceAsString);
                    switch (choice)
                    {
                        case TigersScreenChoices.List:
                            ListTigers();
                            break;

                        case TigersScreenChoices.Create:
                            AddTiger();
                            break;

                        case TigersScreenChoices.Delete:
                            DeleteTiger();
                            break;

                        case TigersScreenChoices.Modify:
                            EditTigerMain();
                            break;

                        case TigersScreenChoices.Exit:
                            _screenDefinitionService.Display(jsonFileNameTigers, 7);
                            return;
                    }

                }
                catch
                {
                    _screenDefinitionService.Display(jsonFileNameTigers, 8);
                }
            }

        }
        #endregion Public Methods

        #region Private Methods
        private void ListTigers()
        {
            Console.WriteLine();
            if (_dataService?.Animals?.Mammals?.Tigers is not null &&
                _dataService.Animals.Mammals.Tigers.Count > 0)
            {
                _screenDefinitionService.Display(jsonFileNameTigers, 9);
                int i = 1;
                foreach (Tiger tiger in _dataService.Animals.Mammals.Tigers)
                {
                    Console.Write($"Tiger number {i}, ");
                    tiger.Display();
                    i++;
                }
            }
            else
            {
                _screenDefinitionService.Display(jsonFileNameTigers, 10);
            }
        }

        /// <summary>
        /// Add a tiger.
        /// </summary>
        private void AddTiger()
        {
            try
            {
                Tiger tiger = AddEditTiger();
                _dataService?.Animals?.Mammals?.Tigers?.Add(tiger);
                Console.WriteLine("Tiger with name: {0} has been added to a list of tigers", tiger.Name);

                if (_dataService != null)
                {
                    _dataService.Write("animals.json"); // Zapis danych do pliku
                    _dataService.Read("animals.json"); // Odczyt danych z pliku
                }
            }
            catch
            {
                _screenDefinitionService.Display(jsonFileNameTigers, 20);
            }
        }

        /// <summary>
        /// Deletes a tiger.
        /// </summary>
        private void DeleteTiger()
        {
            try
            {
                _screenDefinitionService.Display(jsonFileNameTigers, 12);
                string? name = Console.ReadLine();
                if (name is null)
                {
                    throw new ArgumentNullException(nameof(name));

                }
                Tiger? tiger = (Tiger?)(_dataService?.Animals?.Mammals?.Tigers
                    ?.FirstOrDefault(d => d is not null && string.Equals(d.Name, name)));
                if (tiger is not null)
                {
                    _dataService?.Animals?.Mammals?.Tigers?.Remove(tiger);
                    Console.WriteLine("Tiger with name: {0} has been deleted from a list of tigers", tiger.Name);

                }
                else
                {
                    Console.WriteLine("Tiger not found.");
                }
                if (_dataService != null)
                {
                    _dataService.Write("animals.json"); // Zapis danych do pliku
                    _dataService.Read("animals.json"); // Odczyt danych z pliku
                }
            }
            catch
            {
                _screenDefinitionService.Display(jsonFileNameTigers, 20);
            }
        }

        /// <summary>
        /// Edits an existing tiger after choice made.
        /// </summary>
        private void EditTigerMain()
        {
            try
            {
                _screenDefinitionService.Display(jsonFileNameTigers, 11);
                string? name = Console.ReadLine();
                if (name is null)
                {
                    throw new ArgumentNullException(nameof(name));
                }
                Tiger? tiger = (Tiger?)(_dataService?.Animals?.Mammals?.Tigers
                    ?.FirstOrDefault(d => d is not null && string.Equals(d.Name, name)));
                if (tiger is not null)
                {
                    Tiger tigerEdited = AddEditTiger();
                    tiger.Copy(tigerEdited);
                    _screenDefinitionService.Display(jsonFileNameTigers, 23);
                    tiger.Display();
                }
                else
                {
                    _screenDefinitionService.Display(jsonFileNameTigers, 22);
                }
                if (_dataService != null)
                {
                    _dataService.Write("animals.json"); // Zapis danych do pliku
                    _dataService.Read("animals.json"); // Odczyt danych z pliku
                }
            }
            catch
            {
                _screenDefinitionService.Display(jsonFileNameTigers, 21);
            }
        }

        /// <summary>
        /// Adds/edit specific tiger.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        private Tiger AddEditTiger()
        {
            _screenDefinitionService.Display(jsonFileNameTigers, 13);
            string? name = Console.ReadLine();
            _screenDefinitionService.Display(jsonFileNameTigers, 14);
            string? ageAsString = Console.ReadLine();
            _screenDefinitionService.Display(jsonFileNameTigers, 15);
            string? apexPredatorString = Console.ReadLine();
            _screenDefinitionService.Display(jsonFileNameTigers, 16);
            string? size = Console.ReadLine();
            _screenDefinitionService.Display(jsonFileNameTigers, 17);
            string? fur = Console.ReadLine();
            _screenDefinitionService.Display(jsonFileNameTigers, 18);
            string? legs = Console.ReadLine();
            _screenDefinitionService.Display(jsonFileNameTigers, 19);
            string? behavior = Console.ReadLine();


            string apexPredatorText = apexPredatorString == "yes" ? " apex predator" : " not apex predator";
            

            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (ageAsString is null)
            {
                throw new ArgumentNullException(nameof(ageAsString));
            }
            if (apexPredatorString is null)
            {
                throw new ArgumentNullException(nameof(apexPredatorString));
            }
            if (size is null)
            {
                throw new ArgumentNullException(nameof(size));
            }
            if (fur is null)
            {
                throw new ArgumentNullException(nameof(fur));
            }
            if (legs is null)
            {
                throw new ArgumentNullException(nameof(legs));
            }
            if (behavior is null)
            {
                throw new ArgumentNullException(nameof(behavior));
            }

            int age = Int32.Parse(ageAsString);
            string apexPredator = apexPredatorText;
            



            Tiger tiger = new Tiger(name, age, apexPredator, size, fur, legs, behavior);

            return tiger;
        }

        private readonly string jsonFileNameTigers = "TigersScreen.json";
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