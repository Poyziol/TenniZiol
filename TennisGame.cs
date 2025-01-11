using System;
using System.Drawing;
using System.Windows.Forms;

public class TennisGame : Form
{
    private Panel court = null!;
    private CustomBall ball = null!;
    private Panel[] holesTop = null!;
    private Panel[] holesBottom = null!;
    private Random random = null!;
    private Label scoreLabel = null!;
    private int player1Score = 0;
    private int player2Score = 0;
    private int[] holeTopSpeeds = null!;
    private int[] holeBottomSpeeds = null!;
    private System.Windows.Forms.Timer ballMovementTimer = new System.Windows.Forms.Timer() { Interval = 10 }; // Timer for ball movement
    private Point ballDestination; // Ball destination
    private float ballSpeedX, ballSpeedY; // Ball speed components

    public TennisGame()
    {
        InitializeComponent();
        ballMovementTimer.Tick += BallMovementTimer_Tick;
    }

    private void InitializeComponent()
    {
        this.Text = "TenniZiol";
        this.Size = new Size(630, 800);
        this.BackColor = Color.Green;
        this.StartPosition = FormStartPosition.CenterScreen;
    
        court = new Panel()
        {
            Dock = DockStyle.Fill,
            BackColor = Color.Green
        };
        this.Controls.Add(court);
    
        scoreLabel = new Label()
        {
            Text = "Player 1: 0 - Player 2: 0",
            Dock = DockStyle.Top,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Arial", 14, FontStyle.Bold),
            Height = 40
        };
        this.Controls.Add(scoreLabel);
    
        random = new Random();
        ball = new CustomBall()
        {
            Size = new Size(20, 20),
            Location = new Point(random.Next(100, 480), 490)
        };
        court.Controls.Add(ball);
    
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
    
        for(int ni = 0; ni < holesTop.Length; ni++)
        {
            holeTopSpeeds[ni] = random.Next(2, 6);
        }
    
        for(int ni = 0; ni < holesBottom.Length; ni++)
        {
            holeBottomSpeeds[ni] = random.Next(2, 6);
        }
    
        Button shootButton = new Button()
        {
            Text = "Shoot Ball",
            Dock = DockStyle.Bottom,
            Height = 40
        };
        shootButton.Click += ShootButton_Click;
        this.Controls.Add(shootButton);
    
        Button historyButton = new Button()
        {
            Text = "View History",
            Dock = DockStyle.Top,
            Height = 40
        };
        historyButton.Click += ShootButtonPlayer2_Click;
        this.Controls.Add(historyButton);
    
        System.Windows.Forms.Timer holeMovementTimer = new System.Windows.Forms.Timer() { Interval = 100 };
        holeMovementTimer.Tick += HoleMovementTimer_Tick;
        holeMovementTimer.Start();
    
        court.Paint += Court_Paint;
    }

    private void Court_Paint(object? sender, PaintEventArgs e)
    {
        Pen whitePen = new Pen(Color.White, 2);
        e.Graphics.DrawRectangle(whitePen, 50, 30, 500, 550);
        e.Graphics.DrawLine(whitePen, 50, 300, 550, 300);
        e.Graphics.DrawLine(whitePen, 300, 30, 300, 580);
        e.Graphics.DrawLine(whitePen, 100, 30, 100, 580);
        e.Graphics.DrawLine(whitePen, 500, 30, 500, 580);
        e.Graphics.DrawLine(whitePen, 50, 90, 550, 90);
        e.Graphics.DrawLine(whitePen, 50, 520, 550, 520);
    }

    private Panel[] CreateHoles(Point startPosition, int spacing,int holeNumber,int xSize,int indice)
    {
        Panel[] holes = new Panel[holeNumber];
        for (int ni = 0; ni < holes.Length; ni++)
        {
            holes[ni] = new Panel()
            {
                Size = new Size(xSize, 20),
                BackColor = Color.Black,
                Location = new Point(startPosition.X + indice * spacing, startPosition.Y + indice * 40)
            };
            court.Controls.Add(holes[ni]);
        }
        return holes;
    }

    private void ShootButton_Click(object? sender, EventArgs e)
    {
        if(ball.Location.Y > 300)
        StartBallMovement(new Point(random.Next(100, 480), 100)); // Move towards Player 2's side

        if(ball.Location.Y < 300)
        StartBallMovement(new Point(random.Next(100, 480), 490));
    }

    private void ShootButtonPlayer2_Click(object? sender, EventArgs e)
    {
        StartBallMovement(new Point(random.Next(100, 480), 490)); // Move towards Player 1's side
    }

    private void StartBallMovement(Point destination)
    {
        ballDestination = destination;
        int deltaX = ballDestination.X - ball.Location.X;
        int deltaY = ballDestination.Y - ball.Location.Y;
        float distance = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

        // Normalizing direction and setting speed
        ballSpeedX = (deltaX / distance) * 5; // Speed is adjustable
        ballSpeedY = (deltaY / distance) * 5;

        ballMovementTimer.Start();
    }

    private void BallMovementTimer_Tick(object? sender, EventArgs e)
    {
        ball.Location = new Point((int)(ball.Location.X + ballSpeedX), (int)(ball.Location.Y + ballSpeedY));

        // Check if the ball is outside the court bounds
        if (ball.Location.X < 50 || ball.Location.X > 550 - ball.Width ||
            ball.Location.Y < 30 || ball.Location.Y > 580 - ball.Height)
        {
            ballMovementTimer.Stop();
            if((ball.Location.X < 50 && ball.Location.Y < 300) || (ball.Location.X > 580 - ball.Width && ball.Location.Y < 300))
            {
                ball.Location = new Point(ball.Location.X, 100); 
            }
            if((ball.Location.X < 50 && ball.Location.Y > 300) || (ball.Location.X > 580 - ball.Width && ball.Location.Y > 300))
            {
                ball.Location = new Point(ball.Location.X, 490); 
            }
            if(ball.Location.Y < 30)
            {
                ball.Location = new Point(ball.Location.X, 100); 
            }
            if(ball.Location.Y > 580 - ball.Height)
            {
                ball.Location = new Point(ball.Location.X, 490);
            }
        }
        // Check if the ball reaches the holes to score points
        if(ball.Bounds.IntersectsWith(holesTop[0].Bounds) || ball.Bounds.IntersectsWith(holesTop[1].Bounds))
        {
            UpdateScore("Player 1", 15);
            ballMovementTimer.Stop();
            ball.Location = new Point(ball.Location.X, 100); 
        }
        else if(ball.Bounds.IntersectsWith(holesBottom[0].Bounds) || ball.Bounds.IntersectsWith(holesBottom[1].Bounds))
        {
            UpdateScore("Player 2", 15);
            ballMovementTimer.Stop();
            ball.Location = new Point(ball.Location.X, 490);
        }
    }

    private void UpdateScore(string player, int points)
    {
        if (player == "Player 1")
            player1Score += points;
        else
            player2Score += points;

        scoreLabel.Text = $"Player 1: {player1Score} - Player 2: {player2Score}";
    }

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
}

public class CustomBall : Panel
{
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        e.Graphics.FillEllipse(Brushes.Yellow, 0, 0, this.Width, this.Height);
    }
}
