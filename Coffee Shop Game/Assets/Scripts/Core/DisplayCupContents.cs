using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Funksoft.Barista
{
    public class DisplayCupContents : MonoBehaviour
    {
        [SerializeField]
        private Vector3 _spawnPos;

        [SerializeField]
        private float _displayCupHeight;

        [SerializeField]
        private GameObject cupDisplayIngredientPrefab;

        [SerializeField]
        private TextMeshPro _sideIngredientText; //Temporary, displays side ingredients in cup on screen
        
        public DrinkMixture DrinkMixture{ get; set;}

        private Dictionary<MainIngredientData, SpriteRenderer> _ingredientSprites = new Dictionary<MainIngredientData, SpriteRenderer>();


        public void UpdateDisplay()
        {
            //Keep track of how filled the displayCup is between ingredients, so they can be stacked instead of overlapping eachother.
            float filledPortion = 0;
            //Loop through each ingredient in the mixture and use its ingredient amount variables to display and position the ingredient contents.
            foreach(KeyValuePair<MainIngredientData, float> pair in DrinkMixture.MainIngredients)
            {
                GameObject inst;
                if (!_ingredientSprites.ContainsKey(pair.Key))
                {
                    //Create and position a new gameobject that will display this ingredient visually. Add previous relative fill amounts to he height its spawned at.
                    inst = Instantiate(cupDisplayIngredientPrefab, new Vector3(transform.position.x + _spawnPos.x, 
                                                                                transform.position.y + _spawnPos.y, 
                                                                                transform.position.z + _spawnPos.z), 
                                                                                Quaternion.identity);
                    inst.transform.parent = this.transform;
                    var instSpriteRenderer = inst.GetComponent<SpriteRenderer>();
                    //Give a random color, to easier differentiate the ingredient fills, visually. (just for the demo)
                    instSpriteRenderer.color = pair.Key.InCupColor;

                    _ingredientSprites.Add(pair.Key, instSpriteRenderer);
                }
                //Get instance 
                inst = _ingredientSprites[pair.Key].gameObject;
                inst.transform.position = new Vector3(transform.position.x, transform.position.y+filledPortion, transform.position.z);
                //Calculate its fill amount. This is proportionally how much of the cup the liquid fills, and then scaled to the visual height of the cup.
                var fillAmount = _displayCupHeight / (DrinkMixture.MaxTotalLiquid / pair.Value);
                //Set the y-scale of the ingredient display object to the height we just calculated
                inst.transform.localScale = new Vector3(inst.transform.localScale.x, fillAmount, inst.transform.localScale.z);

                //Adds the visual-relative amount filled so far, so the next ingredient can be positioned at the top of existing ingredients
                filledPortion += fillAmount;
            }
        }

        public void UpdateSideIngredientDisplay()
        {
            _sideIngredientText.text = "";
            foreach(SideIngredientData si in DrinkMixture.SideIngredients.HashSet)
            {
                _sideIngredientText.text += si.Name + "\n";
            }
        }

        public void ResetIngredientDisplay()
        {
            //Remove sprite child objects when drink display is cleared
            foreach(KeyValuePair<MainIngredientData, SpriteRenderer> displayIng in _ingredientSprites)
                Destroy(displayIng.Value.gameObject);
            //Remove references to deleted sprite objects
            _ingredientSprites.Clear();
            UpdateDisplay();
            UpdateSideIngredientDisplay();
        }

    }
}
