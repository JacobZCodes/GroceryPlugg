using System;
using System.Data;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MySQLConnection : MonoBehaviour
{
    public Text Store;

    public Text Groceries;
    public Text Prices;
    public Text Total;

    public Button CheckOutButton;
    private string connectionString;

    private MySqlConnection connection;

    private List<string> tables = new List<string> { "Ralphs", "Target", "Aldi" };

    private Dictionary<string, List<List<object>>> storeGroceries = new Dictionary<string, List<List<object>>>();

    void Start()
    {
        // Define your connection string
        connectionString = "Server=localhost;Database=Groceries;User ID=root;Password=passpass;Pooling=false;";
        connection = Connect();
    }
    void Update() {
        
    }

    public void DoGroceryStuff()
    {
        List<string> groceries = new List<string> { "sunflower seeds" , "pork chops", "chicken thighs"};
        List<Dictionary<string, float>> allStores = new List<Dictionary<string, float>>();
        foreach (string table in tables)
        {
            Dictionary<string, float> thisStore;
            thisStore = getTotalCostForStoreAsDictionary(groceries, table);
            allStores.Add(thisStore);
        }

        Dictionary<string, float> theCheapestStore;
        string theCheapestStoreName = "";
        theCheapestStore = findCheapestStore(allStores, groceries);
        foreach (KeyValuePair<string, float> kvp in theCheapestStore)
        {
            theCheapestStoreName = kvp.Key;
        }
        List<List<object>> cheapestGroceries = storeGroceries[theCheapestStoreName];
        foreach (List<object> groceryRow in cheapestGroceries)
        {
            foreach (object value in groceryRow)
            {
                Debug.Log(value);
            }
            
        }
    }
    private MySqlConnection Connect()
    {
        try
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            {
                conn.Open();
                Debug.Log("success!");
                return conn;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to connect to MySQL database: {ex.Message}");
            return null;
        }
    }

    private Dictionary<string, float> findStore(List<Dictionary<string, float>> stores, string store)
    {
        foreach (Dictionary<string, float> thisStore in stores)
        {
            if (thisStore.ContainsKey(store))
            {
                return thisStore;
            }
        }
        return null;
    }
    
    private Dictionary<string, float> findCheapestStore(List<Dictionary<string, float>> stores, List<string> groceries)
    {
        string cheapestStoreName = "";
        float cheapestTotal = 100000f;
        foreach (string table in tables)
        {
            Dictionary<string, float> store = getTotalCostForStoreAsDictionary(groceries, table);
            float costForThisStore = store[table];
            if (costForThisStore < cheapestTotal)
            {
                cheapestTotal = costForThisStore;
                cheapestStoreName = table;
            }
        }
        Debug.Log("Cheapest store is " + cheapestStoreName);
        Dictionary<string, float> cheapestStore = findStore(stores, cheapestStoreName);
        return cheapestStore;
    }
    
    private Dictionary<string, float> getTotalCostForStoreAsDictionary(List<string> groceries, string table)
    {
        Dictionary<string, float> store = new Dictionary<string, float>();
        float totalCost = 0f;
        List<List<object>> groceriesForThisStore = new List<List<object>>();
        List<object> thisGroceryRow;
        foreach (string grocery in groceries)
        {
            thisGroceryRow = getCheapestGroceryFromTable(grocery, table);
            groceriesForThisStore.Add(thisGroceryRow);
            float thisGroceryPrice = (float) thisGroceryRow[1];
            totalCost += thisGroceryPrice;
        }
        storeGroceries[table] = groceriesForThisStore;
        store.Add(table, totalCost);
        return store;
    }
    
    private List<object> getCheapestGroceryFromTable(string grocery, string tableName)
    {
        string query = $"SELECT instanceName, price FROM {tableName} WHERE grocery = '{grocery}' ORDER BY price LIMIT 1";
        MySqlCommand cmd = new MySqlCommand(query, connection);
        MySqlDataReader reader = cmd.ExecuteReader();
        List<object> cheapestGroceryRow = getSingleRowFromReader(reader, grocery, tableName);
        return cheapestGroceryRow;
    }
    
    private List<object> getSingleRowFromReader(MySqlDataReader reader, string grocery, string table)
    {
        List<object> row = new List<object>();
        if (reader.Read()) {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                object columnValue = reader.GetValue(i);
                row.Add(columnValue);
            }
        }
        else
        {
            // No data found for this store
            Debug.Log(table + " No data found for " + grocery);
            row.Add(grocery);
            row.Add(0f);
        }
        reader.Close();
        //printRow(row);
        return row;
    }
}