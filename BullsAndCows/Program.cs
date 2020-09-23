using System;
using System.Collections.Generic;

namespace BullsAndCows
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Добро пожаловать в игру \"Быки и коровы\"!");
            Console.Write("Введите длинну числа: 2 или 3 или 4. -> ");
            string userInput = Console.ReadLine();
            int sizeNumber;
            int numberModeGame;
            if (int.TryParse(userInput,out sizeNumber))
            {
                Console.Write("Введите номер режима игры: 1, 2, 3. ->");
                userInput = Console.ReadLine();
                if (int.TryParse(userInput, out numberModeGame))
                {
                    GameBullsAndCows gameBullsAndCows = new GameBullsAndCows(sizeNumber, numberModeGame);
                    gameBullsAndCows.StartGame();
                }
                else
                {
                    throw new Exception("Введить нужно только цифры!");
                }
            }
            else
            {
                throw new Exception("Введить нужно только цифры!");
            }
            
        }
    }

    class BullsAndCows
    {
        public int Bulls { get; private set; }
        public int Cows { get; private set; }

        public BullsAndCows(int bulls, int cows)
        {
            Bulls = bulls;
            Cows = cows;
        }

        public string GetInfo()
        {
            string bullsWord = (Bulls == 0) ? "быков" : (Bulls == 1) ? "бык" : "быка";
            string cowsWord = (Cows == 0) ? "коров" : (Cows == 1) ? "корова" : "коровы";

            string comment = "";
            if (Bulls == 3 && Cows == 1)
            {
                comment = "Мощный ход!";
            }
            else if (Bulls == 0 && Cows == 0)
            {
                comment = "Повезло!";
            }
            else if(Bulls == 2 && Cows == 2)
            {
                comment = "Сильный ход!";
            }

            return $"{Bulls} {bullsWord}, {Cows} {cowsWord} " + comment;
        }
    }

    class GameBullsAndCows
    {
        private Random _rand;
        private List<int> _hideNumber;
        private List<int> _guessNumber;
        private int _sizeNumber;
        private int _numberModeGame;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sizeNumber">длинна числа(2, 3 или 4-х значное число)</param>
        /// <param name="numberModeGame">тип игры (1, 2, 3)</param>
        /// <param name="countNumbersForBot">количество чисел, которое загадает программа боту</param>
        public GameBullsAndCows(int sizeNumber, int numberModeGame)
        {
            if (IsCorrectInputSizeNumber(sizeNumber) && IsCorrectNumberModeGame(numberModeGame))
            {
                _rand = new Random();
                _sizeNumber = sizeNumber;
                _hideNumber = new List<int>(sizeNumber);
                _guessNumber = new List<int>(sizeNumber);
                _numberModeGame = numberModeGame;
            }
        }

        public void StartGame()
        {
            Console.Clear();
            switch (_numberModeGame)
            {
                case 1:
                    ShowInfoGame("Игрок угадывает");
                    GuessPlayer();
                    break;
                case 2:
                    ShowInfoGame("Бот угадывает");
                    GuessBot();
                    break;
                case 3:
                    ShowInfoGame("Бот угадывает и выводится статистика его попыток");
                    ShowBotStatistic();
                    break;
            }
        }


        private void ShowInfoGame(string name)
        {
            Console.WriteLine($"Вы выбрали режим: {name}. Длина числа равна: {_sizeNumber}");
        }

        /// <summary>
        /// Выводит число и номер попытки с которой бот угадал число
        /// </summary>
        private void ShowBotStatistic()
        {

        }

        /// <summary>
        /// Бот угадывает число загаданное игроком
        /// </summary>
        private void GuessBot()
        {

        }

        /// <summary>
        /// Игрок угадывает число загаданное программой
        /// </summary>
        private void GuessPlayer()
        {
            _hideNumber = GetGenerateNumber(_sizeNumber);
            bool isNextMove = true;
            while (isNextMove)
            {
                Console.Write($"Введите число длинной {_sizeNumber}: ");
                string inputNumber = Console.ReadLine();
                if (IsCorrectInputNumber(inputNumber))
                {
                    _guessNumber = ConvertStringToList(inputNumber);

                    BullsAndCows bullsAndCows = GetBullsAndCows();
                    Console.WriteLine(bullsAndCows.GetInfo());

                    if (IsWin(bullsAndCows))
                    {
                        isNextMove = false;
                        Console.WriteLine("Мууу! Победа!");
                    }
                }
            }
        }

        /// <summary>
        /// Проверка победы
        /// </summary>
        /// <param name="bullsAndCows">количество быков и коров</param>
        /// <returns></returns>
        private bool IsWin(BullsAndCows bullsAndCows)
        {
            return bullsAndCows.Bulls == _sizeNumber;
        }

        /// <summary>
        /// Конвертирует строку в List (число)
        /// </summary>
        /// <param name="number">число в строке</param>
        /// <returns>число в виде списка</returns>
        private List<int> ConvertStringToList(string number)
        {
            List<int> newNumber = new List<int>(number.Length);
            for (int i = 0; i < number.Length; i++)
            {
                int tempNumber;
                if (int.TryParse(number[i].ToString(), out tempNumber))
                {
                    newNumber.Add(tempNumber);
                }
                else
                {
                    throw new Exception("Вводить можно только цифры");
                }
            }
            return newNumber;
        }

        /// <summary>
        /// Проверка числа, которое ввел пользователь
        /// </summary>
        /// <param name="number">число</param>
        /// <returns></returns>
        private bool IsCorrectInputNumber(string number)
        {
            if (IsCorrectSizeNumber(number.Length))
            {
                int uniqueNumber = 0;
                for (int i = 0; i < number.Length; i++)
                {
                    for (int j = 0; j < number.Length; j++)
                    {
                        if (number[i] == number[j])
                            ++uniqueNumber;
                    }
                    if (uniqueNumber > 1)
                    {
                        throw new Exception("Цифры не должны повторяться");
                    }
                    uniqueNumber = 0;
                }
                return true;
            }
            else
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Возвращает быков и коров в числе currentNumber
        /// </summary>
        /// <param name="startNumber">число, которое загадали</param>
        /// <param name="currentNumber">предположительное число</param>
        /// <returns>Количество быков и коров в числе currentNumber</returns>
        private BullsAndCows GetBullsAndCows()
        {
            int bulls = 0;
            int cows = 0;

            for (int i = 0; i < _hideNumber.Count; i++)
            {
                if (_hideNumber[i] == _guessNumber[i])
                {
                    ++bulls;
                    continue;
                }
                for (int j = 0; j < _hideNumber.Count; j++)
                {
                    if (_hideNumber[i] == _guessNumber[j])
                    {
                        ++cows;
                        break;
                    }
                }
            }
            return new BullsAndCows(bulls, cows);
        }

        /// <summary>
        /// Генерирует и возвращает число из цифр (0-9)
        /// </summary>
        /// <param name="sizeNumber">длина числа</param>
        /// <returns >Число длины sizeNumber</returns>
        private List<int> GetGenerateNumber(int sizeNumber)
        {
            List<int> randNumbers = new List<int>(sizeNumber);
            bool isUniqueNumber = true;
            int randNumber = _rand.Next(0, 10);
            randNumbers.Add(randNumber);
            
            while (randNumbers.Count < sizeNumber)
            {
                randNumber = _rand.Next(0, 10);
                for (int j = 0; j < randNumbers.Count; j++)
                {
                    if (randNumber == randNumbers[j])
                    {
                        isUniqueNumber = false;
                        break;
                    }
                }
                if (isUniqueNumber)
                {
                    randNumbers.Add(randNumber);
                }
                isUniqueNumber = true;
            }
            return randNumbers;
        }

        /// <summary>
        /// Проверяет корректность типа игры
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private bool IsCorrectNumberModeGame(int number)
        {
            if (number < 1 || number > 3)
            {
                return false;
                throw new Exception("Режимы игры только - 1, 2, 3.");
            }
            return true;
        }

        /// <summary>
        /// Проверяет корректность длины числа при создании игры
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private bool IsCorrectInputSizeNumber(int number)
        {
            if (number < 2 || number > 4)
            {
                throw new Exception("Число должно быть 2, 3 или 4-x значным");
            }
            return true;
        }

        /// <summary>
        /// Проверка длинны числа
        /// </summary>
        /// <param name="number">длинна числа</param>
        /// <returns></returns>
        private bool IsCorrectSizeNumber(int number)
        {
            if (number != _sizeNumber)
            {
                throw new Exception($"Длинна числа должна быть равна {_sizeNumber}");
            }
            return true;
        }
    }
}
