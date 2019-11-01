using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioIK;

namespace iCook
{
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
        private Queue<RecipeStep> recipeStepQueue = new Queue<RecipeStep>();

        private int iNextRecipeStep = 0;
        private RecipeStep nextRecipeStep = null;

        void Start()
        {
            InitializeiCook();

            StartCooking(eRecipe.RECIPE_AMERICAN_PEPPER_CHICKEN);
        }

        void InitializeiCook()
        {
            udpConnection = new UDPConnection(remoteIPAddress, remotePORT);
            recipeManager = new RecipeManager();
            recipeManager.Init();

            cookHand.OnAngleChanged += JointAngleChanged;
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
            iNextRecipeStep = 0;
            nextRecipeStep = null;

            currentRecipe = recipeManager.GetRecipe(recipeType);
            FillNextRecipeStep();
        }

        public void StopCooking()
        {
            startedCooking = false; 
        }

        public RecipeStep GetRecipeStep(int stepIndex)
        {
            return  currentRecipe.GetRecipeStep(stepIndex);
        }

        void FireNextRecipeStep()
        {
            print("Firing Recipe Step : " + nextRecipeStep.recipeStep + 
                " With Payload : " + nextRecipeStep.payload + " At Time : " + nextRecipeStep.startTime); 

            recipeStepQueue.Enqueue(nextRecipeStep);
            iNextRecipeStep += 1;

            FillNextRecipeStep();
        }

        void FillNextRecipeStep()
        {
            if(currentRecipe.isStepPresent(iNextRecipeStep))
            {
                nextRecipeStep = currentRecipe.GetRecipeStep(iNextRecipeStep);
            }
            else
            {
                nextRecipeStep = null;
            }
        }

        void Update()
        {
            cookHand.UpdateHand();

            if(startedCooking)
            {
                cookingTimer += Time.deltaTime;

                if(nextRecipeStep != null)
                {
                    if(nextRecipeStep.startTime < cookingTimer)
                    {
                        FireNextRecipeStep();
                    }
                }
            }
        }
    }
}
