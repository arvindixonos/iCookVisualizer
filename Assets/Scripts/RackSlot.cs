using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iCook
{
    public enum eIngredientType
    {
        ING_NONE,
        ING_OIL,
        ING_ONION,
        ING_CAPSICUM,
        ING_CHICKEN,
        ING_SALT,
        ING_PEPPER
    }

    public enum eRackState
    {
        RACK_CLOSED,
        RACK_OPEN,
        RACK_CLOSING,
        RACK_OPENING
    }

    [System.Serializable]
    public class RackSlot
    {
        public eIngredientType ingredientPresent;
        public Transform handlePosition;

        public eRackState currentRackState = eRackState.RACK_CLOSED;

        public Transform rackHandleTarget;
    }
}
