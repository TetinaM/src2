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
    public sealed class LionsScreen : Screen
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
        public LionsScreen(IDataService dataService, ISettings settings, ScreenDefinitionService screenDefinitionService)
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

            Console.ForegroundColor = ConvertColorNameToConsoleColor(_settings.LionsScreenColor);
            while (true)
            {
                Console.WriteLine();
                _screenDefinitionService.Display(jsonFileNameLions, 0);
                _screenDefinitionService.Display(jsonFileNameLions, 1);
                _screenDefinitionService.Display(jsonFileNameLions, 2);
                _screenDefinitionService.Display(jsonFileNameLions, 3);
                _screenDefinitionService.Display(jsonFileNameLions, 4);
                _screenDefinitionService.Display(jsonFileNameLions, 5);
                _screenDefinitionService.Display(jsonFileNameLions, 6);

                string? choiceAsString = Console.ReadLine();

                // Validate choice
                try
                {
                    if (choiceAsString is null)
                    {
                        throw new ArgumentNullException(nameof(choiceAsString));
                    }

                    LionsScreenChoices choice = (LionsScreenChoices)Int32.Parse(choiceAsString);
                    switch (choice)
                    {
                        case LionsScreenChoices.List:
                            ListLions();
                            break;

                        case LionsScreenChoices.Create:
                            AddLion(); break;

                        case LionsScreenChoices.Delete:
                            DeleteLion();
                            break;

                        case LionsScreenChoices.Modify:
                            EditLionMain();
                            break;

                        case LionsScreenChoices.Exit:
                            _screenDefinitionService.Display(jsonFileNameLions, 7);
                            return;
                    }
                    
                }
                catch
                {
                    _screenDefinitionService.Display(jsonFileNameLions, 8);
                }
            }
             
        }
        #endregion Public Methods

        #region Private Methods
        private void ListLions()
        {
            Console.WriteLine();
            if (_dataService?.Animals?.Mammals?.Lions is not null &&
                _dataService.Animals.Mammals.Lions.Count > 0)
            {
                _screenDefinitionService.Display(jsonFileNameLions, 9);
                int i = 1;
                foreach (Lion lion in _dataService.Animals.Mammals.Lions)
                {
                    Console.Write($"Lion number {i}, ");
                    lion.Display();
                    i++;
                }
            }
            else
            {
                _screenDefinitionService.Display(jsonFileNameLions, 10);
            }
        }

        /// <summary>
        /// Add a lion.
        /// </summary>
        private void AddLion()
        {
            try
            {
                Lion lion = AddEditLion();
                _dataService?.Animals?.Mammals?.Lions?.Add(lion);
                Console.WriteLine("Lion with name: {0} has been added to a list of lions", lion.Name);

                if (_dataService != null)
                {
                    _dataService.Write("animals.json"); // Zapis danych do pliku
                    _dataService.Read("animals.json"); // Odczyt danych z pliku
                }
            }
            catch
            {
                _screenDefinitionService.Display(jsonFileNameLions, 20);
            }
        }

        /// <summary>
        /// Deletes a lion.
        /// </summary>
        private void DeleteLion()
        {
            try
            {
                _screenDefinitionService.Display(jsonFileNameLions, 12);
                string? name = Console.ReadLine();
                if (name is null)
                {
                    throw new ArgumentNullException(nameof(name));
                   
                }
                Lion? lion = (Lion?)(_dataService?.Animals?.Mammals?.Lions
                    ?.FirstOrDefault(d => d is not null && string.Equals(d.Name, name)));
                if (lion is not null)
                {
                    _dataService?.Animals?.Mammals?.Lions?.Remove(lion);
                    Console.WriteLine("Lion with name: {0} has been deleted from a list of lions", lion.Name);
                    
                }
                else
                {
                    Console.WriteLine("Lion not found.");
                }
                if (_dataService != null)
                {
                    _dataService.Write("animals.json"); // Zapis danych do pliku
                    _dataService.Read("animals.json"); // Odczyt danych z pliku
                }
            }
            catch
            {
                _screenDefinitionService.Display(jsonFileNameLions, 20);
            }
        }

        /// <summary>
        /// Edits an existing lion after choice made.
        /// </summary>
        private void EditLionMain()
        {
            try
            {
                _screenDefinitionService.Display(jsonFileNameLions, 11);
                string? name = Console.ReadLine();
                if (name is null)
                {
                    throw new ArgumentNullException(nameof(name));
                }
                Lion? lion = (Lion?)(_dataService?.Animals?.Mammals?.Lions
                    ?.FirstOrDefault(d => d is not null && string.Equals(d.Name, name)));
                if (lion is not null)
                {
                    Lion lionEdited = AddEditLion();
                    lion.Copy(lionEdited);
                    _screenDefinitionService.Display(jsonFileNameLions, 23);
                    lion.Display();
                }
                else
                {
                    _screenDefinitionService.Display(jsonFileNameLions, 22);
                }
                if (_dataService != null)
                {
                    _dataService.Write("animals.json"); // Zapis danych do pliku
                    _dataService.Read("animals.json"); // Odczyt danych z pliku
                }
            }
            catch
            {
                _screenDefinitionService.Display(jsonFileNameLions, 21);
            }
        }

        /// <summary>
        /// Adds/edit specific lion.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        private Lion AddEditLion()
        {
            _screenDefinitionService.Display(jsonFileNameLions, 13);
            string? name = Console.ReadLine();
            _screenDefinitionService.Display(jsonFileNameLions, 14);
            string? ageAsString = Console.ReadLine();
            _screenDefinitionService.Display(jsonFileNameLions, 15);
            string? apexPredatorString = Console.ReadLine();
            _screenDefinitionService.Display(jsonFileNameLions, 16);
            string? packHunterString = Console.ReadLine();
            _screenDefinitionService.Display(jsonFileNameLions, 17);
            string? mane = Console.ReadLine();
            _screenDefinitionService.Display(jsonFileNameLions, 18);
            string? communication = Console.ReadLine();
            _screenDefinitionService.Display(jsonFileNameLions, 19);
            string? territoryDefenceString = Console.ReadLine();


            string apexPredatorText = apexPredatorString == "yes" ? " apex predator" : " not apex predator";
            string packHunterText = packHunterString == "yes" ? " pack hunter" : " not pack hunter";
            string territoryDefenceText = territoryDefenceString == "yes" ? " defend territory" : " don't defend territory";

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
            if (packHunterString is null)
            {
                throw new ArgumentNullException(nameof(packHunterString));
            }
            if (mane is null)
            {
                throw new ArgumentNullException(nameof(mane));
            }
            if (communication is null)
            {
                throw new ArgumentNullException(nameof(communication));
            }
            if (territoryDefenceString is null)
            {
                throw new ArgumentNullException(nameof(territoryDefenceString));
            }

            int age = Int32.Parse(ageAsString);
            string apexPredator = apexPredatorText;
            string packHunter = packHunterText;
            string territoryDefence = territoryDefenceText;



            Lion lion = new Lion(name, age, apexPredator, packHunter, mane, communication, territoryDefence);

            return lion;
        }
         private readonly string jsonFileNameLions = "LionsScreen.json";
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
