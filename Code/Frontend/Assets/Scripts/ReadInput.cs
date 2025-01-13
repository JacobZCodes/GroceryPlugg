using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadInput : MonoBehaviour
{

    public Text runningList;

    public Text suggestionBox; 
    public InputField inputField;
    private string input;

    private List<string> predefinedWords = new List<string>
    {
        "cheddar cheese", "mozzarella cheese", "parmesan cheese", "feta cheese", "butter", "greek yogurt",
        "flavored yogurt",
        "chicken breast", "chicken thighs", "ground beef", "pork chops", "bacon", "salmon", "tilapia",
        "all-purpose flour",
        "brown sugar", "sea salt", "kosher salt", "yellow onions", "red onions",
        "russet potatoes", "sweet potatoes", "baby carrots", "whole carrots", "broccoli florets",
        "cauliflower", "baby spinach", "romaine lettuce", "iceberg lettuce", "cucumbers", "bell peppers",
        "portobello mushrooms",
        "white mushrooms", "apples", "bananas", "red grapes", "green grapes", "navel oranges",
        "blueberries", "blackberries", "pineapple chunks", "seedless watermelon", "cherries",
        "granola clusters", "steel-cut oats", "orange juice", "black tea", "green tea",
        "white vinegar", "ketchup", "mustard", "soy sauce", "sriracha sauce",
        "hot sauce", "peanut butter", "almond butter", "potato chips", "tortilla chips",
        "vanilla ice cream", "frozen pepperoni pizza", "frozen lasagna", "dark chocolate",
        "gummy candy", "trail mix", "sunflower seeds", "chicken sausage", "smoked ham", "deli turkey slices",
        "large shrimp", "canned tuna", "bread rolls", "plain bagels", "blueberry muffins",
        "whipped cream cheese", "vanilla pudding", "fruit-flavored gelatin", "dried spices",
        "active yeast", "canned green beans", "canned tomato soup", "canned black beans", "canned pineapple",
        "marinara sauce", "dill pickles", "green olives", "chewing gum", "microwave popcorn", "energy drinks",
        "sparkling water"
    };

    // Update suggestionbox whenever user changes input
    public void OnInputChanged(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            suggestionBox.text = "\n";
            return;
        }
        // For each word in predefined words, if the word starts with input.lower(), add it to predefined words
        List<string> suggestions = predefinedWords.FindAll(word => word.StartsWith(input.ToLower()));


        suggestionBox.text = "";
        suggestionBox.text += string.Join("\n", suggestions);
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void UpdateRunningList(string input)
    {
        runningList.text += " " + input + "\n";
    }
    
    
     public void ReadStringInput(string s)
     { 
         input = s;
         UpdateRunningList(input);
         inputField.text = "";
     }


}
