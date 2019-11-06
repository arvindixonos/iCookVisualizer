using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iCook
{
    public enum eRackState
    {
        RACK_CLOSED,
        RACK_OPEN,
        RACK_CLOSING,
        RACK_OPENING
    }

    [System.Serializable]
    public class RackSlot : MonoBehaviour
    {
        public eIngredientType ingredientPresent = eIngredientType.ING_NONE;
        public Transform handleHoldPosition;
        public eRackState currentRackState = eRackState.RACK_CLOSED;

        public void AttachToClaw()
        {
            transform.parent = iCookVisualizer.Instance.clawTip;
        }

        public void DetchFromClaw()
        {
            transform.parent = iCookVisualizer.Instance.rack;
        }
    }
}
