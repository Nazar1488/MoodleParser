using System;
using System.Linq;
using System.Windows.Forms;

namespace MoodleParser
{
    public partial class Form1 : Form
    {
        private string htmlFilePath;

        public Form1()
        {
            InitializeComponent();
        }

        private void parseButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(htmlFilePath))
            {
                Parser.Parse(htmlFilePath);
                qCount.Text = Parser.QuestionsList.Count.ToString();
                qCorrect.Text = Parser.QuestionsList.Count(q => q.IsCorrect).ToString();
            }
        }

        private void saveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Parser.Save(openFileDialog1.FileName);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                htmlFilePath = openFileDialog1.FileName;
            }
        }
    }
}
