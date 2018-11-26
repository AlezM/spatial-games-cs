using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace spatialgamescs
{
    class Field 
    {
        int _size;
        bool[,] _field, _nextField;
        float[,] _scores;
        float _b;

        public Field(int size, float b)
        {
            _size = size;
            _b = b;

            _field = new bool[size, size];
            _nextField = new bool[size, size];
            _scores = new float[size, size];

            var rand = new Random();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    _field[i, j] = rand.NextDouble() < 0.21;
                }
            }            
        }

        public void Evolve() 
        {



            for (int col = 0; col < _size; col++)
            {
                for (int row = 0; row < _size; row++)
                {
                    float score = 0;

                    for (int i = -1; i <= 1; i++) //Row
                    {
                        for (int j = -1; j <= 1; j++) //Col
                        {
                            if (_field[(col + i + _size) % _size, (row + j + _size) % _size])
                                score++;
                        }
                    }

                    _scores[row, col] = !_field[row, +col] ? score * _b : score;
                }
            }


            for (int col = 0; col < _size; col++)
            {
                for (int row = 0; row < _size; row++)
                {
                    int[] bestStrategyIndex = { row, col };
                    int[] memberIndex = { row, col };

                    for (int i = -1; i <= 1; i++) //Row
                    {
                        for (int j = -1; j <= 1; j++) //Col
                        {
                            memberIndex = new int[2] { (row + j + _size) % _size, (col + i + _size) % _size };

                            if (_scores[bestStrategyIndex[0], bestStrategyIndex[1]] < _scores[memberIndex[0], memberIndex[1]])
                            {
                                bestStrategyIndex = memberIndex;
                            }
                        }
                    }

                    _nextField[row, col] = _field[bestStrategyIndex[0], bestStrategyIndex[1]];
                }
            }


        }
    }

    class MainClass
    {
        public static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");

            var watch = new Stopwatch();

            for (int size = 100; size <= 4001; size += 100)
            {
                watch.Restart();

                var field = new Field(size, 1.81f);

                for (int i = 0; i < 10; i++)
                {
                    field.Evolve();
                }

                watch.Stop();
                Console.Write($"[{size}, {watch.ElapsedMilliseconds * 0.001f / 20}], ");
            }
        }
    }
}




