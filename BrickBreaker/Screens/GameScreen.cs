/*  Created by: 
 *  Project: Brick Breaker
 *  Date: 
 */ 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Xml;

namespace BrickBreaker
{
    public partial class GameScreen : UserControl
    {
        #region global value

        Random r = new Random();
        //player1 button control keys - DO NOT CHANGE
        Boolean leftArrowDown, rightArrowDown;

        // Game values
        int lives;

        // Paddle and Ball objects
        Paddle paddle;
        Ball ball;
        PowerUp powerUp;

        //list of powerups for current level
        List<PowerUp> powerups = new List<PowerUp>();
        int powerUpCheck;
        // list of all blocks for current level
        List<Block> blocks = new List<Block>();

        // Brushes
        SolidBrush paddleBrush = new SolidBrush(Color.White);
        SolidBrush ballBrush = new SolidBrush(Color.White);
        SolidBrush blockBrush = new SolidBrush(Color.Red);
        SolidBrush powerupBrush = new SolidBrush(Color.Green);

        #endregion

        //game values
        int currentLevel;

        public GameScreen()
        {
            InitializeComponent();
            OnStart();
        }

        public void OnStart()
        {
            //set life counter
            lives = 3;

            //set all button presses to false.
            leftArrowDown = rightArrowDown = false;

            // setup starting paddle values and create paddle object
            int paddleWidth = 80;
            int paddleHeight = 20;
            int paddleX = ((this.Width / 2) - (paddleWidth / 2));
            int paddleY = (this.Height - paddleHeight) - 60;
            int paddleSpeed = 8;
            paddle = new Paddle(paddleX, paddleY, paddleWidth, paddleHeight, paddleSpeed, Color.White);

            // setup starting ball values
            int ballX = this.Width / 2 - 10;
            int ballY = this.Height - paddle.height - 80;

            // Creates a new ball
            Random Randgen = new Random();
            int xSpeed = 6;
            int ySpeed = 6;
            int ballSize = 20;
            ball = new Ball(ballX, ballY, xSpeed, ySpeed, ballSize);

            //go to next level
            nextLevel();
       
            // start the game engine loop
            gameTimer.Enabled = true;

            //setup powerup values for testing purposes
            int powerUpX;
            int powerUpY;
            int powerUpSpeed = 3;
            int powerUpSize = ballSize / 2;

        }

        //code to go from one level to the next
        public void nextLevel()
        {
            blocks.Clear();
            string level = $"level0{currentLevel}.xml";

            try
            {
                XmlReader reader = XmlReader.Create(level);

                int newX, newY, newHp, newWidth, newHeight;
                Color newColour;

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Text)
                    {
                        newX = Convert.ToInt32(reader.ReadString());

                        reader.ReadToNextSibling("y");
                        newY = Convert.ToInt32(reader.ReadString());

                        reader.ReadToNextSibling("hp");
                        newHp = Convert.ToInt32(reader.ReadString());

                        reader.ReadToNextSibling("width");
                        newWidth = Convert.ToInt32(reader.ReadString());

                        reader.ReadToNextSibling("height");
                        newHeight = Convert.ToInt32(reader.ReadString());

                        reader.ReadToNextSibling("colour");
                        newColour = Color.FromName(reader.ReadString());

                        Block b = new Block(newX, newY, newHp, newWidth, newHeight, newColour);
                        blocks.Add(b);
                    }
                }
                reader.Close();
            }
            catch
            {
                //if level doesnt exist then switch to either winner or loser screen
                return;
            }

        }
        private void GameScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //player 1 button presses
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
                    break;
                default:
                    break;
            }
        }

        private void GameScreen_KeyUp(object sender, KeyEventArgs e)
        {
            //player 1 button releases
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = false;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    break;
                default:
                    break;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            // Move the paddle
            if (leftArrowDown && paddle.x > 0)
            {
                paddle.Move("left");
            }
            if (rightArrowDown && paddle.x < (this.Width - paddle.width))
            {
                paddle.Move("right");
            }

            // Move ball
            ball.Move();

            //Drop powerups down
            foreach (PowerUp powerUp in powerups)
            {
                powerUp.Move();
            }

            // Check for collision with top and side walls
            ball.WallCollision(this);

            // Check for ball hitting bottom of screen
            if (ball.BottomCollision(this))
            {
                lives--;

                // Moves the ball back to origin
                ball.x = ((paddle.x - (ball.size / 2)) + (paddle.width / 2));
                ball.y = (this.Height - paddle.height) - 85;

                if (lives == 0)
                {
                    gameTimer.Enabled = false;
                    OnEnd();
                }
            }

            // Check for collision of ball with paddle, (incl. paddle movement)
            ball.PaddleCollision(paddle);

            //Check for collision of powerup and paddle

            try
            {
                foreach (PowerUp p in powerups)
                {
                    powerUp.PaddleCollide(paddle);
                    if (p.y >= paddle.y)
                    {
                        powerups.Remove(powerUp);
                    }
                }
            }
            catch
            {

            }
            // Check if ball has collided with any blocks
            foreach (Block b in blocks)
            {
                if (ball.BlockCollision(b))
                {
                    
                    blocks.Remove(b);

                    //check if powerups spawn
                    powerUpCheck = r.Next(0, 2);
                    if (powerUpCheck == 1)
                    {
                        int powerUpX = b.x;
                        int powerUpY = b.y;
                        int powerUpSpeed = 3;
                        int powerUpSize = 10;
                        powerUp = new PowerUp(powerUpX, powerUpY, powerUpSpeed, powerUpSize);
                        powerups.Add(powerUp);
                    }
                    if (blocks.Count == 0)
                    {
                        gameTimer.Enabled = false;
                        OnEnd();
                    }

                    break;
                }
            }

            //redraw the screen
            Refresh();
        }

        public void OnEnd()
        {
            // Goes to the game over screen
            Form form = this.FindForm();
            MenuScreen ps = new MenuScreen();
            
            ps.Location = new Point((form.Width - ps.Width) / 2, (form.Height - ps.Height) / 2);

            form.Controls.Add(ps);
            form.Controls.Remove(this);
        }

        public void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            // Draws paddle
            paddleBrush.Color = paddle.colour;
            e.Graphics.FillRectangle(paddleBrush, paddle.x, paddle.y, paddle.width, paddle.height);

            // Draws blocks
            foreach (Block b in blocks)
            {
                e.Graphics.FillRectangle(blockBrush, b.x, b.y, b.width, b.height);
            }

            // Draws ball
            e.Graphics.FillRectangle(ballBrush, ball.x, ball.y, ball.size, ball.size);

            //Draws PowerUp
            foreach (PowerUp powerUp in powerups )
            {
                e.Graphics.FillRectangle(powerupBrush, powerUp.x, powerUp.y, powerUp.size, powerUp.size);
            }
        }
    }
}
