using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iCook
{
    public class RackManager : MonoBehaviour
    {
        public RackSlot[] rackSlots;

        public RackSlot GetSlotWithIngredient(eIngredientType ingredientType)
        {
            foreach(RackSlot rackSlot in rackSlots)
            {
                if(rackSlot.ingredientPresent == ingredientType)
                {
                    return rackSlot;
                }
            }

            return null;
        }

    }
}