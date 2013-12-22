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
            sqliteDatabase = new SQLiteDatabase("Resources/sqlite_database.db");
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string spellname = await sqliteDatabase.GetSpellNameById(Convert.ToInt32(textBox1.Text));
            MessageBox.Show(spellname);
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            string spellname = await worldDatabase.GetSpellCommentById(Convert.ToInt32(textBox1.Text));
            MessageBox.Show(spellname);
        }
    }
}
