using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BrickBreaker
{
    internal class PowerUp
    {
        public int speed, x, y, size;
        public static Random Random = new Random();

        public PowerUp( int _x, int _y, int _speed, int _size)
        {
            speed = _speed;
            x = _x;
            y = _y;
            size = _size;
        }
        public void Move()
        {
            y = y + speed;
        }
        public bool PaddleCollide(Paddle p)
        {
            Rectangle powerUpRec = new Rectangle(x, y, size, size);
            Rectangle paddleRec = new Rectangle(p.x, p.y, p.width, p.height);

            if (powerUpRec.IntersectsWith(paddleRec))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void PowerUpActive()
        {

        }
    }
}
