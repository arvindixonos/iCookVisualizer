using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace iCook
{
    public enum eRecipe
    {
        RECIPE_AMERICAN_PEPPER_CHICKEN,
        RECIPE_COUNT
    }

    public enum eRecipeStep
    {
        STEP_ADD_INGREDIENT,
        STEP_STIR,
        STEP_SET_TEMPERATURE,
        STEP_COUNT
    }

    [System.Serializable]
    public class RecipeStep
    {
        public eRecipeStep recipeStep;
        public string payload;
    }

    [System.Serializable]
    public class Recipe
    {
        public string recipeName;
        public eRecipe recipe;
        public List<RecipeStep> recipeSteps = new List<RecipeStep>();

        public void AddRecipeStep(RecipeStep recipeStep)
        {
            recipeSteps.Add(recipeStep);
        }

        public RecipeStep GetRecipeStep(int recipeStep)
        {
            return recipeSteps[recipeStep];
        }
    }

    public class RecipeManager
    {
        private List<Recipe> recipes = new List<Recipe>();
        private bool inited = false;

        public void AddRecipe()
        {
            
        }

        public void Init()
        {
            if (inited)
                return;

            LoadAllRecipes();

            inited = true;
        }

        private void LoadAllRecipes()
        {
            int numRecipes = (int)eRecipe.RECIPE_COUNT;

            for(int i = 0; i < numRecipes; i++)
            {
                string jsonText = File.ReadAllText(Application.streamingAssetsPath + "/Recipe/" + "American Pepper Chicken.txt");
                Recipe recipe = JsonUtility.FromJson<Recipe>(jsonText);

                recipes.Add(recipe);
            }
        }
    }
}