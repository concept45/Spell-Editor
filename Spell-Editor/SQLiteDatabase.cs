using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace Spell_Editor
{
    class SQLiteDatabase : Database<SQLiteConnection, SQLiteConnectionStringBuilder, SQLiteParameter, SQLiteCommand, SQLiteTransaction>
    {
        public SQLiteDatabase(string file)
        {
            connectionString = new SQLiteConnectionStringBuilder();
            connectionString.DataSource = file;
            connectionString.Version = 3;
        }

        //public async Task<List<Spell>> GetSpells()
        //{
        //    DataTable dt = await ExecuteQuery("SELECT * FROM spells");

        //    if (dt.Rows.Count == 0)
        //        return null;

        //    List<Spell> spells = new List<Spell>();

        //    foreach (DataRow row in dt.Rows)
        //        spells.Add(BuidSpell(row));

        //    return spells;
        //}

        public async Task<string> GetSpellNameById(int id)
        {
            DataTable dt = await ExecuteQuery("SELECT spellName FROM spells WHERE id = '" + id + "'");

            if (dt.Rows.Count == 0)
                return "<Spell not found!>";

            return (string)dt.Rows[0]["spellName"]; //! Always take first index; should not be possible to have multiple instances per id, but still
        }
    }
}
