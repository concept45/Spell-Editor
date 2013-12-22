using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Spell_Editor
{
    class WorldDatabase : Database<MySqlConnection, MySqlConnectionStringBuilder, MySqlParameter, MySqlCommand, MySqlTransaction>
    {
        public WorldDatabase(string host, uint port, string username, string password, string databaseName)
        {
            connectionString = new MySqlConnectionStringBuilder();
            connectionString.Server = host;
            connectionString.Port = (uint)port;
            connectionString.UserID = username;
            connectionString.Password = password;
            connectionString.Database = databaseName;
            connectionString.AllowUserVariables = true;
            connectionString.AllowZeroDateTime = true;
        }

        public async Task<int> GetCreatureIdByGuid(int guid)
        {
            DataTable dt = await ExecuteQuery("SELECT id FROM creature WHERE guid = '" + guid + "'");

            if (dt.Rows.Count == 0)
                return 0;

            return Convert.ToInt32(dt.Rows[0]["id"]);
        }

        public async Task<int> GetGameobjectIdByGuid(int guid)
        {
            DataTable dt = await ExecuteQuery("SELECT id FROM gameobject WHERE guid = '" + guid + "'");

            if (dt.Rows.Count == 0)
                return 0;

            return Convert.ToInt32(dt.Rows[0]["id"]);
        }

        public async Task<string> GetCreatureNameById(int id)
        {
            DataTable dt = await ExecuteQuery("SELECT name FROM creature_template WHERE entry = '" + id + "'");

            if (dt.Rows.Count == 0)
                return "_replaceBecauseOfError_ GetCreatureNameById not found";

            string name = dt.Rows[0]["name"].ToString();
            return name.Replace('"', '\'');
        }

        public async Task<string> GetCreatureNameByGuid(int guid)
        {
            DataTable dt = await ExecuteQuery("SELECT name FROM creature_template WHERE entry = '" + await GetCreatureIdByGuid(guid) + "'");

            if (dt.Rows.Count == 0)
                return "_replaceBecauseOfError_ GetCreatureNameByGuid not found";

            string name = dt.Rows[0]["name"].ToString();
            return name.Replace('"', '\'');
        }

        public async Task<string> GetGameobjectNameById(int id)
        {
            DataTable dt = await ExecuteQuery("SELECT name FROM gameobject_template WHERE entry = '" + id + "'");

            if (dt.Rows.Count == 0)
                return "_replaceBecauseOfError_ GetGameobjectNameById not found";

            string name = dt.Rows[0]["name"].ToString();
            return name.Replace('"', '\'');
        }

        public async Task<string> GetGameobjectNameByGuid(int guid)
        {
            DataTable dt = await ExecuteQuery("SELECT name FROM gameobject_template WHERE entry = '" + await GetGameobjectIdByGuid(guid) + "'");

            if (dt.Rows.Count == 0)
                return "_replaceBecauseOfError_ GetGameobjectNameByGuid not found";

            string name = dt.Rows[0]["name"].ToString();
            return name.Replace('"', '\'');
        }

        public async Task<string> GetQuestNameById(int id)
        {
            DataTable dt = await ExecuteQuery("SELECT title FROM quest_template WHERE id = " + id);

            if (dt.Rows.Count == 0)
                return "_replaceBecauseOfError_<Quest '" + id + "' not found!>";

            return dt.Rows[0]["title"].ToString();
        }

        public async Task<string> GetQuestNameForCastedByCreatureOrGo(int requiredNpcOrGo1, int requiredNpcOrGo2, int requiredNpcOrGo3, int requiredNpcOrGo4, int requiredSpellCast1)
        {
            DataTable dt = await ExecuteQuery(String.Format("SELECT title FROM quest_template WHERE (RequiredNpcOrGo1 = {0} OR RequiredNpcOrGo2 = {1} OR RequiredNpcOrGo3 = {2} OR RequiredNpcOrGo4 = {3}) AND RequiredSpellCast1 = {4}", requiredNpcOrGo1, requiredNpcOrGo2, requiredNpcOrGo3, requiredNpcOrGo4, requiredSpellCast1));

            if (dt.Rows.Count == 0)
                return "_replaceBecauseOfError_<Quest not found (GetQuestNameForCastedByCreatureOrGo)!>";

            return dt.Rows[0]["title"].ToString();
        }

        public async Task<string> GetQuestNameForKilledMonster(int requiredNpcOrGo1, int requiredNpcOrGo2, int requiredNpcOrGo3, int requiredNpcOrGo4)
        {
            DataTable dt = await ExecuteQuery(String.Format("SELECT title FROM quest_template WHERE (RequiredNpcOrGo1 = {0} OR RequiredNpcOrGo2 = {1} OR RequiredNpcOrGo3 = {2} OR RequiredNpcOrGo4 = {3})", requiredNpcOrGo1, requiredNpcOrGo2, requiredNpcOrGo3, requiredNpcOrGo4));

            if (dt.Rows.Count == 0)
                return "_replaceBecauseOfError_<Quest not found (GetQuestNameForKilledMonster)!>";

            return dt.Rows[0]["title"].ToString();
        }

        public async Task<string> GetItemNameById(int id)
        {
            DataTable dt = await ExecuteQuery("SELECT name FROM item_template WHERE entry = " + id);

            if (dt.Rows.Count == 0)
                return "_replaceBecauseOfError_<Item '" + id + "' not found!>";

            return dt.Rows[0]["name"].ToString();
        }
    }
}
