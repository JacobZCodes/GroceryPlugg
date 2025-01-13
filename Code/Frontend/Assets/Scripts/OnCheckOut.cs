using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Data;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class OnCheckOut : MonoBehaviour
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
    public CanvasGroup BackgroundScreen;
    public CanvasGroup ResultScreen;
    public CanvasGroup CartScreen;
    public bool IsCheckedOut = false;
    public Text CheckOutList;
    public void DoGroceryStuff(List<string> checkedOutGroceries)
    {
        
        List<string> groceries = checkedOutGroceries;
        for (int i = 0; i < groceries.Count; i++)
        {
            
            if (groceries[i] == "")
            {
                Debug.Log("remvoing!");
                groceries.RemoveAt(i);
            }
        }
        

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
        float totalPriceForCheapestStore = (float)Math.Round(kvp.Value,2);
        Total.text = "$" + totalPriceForCheapestStore.ToString();
        theCheapestStoreName = kvp.Key;
        Store.text = theCheapestStoreName + ": ";
    }
    List<List<object>> cheapestGroceries = storeGroceries[theCheapestStoreName];
    foreach (List<object> groceryRow in cheapestGroceries)
    {
        for (int i = 0; i < groceryRow.Count; i++)
        {
            if (i == 0)
            {
                string currentGrocery = groceryRow[i].ToString();
                Groceries.text += "\n" + currentGrocery + "\n";
            }
            else
            {
                if ((float)groceryRow[i] == 0f)
                {
                    Prices.text += "\n" + "NOT FOUND" + "\n";
                }
                else
                {
                    string currentPrice = "$" + groceryRow[i].ToString();
                    Prices.text += "\n" + currentPrice + "\n";
                }
            }
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
            Debug.Log("Succesful connection!");
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

    // Start is called before the first frame update
    void Start()
    {
        CanvasGroupDisplayer.Hide(CartScreen);
        CanvasGroupDisplayer.Hide(ResultScreen);
        CanvasGroupDisplayer.Show(BackgroundScreen);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ShowResultScreen()
    {
        CanvasGroupDisplayer.Hide(BackgroundScreen);
        CanvasGroupDisplayer.Show(ResultScreen);
    }
    public void CheckOut()
    {
        IsCheckedOut = true;
        Debug.Log("CheckOut! " + CheckOutList.text);
        connectionString = "Server=localhost;Database=Groceries;User ID=root;Password=passpass;Pooling=false;";
        connection = Connect();
        List<string> checkedOutGroceriesAsList = new List<string>(CheckOutList.text.Split(new[] { '\n' }));
        Debug.Log(checkedOutGroceriesAsList.Count + " checkedOutGroceries");
        int totalGroceries = checkedOutGroceriesAsList.Count;
        for (int i = 0; i < totalGroceries-1; i++)
        {
            if (i == 0)
            {
                // First string is always GROCERIES : NAME, so clean it...
                string groceryNameWithoutColon =
                    checkedOutGroceriesAsList[i].Substring(checkedOutGroceriesAsList[i].IndexOf(":") + 1);
                checkedOutGroceriesAsList[i] = groceryNameWithoutColon;
                checkedOutGroceriesAsList[i] = checkedOutGroceriesAsList[i].Trim();
            }
            else
            {
                checkedOutGroceriesAsList[i] = checkedOutGroceriesAsList[i].Trim();
            }
        }
        
        DoGroceryStuff(checkedOutGroceriesAsList);
        ShowResultScreen();
    }
}







