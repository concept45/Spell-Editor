using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spell_Editor
{
    public partial class MainForm : Form
    {
        WorldDatabase worldDatabase = null;
        SQLiteDatabase sqliteDatabase = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            worldDatabase = new WorldDatabase("localhost", 3306, "root", "123", "clean_world_malcrom");
            sqliteDatabase = new SQLiteDatabase("sqlite_database.db");
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            int spellId;

            if (String.IsNullOrWhiteSpace(textBox1.Text) || !Int32.TryParse(textBox1.Text, out spellId))
                return;

            string spellname = await sqliteDatabase.GetSpellNameById(spellId);
            MessageBox.Show(spellname);
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            int spellId;

            if (String.IsNullOrWhiteSpace(textBox1.Text) || !Int32.TryParse(textBox1.Text, out spellId))
                return;

            string spellname = await worldDatabase.GetSpellCommentById(spellId);
            MessageBox.Show(spellname);
        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void textBox73_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void exiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
