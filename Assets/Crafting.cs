using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    Slot[] ingredientSlots = new Slot[3];
    Slot resultsSlot;
    Dictionary<string[], string> craftingDict = new Dictionary<string[], string>{
        {new string[]{"tomato", "tomato", "blank"}, "tomatoClub"}
    };

    void Awake() {
        ingredientSlots[0] = transform.Find("Slot").gameObject.GetComponent<Slot>();
        ingredientSlots[1] = transform.Find("Slot2").gameObject.GetComponent<Slot>();
        ingredientSlots[2] = transform.Find("Slot3").gameObject.GetComponent<Slot>();
        resultsSlot = transform.Find("CraftingResults").gameObject.GetComponent<Slot>();
    }

    void Cook() {
        List<string> ingred = new List<string>();
        foreach (Slot slot in ingredientSlots) {
            ingred.Add(slot.item);
        }
        if (GetRecipe(ingred) != null) {
            //delete ingredient slot contents and add recipe result to result slot
            //make sure slots are compatible with slots that are not in the same inventory
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
