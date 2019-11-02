using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioIK;

namespace iCook
{
    public enum ePositionType
    {
        POSITION_IDLE,
        POSITION_TEMPERATURE_UP,
        POSITION_TEMPERATURE_DOWN,
        POSITION_INGREDIENT_RACK
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

        public BioIK.BioIK bioIK;
        private BioIK.Position handBioObjectivePosition;
        public Transform clawTip;

        private Stove stove;

        private RecipeTask currentRecipeTask;
        private int recipeTaskIndex = 0;


        void Start()
        {
            InitializeiCook();

            StartCooking(eRecipe.RECIPE_AMERICAN_PEPPER_CHICKEN);
        }

        public UnityEngine.Object GetPosition(ePositionType positionType, string payload = null)
        {
            switch(positionType)
            {
                case ePositionType.POSITION_IDLE: return positionsInfos[(int)positionType].targetPosition;

                case ePositionType.POSITION_INGREDIENT_RACK:
                        eIngredientType ingredientType = RecipeManager.GetIngredientTypeEnum(payload);
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

            handBioObjectivePosition = bioIK.Segments[142].Objectives[0] as BioIK.Position;

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
            handBioObjectivePosition.SetTargetTransform(target);
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
                currentRecipeTask = currentRecipe.GetRecipeTask(recipeTaskIndex);

                taskMachine.SetCurrentTask(currentRecipeTask);
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
            print("Task Completed: " + currentRecipeTask.recipeTaskEnum);

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
