using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingVisualApp
{
    public class Vector2D
    {
        public double x { get; set; }

        public double y { get; set; }

        public Vector2D(double x = 0.0, double y = 0.0)
        {
            this.x = x;
            this.y = y;
        }

        //
        // Summary:
        //     Копирование вектора
        //
        // Returns:
        //     Новый вектор
        public Vector2D CopyToVector()
        {
            return new Vector2D(x, y);
        }

        //
        // Summary:
        //     Копирование вектора в массив
        //
        // Returns:
        //     Новый массив [x, y]
        public double[] CopyToArray()
        {
            return new double[2] { x, y };
        }

        //
        // Summary:
        //     Присвоение скалярных значений вектору
        //
        // Parameters:
        //   x:
        //
        //   y:
        public void Set(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        //
        // Summary:
        //     Текстовое представление вектора
        //
        // Returns:
        //     Строка вида "[ x, y ]"
        public override string ToString()
        {
            return $"Vector2D Object: [ {x}, {y} ]";
        }

        //
        // Summary:
        //     Сложение вектора со скалярами
        //
        // Parameters:
        //   vector:
        //
        // Returns:
        //     Возвращает новый вектор и изменяет текущий
        public Vector2D Add(double x, double y)
        {
            Vector2D vector2D = new Vector2D();
            this.x += x;
            this.y += y;
            return CopyToVector();
        }

        //
        // Summary:
        //     Сложение двух векторов
        //
        // Parameters:
        //   v:
        //
        // Returns:
        //     Возвращает новый вектор и изменяет текущий
        public Vector2D Add(Vector2D v)
        {
            Vector2D vector2D = new Vector2D();
            x += v.x;
            y += v.y;
            return CopyToVector();
        }

        //
        // Summary:
        //     Сложение двух векторов
        //
        // Parameters:
        //   v1:
        //
        //   v2:
        //
        // Returns:
        //     Возвращает новый вектор
        public static Vector2D Add(Vector2D v1, Vector2D v2)
        {
            Vector2D vector2D = new Vector2D();
            vector2D = v1.CopyToVector();
            vector2D.Add(v2);
            return vector2D;
        }

        //
        // Summary:
        //     Вычитание из вектора скаляров
        //
        // Parameters:
        //   x:
        //
        //   y:
        //
        // Returns:
        //     Возвращает новый вектор и изменяет текущий
        public Vector2D Sub(double x, double y)
        {
            Vector2D vector2D = new Vector2D();
            this.x -= x;
            this.y -= y;
            return CopyToVector();
        }

        //
        // Summary:
        //     Вычитание из вектора другого вектора
        //
        // Parameters:
        //   v:
        //
        // Returns:
        //     Возвращает новый вектор и изменяет текущий
        public Vector2D Sub(Vector2D v)
        {
            Vector2D vector2D = new Vector2D();
            x -= v.x;
            y -= v.y;
            return CopyToVector();
        }

        //
        // Summary:
        //     Вычитание двух векторов
        //
        // Parameters:
        //   v1:
        //
        //   v2:
        //
        // Returns:
        //     Возвращает новый вектор
        public static Vector2D Sub(Vector2D v1, Vector2D v2)
        {
            Vector2D vector2D = new Vector2D();
            vector2D = v1.CopyToVector();
            vector2D.Sub(v2);
            return vector2D;
        }

        //
        // Summary:
        //     Деление вектора на скаляр
        //
        // Parameters:
        //   n:
        //
        // Returns:
        //     Возвращает новый вектор и изменяет текущий
        public Vector2D Div(double n)
        {
            Vector2D vector2D = new Vector2D();
            x /= n;
            y /= n;
            return CopyToVector();
        }

        //
        // Summary:
        //     Деление вектора на другой вектор
        //
        // Parameters:
        //   v:
        //
        // Returns:
        //     Возвращает новый вектор и изменяет текущий
        public Vector2D Div(Vector2D v)
        {
            Vector2D vector2D = new Vector2D();
            x /= v.x;
            y /= v.y;
            return CopyToVector();
        }

        //
        // Summary:
        //     Divide (деление) двух векторов
        //
        // Parameters:
        //   val:
        //
        // Returns:
        //     Возвращает новый вектор
        public static Vector2D Div(Vector2D v1, Vector2D v2)
        {
            Vector2D vector2D = new Vector2D();
            vector2D = v1.CopyToVector();
            vector2D.Div(v2);
            return vector2D;
        }

        //
        // Summary:
        //     Деление вектора на скаляр
        //
        // Parameters:
        //   v1:
        //     Вектор
        //
        //   n:
        //     Скаляр
        //
        // Returns:
        //     Возвращает новый вектор
        public static Vector2D Div(Vector2D v1, double n)
        {
            Vector2D vector2D = new Vector2D();
            vector2D = v1.CopyToVector();
            vector2D.Div(n);
            return vector2D;
        }

        //
        // Summary:
        //     Multiply (умножение) вектора на число
        //
        // Parameters:
        //   n:
        //
        // Returns:
        //     Возвращает новый вектор и изменяет текущий
        public Vector2D Mult(double n)
        {
            Vector2D vector2D = new Vector2D();
            x *= n;
            y *= n;
            return CopyToVector();
        }

        //
        // Summary:
        //     Multiply (умножение) вектора на число
        //
        // Parameters:
        //   v:
        //
        //   val:
        //
        // Returns:
        //     Возвращает новый вектор
        public static Vector2D Mult(Vector2D v, double val)
        {
            Vector2D vector2D = new Vector2D();
            vector2D = v.CopyToVector();
            vector2D.Mult(val);
            return vector2D;
        }

        //
        // Summary:
        //     Скалярное (Dot) умножение векторов
        //
        // Parameters:
        //   v:
        //
        // Returns:
        //     Скаляр
        public double Dot(Vector2D v)
        {
            return x * v.x + y * v.y;
        }

        //
        // Summary:
        //     Векторное произведение векторов. Для 2D результат всегда скаляр, т.к. z = 0
        //
        // Parameters:
        //   v:
        //
        // Returns:
        //     Скаляр
        public double Cross(Vector2D v)
        {
            return x * v.y - y * v.x;
        }

        //
        // Summary:
        //     Интерполяция вектора к другому вектору
        //
        // Parameters:
        //   x:
        //
        //   y:
        //
        //   amt:
        //     Value between 0.0 (old vector) and 1.0 (new vector). 0.9 is very near the new
        //     vector. 0.5 is halfway in between
        //
        // Returns:
        //     Возвращает новый вектор и изменяет текущий
        public Vector2D Lerp(double x, double y, double amt)
        {
            Vector2D vector2D = new Vector2D();
            this.x += (x - this.x) * amt;
            this.y += (y - this.y) * amt;
            return CopyToVector();
        }

        //
        // Summary:
        //     Интерполяция вектора к другому вектору
        //
        // Parameters:
        //   v:
        //
        //   amt:
        //     Value between 0.0 (old vector) and 1.0 (new vector). 0.9 is very near the new
        //     vector. 0.5 is halfway in between
        public Vector2D Lerp(Vector2D v, double amt)
        {
            Vector2D vector2D = new Vector2D();
            x += (v.x - x) * amt;
            y += (v.y - y) * amt;
            return CopyToVector();
        }

        //
        // Summary:
        //     Интерполяция вектора к другому вектору
        //
        // Parameters:
        //   v1:
        //
        //   v2:
        //
        //   amt:
        //     Value between 0.0 (old vector) and 1.0 (new vector). 0.9 is very near the new
        //     vector. 0.5 is halfway in between
        //
        // Returns:
        //     Новый вектор
        public static Vector2D Lerp(Vector2D v1, Vector2D v2, double amt)
        {
            Vector2D vector2D = new Vector2D();
            vector2D = v1.CopyToVector();
            vector2D.Lerp(v2, amt);
            return vector2D;
        }

        //
        // Summary:
        //     Возвращает угол вектора
        //
        // Returns:
        //     Значение угла в радианах
        public double HeadingRad()
        {
            return Math.Atan2(y, x);
        }

        //
        // Summary:
        //     Возвращает угол вектора
        //
        // Returns:
        //     Значение угла в градусах [от 0 до 359]
        public double HeadingDeg()
        {
            double num = HeadingRad();
            return (num >= 0.0) ? (num * 180.0 / Math.PI) : ((Math.PI * 2.0 + num) * 360.0 / (Math.PI * 2.0));
        }

        //
        // Summary:
        //     Вычисление угла между двумя векторами
        //
        // Parameters:
        //   v:
        //
        // Returns:
        //     Возвращает угол в радианах
        public double angleBetween(Vector2D v)
        {
            double val = Dot(v) / (Mag() * v.Mag());
            return Math.Acos(Math.Min(1.0, Math.Max(-1.0, val)));
        }

        //
        // Summary:
        //     Поворот вектора на заданный угол
        //
        // Parameters:
        //   a:
        //     Угол в радианах
        //
        // Returns:
        //     Возвращает новый вектор и изменяет текущий
        public Vector2D Rotate(double a)
        {
            Vector2D vector2D = new Vector2D();
            double num = HeadingRad() + a;
            double num2 = Mag();
            x = Math.Cos(num) * num2;
            y = Math.Sin(num) * num2;
            return CopyToVector();
        }

        //
        // Summary:
        //     Получение квадрата длины вектора
        //
        // Returns:
        //     Скаляр
        public double MagSq()
        {
            double num = x;
            double num2 = y;
            return num * num + num2 * num2;
        }

        //
        // Summary:
        //     Ограничение (Limit) длины вектора до max значения
        //
        // Parameters:
        //   max:
        //     Требуемая максимальная длина вектора
        //
        // Returns:
        //     Вектор с лимитированной длиной
        public Vector2D Limit(double max)
        {
            Vector2D vector2D = new Vector2D();
            double num = MagSq();
            if (num > max * max)
            {
                Div(Math.Sqrt(num)).Mult(max);
            }

            return CopyToVector();
        }

        //
        // Summary:
        //     Вычисление длины вектора
        //
        // Parameters:
        //   vecT:
        //
        // Returns:
        //     Скаляр
        public double Mag()
        {
            return Math.Sqrt(MagSq());
        }

        //
        // Summary:
        //     Нормализация вектора
        //
        // Returns:
        //     Вектор единичной длины
        public Vector2D Normalize()
        {
            double num = Mag();
            if (num != 0.0)
            {
                Mult(1.0 / num);
            }

            return this;
        }

        //
        // Summary:
        //     Задание длины вектора
        //
        // Parameters:
        //   n:
        //     целочисленная длина
        public void SetMag(int n)
        {
            Normalize().Mult(n);
        }

        //
        // Summary:
        //     Задание длины вектора
        //
        // Parameters:
        //   n:
        //     вещественная длина
        public void SetMag(double n)
        {
            Normalize().Mult(n);
        }

        //
        // Summary:
        //     Создание вектора по углу
        //
        // Parameters:
        //   angle:
        //     Угол в радианах
        //
        //   length:
        //     Длина вектора. По умолчанию длина = 1
        //
        // Returns:
        //     Возвращает новый вектор
        public static Vector2D FromAngle(double angle, double length = 1.0)
        {
            return new Vector2D(length * Math.Cos(angle), length * Math.Sin(angle));
        }

        //
        // Summary:
        //     Создание единичного вектора по случайному углу 2PI
        //
        // Parameters:
        //   rnd:
        //
        // Returns:
        //     Возвращает новый вектор
        public Vector2D Random2D(Random rnd)
        {
            return FromAngle(rnd.NextDouble() * Math.PI * 2.0);
        }

        //
        // Summary:
        //     Вычисление расстояния между векторами
        //
        // Parameters:
        //   v:
        //
        // Returns:
        //     Cкаляр
        public double Dist(Vector2D v)
        {
            return Sub(this, v).Mag();
        }

        //
        // Summary:
        //     Вычисление расстояния между двумя векторами
        //
        // Parameters:
        //   v1:
        //     Вектор 1
        //
        //   v2:
        //     Вектор 2
        //
        // Returns:
        //     Cкаляр
        public static double Dist(Vector2D v1, Vector2D v2)
        {
            return v1.Dist(v2);
        }

        //
        // Summary:
        //     Вычисление перпендикулярного вектора к вектору a
        //
        // Parameters:
        //   v:
        //     Вектор, к которому строится нормаль
        //
        // Returns:
        //     Новый перпендикулярный вектор
        public static Vector2D NormalVector(Vector2D a)
        {
            double num = 0.0;
            double num2 = 0.0;
            if (a.x != 0.0)
            {
                num2 = 1.0;
                num = (0.0 - a.y) * num2 / a.x;
            }
            else if (a.y != 0.0)
            {
                num = 1.0;
                num2 = (0.0 - a.x) * num / a.y;
            }

            return new Vector2D(num, num2);
        }

        //
        // Summary:
        //     Adds two vectors together
        //
        // Parameters:
        //   left:
        //     Вектор 1
        //
        //   right:
        //     Вектор 2
        //
        // Returns:
        //     Новый суммированный вектор
        public static Vector2D operator +(Vector2D left, Vector2D right)
        {
            return Add(left, right);
        }

        //
        // Summary:
        //     Изменение вектора на негативный
        //
        // Parameters:
        //   value:
        //     Вектор
        //
        // Returns:
        //     Новый отрицательный вектор
        public static Vector2D operator -(Vector2D value)
        {
            return value.Mult(-1.0);
        }

        //
        // Summary:
        //     Разность векторов
        //
        // Parameters:
        //   left:
        //     Вектор 1
        //
        //   right:
        //     Вектор 2
        //
        // Returns:
        //     Новый вектор
        public static Vector2D operator -(Vector2D left, Vector2D right)
        {
            return Sub(left, right);
        }

        //
        // Summary:
        //     Умножение скаляра на вектор
        //
        // Parameters:
        //   left:
        //     Скаляр
        //
        //   right:
        //     Вектор
        //
        // Returns:
        //     Новый вектор
        public static Vector2D operator *(float left, Vector2D right)
        {
            return Mult(right, left);
        }

        //
        // Summary:
        //     Умножение пар элементов обоих векторов
        //
        // Parameters:
        //   left:
        //     Вектор 1
        //
        //   right:
        //     Вектор 2
        //
        // Returns:
        //     Новый вектор
        public static Vector2D operator *(Vector2D left, Vector2D right)
        {
            return new Vector2D
            {
                x = left.x * right.x,
                y = left.y * right.y
            };
        }

        //
        // Summary:
        //     Умножение вектора на скаляр
        //
        // Parameters:
        //   left:
        //     Вектор
        //
        //   right:
        //     Скаляр
        //
        // Returns:
        //     Новый вектор
        public static Vector2D operator *(Vector2D left, float right)
        {
            return Mult(left, right);
        }

        //
        // Summary:
        //     Деление первого вектора на второй вектор
        //
        // Parameters:
        //   left:
        //     Вектор 1
        //
        //   right:
        //     Вектор 2
        //
        // Returns:
        //     Новый вектор
        public static Vector2D operator /(Vector2D left, Vector2D right)
        {
            return Div(left, right);
        }

        //
        // Summary:
        //     Деление вектора на скаляр
        //
        // Parameters:
        //   value1:
        //     Вектор
        //
        //   value2:
        //     Скаляр
        //
        // Returns:
        //     Новый вектор
        public static Vector2D operator /(Vector2D value1, float value2)
        {
            return Div(value1, value2);
        }

        //
        // Summary:
        //     Returns a value that indicates whether each pair of elements in two specified
        //     vectors is equal.
        //
        // Parameters:
        //   left:
        //     Вектор 1
        //
        //   right:
        //     Вектор 2
        //
        // Returns:
        //     true if left and right are equal; otherwise, false.
        public static bool operator ==(Vector2D left, Vector2D right)
        {
            if (left.x == right.x && left.y == right.y)
            {
                return true;
            }

            return false;
        }

        //
        // Summary:
        //     Returns a value that indicates whether two specified vectors are not equal.
        //
        // Parameters:
        //   left:
        //     Вектор 1
        //
        //   right:
        //     Вектор 2
        //
        // Returns:
        //     true if left and right are not equal; otherwise, false.
        public static bool operator !=(Vector2D left, Vector2D right)
        {
            if (left.x != right.x || left.y != right.y)
            {
                return true;
            }

            return false;
        }
    }
}
