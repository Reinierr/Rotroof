using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;

namespace RotRoof
{
  class DBConnection
  {
    private MySqlConnection connection;
    private string server;
    private string database;
    private string uid;
    private string password;

    public DBConnection()
    {
      Initialize();
    }
    private void Initialize()
    {
      server = "localhost";
      database = "md325812db355195";
      uid = "root";
      password = "admin";
      string connectionString;
      connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

      connection = new MySqlConnection(connectionString);

    }
    private bool OpenConnection()
    {
      try
      {
        connection.Open();
        return true;
      }
      catch (MySqlException ex)
      {
        switch (ex.Number)
        {
          case 0:
            MessageBox.Show("Cannot connect to server");
            break;
          case 1045:
            MessageBox.Show("Invalid User/pass");
            break;
        }
        return false;
      }
    }
    private bool CloseConnection()
    {
      try
      {
        connection.Close();
        return true;
      }
      catch (MySqlException ex)
      {
        MessageBox.Show(ex.Message);
        return false;
      }

    }
    public List<Dictionary<string, string>> Select(string query, List<string> properties)
    {
      //Create a list to store the results
      List<Dictionary<string, string>> results = new List<System.Collections.Generic.Dictionary<string, string>>();
      Dictionary<string, string> result = null;

      //Open connection
      if (this.OpenConnection() == true)
      {
        //Create Command
        MySqlCommand cmd = new MySqlCommand(query, connection);
        //Create a data reader and Execute the command
        MySqlDataReader dataReader = cmd.ExecuteReader();
        int cnt = dataReader.FieldCount;
        //Read the data and store them in the list
        while (dataReader.Read())
        {
          int i = 0;
          foreach (string p in properties) {
            if (i % properties.Count == 0)
            {
              if (result != null) {
                results.Add(result);
              }
              result = new Dictionary<string, string>();
            }
            Debug.WriteLine(p);
            result.Add(p ,Convert.ToString(dataReader[p]));
          }
        }

        //close Data Reader
        dataReader.Close();

        //close Connection
        this.CloseConnection();

        //return list to be displayed
        return results;
      }
      else
      {
        return results;
      }
    }
  }
}
