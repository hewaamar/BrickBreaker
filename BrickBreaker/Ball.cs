using System;
using System.Drawing;
using System.Windows.Forms;

namespace BrickBreaker
{
    public class Ball
    {
        public int x, y, size;
        public double xSpeed, ySpeed;
        public Color colour;

        public static Random rand = new Random();

        
 

        public Ball(int _x, int _y, int _xSpeed, int _ySpeed, int _ballSize)
        {
            x = _x;
            y = _y;
            xSpeed = _xSpeed;
            ySpeed = _ySpeed;
            size = _ballSize;
        }

        public void Move()
        {
            x = Convert.ToInt32(x + xSpeed);
            y = Convert.ToInt32(y + ySpeed);
            if (xSpeed >= 0.1)
            {
                xSpeed = 2;
            }
            else if (xSpeed <= 10)
            {
                xSpeed = 2;
            }
            if (ySpeed >= 0.1)
            {
                ySpeed *= -1;
            }
            else if (ySpeed <= 10)
            {
                ySpeed *= -1;
            }
        }

        public bool BlockCollision(Block b)
        {


            Rectangle blockRec = new Rectangle(b.x, b.y, b.width, b.height);
            Rectangle ballRec = new Rectangle(x, y, size, size);
            while (true)
            {
                if (ballRec.IntersectsWith(blockRec))
                {
                    ySpeed *= -0.5;
                    xSpeed *= rand.Next(-1, 1);
                }
                
                return blockRec.IntersectsWith(ballRec);
            }
        }

        public void PaddleCollision(Paddle p)
        {
            Rectangle ballRec = new Rectangle(x, y, size, size);
            Rectangle paddleRec = new Rectangle(p.x, p.y, p.width, p.height);

            if (ballRec.IntersectsWith(paddleRec))
            {
                ySpeed *= -1;

                xSpeed *= 0.5;
             
                
            }
        }

        public void WallCollision(UserControl UC)
        {
            // Collision with left wall
            if (x <= 0)
            {
                xSpeed *= -1;
            }
            // Collision with right wall
            if (x >= (UC.Width - size))
            {
                xSpeed *= -1;
            }
            // Collision with top wall
            if (y <= 2)
            {
                ySpeed *= -1;
            }
        }

      

        public bool BottomCollision(UserControl UC)
        {
            Boolean didCollide = false;

            if (y >= UC.Height)
            {
                didCollide = true;
            }

            return didCollide;
        }
    }
}
