using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iCook
{
    public class State
    {
        protected System.Object payload;

        public State(System.Object payload)
        {
            this.payload = payload;
        }

        public virtual void EnterState() { }
        public virtual void UpdateState() { }
        public virtual void ExitState() { }

        public static Action OnStateComplete = null;
    }

    public class TaskMachine
    {
        private bool running = false;
        public bool isRunning
        {
            get { return running; }
        }

        public TaskMachine()
        {
            State.OnStateComplete += StateComplete;
        }

        public Queue<State> states = new Queue<State>();

        public State currentState = null;

        public static Action OnTaskComplete;

        public void StateComplete()
        {
            //Debug.Log("State Complete: " + currentState.ToString());

            currentState.ExitState();

            SetCurrentState();
        }

        public void RunTaskMachine()
        {
            running = true;
        }

        public void StopTaskMachine()
        {
            running = false;    
        }

        private bool SetCurrentState()
        {
            if (states.Count > 0)
            {
                State state = states.Dequeue();
                currentState = state;
                currentState.EnterState();

                return true;
            }

            currentState = null;
            StopTaskMachine();
            OnTaskComplete();

            return false;
        }

        public void SetCurrentTask(RecipeTask recipeTask)
        {
            Debug.Log("Entering Task: " + recipeTask.recipeTaskEnum);

            switch (recipeTask.recipeTaskEnum)
            {
                case eRecipeTask.TASK_IDLE:
                    SetIdleTask();
                    break;

                case eRecipeTask.TASK_SET_TEMPERATURE:
                    SetTemperatureTask(recipeTask);
                    break;

                case eRecipeTask.TASK_ADD_INGREDIENT:
                    SetAddIngredientTask(recipeTask);
                    break;
            }
        }

        void SetIdleTask()
        {
            Transform idleTransform = iCookVisualizer.Instance.GetTargetPosition(ePositionType.POSITION_IDLE);
            State moveToPositionState = new MoveToPositionState(idleTransform);
            states.Enqueue(moveToPositionState);
        }

        void SetAddIngredientTask(RecipeTask recipeTask)
        {
            string ingredient = recipeTask.payload;

            Transform ingredientTransform = iCookVisualizer.Instance.GetTargetPosition(ePositionType.POSITION_INGREDIENT_RACK, ingredient);
            State moveToPositionState = new MoveToPositionState(ingredientTransform);
            states.Enqueue(moveToPositionState);
        }

        void SetTemperatureTask(RecipeTask recipeTask)
        {
            int targetTemperature = int.Parse(recipeTask.payload);
            int currentTemperature = iCookVisualizer.Instance.CurrentTemperature;
            int differenceTemperature = currentTemperature - targetTemperature;

            if (differenceTemperature == 0)
                return;

            int tapCount = Mathf.Abs(differenceTemperature / iCookVisualizer.Instance.TemperatureStep);
            TapInfo tapInfo = new TapInfo();
            tapInfo.tapCount = tapCount;

            if (differenceTemperature > 0)
            {
                Transform temperatureDownTransform = iCookVisualizer.Instance.GetTargetPosition(ePositionType.POSITION_TEMPERATURE_DOWN);
                State moveToPositionState = new MoveToPositionState(temperatureDownTransform);
                states.Enqueue(moveToPositionState);

                tapInfo.OnTapComplete = iCookVisualizer.Instance.TemperatureDecreased;
            }
            else if (differenceTemperature < 0)
            {
                Transform temperatureUpTransform = iCookVisualizer.Instance.GetTargetPosition(ePositionType.POSITION_TEMPERATURE_UP);
                State moveToPositionState = new MoveToPositionState(temperatureUpTransform);
                states.Enqueue(moveToPositionState);

                tapInfo.OnTapComplete = iCookVisualizer.Instance.TemperatureIncreased;
            }

            TapState tapState = new TapState(tapInfo);
            states.Enqueue(tapState);
        }

        public void UpdateTaskMachine()
        {
            if(!running)
            {
                return;
            }

            if(currentState == null)
            {
                SetCurrentState();
            }
            else
            {
                currentState.UpdateState();
            }
        }
    }
}