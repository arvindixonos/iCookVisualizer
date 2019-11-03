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
            Debug.Log("State Complete: " + currentState.ToString());

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
            switch (recipeTask.recipeTaskEnum)
            {
                case eRecipeTask.TASK_IDLE:
                    State moveToPositionState = new MoveToPositionState(recipeTask.payload); states.Enqueue(moveToPositionState);
                    break;

                case eRecipeTask.TASK_ADD_INGREDIENT:
                    //state = new AddIngredientState();
                    break;
            }
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