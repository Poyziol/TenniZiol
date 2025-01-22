using System;
using System.Windows.Forms;
using System.Drawing;

namespace src
{
    public class Input : Form
    {
        private TextBox input1TextBox;
        private TextBox input2TextBox;
        private Button submitButton;
        private int input1;  // Variable privée pour Input1
        private int input2;  // Variable privée pour Input2

        public Input()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Input Form";
            this.Size = new Size(300, 200);
            this.StartPosition = FormStartPosition.CenterParent;

            Label input1Label = new Label() { Text = "Vitesse1:", Location = new Point(20, 20), AutoSize = true };
            input1TextBox = new TextBox() { Location = new Point(100, 20), Width = 150 };
            Label input2Label = new Label() { Text = "Vit2 et Pts2:", Location = new Point(20, 60), AutoSize = true };
            input2TextBox = new TextBox() { Location = new Point(100, 60), Width = 150 };

            submitButton = new Button() { Text = "Submit", Location = new Point(100, 100) };
            submitButton.Click += SubmitButton_Click;

            this.Controls.Add(input1Label);
            this.Controls.Add(input1TextBox);
            this.Controls.Add(input2Label);
            this.Controls.Add(input2TextBox);
            this.Controls.Add(submitButton);
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            // Si les valeurs saisies sont valides, on les assigne directement aux variables
            if (int.TryParse(input1TextBox.Text, out int input1Value) && int.TryParse(input2TextBox.Text, out int input2Value))
            {
                input1 = input1Value;
                input2 = input2Value;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please enter valid integers.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Méthodes publiques pour récupérer les valeurs des variables
        public int GetInput1()
        {
            return input1;
        }

        public int GetInput2()
        {
            return input2;
        }
    }
}
