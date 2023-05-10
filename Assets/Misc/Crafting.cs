using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    GameManager gameManager;
    Slot[] ingredientSlots = new Slot[3];
    Slot resultsSlot;
    Dictionary<string[], string> craftingDict = new Dictionary<string[], string>{
        //tomato, mushroom, garlic, carrot, butter, onion, potato, egg
        {new string[]{"tomato", "tomato", "blank"}, "tomatoSoup"},
        {new string[]{"tomato", "egg", "blank"}, ""},
        {new string[]{"tomato", "potato", "blank"}, ""},
        {new string[]{"tomato", "onion", "blank"}, "stirFryVeggies"},
        {new string[]{"tomato", "butter", "blank"}, ""},
        {new string[]{"tomato", "carrot", "blank"}, "stirFryVeggies"},
        {new string[]{"tomato", "garlic", "blank"}, "stirFryVeggies"},
        {new string[]{"mushroom", "mushroom", "blank"}, "creamMushroomSoup"},
        {new string[]{"mushroom", "garlic", "blank"}, "stirFryVeggies"},
        {new string[]{"mushroom", "carrot", "blank"}, "stirFryVeggies"},
        {new string[]{"mushroom", "butter", "blank"}, ""},
        {new string[]{"mushroom", "onion", "blank"}, "stirFryVeggies"},
        {new string[]{"mushroom", "potato", "blank"}, ""},
        {new string[]{"mushroom", "egg", "blank"}, ""},
        {new string[]{"garlic", "garlic", "blank"}, ""},
        {new string[]{"garlic", "carrot", "blank"}, "stirFryVeggies"},
        {new string[]{"garlic", "butter", "blank"}, ""},
        {new string[]{"garlic", "onion", "blank"}, ""},
        {new string[]{"garlic", "potato", "blank"}, ""},
        {new string[]{"garlic", "egg", "blank"}, ""},
        {new string[]{"carrot", "carrot", "blank"}, "carrotSoup"},
        {new string[]{"carrot", "butter", "blank"}, ""},
        {new string[]{"carrot", "onion", "blank"}, "stirFryVeggies"},
        {new string[]{"carrot", "potato", "blank"}, ""},
        {new string[]{"carrot", "egg", "blank"}, ""},
        {new string[]{"butter", "butter", "blank"}, ""},
        {new string[]{"butter", "onion", "blank"}, ""},
        {new string[]{"butter", "potato", "blank"}, ""},
        {new string[]{"onion", "onion", "blank"}, ""},
        {new string[]{"onion", "potato", "blank"}, ""},
        {new string[]{"onion", "egg", "blank"}, ""},
        {new string[]{"potato", "potato", "blank"}, ""},
        {new string[]{"egg", "egg", "blank"}, "twoEggs"},
    };

    /*
    20 total effects
    8c2 = 28
    just don't implement one of the ingredients?

    10 flat stat buffs
    +1 HP
    +1 MS
    +1 AD
    +1 AS
    +2 MS -1 AD
    +2 MS -1 AS
    +2 AD -1 MS 
    +2 AD -1 AS
    +2 AS -1 MS
    +2 AS -1 AD

    10 temp effects
    +1 heal
    +2 MS
    +2 AD
    +2 AS
    +3 MS -1 AD
    +3 MS -1 AS
    +3 AD -1 MS 
    +3 AD -1 AS
    +3 AS -1 MS
    +3 AS -1 AD
    */

    void Awake() {
        ingredientSlots[0] = transform.Find("Slot").gameObject.GetComponent<Slot>();
        ingredientSlots[1] = transform.Find("Slot1").gameObject.GetComponent<Slot>();
        ingredientSlots[2] = transform.Find("Slot2").gameObject.GetComponent<Slot>();
        resultsSlot = transform.Find("CraftingResults").gameObject.GetComponent<Slot>();
        gameManager = GameManager.inst;
        foreach (Slot slot in ingredientSlots) {
            gameManager.player.inventory.slots[0].Add(slot);
        }
        gameManager.player.inventory.slots[0].Add(resultsSlot);
    }

    public void Cook() {
        print("just cooked up a dish!");
        List<string> ingred = new List<string>();
        foreach (Slot slot in ingredientSlots) {
            ingred.Add(slot.item.itemName);
        }
        string recipe = GetRecipe(ingred);
        if (recipe != null) {
            foreach(Slot ingredientSlot in ingredientSlots) {
                ingredientSlot.ChangeItem((Item)null);
            }
            resultsSlot.ChangeItem(recipe);
            resultsSlot.draggable = true;
            //make sure slots are compatible with slots that are not in the same inventory
        } else {
            print("invalid recipe!");
        }
    }

    public string GetRecipe(List<string> providedIngredients) {
        foreach(string[] recipe in craftingDict.Keys) {
            List<string> providedIngredientsList = new List<string>(providedIngredients);
            List<string> recipeList = new List<string>(recipe);
            foreach(string ingredient in recipe) {
                if (providedIngredientsList.Contains(ingredient)){
                    providedIngredientsList.Remove(ingredient);
                } else {
                    break;
                }
                return craftingDict[recipe];
            }
        }
        return null;
    }


}
