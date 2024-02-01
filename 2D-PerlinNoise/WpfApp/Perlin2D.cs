using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerlinNoise
{
    class Perlin2D
    {
        byte[] permutationTable;

        public Perlin2D(int seed = 0)
        {
            var rand = new System.Random(seed);
            permutationTable = new byte[1024];
            rand.NextBytes(permutationTable);
        }

        /// <summary>
        /// Линейная интерполяция
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private double Lerp(double a, double b, double t)
        {
            return a + (b - a) * t;
        }

        // Для интерполяции: Уравнение пятой степени
        static double QunticCurve(double t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        /// <summary>
        /// хэш-функция с Простыми числами, обрезкой результата до размера массива со случайными байтами
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        double[] GetPseudoRandomGradientVector(int x, int y)
        {
            // v - это псевдо-случайное число от 0 до 3 которое всегда неизменно при данных x и y
            int v = (int)(((x * 1836311903) ^ (y * 2971215073) + 4807526976) & 1023);
            v = permutationTable[v] & 3; // & 3 здесь обрезает любое int32 число до 3

            switch (v)
            {
                case 0: return new double[] { 1, 0 };
                case 1: return new double[] { -1, 0 };
                case 2: return new double[] { 0, 1 };
                default: return new double[] { 0, -1 };
            }
        }

        /// <summary>
        /// Скалярное произведение векторов
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        static double Dot(double[] a, double[] b)
        {
            return a[0] * b[0] + a[1] * b[1];
        }

        /// <summary>
        /// Главный метод
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public double Noise(double fx, double fy)
        {
            // сразу находим координаты левой верхней вершины квадрата
            int left = (int)System.Math.Floor(fx);
            int top = (int)System.Math.Floor(fy);

            // а теперь локальные координаты точки внутри квадрата
            double pointInQuadX = fx - left;
            double pointInQuadY = fy - top;

            // извлекаем градиентные векторы для всех вершин квадрата:
            double[] topLeftGradient = GetPseudoRandomGradientVector(left, top);
            double[] topRightGradient = GetPseudoRandomGradientVector(left + 1, top);
            double[] bottomLeftGradient = GetPseudoRandomGradientVector(left, top + 1);
            double[] bottomRightGradient = GetPseudoRandomGradientVector(left + 1, top + 1);

            // вектора от вершин квадрата до точки внутри квадрата:
            double[] distanceToTopLeft = new double[] { pointInQuadX, pointInQuadY };
            double[] distanceToTopRight = new double[] { pointInQuadX - 1, pointInQuadY };
            double[] distanceToBottomLeft = new double[] { pointInQuadX, pointInQuadY - 1 };
            double[] distanceToBottomRight = new double[] { pointInQuadX - 1, pointInQuadY - 1 };

            // считаем скалярные произведения между которыми будем интерполировать
            /*
             tx1--tx2
              |    |
             bx1--bx2
            */
            double tx1 = Dot(distanceToTopLeft, topLeftGradient);
            double tx2 = Dot(distanceToTopRight, topRightGradient);
            double bx1 = Dot(distanceToBottomLeft, bottomLeftGradient);
            double bx2 = Dot(distanceToBottomRight, bottomRightGradient);

            // готовим параметры интерполяции, чтобы она не была линейной:
            pointInQuadX = QunticCurve(pointInQuadX);
            pointInQuadY = QunticCurve(pointInQuadY);

            // собственно, интерполяция:
            double tx = Lerp(tx1, tx2, pointInQuadX);
            double bx = Lerp(bx1, bx2, pointInQuadX);
            double tb = Lerp(tx, bx, pointInQuadY);

            // возвращаем результат:
            return tb;  // Возвращает число от -1.0 до 1.0
        }

        public double Noise(double fx, double fy, int octaves, double persistence = 0.5)
        {
            double amplitude = 1;
            double max = 0;
            double result = 0;

            while (octaves-- > 0)
            {
                max += amplitude;
                result += Noise(fx, fy) * amplitude;
                amplitude *= persistence;
                fx *= 2;
                fy *= 2;
            }

            return result / max;
        }

    }
}
