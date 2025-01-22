using System;
using System.Drawing;
using System.Windows.Forms;

namespace src
{
    public class MainMenu : Form
    {
        private Panel panel_mainMenu = null!;
        private Button playButton = null!;
        private Button optionsButton = null!;
        private Button exitButton = null!;
        private Label titleLabel = null!;
        private PictureBox logoPictureBox = null!;

        public MainMenu()
        {
            InitializeComponent();
            this.Name = "MainMenu";
        }

        private void InitializeComponent()
        {
            // Configurer la fenêtre
            this.Text = "TenniZiol - Main Menu";
            this.Size = new Size(630, 800);
            this.BackColor = Color.Green;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Initialiser le panneau du menu principal
            panel_mainMenu = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Green
            };
            this.Controls.Add(panel_mainMenu);

            // Titre
            titleLabel = new Label()
            {
                Text = "TenniZiol",
                Font = new Font("Arial", 24, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 100
            };
            panel_mainMenu.Controls.Add(titleLabel);

            // Logo
            logoPictureBox = new PictureBox()
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                Dock = DockStyle.Top,
                Height = 500
            };

            try
            {
                logoPictureBox.Image = Image.FromFile("ressources/logo.jpg");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Image not found: " + ex.Message);
            }

            panel_mainMenu.Controls.Add(logoPictureBox);

            // Bouton Play
            playButton = new Button()
            {
                Text = "Play",
                Font = new Font("Arial", 14, FontStyle.Regular),
                Dock = DockStyle.Top,
                Height = 60
            };
            playButton.Click += PlayButton_Click;
            panel_mainMenu.Controls.Add(playButton);

            // Bouton Options
            optionsButton = new Button()
            {
                Text = "Options",
                Font = new Font("Arial", 14, FontStyle.Regular),
                Dock = DockStyle.Top,
                Height = 60
            };
            optionsButton.Click += OptionsButton_Click;
            panel_mainMenu.Controls.Add(optionsButton);

            // Bouton Exit
            exitButton = new Button()
            {
                Text = "Exit",
                Font = new Font("Arial", 14, FontStyle.Regular),
                Dock = DockStyle.Top,
                Height = 60
            };
            exitButton.Click += ExitButton_Click;
            panel_mainMenu.Controls.Add(exitButton);
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            TennisGame tennisGame = new TennisGame();
            tennisGame.FormClosed += (s, args) =>
            {
                this.Show(); 
            };
            this.Hide(); 
            tennisGame.Show();
        }


        private void OptionsButton_Click(object sender, EventArgs e)
        {
            // Code pour ouvrir les options
        }
    }
}

