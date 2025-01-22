using System;
using System.Drawing;
using System.Windows.Forms;

namespace src
{
    public class TennisGame : Form
    {
        private Panel panel_tennis = null!;
        private Functions functions = null!;

        public TennisGame()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "TenniZiol";
            this.Size = new Size(630, 850);
            this.BackColor = Color.Green;
            this.StartPosition = FormStartPosition.CenterScreen;

            panel_tennis = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Green,
            };
            this.Controls.Add(panel_tennis);

            functions = new Functions(panel_tennis);
            this.KeyPreview = true;  // Permet de capter les événements de clavier au niveau du Form
            this.KeyDown += Panel_tennis_KeyDown;

            Button shootButton = new Button()
            {
                Text = "Shoot Ball",
                Dock = DockStyle.Bottom,
                Height = 40
            };
            shootButton.Click += functions.ShootButton_Click;
            this.Controls.Add(shootButton);

            Button historyButton = new Button()
            {
                Text = "View History",
                Dock = DockStyle.Top,
                Height = 40
            };
            historyButton.Click += functions.ViewHistory_Click;
            this.Controls.Add(historyButton);
            
            Button mainMenuButton = new Button()
            {
                Text = "Main Menu",
                Dock = DockStyle.Top,
                Height = 40
            };
            mainMenuButton.Click += MainMenuButton_Click;
            this.Controls.Add(mainMenuButton);

            Button inputButton = new Button()
            {
                Text = "Enter Inputs",
                Dock = DockStyle.Top,
                Height = 40
            };
            inputButton.Click += InputButton_Click;
            this.Controls.Add(inputButton);
        }

        private void MainMenuButton_Click(object? sender, EventArgs e)
        {
            MainMenu mainMenu = new MainMenu();
            mainMenu.FormClosed += (s, args) => this.Show();
            mainMenu.Show();
            this.Hide(); 
        }

        private void Panel_tennis_KeyDown(object? sender, KeyEventArgs e)
        {
            functions.HandleKeyPress(e);
        }

        private void InputButton_Click(object? sender, EventArgs e)
        {
            Input inputForm = new Input();
            if (inputForm.ShowDialog() == DialogResult.OK)
            {
                functions.SetInputs(inputForm.GetInput1(), inputForm.GetInput2());
            }
        }
    }
}
