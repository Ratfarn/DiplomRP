using KIP_Translator.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KIP_Translator
{
    internal class CoreProject
    {
        private static string connectionString = "Data Source=DBLang.db";
        private static SQLiteConnection sqLiteConnection;
        //получение только одной строки
        public static T RunQuery<T>(string query) where T : new()
        {
            sqLiteConnection = new SQLiteConnection(connectionString);
            sqLiteConnection.Open();
            SQLiteCommand command = new SQLiteCommand(query, sqLiteConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            DataTable result = new DataTable();
            result.Load(reader);
            sqLiteConnection.Close();
            T instance = new T();
            if (result.Rows.Count != 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    foreach (DataColumn column in result.Columns)
                    {
                        object value = row[column];
                        if (!(value is DBNull))
                        {
                            PropertyInfo propertyInfo = instance.GetType().GetProperty(column.ColumnName);
                            value = Convert.ChangeType(value, propertyInfo.PropertyType);
                            propertyInfo.SetValue(instance, value);
                        }
                    }
                }
            }
            return instance;
        }
        // получение всей таблицы
        public static List<T> RunQueryList<T>(string query) where T : new()
        {
            sqLiteConnection = new SQLiteConnection(connectionString);
            sqLiteConnection.Open();
            SQLiteCommand command = new SQLiteCommand(query, sqLiteConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            DataTable result = new DataTable();
            result.Load(reader);
            sqLiteConnection.Close();
            List<T> instances = new List<T>();
            T instance;
            if (result.Rows.Count != 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    instance = new T();
                    foreach (DataColumn column in result.Columns)
                    {
                        object value = row[column];
                        if (!(value is DBNull))
                        {
                            PropertyInfo propertyInfo = instance.GetType().GetProperty(column.ColumnName);
                            value = Convert.ChangeType(value, propertyInfo.PropertyType);
                            propertyInfo.SetValue(instance, value);
                        }
                    }
                    instances.Add(instance);
                }
            }
            return instances;
        }
        // для добавления, изменения или удаление
        public static bool RunNonQuery(string query)
        {
            sqLiteConnection = new SQLiteConnection(connectionString);
            sqLiteConnection.Open();
            SQLiteCommand command = new SQLiteCommand(query, sqLiteConnection);
            int result = command.ExecuteNonQuery();
            sqLiteConnection.Close();
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}