using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    GameManager gameManager;
    Slot[] ingredientSlots = new Slot[3];
    Slot resultsSlot;
    Dictionary<string[], string> craftingDict = new Dictionary<string[], string>{
        {new string[]{"tomato", "tomato", "blank"}, "tomatoSoup"}
    };

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
        print("cookery");
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
