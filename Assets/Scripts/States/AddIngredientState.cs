using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iCook
{
    public class AddIngredientState : State
    {
        private eIngredientType ingredient = eIngredientType.ING_NONE;

        public AddIngredientState(string payload) : base(payload)
        {

        }

        public override void EnterState()
        {
            ingredient = RecipeManager.GetIngredientTypeEnum(payload);
            iCookVisualizer.Instance.GetPosition(ePositionType.POSITION_INGREDIENT_RACK, payload);
        }

        public override void ExitState()
        {
           
        }

        public override void UpdateState()
        {

        }
    }
}
