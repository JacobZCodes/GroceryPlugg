Overview
GroceryPlugg is an application designed to help consumers quickly compare grocery prices across different stores, helping them save time and money. The backend scrapes product data from various grocery store websites and stores it in a MySQL database. The frontend, built with Unity, allows users to input grocery items and view a detailed comparison of prices at different stores. The store with the lowest total price for the selected items is displayed, along with a breakdown of individual item costs.

Features
Web Scraping: Scrapes grocery data from multiple stores (Aldi, Ralphs, Target) using Selenium and BeautifulSoup.
MySQL Database: Stores product data in a MySQL database for efficient querying and comparison.
Unity Frontend: User-friendly interface built with Unity, allowing users to enter grocery items and see price comparisons.
Autocomplete Search: Users can type in items, and the system will suggest predefined grocery items to ensure consistency in searches.
Store Comparison: Displays the total cost of items at each store, with the store offering the best deal highlighted.
Receipt Display: Users see a detailed "receipt" showing individual item prices from the selected store.
Requirements
Backend:
Python 3.x
Selenium
BeautifulSoup
MySQL (or compatible database)
Frontend:
Unity (latest version)
C# for backend scripting
Database Setup:
MySQL Server or compatible database for storing product data
Required libraries: MySQL.Data for Unity (DLL)
Usage
Backend Setup
Scrape Data:
Run Aldi.py, ralphs.py, and target.py to scrape grocery data from the respective websites.
Database Operations:
dbOperations.py is used to interact directly with the MySQL database while scraping.
Create Tables:
Periodically run createTables.py to add tables for new grocery stores.
Define Keywords:
Edit grocery_list.py to define a list of grocery keywords used during scraping.
Frontend Setup
Install MySQL DLL:
Place the required MySQL Data DLL in your Unity project folder to enable MySQL connectivity.
Enter Items:
Launch the Unity app and enter your grocery items. The system will auto-complete based on predefined keywords.
Checkout:
Click the “Checkout” button to calculate the total price at each store. The store with the lowest total is displayed along with the item-wise breakdown.
Running the Application
Simply use the interface to enter the grocery items you wish to search for, hit “Checkout”, and view the store with the best prices for your selected items.

Screenshots
(Add appropriate screenshots of your application here, such as the landing page, cart page, and results page)
