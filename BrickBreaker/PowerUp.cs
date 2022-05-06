using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public void PaddleCollide(Paddle p)
        {
        }
    }
}
