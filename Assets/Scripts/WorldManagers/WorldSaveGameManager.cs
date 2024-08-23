using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SG
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager instance;

        [SerializeField] PlayerManager player;

        [Header("SAVE/LOAD")]
        [SerializeField] bool saveGame;
        [SerializeField] bool loadGame;

        [Header("World Scene Index")]
        [SerializeField] int worldSceneIndex = 1;

        [Header("Svae Data Writer")]
        private SaveFileDataWriter saveFileDataWriter;

        [Header("Current Character Data")]
        public CharacterSlot currentCharacterSlotBeingUsed;
        public CharacterSaveData currentCharacterData;
        private string saveFileName;

        [Header("Character Slots")]
        public CharacterSaveData characerSlot01;
        /*public CharacterSaveData characerSlot02;
        public CharacterSaveData characerSlot03;
        public CharacterSaveData characerSlot04;
        public CharacterSaveData characerSlot05;
        public CharacterSaveData characerSlot06;
        public CharacterSaveData characerSlot07;
        public CharacterSaveData characerSlot08;
        public CharacterSaveData characerSlot09;
        public CharacterSaveData characerSlot10;*/
        private void Awake()
        {
            // 이 스크립트의 인스턴스는 오직 하나만 존재할 수 있다. 다른 인스턴스가 존재할 시 삭제한다.
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (saveGame)
            {
                saveGame = false;
                SaveGame();
            }
            
            if (loadGame)
            {
                loadGame = false;
                LoadGame();
            }
        }

        private void DecideCharacterFileNameBasedOnCharacterSlotBeingUsed()
        {
            switch (currentCharacterSlotBeingUsed)
            {
                case CharacterSlot.CharacterSlot_01:
                    saveFileName = "CharacterSlot_01";
                    break;
                case CharacterSlot.CharacterSlot_02:
                    saveFileName = "CharacterSlot_02";
                    break;
                case CharacterSlot.CharacterSlot_03:
                    saveFileName = "CharacterSlot_03";
                    break;
                case CharacterSlot.CharacterSlot_04:
                    saveFileName = "CharacterSlot_04";
                    break;
                case CharacterSlot.CharacterSlot_05:
                    saveFileName = "CharacterSlot_05";
                    break;
                case CharacterSlot.CharacterSlot_06:
                    saveFileName = "CharacterSlot_06";
                    break;
                case CharacterSlot.CharacterSlot_07:
                    saveFileName = "CharacterSlot_07";
                    break;
                case CharacterSlot.CharacterSlot_08:
                    saveFileName = "CharacterSlot_08";
                    break;
                case CharacterSlot.CharacterSlot_09:
                    saveFileName = "CharacterSlot_09";
                    break;
                case CharacterSlot.CharacterSlot_10:
                    saveFileName = "CharacterSlot_10";
                    break;
            }
        }

        public void CreateNewGame()
        {
            DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();

            currentCharacterData = new CharacterSaveData();
        }

        public void LoadGame()
        {
            DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();

            saveFileDataWriter = new SaveFileDataWriter();
            // GENERALLY WORKS ON MULTIPLE MACHINE TYPE ( Application.persistentDataPath )
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.saveFileName = saveFileName;
            currentCharacterData = saveFileDataWriter.LoadSaveFile();

            StartCoroutine(LoadWorldScene());
        }
        
        public void SaveGame()
        {
            // SAVE THE CURRENT FILE UNDER A FILE NAME DEPENDING ON WHICH SLOT WE ARE USING
            DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();

            saveFileDataWriter = new SaveFileDataWriter();

            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.saveFileName = saveFileName;

            player.SaveGameDataToCurrentCharacterData(ref currentCharacterData);

            saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);
        }

        public IEnumerator LoadWorldScene()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);

            yield return null;
        }

        public int GetWorldSceneIndex()
        {
            return worldSceneIndex;
        }
    }
}
