using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

namespace iCook
{
    public enum eStirPattern 
    {
        STIR_NONE,
        STIR_CIRCLE,
        STIR_SWIPE,
        STIR_COUNT
    }

    [System.Serializable]
    public class StirInfo
    {
        public eStirPattern stirPattern;
        public Spline spline;
    }

    public class StirManager : Singleton<StirManager>
    {
        public StirInfo[] stirInfos;

        public Spline GetStirSpline(eStirPattern stirPattern)
        {
            foreach(StirInfo stirInfo in stirInfos)
            {
                if(stirInfo.stirPattern == stirPattern)
                {
                    return stirInfo.spline;
                }
            }

            return null;
        }

        public eStirPattern GetStirPattern(string stirPatternName)
        {
            stirPatternName = stirPatternName.ToLower();

            switch(stirPatternName)
            {
                case "circle": return eStirPattern.STIR_CIRCLE;
                case "swipe": return eStirPattern.STIR_SWIPE;
            }


            return eStirPattern.STIR_NONE;
        }

        public Spline GetStirSpline(string stirPatterName)
        {
            eStirPattern stirPattern = GetStirPattern(stirPatterName);

            return GetStirSpline(stirPattern);
        }
    }
}
