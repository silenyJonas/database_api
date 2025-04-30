using Npgsql;
using database_api.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using NpgsqlTypes;

namespace database_api.DatabaseController
{
    public static class AppInterface
    {
        static NpgsqlConnection connection { get; set; }

        public static void OpenConnection(string connectionData)
        {
            connection = new NpgsqlConnection(connectionData);
            connection.Open();
        }
        public static void CloseConnection()
        {
            connection.Close();
        }
        public static void Insert(string tableName, List<string[]> columnData)
        {
            // Seznam sloupců pro vložení
            string columns = string.Join(", ", columnData.Select(data => data[0]));

            // Seznam hodnot pro vložení
            string values = string.Join(", ", Enumerable.Range(0, columnData.Count).Select(i => $"@value{i}"));

            // Vytvoření SQL příkazu pro vložení
            string query = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";

            using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
            {
                // Přidání parametrů
                for (int i = 0; i < columnData.Count; i++)
                {
                    // Získání datového typu pro parametr
                    NpgsqlDbType dbType = GetNpgsqlDbType(columnData[i][2]);

                    // Přidání parametru s hodnotou a datovým typem
                    NpgsqlParameter parameter = new NpgsqlParameter($"@value{i}", dbType);
                    parameter.Value = Convert.ChangeType(columnData[i][1], GetSystemTypeFromNpgsqlDbType(dbType));
                    cmd.Parameters.Add(parameter);
                }

                // Spuštění SQL příkazu
                cmd.ExecuteNonQuery();
            }
        }

        private static Type GetSystemTypeFromNpgsqlDbType(NpgsqlDbType dbType)
        {
            switch (dbType)
            {
                case NpgsqlDbType.Integer:
                    return typeof(int);
                case NpgsqlDbType.Text:
                    return typeof(string);
                case NpgsqlDbType.Varchar:
                    return typeof(string);
                // Další případy pro další běžné typy
                default:
                    throw new ArgumentException("Unknown NpgsqlDbType");
            }
        }
        private static NpgsqlDbType GetNpgsqlDbType(string typeName)
        {
            switch (typeName.ToLower())
            {
                case "int":
                case "integer":
                    return NpgsqlDbType.Integer;
                case "text":
                    return NpgsqlDbType.Text;
                case "varchar":
                    return NpgsqlDbType.Varchar;
                // Přidejte další případy pro další běžné typy sloupců, pokud je potřeba
                default:
                    return NpgsqlDbType.Text; // Defaultní hodnota
            }
        }
        public async static void Update(string tableName, string identifierColumnName, object identifierValue, string dataType, Dictionary<string, object> updatedValues)
        {
            var setStatements = string.Join(", ", updatedValues.Select(kv => $"{kv.Key} = @{kv.Key}"));
            var query = $"UPDATE {tableName} SET {setStatements} WHERE {identifierColumnName} = @IdentifierValue";

            await using (var cmd = new NpgsqlCommand(query, connection))
            {
                foreach (var kvp in updatedValues)
                {
                    // Zjistíme typ hodnoty
                    Type valueType = kvp.Value.GetType();

                    // Přetypujeme hodnotu na odpovídající datový typ
                    string dataTypeString = GetDataTypeString(valueType);

                    // Získáme odpovídající NpgsqlDbType
                    NpgsqlDbType dbType = GetNpgsqlDbType(dataTypeString);

                    // Přidáme parametr s odpovídajícím datovým typem
                    cmd.Parameters.AddWithValue("@" + kvp.Key, Convert.ChangeType(kvp.Value, GetSystemTypeFromNpgsqlDbType(dbType)));
                    cmd.Parameters["@" + kvp.Key].NpgsqlDbType = dbType;
                }

                // Přidání parametru pro hodnotu identifikátoru
                NpgsqlDbType identifierType = GetNpgsqlDbType(dataType);
                cmd.Parameters.AddWithValue("@IdentifierValue", Convert.ChangeType(identifierValue, GetSystemTypeFromNpgsqlDbType(identifierType)));
                cmd.Parameters["@IdentifierValue"].NpgsqlDbType = identifierType;

                await cmd.ExecuteNonQueryAsync();
            }
        }
        private static string GetDataTypeString(Type type)
        {
            if (type == typeof(int))
            {
                return "int";
            }
            else if (type == typeof(string))
            {
                return "text";
            }
            // Přidejte další podmínky pro další typy, pokud je potřeba
            else
            {
                return "text"; // Defaultní datový typ
            }
        }
        public async static void Delete(string tableName, string idColumnName, object id, string dataType)
        {
            // Konstrukce SQL dotazu s parametrem
            var query = $"DELETE FROM {tableName} WHERE {idColumnName} = @Id";

            // Vytvoření příkazu s parametrem
            await using var cmd = new NpgsqlCommand(query, connection);

            // Přidání parametru pro identifikátor
            NpgsqlDbType dbType = GetNpgsqlDbType(dataType);
            cmd.Parameters.AddWithValue("@Id", Convert.ChangeType(id, GetSystemTypeFromNpgsqlDbType(dbType)));
            cmd.Parameters["@Id"].NpgsqlDbType = dbType;


            // Spuštění SQL příkazu
            await cmd.ExecuteNonQueryAsync();
        }

        public static string[] Select(string tableName)
        {
            var columnsQuery = $"SELECT column_name FROM information_schema.columns WHERE table_name = '{tableName}' ORDER BY ordinal_position";
            var columns = new List<string>();

            using (var columnsCmd = new NpgsqlCommand(columnsQuery, connection))
            using (var readerColumns = columnsCmd.ExecuteReader())
            {
                while (readerColumns.Read())
                {
                    columns.Add((readerColumns.GetString(0) + ":").PadRight(10));
                }
            }

            var query = $"SELECT * FROM {tableName}";

            using var cmd = new NpgsqlCommand(query, connection);

            var results = new List<string>();

            results.Add(string.Join("", columns));

            using (var readerData = cmd.ExecuteReader())
            {
                while (readerData.Read())
                {
                    var result = "";
                    for (int i = 0; i < readerData.FieldCount; i++)
                    {
                        result += readerData[i].ToString().PadRight(10);
                    }
                    results.Add(result.Trim());
                }
            }

            return results.ToArray();
        }


        public async static void CreateTable(string tableName, Dictionary<string, object> columns)
        {
            try
            {
                var quer = $"DROP TABLE {tableName}";
                await using (var cmd = new NpgsqlCommand(quer, connection))
                {
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch { }
            

            var query = $"CREATE TABLE {tableName} (";

            foreach (var column in columns)
            {
                query += $"{column.Key} {column.Value}, ";
            }

            query = query.TrimEnd(' ', ',');

            query += ");";


            await using (var cmd = new NpgsqlCommand(query, connection))
            {
                await cmd.ExecuteNonQueryAsync();
            }
        }
        public async static void DropTable(string name)
        {
            var query = $"DROP TABLE IF EXISTS {name};";

            await using (var cmd = new NpgsqlCommand(query, connection))
            {
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async static void CreateDatabase(string name)
        {
            // Zkontrolujeme, zda databáze již existuje
            if (await DatabaseExists(name))
            {                
                return;
            }

            // Pokud databáze neexistuje, provedeme vytvoření
            var query = $"CREATE DATABASE {name};";

            await using (var cmd = new NpgsqlCommand(query, connection))
            {
                await cmd.ExecuteNonQueryAsync();
                
            }
        }

        // Metoda pro ověření existence databáze
        private async static Task<bool> DatabaseExists(string name)
        {
            // Dotaz na systémovou tabulku s informacemi o databázích
            var query = $"SELECT 1 FROM pg_database WHERE datname = '{name}';";

            await using (var cmd = new NpgsqlCommand(query, connection))
            {
                // Výsledek dotazu
                var result = await cmd.ExecuteScalarAsync();

                // Pokud výsledek není null, databáze existuje
                return result != null;
            }
        }

        public async static void DropDatabase(string name)
        {
            var query = $"DROP DATABASE IF EXISTS {name};";

            await using (var cmd = new NpgsqlCommand(query, connection))
            {
                await cmd.ExecuteNonQueryAsync();
            }
        }
        public async static void CreateUser(string name, string password)
        {
            // Zkontrolujte, zda uživatel existuje
            if (await UserExists(name))
            {
                return;
            }

            // Vytvořte uživatele
            var query = $"CREATE USER {name} WITH PASSWORD '{password}'";

            await using (var cmd = new NpgsqlCommand(query, connection))
            {
                await cmd.ExecuteNonQueryAsync();
            }

            // Udělte administrátorská práva
            await GrantAdminPrivileges(name);
        }

        public async static Task<bool> UserExists(string name)
        {
            var query = $"SELECT COUNT(*) FROM pg_catalog.pg_user WHERE usename = '{name}'";

            await using (var cmd = new NpgsqlCommand(query, connection))
            {
                var result = await cmd.ExecuteScalarAsync();
                return Convert.ToInt32(result) > 0;
            }
        }

        public async static Task DeleteUser(string name)
        {
            var query = $"DROP USER IF EXISTS {name}";

            await using (var cmd = new NpgsqlCommand(query, connection))
            {
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async static Task GrantAdminPrivileges(string name)
        {
            var query = $"ALTER USER {name} WITH SUPERUSER";

            await using (var cmd = new NpgsqlCommand(query, connection))
            {
                await cmd.ExecuteNonQueryAsync();
            }
        }
        public async static void DropUser(string name)
        {
            var query = $"DROP ROLE IF EXISTS {name};";

            await using (var cmd = new NpgsqlCommand(query, connection))
            {
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
