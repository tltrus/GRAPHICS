using System.Windows.Media.Imaging;

namespace WpfApp
{
    internal class WaterRipples
    {
        int cols, rows;
        float[,] current, previous;
        float dampening = 0.89f;

        public WaterRipples(int width, int height) 
        {
            cols = width;
            rows = height;
            current = new float[rows, cols];
            previous = new float[rows, cols];
        }

        public void Drawing(WriteableBitmap wb)
        {
            for (int i = 1; i < rows - 1; ++i)
            {
                for (int j = 1; j < cols - 1; ++j)
                {
                    current[i, j] = (previous[i - 1, j] + previous[i + 1, j] + previous[i, j - 1] + previous[i, j + 1]) / 2 - current[i, j];

                    current[i, j] *= dampening;

                    byte R = (byte)current[i, j];
                    byte G = (byte)current[i, j];
                    byte B = (byte)current[i, j];

                    wb.SetPixel(j, i, R, G, B);
                }
            }

            Tools.Swap(current, previous, rows, cols);
        }

        public void Click(int x, int y) => previous[y, x] = 400;
    }
}
