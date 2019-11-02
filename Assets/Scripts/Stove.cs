using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iCook
{
    public class Stove
    {
        private bool on = false;
        public bool isOn
        {
            get { return on; }
        }

        private int currentTemperature = 700;
        public int CurrentTemperature
        {
            get { return currentTemperature; }
        }

        private int maxTemperature = 2000;
        private int minTemperature = 700;

        private int tempIncrementStep = 100;

        public Action<int> OnTemperatureChanged;

        public void SwitchOnStove()
        {
            on = true;
        }

        public void SwitchOffStove()
        {
            on = false;
        }

        public void TemperatureUp()
        {
            currentTemperature += tempIncrementStep;
            currentTemperature = Mathf.Clamp(currentTemperature, minTemperature, maxTemperature);
            OnTemperatureChanged(currentTemperature);
        }

        public void TemperatureDown()
        {
            currentTemperature -= tempIncrementStep;
            currentTemperature = Mathf.Clamp(currentTemperature, minTemperature, maxTemperature);
            OnTemperatureChanged(currentTemperature);
        }
    }
}

