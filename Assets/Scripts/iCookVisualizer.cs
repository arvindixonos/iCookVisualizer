using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iCook
{
    public enum ePositionType
    {
        POSITION_IDLE,
        POSITION_TEMPERATURE_UP,
        POSITION_TEMPERATURE_DOWN,
        POSITION_INGREDIENT_RACK_START,
        POSITION_INGREDIENT_RACK_HOLD,
        POSITION_INGREDIENT_RACK_FETCH,
        POSITION_DROP_TO_PAN
    }

    [System.Serializable]
    public class PositionsInfo
    {
        public ePositionType positionType;
        public Transform targetPosition;
    }

    public class iCookVisualizer : Singleton<iCookVisualizer>
    {
        public static string remoteIPAddress = "192.168.0.31";
        public static int remotePORT = 5555;

        public iCookHand cookHand;
        private UDPConnection udpConnection;

        private RecipeManager recipeManager;
        private Recipe currentRecipe;
        private float cookingTimer = 0f;
        private bool startedCooking = false;

        private TaskMachine taskMachine;

        public PositionsInfo[] positionsInfos;

        public RackManager rackManager;

        public Transform clawTip;
        public Transform rack;

        private Stove stove;

        private RecipeTask currentRecipeTask;
        public iCook.RecipeTask CurrentRecipeTask
        {
            get { return currentRecipeTask; }
            set { currentRecipeTask = value; }
        }
        private int recipeTaskIndex = 0;

        public RootMotion.FinalIK.CCDIK ccdIK;

        public int currentTemperature = 700;
        public int CurrentTemperature
        {
            get { return currentTemperature; }
            set { currentTemperature = value; }
        }
        private int temperatureStep = 100;
        public int TemperatureStep
        {
            get { return temperatureStep; }
            set { temperatureStep = value; }
        }
        private int minTemperature = 700;
        private int maxTemperature = 2000;

        void Start()
        {
            InitializeiCook();

            StartCooking(eRecipe.RECIPE_AMERICAN_PEPPER_CHICKEN);
        }

        public void TemperatureIncreased()
        {
            CurrentTemperature += TemperatureStep;
            CurrentTemperature = Mathf.Clamp(CurrentTemperature, minTemperature, maxTemperature);
        }

        public void TemperatureDecreased()
        {
            CurrentTemperature -= TemperatureStep;
            CurrentTemperature = Mathf.Clamp(CurrentTemperature, minTemperature, maxTemperature);
        }

        public RackSlot GetRackSlot(eIngredientType ingredientType)
        {
            return rackManager.GetSlotWithIngredient(ingredientType);
        }

        private Transform GetPositionTransform(ePositionType positionType)
        {
            foreach(PositionsInfo positionsInfo in positionsInfos)
            {
                if(positionsInfo.positionType == positionType)
                {
                    return positionsInfo.targetPosition;
                }
            }

            return null;
        }

        public Transform GetTargetPosition(ePositionType positionType, System.Object payload = null)
        {
            switch(positionType)
            {
                case ePositionType.POSITION_IDLE: 
                case ePositionType.POSITION_TEMPERATURE_UP:
                case ePositionType.POSITION_TEMPERATURE_DOWN:
                case ePositionType.POSITION_DROP_TO_PAN: return GetPositionTransform(positionType);
                case ePositionType.POSITION_INGREDIENT_RACK_START:
                    RackSlot rackSlot = rackManager.GetSlotWithIngredient(RecipeManager.GetIngredientTypeEnum((string)payload));
                    if(rackSlot != null)
                    {
                        return rackSlot.handleStartPosition;
                    }

                    break;

                case ePositionType.POSITION_INGREDIENT_RACK_HOLD:
                    rackSlot = rackManager.GetSlotWithIngredient(RecipeManager.GetIngredientTypeEnum((string)payload));
                    if (rackSlot != null)
                    {
                        return rackSlot.handleHoldPosition;
                    }

                    break;

                case ePositionType.POSITION_INGREDIENT_RACK_FETCH:
                    rackSlot = rackManager.GetSlotWithIngredient(RecipeManager.GetIngredientTypeEnum((string)payload));
                    if (rackSlot != null)
                    {
                        return rackSlot.handleFetchPosition;
                    }

                    break;
            }

            return null;
        }

        void InitializeiCook()
        {
            udpConnection = new UDPConnection(remoteIPAddress, remotePORT);
            recipeManager = new RecipeManager();
            recipeManager.Init();

            taskMachine = new TaskMachine();

            stove = new Stove();
            stove.SwitchOnStove();

            TaskMachine.OnTaskComplete += TaskComplete;
            cookHand.OnAngleChanged += JointAngleChanged;
            stove.OnTemperatureChanged += TemperatureChanged;
        }

        private void TemperatureChanged(int newTemperature)
        {
            print(newTemperature);
        }

        public void SetHandTarget(Transform target)
        {
            ccdIK.solver.target = target;
        }

        public void JointAngleChanged(eJointType jointType, float currentAngle)
        {
            byte[] jointPayload = GetJointPayload(jointType, currentAngle);
            udpConnection.SendNetworkMessage(eNetworkMessageType.NM_JOINT_INFO, jointPayload);
        }

        public byte[] GetJointPayload(eJointType jointType, float currentAngle)
        {
            byte[] jointTypeBytes = SerializerDeserializer.ToByteArray((int)jointType);
            byte[] currentAngleBytes = SerializerDeserializer.ToByteArray(currentAngle);

            byte[] accumulatedBytes = new byte[jointTypeBytes.Length + currentAngleBytes.Length];

            int destinationIndex = 0;
            int copyLength = 0;

            copyLength = SerializerDeserializer.AddByteArray(accumulatedBytes, jointTypeBytes, destinationIndex);
            destinationIndex += copyLength;

            copyLength = SerializerDeserializer.AddByteArray(accumulatedBytes, currentAngleBytes, destinationIndex);
            destinationIndex += copyLength;

            return accumulatedBytes;
        }

        public void StartCooking(eRecipe recipeType)
        {
            startedCooking = true;
            cookingTimer = 0f;

            currentRecipe = recipeManager.GetRecipe(recipeType);

            recipeTaskIndex = -1;
            GotoNextTask();
        }

        public bool GotoNextTask()
        {
            recipeTaskIndex += 1;

            if(currentRecipe.isTaskPresent(recipeTaskIndex))
            {
                CurrentRecipeTask = currentRecipe.GetRecipeTask(recipeTaskIndex);

                taskMachine.SetCurrentTask(CurrentRecipeTask);
                taskMachine.RunTaskMachine();

                return true;
            }

            return false;
        }

        public void StopCooking()
        {
            startedCooking = false; 
        }

        void TaskComplete()
        {
            print("Task Completed: " + CurrentRecipeTask.recipeTaskEnum);

            GotoNextTask();
        }

        void Update()
        {
            if(startedCooking)
            {
                cookHand.UpdateHand();

                cookingTimer += Time.deltaTime;

                if(taskMachine != null)
                {
                    taskMachine.UpdateTaskMachine();
                }
            }
        }
    }
}
