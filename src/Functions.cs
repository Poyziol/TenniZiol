using System;
using System.Data;
using Npgsql;
using System.Drawing;
using System.Windows.Forms;

namespace src
{
    public class Functions
    {
        private int input1 = 0;
        private int input2 = 0;
        private Panel panel_tennis = null!;
        private CustomBall ball = null!;
        private Panel[] holesTop = null!;
        private Panel[] holesBottom = null!;
        private Random random = null!;
        private Label scoreLabel = null!;
        private int player1Score = 0;
        private int player2Score = 0;
        private int[] holeTopSpeeds = null!;
        private int[] holeBottomSpeeds = null!;
        private System.Windows.Forms.Timer ballMovementTimer  = null!;
        private Point ballDestination;
        private float ballSpeedX, ballSpeedY;

        private Panel player1Rect, player2Rect;
        private const int playerWidth = 60, playerHeight = 10;
        private const int moveSpeed = 20;

        public Functions(Panel panel_tennis)
        {
            this.panel_tennis = panel_tennis;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            scoreLabel = new Label()
            {
                Text = "Player 1: 0 - Player 2: 0",
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 14, FontStyle.Bold),
                Height = 40,
                Location = new Point(0, 50)
            };
            panel_tennis.Controls.Add(scoreLabel);

            random = new Random();
            ball = new CustomBall()
            {
                Size = new Size(20, 20),
                Location = new Point(random.Next(100, 480), 540)
            };
            panel_tennis.Controls.Add(ball);

            Panel[] holesTop_tmp1 = CreateHoles(new Point(400, 30), 50, 1, 20, 0);
            Panel[] holesBottom_tmp1 = CreateHoles(new Point(150, 520), 50, 1, 40, 0);
            Panel[] holesTop_tmp2 = CreateHoles(new Point(400, 30), 50, 1, 40, 1);
            Panel[] holesBottom_tmp2 = CreateHoles(new Point(150, 520), 50, 1, 20, 1);

            holesTop = new Panel[2];
            holesBottom = new Panel[2];

            holesTop[0] = holesTop_tmp1[0];
            holesTop[1] = holesTop_tmp2[0];
            holesBottom[0] = holesBottom_tmp1[0];
            holesBottom[1] = holesBottom_tmp2[0];

            holeTopSpeeds = new int[holesTop.Length];
            holeBottomSpeeds = new int[holesBottom.Length];

            holeTopSpeeds[0] = input2 * input1;
            holeTopSpeeds[1] = input1;
            holeBottomSpeeds[0] = input1;
            holeBottomSpeeds[1] = input2 * input1;

            ballMovementTimer = new System.Windows.Forms.Timer() { Interval = 10 };
            ballMovementTimer.Tick += BallMovementTimer_Tick;

            System.Windows.Forms.Timer holeMovementTimer = new System.Windows.Forms.Timer() { Interval = 100 };
            holeMovementTimer.Tick += HoleMovementTimer_Tick;
            holeMovementTimer.Start();

            panel_tennis.Paint += Panel_tennis_Paint;

            // Créer les rectangles pour les joueurs
            player1Rect = new Panel()
            {
                Size = new Size(playerWidth, playerHeight),
                BackColor = Color.Blue,
                Location = new Point(200, 560)
            };
            panel_tennis.Controls.Add(player1Rect);

            player2Rect = new Panel()
            {
                Size = new Size(playerWidth, playerHeight),
                BackColor = Color.Red,
                Location = new Point(200, 140)
            };
            panel_tennis.Controls.Add(player2Rect);

        }

        public void HandleKeyPress(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                MovePlayer(player1Rect, -moveSpeed);
            }
            else if (e.KeyCode == Keys.X)
            {
                MovePlayer(player1Rect, moveSpeed);
            }
            else if (e.KeyCode == Keys.L)
            {
                MovePlayer(player2Rect, -moveSpeed);
            }
            else if (e.KeyCode == Keys.M)
            {
                MovePlayer(player2Rect, moveSpeed);
            }
        }

        public void SetInputs(int input1, int input2)
        {
            this.input1 = input1;
            this.input2 = input2;

            holeTopSpeeds[0] = input2;
            holeTopSpeeds[1] = input1;
            holeBottomSpeeds[0] = input1;
            holeBottomSpeeds[1] = input2;
        }


        private void MovePlayer(Panel playerRect, int deltaX)
        {
            int newX = playerRect.Location.X + deltaX;
            if(newX >= 50 && newX <= 490)
            {
                playerRect.Location = new Point(newX, playerRect.Location.Y);
            }
        }

        private void Panel_tennis_Paint(object? sender, PaintEventArgs e)
        {
            Pen whitePen = new Pen(Color.White, 2);
            e.Graphics.DrawRectangle(whitePen, 50, 80, 500, 550); 
            e.Graphics.DrawLine(whitePen, 50, 350, 550, 350); 
            e.Graphics.DrawLine(whitePen, 300, 80, 300, 630); 
            e.Graphics.DrawLine(whitePen, 100, 80, 100, 630); 
            e.Graphics.DrawLine(whitePen, 500, 80, 500, 630); 
            e.Graphics.DrawLine(whitePen, 50, 140, 550, 140); 
            e.Graphics.DrawLine(whitePen, 50, 570, 550, 570); 
        }

        private Panel[] CreateHoles(Point startPosition, int spacing, int holeNumber, int xSize, int indice)
        {
            Panel[] holes = new Panel[holeNumber];
            for (int ni = 0; ni < holes.Length; ni++)
            {
                holes[ni] = new Panel()
                {
                    Size = new Size(xSize, 20),
                    BackColor = Color.Black,
                    Location = new Point(startPosition.X + indice * spacing, startPosition.Y + indice * 40 + 50) 
                };
                panel_tennis.Controls.Add(holes[ni]);
            }
            return holes;
        }

        public void ShootButton_Click(object? sender, EventArgs e)
        {
            if (ball.Location.Y > 350) 
                StartBallMovement(new Point(random.Next(50, 450), 150)); 

            if (ball.Location.Y < 350) 
                StartBallMovement(new Point(random.Next(50, 450), 540)); 
        }

       public void ViewHistory_Click(object? sender, EventArgs e)
        {
            History historyForm = new History();
            historyForm.Show();
        }


        private void StartBallMovement(Point destination)
        {
            ballDestination = destination;
            int deltaX = ballDestination.X - ball.Location.X;
            int deltaY = ballDestination.Y - ball.Location.Y;
            float distance = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

            ballSpeedX = (deltaX / distance) * 5;
            ballSpeedY = (deltaY / distance) * 5;

            ballMovementTimer.Start();
        }

       private void BallMovementTimer_Tick(object? sender, EventArgs e)
        {
            ball.Location = new Point((int)(ball.Location.X + ballSpeedX), (int)(ball.Location.Y + ballSpeedY));

            if (ball.Location.X < 50 || ball.Location.X > 550 - ball.Width || ball.Location.Y < 80 || ball.Location.Y > 630 - ball.Height) 
            {
                ballMovementTimer.Stop();
                ball.Location = new Point(ball.Location.X, ball.Location.Y < 350 ? 150 : 540); 
            }

            if (ball.Bounds.IntersectsWith(holesTop[0].Bounds))
            {
                int points = input2;
                UpdateScore("Player 1", points); 
                ballMovementTimer.Stop();
                ball.Location = new Point(ball.Location.X, 150); 
                InsertScoreToDatabase(1, GetDisplayScore(points));
            }
            else if(ball.Bounds.IntersectsWith(holesBottom[0].Bounds))
            {
                int points = 1;
                UpdateScore("Player 2", points); 
                ballMovementTimer.Stop();
                ball.Location = new Point(ball.Location.X, 540); 
                InsertScoreToDatabase(2, GetDisplayScore(points)); 
            }
            else if(ball.Bounds.IntersectsWith(holesTop[1].Bounds))
            {
                int points = 1;
                UpdateScore("Player 1", points); 
                ballMovementTimer.Stop();
                ball.Location = new Point(ball.Location.X, 150); 
                InsertScoreToDatabase(1, GetDisplayScore(points)); 
            }
            else if(ball.Bounds.IntersectsWith(holesBottom[1].Bounds))
            {
                int points = input2;
                UpdateScore("Player 2", points);
                ballMovementTimer.Stop();
                ball.Location = new Point(ball.Location.X, 540); 
                InsertScoreToDatabase(2, GetDisplayScore(points)); 
            }

            if(ball.Bounds.IntersectsWith(player1Rect.Bounds))
            {
                ballSpeedY = -ballSpeedY;
                ballSpeedX = (ball.Location.X - player1Rect.Location.X) * 0.1f;
            }

            if(ball.Bounds.IntersectsWith(player2Rect.Bounds))
            {
                ballSpeedY = -ballSpeedY;
                ballSpeedX = (ball.Location.X - player2Rect.Location.X) * 0.1f;
            }
        }

        //verticale
        private void HoleMovementTimer_Tick(object? sender, EventArgs e)
        {
            for (int ni = 0; ni < holesTop.Length; ni++)
            {
                holesTop[ni].Location = new Point(holesTop[ni].Location.X + holeTopSpeeds[ni], holesTop[ni].Location.Y);
                if (holesTop[ni].Location.X <= 50 || holesTop[ni].Location.X >= 510)
                {
                    holeTopSpeeds[ni] = -holeTopSpeeds[ni];
                }
            }

            for (int ni = 0; ni < holesBottom.Length; ni++)
            {
                holesBottom[ni].Location = new Point(holesBottom[ni].Location.X + holeBottomSpeeds[ni], holesBottom[ni].Location.Y);
                if (holesBottom[ni].Location.X <= 50 || holesBottom[ni].Location.X >= 510)
                {
                    holeBottomSpeeds[ni] = -holeBottomSpeeds[ni];
                }
            }
        }

        //circulaire
        /*private void HoleMovementTimer_Tick(object? sender, EventArgs e)
        {
            const int centerX = 280;
            const int centerYTop1 = 30;
            const int centerYTop2 = 0;
            const int centerYBottom1 = 690;
            const int centerYBottom2 = 660;
            const double speed = 25;

            const int radiusTop1 = 60;
            const int radiusTop2 = 120; 
            const int radiusBottom1 = 120;
            const int radiusBottom2 = 60;

            double angleTop1 = (holeTopSpeeds[0] % 360) * Math.PI / 180;  
            holesTop[0].Location = new Point(
                (int)(centerX + radiusTop1 * Math.Cos(angleTop1)),
                (int)(centerYTop1 + radiusTop1 * Math.Sin(angleTop1))
            );
            holeTopSpeeds[0] += (int)speed;

            double angleTop2 = (holeTopSpeeds[1] % 360) * Math.PI / 180; 
            holesTop[1].Location = new Point(
                (int)(centerX + radiusTop2 * Math.Cos(angleTop2)),
                (int)(centerYTop2 + radiusTop2 * Math.Sin(angleTop2))
            );
            holeTopSpeeds[1] += (int)speed;

            // Déplacement circulaire du premier trou du bas
            double angleBottom1 = (holeBottomSpeeds[0] % 360) * Math.PI / 180;
            holesBottom[0].Location = new Point(
                (int)(centerX + radiusBottom1 * Math.Cos(angleBottom1)),
                (int)(centerYBottom1 + radiusBottom1 * Math.Sin(angleBottom1))
            );
            holeBottomSpeeds[0] += (int)speed;

            // Déplacement circulaire du deuxième trou du bas
            double angleBottom2 = (holeBottomSpeeds[1] % 360) * Math.PI / 180;
            holesBottom[1].Location = new Point(
                (int)(centerX + radiusBottom2 * Math.Cos(angleBottom2)),
                (int)(centerYBottom2 + radiusBottom2 * Math.Sin(angleBottom2))
            );
            holeBottomSpeeds[1] += (int)speed;
        }*/

                private void UpdateScore(string player, int points)
        {
            if (player == "Player 1")
            {
                player1Score += points;
            }
            else
            {
                player2Score += points;
            }

            scoreLabel.Text = $"Player 1: {GetDisplayScore(player1Score)} - Player 2: {GetDisplayScore(player2Score)}";

            if(player1Score >= 5 || player2Score >= 5)
            {
                GameOver(player1Score >= 5 ? "Player 1" : "Player 2");
                player1Score = 0;
                player2Score = 0;
                scoreLabel.Text = $"Player 1: {GetDisplayScore(player1Score)} - Player 2: {GetDisplayScore(player2Score)}";
            }
        }

        private string GetDisplayScore(int points)
        {
            return points switch
            {
                1 => "15",
                2 => "30",
                3 => "40",
                4 => "Avt",
                _ => "0"
            };
        }

        private void GameOver(string winner)
        {
            ballMovementTimer.Stop();
            ball.Location = new Point(ball.Location.X, 300);
            MessageBox.Show($"{winner} wins!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void InsertScoreToDatabase(int playerId, string points)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=123;Database=etu003247"))
            {
                connection.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO tennis_score (id_joueur, valeur, date_score) VALUES (@playerId, @points, @date)", connection))
                {
                    cmd.Parameters.AddWithValue("playerId", playerId);
                    cmd.Parameters.AddWithValue("points", points);
                    cmd.Parameters.AddWithValue("date", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    } 

    public class CustomBall : Panel
    {
        public CustomBall()
        {
            this.BackColor = Color.Transparent;
            this.Size = new Size(20, 20);
            this.Paint += CustomBall_Paint;
        }

        private void CustomBall_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using (Brush brush = new SolidBrush(Color.White))
            {
                g.FillEllipse(brush, 0, 0, this.Width - 1, this.Height - 1);
            }
        }
    }

}