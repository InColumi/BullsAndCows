using System;
using System.Collections.Generic;
using System.Threading;

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
            if (int.TryParse(userInput, out sizeNumber))
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
            Console.ReadKey();
        }
    }

    class BullsAndCows
    {
        public int Bulls { get; private set; }
        public int Cows { get; private set; }

        public BullsAndCows(int bulls, int cows)
        {
            if(IsCorrect(bulls, cows))
            {
                Bulls = bulls;
                Cows = cows;
            }
            else
            {
                throw new Exception("Погугли как задавать быков и коров");
            }
        }
        private bool IsCorrect(int bulls, int cows)
        {
            return bulls >= 0 && bulls <= 4 && cows >= 0 && cows <= 4;
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
            else if (Cows == 4)
            {
                comment = "Нормаль так...";
            }

            return $"{Bulls} {bullsWord}, {Cows} {cowsWord} " + comment;
        }

        public static bool operator == (BullsAndCows value1, BullsAndCows value2)
        {
            return value1.Bulls == value2.Bulls && value1.Cows == value2.Cows;
        }

        public static bool operator != (BullsAndCows value1, BullsAndCows value2)
        {
            return value1.Bulls != value2.Bulls || value1.Cows != value2.Cows;
        }
    }

    class GameBullsAndCows
    {
        private Random _rand;
        private int[] _hideNumber;
        private int[] _guessNumber;
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
                _hideNumber = new int[sizeNumber];
                _guessNumber = new int[sizeNumber];
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
                    if (_sizeNumber == 4)
                    {
                        ShowInfoGame("Бот угадывает");
                        Console.WriteLine("Запомните или запишите число, которое нужно угадать.");
                        Console.WriteLine();
                        GuessBot();
                    }
                    else
                    {
                        Console.WriteLine("Режим \"Бот угадывает\" работает только для размерности 4.");
                    }
                    break;
                case 3:
                    if (_sizeNumber == 4)
                    {
                        ShowInfoGame("Бот угадывает и выводится статистика его попыток");
                        DebagBot();
                    }
                    else
                    {
                        Console.WriteLine("Режим \"Бот угадывает и выводится статистика его попыток\" работает только для размерности 4.");
                    }
                    break;
            }
        }

        private void ShowInfoGame(string name)
        {
            Console.WriteLine($"Вы выбрали режим: {name}. Длина числа равна: {_sizeNumber}");
        }

        /// <summary>
        /// Бот отгадывает список чисел
        /// </summary>
        private void DebagBot()
        {
            List<int[]> allNumbers = GetAllNumbers();
            List<int[]> numbersForGuess;
            bool isCorrect = true;
            while (isCorrect)
            {
                Console.Write("Введите сколько чисел нужно угадать:");
                string userInput = Console.ReadLine();
                int countNumbers;
                if (int.TryParse(userInput, out countNumbers))
                {
                    if (countNumbers > 0)
                    {
                        List<int[]> randNumbers = GetRandomNumbers(allNumbers, countNumbers);
                        for (int i = 0; i < randNumbers.Count; i++)
                        {
                            bool isNextNumber = true;
                            int countTries = 0;
                            
                            numbersForGuess = allNumbers;
                            while (isNextNumber)
                            {
                                int[] randNumberFromNumbersForGuess = numbersForGuess[_rand.Next(0, numbersForGuess.Count)];
                                BullsAndCows bullsAndCowsFromRandomNumber = GetBullsAndCows(randNumbers[i], randNumberFromNumbersForGuess);
                                if (bullsAndCowsFromRandomNumber.Bulls == 4)
                                {
                                    Console.WriteLine($"Число {ConvertNumberToString(randNumbers[i])} отгадал за {countTries} попыток.");
                                    isNextNumber = false;
                                    continue;
                                }
                                numbersForGuess = GetNumbersByRule(numbersForGuess, randNumberFromNumbersForGuess, bullsAndCowsFromRandomNumber);
                                ++countTries;
                            }
                        }
                        isCorrect = false;
                        continue;
                    }
                }
                Console.WriteLine("Введите корректный данные.");
                Thread.Sleep(1000);
                Console.Clear();
            }
        }

        /// <summary>
        /// Вернет список случайных чисел из списка numbers длинной count
        /// </summary>
        /// <param name="numbers">список откуда будет браться случайные числа</param>
        /// <param name="count">размер нового списка</param>
        /// <returns>список длинной count</returns>
        private List<int[]> GetRandomNumbers(List<int[]> numbers, int count)
        {
            List<int[]> randNumbers = new List<int[]>();
            while (count > 0)
            {
                randNumbers.Add(numbers[_rand.Next(0, numbers.Count)]);
                --count;
            }
            return randNumbers;
        }

        /// <summary>
        /// Бот угадывает число загаданное игроком
        /// </summary>
        private void GuessBot()
        {
            List<int[]> allNumbers = GetAllNumbers();
            bool isGuess = false;
            int countTries = 1;
            while (isGuess == false)
            {
                Console.WriteLine($"Попытка №{countTries}.");
                int[] randomNumber = allNumbers[_rand.Next(0, allNumbers.Count)];
                Console.Write($"Ваше число {ConvertNumberToString(randomNumber)}? Введите подсказку: \nБыков: ");
                
                string userInputBulls = Console.ReadLine();
                Console.Write("Коров: ");
                string userInputCows = Console.ReadLine();
                Console.WriteLine();
                int bulls;
                int cows;

                if (int.TryParse(userInputBulls, out bulls) && int.TryParse(userInputCows, out cows))
                {
                    BullsAndCows bullsAndCowsUserInput = new BullsAndCows(bulls, cows);
                    if (bullsAndCowsUserInput.Bulls == 4)
                    {
                        ShowInfoGame(randomNumber, countTries);
                        isGuess = true;
                    }
                    else
                    {
                        allNumbers = GetNumbersByRule(allNumbers, randomNumber, bullsAndCowsUserInput);
                        ++countTries;
                        if (allNumbers.Count == 1)
                        {
                            ShowInfoGame(randomNumber, countTries);
                            isGuess = true;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Введите корректный данные.");
                    Thread.Sleep(1000);
                    Console.Clear();
                }        
            }
        }

        private void ShowInfoGame(int[] randomNumber, int countRounds)
        {
            Console.WriteLine($"Ваше число: {ConvertNumberToString(randomNumber)}. Отгадал с {countRounds} попытки.");
        }

        /// <summary>
        /// Возвращает список чисел, которые удовлетворяют правилу rule
        /// </summary>
        /// <param name="numbers">список чисел</param>
        /// <param name="number">возможное число</param>
        /// <param name="rule">кол-во быков и коров</param>
        /// <returns>список чисел удовлетворяющий правилу rule</returns>
        private List<int[]> GetNumbersByRule(List<int[]> numbers, int[] number, BullsAndCows rule)
        {

            List<int[]> newNumbers = new List<int[]>();
            BullsAndCows bullsAndCows;
            for (int i = 0; i < numbers.Count; i++)
            {
                bullsAndCows = GetBullsAndCows(number, numbers[i]);
                if (rule == bullsAndCows)
                {
                    newNumbers.Add(numbers[i]);
                }
            }
            return newNumbers;
        }

        /// <summary>
        /// Возвращает список всех чисел, которые возможны в игре
        /// </summary>
        /// <param name="size">количество чисел(по умолчанию 5040)</param>
        /// <returns></returns>
        private List<int[]> GetAllNumbers(int size = 5040)
        {
            List<int[]> numbers = new List<int[]>(size);
            int[] number = new int[4];
            for (int i = 0; i <= 9; i++)
            {
                number[0] = i;
                for (int j = 0; j <= 9; j++)
                {
                    number[1] = j;
                    if (i == j)
                    {
                        continue;
                    }
                    for (int k = 0; k <= 9; k++)
                    {
                        if (j == k || i == k)
                        {
                            continue;
                        }
                        number[2] = k;
                        for (int p = 0; p <= 9; p++)
                        {
                            if (k == p || j == p)
                            {
                                continue;
                            }
                            number[3] = p;
                            if (CheckNumber(number))
                            {
                                numbers.Add(new int[4] { i, j, k, p });
                            }
                        }
                    }
                }
            }

            return numbers;
        }

        /// <summary>
        /// Проверка числа на уникальность цифр
        /// </summary>
        /// <param name="number">число</param>
        /// <returns></returns>
        private bool CheckNumber(int[] number)
        {
            int countUnique = 0;
            for (int i = 0; i < number.Length; i++)
            {
                for (int j = 0; j < number.Length; j++)
                {
                    if (number[i] == number[j])
                        ++countUnique;
                }
                if (countUnique > 1)
                    return false;
                countUnique = 0;
            }
            return true;
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

                    BullsAndCows bullsAndCows = GetBullsAndCows(_guessNumber, _hideNumber);
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
        private  int[] ConvertStringToList(string number)
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
            return newNumber.ToArray();
        }

        /// <summary>
        /// Проверка числа, которое ввел пользователь
        /// </summary>
        /// <param name="numbersString">число</param>
        /// <returns></returns>
        private bool IsCorrectInputNumber(string numbersString)
        {
            if (IsCorrectSizeNumber(numbersString.Length))
            {
                int uniqueNumber = 0;
                for (int i = 0; i < numbersString.Length; i++)
                {
                    for (int j = 0; j < numbersString.Length; j++)
                    {
                        if (numbersString[i] == numbersString[j])
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
        private BullsAndCows GetBullsAndCows(int[] a, int[] b)
        {
            int bulls = 0;
            int cows = 0;

            for (int i = 0; i < b.Length; i++)
            {
                if (b[i] == a[i])
                {
                    ++bulls;
                    continue;
                }
                for (int j = 0; j < b.Length; j++)
                {
                    if (b[i] == a[j])
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
        private int[] GetGenerateNumber(int sizeNumber)
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
            return randNumbers.ToArray();
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

        /// <summary>
        /// Конертирует число в строку
        /// </summary>
        /// <param name="number">число</param>
        /// <returns>число в виде строки</returns>
        private string ConvertNumberToString(int[] number)
        {
            return "" + number[0] + number[1] + number[2] + number[3];
        }
    }
}
