﻿using System;
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
        public Transform handlePosition;
        public eRackState currentRackState = eRackState.RACK_CLOSED;
    }
}
