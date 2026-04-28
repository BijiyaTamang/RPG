using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Managers")]
    public DialogueManager DialogueManager;
    public DialogueHistoryTracker DialogueHistoryTracker;
    public LocationHistoryTracker LocationHistoryTracker;
    public QuestManager QuestManager;
    public StatsManager StatsManager;
    public ExpManager ExpManager;
    public SkillTreeManager SkillTreeManager;
    public InventoryManager InventoryManager;
    public SkillSlot[] allSkillSlots;
    public List<ItemSO> allItems;

    [Header("Save System")]
    public PlayerDataSO playerData;

    [Header("Persistent Objects")]
    public GameObject[] persistentObjects;

    private bool hasLoaded = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        MarkPersistentObjects();
    }

    private void Start()
    {
        if (hasLoaded) return;
        hasLoaded = true;
        LoadGame();
    }

    // ── INPUT ─────────────────────────────────────────────────
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager uiManager = FindObjectOfType<UIManager>();
            if (uiManager != null)
                uiManager.ShowExitCanvas();
        }
    }

    public void QuitGame()
    {
        SaveGame();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
#else
    Application.Quit(); // ← works in actual build
#endif
    }

    private void MarkPersistentObjects()
    {
        foreach (GameObject obj in persistentObjects)
        {
            if (obj != null)
            {
                obj.transform.SetParent(null);
                DontDestroyOnLoad(obj);
            }
        }
    }

    private void CleanUpAndDestroy()
    {
        foreach (GameObject obj in persistentObjects)
            Destroy(obj);
        Destroy(gameObject);
    }

    // ── SAVE ──────────────────────────────────────────────────
    public void SaveGame()
    {
        if (playerData == null)
        {
            Debug.LogError("[GameManager] playerData SO is not assigned!");
            return;
        }

        // Stats
        playerData.damage = StatsManager.damage;
        playerData.weaponRange = StatsManager.weaponRange;
        playerData.knockbackForce = StatsManager.knockbackForce;
        playerData.knockbackDuration = StatsManager.knockbackDuration;
        playerData.stunTime = StatsManager.stunTime;
        playerData.moveSpeed = StatsManager.moveSpeed;
        playerData.arrowDamage = StatsManager.arrowDamage;
        playerData.arrowSpeed = StatsManager.arrowSpeed;
        playerData.arrowStunTime = StatsManager.arrowStunTime;
        playerData.currentHealth = StatsManager.currentHealth;
        playerData.maxHealth = StatsManager.maxHealth;

        // Exp
        playerData.expLevel = ExpManager.level;
        playerData.currentExp = ExpManager.currentExp;
        playerData.expToLevel = ExpManager.expToLevel;

        // Skill tree
        playerData.availableSkillPoints = SkillTreeManager.availableSkillPoints;
        playerData.bowUnlocked = SkillManager.bowUnlocked;

        // Skill slots
        playerData.skillSlots = new List<SkillSlotData>();
        foreach (var slot in allSkillSlots)
        {
            playerData.skillSlots.Add(new SkillSlotData
            {
                skillName = slot.skillSO.skillName,
                currentLevel = slot.currentLevel,
                isUnlocked = slot.isUnlocked
            });
        }

        // Inventory
        playerData.gold = InventoryManager.gold;
        playerData.inventorySlots = new List<InventorySlotData>();
        foreach (var slot in InventoryManager.itemSlots)
        {
            playerData.inventorySlots.Add(new InventorySlotData
            {
                itemName = slot.itemSO != null ? slot.itemSO.itemName : "",
                quantity = slot.quantity
            });
        }

        // Quests
        playerData.activeQuests = new List<string>();
        foreach (var quest in QuestManager.GetActiveQuests())
            playerData.activeQuests.Add(quest.questName);

        playerData.completedQuests = new List<string>();
        foreach (var quest in QuestManager.GetCompletedQuests())
            playerData.completedQuests.Add(quest.questName);

        // Dialogue & Location history
        playerData.spokenNPCs = DialogueHistoryTracker.GetSpokenNPCNames();
        playerData.visitedLocations = LocationHistoryTracker.GetVisitedLocationNames();

        // NPC dialogue states
        playerData.npcDialogueStates = new List<NPCDialogueData>();
        var allNPCTalks = FindObjectsByType<NPC_Talk>(
            FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var npc in allNPCTalks)
            npc.SaveDialogueState();

        playerData.SaveToFile();
        Debug.Log("[GameManager] Game saved.");
    }

    // ── LOAD ──────────────────────────────────────────────────
    public void LoadGame()
    {
        if (!SaveSystem.HasSave())
        {
            Debug.Log("[GameManager] No save found, skipping load.");
            return;
        }
        if (playerData == null)
        {
            Debug.LogError("[GameManager] playerData SO is not assigned!");
            return;
        }

        playerData.LoadFromFile();

        // Stats
        StatsManager.damage = playerData.damage;
        StatsManager.weaponRange = playerData.weaponRange;
        StatsManager.knockbackForce = playerData.knockbackForce;
        StatsManager.knockbackDuration = playerData.knockbackDuration;
        StatsManager.stunTime = playerData.stunTime;
        StatsManager.moveSpeed = playerData.moveSpeed;
        StatsManager.arrowDamage = playerData.arrowDamage;
        StatsManager.arrowSpeed = playerData.arrowSpeed;
        StatsManager.arrowStunTime = playerData.arrowStunTime;
        StatsManager.currentHealth = playerData.currentHealth;
        StatsManager.maxHealth = (int)playerData.maxHealth;

        // Exp
        ExpManager.level = playerData.expLevel;
        ExpManager.currentExp = playerData.currentExp;
        ExpManager.expToLevel = playerData.expToLevel;
        ExpManager.UpdateExpUI();

        // Skill tree
        SkillTreeManager.availableSkillPoints = playerData.availableSkillPoints;
        SkillManager.bowUnlocked = playerData.bowUnlocked;

        // Skill slots
        foreach (var savedSlot in playerData.skillSlots)
        {
            foreach (var slot in allSkillSlots)
            {
                if (slot.skillSO.skillName == savedSlot.skillName)
                {
                    slot.currentLevel = savedSlot.currentLevel;
                    slot.isUnlocked = savedSlot.isUnlocked;
                    break;
                }
            }
        }

        // Inventory
        InventoryManager.gold = playerData.gold;
        for (int i = 0; i < playerData.inventorySlots.Count
                     && i < InventoryManager.itemSlots.Length; i++)
        {
            var savedSlot = playerData.inventorySlots[i];
            if (!string.IsNullOrEmpty(savedSlot.itemName))
            {
                ItemSO item = allItems.Find(x => x.itemName == savedSlot.itemName);
                InventoryManager.itemSlots[i].itemSO = item;
                InventoryManager.itemSlots[i].quantity = savedSlot.quantity;
            }
            else
            {
                InventoryManager.itemSlots[i].itemSO = null;
                InventoryManager.itemSlots[i].quantity = 0;
            }
            InventoryManager.itemSlots[i].UpdateUI();
        }

        // NPC dialogue states
        var allNPCTalks = FindObjectsByType<NPC_Talk>(
            FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var npc in allNPCTalks)
            npc.LoadDialogueState();

        // Dialogue & Location history — load BEFORE quests
        DialogueHistoryTracker.LoadSpokenNPCs(playerData.spokenNPCs);
        LocationHistoryTracker.LoadVisitedLocations(playerData.visitedLocations);

        // Quests — load AFTER dialogue/location history
        QuestManager.LoadQuests(playerData.activeQuests, playerData.completedQuests);

        Debug.Log("[GameManager] Game loaded.");
    }

    // ── NEW GAME ──────────────────────────────────────────────
    public void NewGame()
    {
        SaveSystem.Delete();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ── AUTO-SAVE ─────────────────────────────────────────────
    private void OnApplicationQuit() => SaveGame();
    private void OnApplicationPause(bool pausing) { if (pausing) SaveGame(); }
}