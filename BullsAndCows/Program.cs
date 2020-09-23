using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullsAndCows
{
    class Program
    {
        static void Main(string[] args)
        {
            GameBullsAndCows gameBullsAndCows = new GameBullsAndCows(4, 1);
            gameBullsAndCows.StartGame();
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

            return $"{Bulls} {bullsWord}, {Cows} {cowsWord}";
        }
    }

    class GameBullsAndCows
    {
        private Random _rand;
        private List<int> _hideNumber;
        private List<int> _guessNumber;
        private int _sizeNumber;
        private int _numberModeGame;
        private int _countNumbersForBot;
        private bool _isWin;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sizeNumber">длинна числа(2, 3 или 4-х значное число)</param>
        /// <param name="numberModeGame">тип игры (1, 2, 3)</param>
        /// <param name="countNumbersForBot">количество чисел, которое загадает программа боту</param>
        public GameBullsAndCows(int sizeNumber, int numberModeGame, int countNumbersForBot = 5)
        {
            if (IsCorrectInputSizeNumber(sizeNumber) && IsCorrectNumberModeGame(numberModeGame))
            {
                _rand = new Random();
                _sizeNumber = sizeNumber;
                _hideNumber = new List<int>(sizeNumber);
                _guessNumber = new List<int>(sizeNumber);
                _numberModeGame = numberModeGame;
                _countNumbersForBot = countNumbersForBot;
                _isWin = false;
            }
        }

        /// <summary>
        /// Запуск игры
        /// </summary>
        public void StartGame()
        {
            switch (_numberModeGame)
            {
                case 1:
                    GuessPlayer();
                    break;
                case 2:
                    GuessBot();
                    break;
                case 3:
                    ShowBotStatistic();
                    break;
            }
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
            Console.Write($"Введите число длинной {_sizeNumber}: ");
            string inputNumber = Console.ReadLine();
            
            List<int> _guessNumbes = new List<int>();
            if (IsCorrectInputNumber(inputNumber))
            {
                _guessNumber = ConvertStringToList(inputNumber);
                _guessNumbes.ToString();
            }
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
        private BullsAndCows GetBullsAndCows(List<int> startNumber, List<int> currentNumber)
        {
            int bulls = 0;
            int cows = 0;

            for (int i = 0; i < startNumber.Count; i++)
            {
                if (startNumber[i] == currentNumber[i])
                {
                    ++bulls;
                    continue;
                }
                for (int j = 0; j < startNumber.Count; j++)
                {
                    if (startNumber[i] == currentNumber[j])
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
