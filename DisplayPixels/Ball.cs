using System.Drawing;


namespace Read_dispaly_pixels
{
    internal class Ball
    {
        public int x, y, size, speed, screenHeight;
        public SolidBrush brush;

        public Ball(int x, int y, int size, SolidBrush brush, int speed, int screenHeight) 
        { 
            this.x = x;
            this.y = y;
            this.size = size;
            this.brush = brush;
            this.speed = speed;
            this.screenHeight = screenHeight;
        }

        public void Update()
        {
            y = y + speed;

            if (y > screenHeight)
                y = 0;
        }

        public void Draw(Graphics graphic)
        {
            graphic.FillEllipse(brush, new Rectangle(x - 25, y - 25, size, size));
        }
    }
}
