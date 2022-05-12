using System;
using System.Drawing;
using System.Windows.Forms;

namespace BrickBreaker
{
    public class Ball
    {
        public int x, y, size, defaultSpeed;
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
            if (xSpeed == 0.1)
            {
                xSpeed *= -1.5;
            }
            else if (xSpeed >= 10)
            {
                xSpeed *= -1;
            }
            if (ySpeed == 0.1)
            {
                ySpeed *= -1.5;
            }
            else if (ySpeed >= 10)
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
                    if(xSpeed <= 0.2 && xSpeed >= -0.2)
                    {
                        xSpeed *= rand.Next(Convert.ToInt32(-1.5), -1);
                    }
                    else
                    {
                        xSpeed *= -1;
                    }

                    if (ySpeed <= 0.2 && xSpeed >= -0.2)
                    {
                        ySpeed = -2;
                    }
                    else
                    {
                        ySpeed *= -1;
                    }
                    
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
                x = p.x + size/2;
                y = p.y - size;
                ySpeed *= -1.5;

                xSpeed *= 1;

                if (xSpeed < 0.2 && xSpeed > -0.2)
                {
                    xSpeed = rand.Next(1, 3);
                }
                else if (xSpeed > 9 || xSpeed < -9)
                {
                    xSpeed *= 0.5;
                }
                else if (ySpeed < 0.5 && ySpeed > -0.5)
                {
                    ySpeed *= rand.Next(Convert.ToInt32(-1.5), -1);
                }
                else if (ySpeed > 9 || ySpeed < -9)
                {
                    ySpeed *= rand.Next(-1, Convert.ToInt32(-0.5));
                }



            }
        }
        //public void RandomDirection()
        //{
        //    //change angle
        //    int xDifAngle = rand.Next(0, 2);
        //    int yDifAngle = rand.Next(0, 2);

        //    //change speed depending on original direction
        //    if (xSpeed > 0)
        //    {
        //        xSpeed = defaultSpeed;
        //        xSpeed += xDifAngle;
        //    }
        //    else
        //    {
        //        xSpeed = -defaultSpeed;
        //        xSpeed -= xDifAngle;
        //    }

        //    if (ySpeed > 0)
        //    {
        //        ySpeed = defaultSpeed;
        //        ySpeed += yDifAngle;
        //    }
        //    else
        //    {
        //        ySpeed = -defaultSpeed;
        //        ySpeed -= yDifAngle;
        //    }
        //}
        public void Movement()
        {

            //if (xSpeed == 0 || ySpeed == 0)
            //{
            //    ySpeed += 5;
            //    xSpeed += 5;
            //}
            if(xSpeed > 0)
            {
                xSpeed *= rand.Next(1, 3);
            }
            else if(ySpeed > 0)
            {
                ySpeed *= rand.Next(-1, 3);
            }
            else if(ySpeed <= 0)
            {
                ySpeed *= -1;
            }
            else if (xSpeed <= 0)
            {
                xSpeed *= -1;
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
