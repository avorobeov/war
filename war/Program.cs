using System;
using System.Collections.Generic;

namespace war
{
    class Program
    {
        static void Main(string[] args)
        {
            Arena arena = new Arena();

            Squad squad1 = new Squad();
            Squad squad2 = new Squad();

            string userInput;
            bool isExit = false;

            while (isExit == false)
            {
                ShowMenu();

                userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        squad1.CreateSquad();
                        break;

                    case "2":
                        squad2.CreateSquad();
                        break;

                    case "3":
                        arena.StartFight(squad1, squad2);
                        break;

                    case "4":
                        isExit = true;
                        break;
                }
            }
        }
        private static void ShowMenu()
        {
            Console.WriteLine("\nДля создания первого отряда нажмите 1\n" +
                              "\nДля создания второго отряда нажмите 2\n" +
                              "\nДля начала боя нажмите 3\n" +
                              "\nДля выхода нажмите 4\n");
        }
    }

    class Soldier
    {
        public string Name { get; private set; }
        public int Health { get; private set; }
        public int Armor { get; private set; }
        public int Damage { get; private set; }

        public Soldier(string name, int health, int armor, int damage)
        {
            Name = name;
            Health = health;
            Armor = armor;
            Damage = damage;
        }

        public  void TakeDamage(int damage)
        {
            int getPercentages = 100;

            double amountDamage = damage - damage * Armor / getPercentages;

            Health -= (int)amountDamage;
        }
    }

    class Squad
    {
        private Random _random = new Random();

        private List<Soldier> _soldiers = new List<Soldier>();

        private int _maximumPercentageValue = 99;
        private int _maximumAmountArmor = 90;
        private int _maxHealth;
        private int _maxArmor;
        private int _maxDamage;
        private int _deviationPercentage;

        public int NumberIivingSoldiers => _soldiers.Count;

        public int NumberSoldiers { get; private set; }

        public void CreateSquad()
        {
            CheckValidValue();

            for (int i = 0; i < NumberSoldiers; i++)
            {
                int health, armor, damage;

                GetCharacteristicsSoldier(out health, out armor, out damage);

                _soldiers.Add(new Soldier(i.ToString(), health, armor, damage));

                ShowMessage($"Солдат создан и у него вот такие показатели Xp{health} Ar{armor} Dm{damage}",ConsoleColor.Blue);
            }

            ShowMessage($"Отряд создан!\nВ отряде {NumberSoldiers} количество бойцов", ConsoleColor.Green) ;
        }

        public void InflictDamageSoldier(int indexSoldier,int damage)
        {
            if(indexSoldier <= _soldiers.Count && indexSoldier >= 0 && _soldiers.Count != 0)
            {
                _soldiers[indexSoldier].TakeDamage(damage);

                CheckIfSoldierAlive(_soldiers[indexSoldier]);
            }
        }

        public int GetDamageSoldier(int indexSoldier)
        {
            return _soldiers[indexSoldier].Damage;
        }
      
        private void CheckValidValue()
        {

            bool isValidValue = false;

            while (isValidValue == false)
            {
                SetCharacteristicsSoldiers();
                if (_maxArmor <= _maximumAmountArmor && _deviationPercentage <= _maximumPercentageValue)
                {
                    isValidValue = true;
                }
                else if(_maxArmor > _maximumAmountArmor)
                {
                    ShowMessage("Вы вели слишком большое значение брони", ConsoleColor.Red);
                }
                else
                {
                    ShowMessage("Вы вели слишком большое значение процента", ConsoleColor.Red);
                }
            }
        }

        private void CheckIfSoldierAlive(Soldier soldier)
        {
            if (soldier.Health <= 0)
            {
                _soldiers.Remove(soldier);

                ShowMessage("Солдат умер", ConsoleColor.DarkMagenta);
            }
        }

        private void GetCharacteristicsSoldier(out int health, out int armor, out int damage)
        {
            int getPercentages = 100;
            bool isIncrease = Convert.ToBoolean(_random.Next(0, 2));

            if (isIncrease == true)
            {
                health = (int)Math.Ceiling(((double)(_maxHealth / getPercentages * _random.Next(0, _deviationPercentage)) + _maxHealth));
                armor = (int)Math.Ceiling(((double)_maxArmor / getPercentages * _random.Next(0, _deviationPercentage)) + _maxArmor);
                damage = (int)Math.Ceiling(((double)_maxDamage / getPercentages * _random.Next(0, _deviationPercentage)) + _maxDamage);
            }
            else
            {
                health = (int)Math.Ceiling(_maxHealth - ((double)_maxHealth / getPercentages * _random.Next(0, _deviationPercentage)));
                armor = (int)Math.Ceiling(_maxArmor - ((double)_maxArmor / getPercentages * _random.Next(0, _deviationPercentage)));
                damage = (int)Math.Ceiling(_maxDamage - ((double)_maxDamage / getPercentages * _random.Next(0, _deviationPercentage)));
            }
        }
      
        private void ShowMessage(string message, ConsoleColor color)
        {
            ConsoleColor preliminaryColor = Console.ForegroundColor;

            Console.ForegroundColor = color;
            Console.WriteLine(message + "\n");

            Console.ForegroundColor = preliminaryColor;
        }

        private void SetCharacteristicsSoldiers()
        {
            ShowMessage("Создание отряда", ConsoleColor.Red);

            _maxHealth = GetNumber("Ведите максимальное количество здоровья");
            _maxDamage = GetNumber("Ведите максимальное количество урона");
            _maxArmor = GetNumber("Ведите максимальное количество брони");
            _deviationPercentage = GetNumber("Ведите процент увеличение или уменьшения характеристик");
            NumberSoldiers = GetNumber("Ведите количество бойцов в отряде");
        }

        private int GetNumber(string text)
        {
            string inputUser;

            int meaning = 0;

            bool isCorrect = false;

            while (isCorrect == false)
            {
                ShowMessage(text, ConsoleColor.Green);

                inputUser = Console.ReadLine();

                if (Int32.TryParse(inputUser, out meaning))
                {
                    return meaning;
                }
                else
                {
                    ShowMessage("Вы вели вместо числа строку", ConsoleColor.Red);
                }
            }

            return meaning;
        }

    }

    class Arena
    {
        private Random _random = new Random();

        public void StartFight(Squad squad1, Squad squad2)
        {
            int takesDamage;
            int amountDamage;

            if (squad1.NumberIivingSoldiers > 0 && squad2.NumberIivingSoldiers > 0)
            {
                ShowMessage("Бой начался", ConsoleColor.Green);

                while (squad1.NumberIivingSoldiers > 0 && squad2.NumberIivingSoldiers > 0)
                {
                    for (int i = 0; i < squad1.NumberIivingSoldiers; i++)
                    {
                        takesDamage = _random.Next(0, squad2.NumberIivingSoldiers);
                        amountDamage = squad1.GetDamageSoldier(i);

                        squad2.InflictDamageSoldier(takesDamage, amountDamage);
                    }

                    for (int i = 0; i < squad2.NumberIivingSoldiers; i++)
                    {
                        takesDamage = _random.Next(0, squad1.NumberIivingSoldiers);
                        amountDamage = squad2.GetDamageSoldier(i);

                        squad1.InflictDamageSoldier(takesDamage, amountDamage);
                    }
                }

                DetermineWinner(squad1, squad2);
            }
            else
            {
                ShowMessage("Отряд не готов к бою", ConsoleColor.Red);
            }
        }

        private void DetermineWinner(Squad squad1, Squad squad2)
        {
            if (squad1.NumberIivingSoldiers > 0 && squad2.NumberIivingSoldiers > 0)
            {
                ShowMessage("Оба отряда погибли в этом бою", ConsoleColor.DarkYellow);
            }
            else if (squad1.NumberIivingSoldiers > 0)
            {
                ShowMessage("Первый отряд победил", ConsoleColor.DarkGreen);
            }
            else if (squad2.NumberIivingSoldiers > 0)
            {
                ShowMessage("Второй отряд победил", ConsoleColor.DarkBlue);
            }
        }

        private void ShowMessage(string message, ConsoleColor color)
        {
            ConsoleColor preliminaryColor = Console.ForegroundColor;

            Console.ForegroundColor = color;
            Console.WriteLine(message + "\n");

            Console.ForegroundColor = preliminaryColor;
        }
    }
}
