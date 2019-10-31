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
        STEP_IDLE,
        STEP_COUNT
    }

    [System.Serializable]
    public class RecipeStep
    {
        public string recipeStep;
        public eRecipeStep recipeStepEnum;
        public string payload;
        public float StartTime;

        private bool inited = false;

        public static eRecipeStep GetRecipeStepEnum(string recipeStep)
        {
            switch (recipeStep)
            {
                case "Add Ingredient": return eRecipeStep.STEP_ADD_INGREDIENT;
                case "Stir": return eRecipeStep.STEP_STIR;
                case "Set Temperature": return eRecipeStep.STEP_SET_TEMPERATURE;
                case "Idle": return eRecipeStep.STEP_IDLE;
            }

            return eRecipeStep.STEP_ADD_INGREDIENT;
        }

        public void Init()
        {
            if(inited)
            {
                return;
            }

            inited = true;

            recipeStepEnum = GetRecipeStepEnum(recipeStep);
        }
    }

    [System.Serializable]
    public class Recipe
    {
        public string recipeName;
        public eRecipe recipe;
        public List<RecipeStep> recipeSteps = new List<RecipeStep>();

        private bool inited = false;

        public void Init()
        {
            if(inited)
            {
                return;
            }

            inited = true;

            foreach(RecipeStep recipeStep in recipeSteps)
            {
                recipeStep.Init();
            }
        }

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
                recipe.Init();
                recipes.Add(recipe);
            }
        }
    }
}