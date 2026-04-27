using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PlayerData", menuName = "Game/Player Data")]
public class PlayerDataSO : ScriptableObject
{
    public int level = 1;
    public float health = 100f;
    public float maxHealth = 100f;
    public int gold = 0;
    public float playTimeSeconds = 0f;
    public List<string> unlockedAbilities = new List<string>();
    public List<string> inventoryItemIds = new List<string>();
    public List<string> activeQuests = new List<string>();
    public List<string> completedQuests = new List<string>();

    // Stats
    public int damage;
    public float weaponRange;
    public float knockbackForce;
    public float knockbackDuration;
    public float stunTime;
    public float moveSpeed;
    public int arrowDamage;
    public float arrowSpeed;
    public float arrowStunTime;
    public int currentHealth;

    // Exp
    public int expLevel;
    public int currentExp;
    public int expToLevel;

    // Skill tree
    public int availableSkillPoints;
    public bool bowUnlocked;
    public List<SkillSlotData> skillSlots = new List<SkillSlotData>();

    // Inventory
    public List<InventorySlotData> inventorySlots = new List<InventorySlotData>();

    // Dialogue & Location history
    public List<string> spokenNPCs = new List<string>();
    public List<string> visitedLocations = new List<string>();
    public List<NPCDialogueData> npcDialogueStates = new List<NPCDialogueData>();

    public void SaveToFile()
    {
        var data = new PlayerSaveData
        {
            level = this.level,
            health = this.health,
            maxHealth = this.maxHealth,
            gold = this.gold,
            playTimeSeconds = this.playTimeSeconds,
            unlockedAbilities = new List<string>(this.unlockedAbilities),
            inventoryItemIds = new List<string>(this.inventoryItemIds),
            activeQuests = new List<string>(this.activeQuests),
            completedQuests = new List<string>(this.completedQuests),
            damage = this.damage,
            weaponRange = this.weaponRange,
            knockbackForce = this.knockbackForce,
            knockbackDuration = this.knockbackDuration,
            stunTime = this.stunTime,
            moveSpeed = this.moveSpeed,
            arrowDamage = this.arrowDamage,
            arrowSpeed = this.arrowSpeed,
            arrowStunTime = this.arrowStunTime,
            currentHealth = this.currentHealth,
            expLevel = this.expLevel,
            currentExp = this.currentExp,
            expToLevel = this.expToLevel,
            availableSkillPoints = this.availableSkillPoints,
            bowUnlocked = this.bowUnlocked,
            skillSlots = new List<SkillSlotData>(this.skillSlots),
            inventorySlots = new List<InventorySlotData>(this.inventorySlots),
            spokenNPCs = new List<string>(this.spokenNPCs),
            visitedLocations = new List<string>(this.visitedLocations),
            npcDialogueStates = new List<NPCDialogueData>(this.npcDialogueStates),
        };
        SaveSystem.Save(data);
    }

    public void LoadFromFile()
    {
        PlayerSaveData data = SaveSystem.Load();
        level = data.level;
        health = data.health;
        maxHealth = data.maxHealth;
        gold = data.gold;
        playTimeSeconds = data.playTimeSeconds;
        unlockedAbilities = data.unlockedAbilities ?? new List<string>();
        inventoryItemIds = data.inventoryItemIds ?? new List<string>();
        activeQuests = data.activeQuests ?? new List<string>();
        completedQuests = data.completedQuests ?? new List<string>();
        damage = data.damage;
        weaponRange = data.weaponRange;
        knockbackForce = data.knockbackForce;
        knockbackDuration = data.knockbackDuration;
        stunTime = data.stunTime;
        moveSpeed = data.moveSpeed;
        arrowDamage = data.arrowDamage;
        arrowSpeed = data.arrowSpeed;
        arrowStunTime = data.arrowStunTime;
        currentHealth = data.currentHealth;
        expLevel = data.expLevel;
        currentExp = data.currentExp;
        expToLevel = data.expToLevel;
        availableSkillPoints = data.availableSkillPoints;
        bowUnlocked = data.bowUnlocked;
        skillSlots = data.skillSlots ?? new List<SkillSlotData>();
        inventorySlots = data.inventorySlots ?? new List<InventorySlotData>();
        spokenNPCs = data.spokenNPCs ?? new List<string>();
        visitedLocations = data.visitedLocations ?? new List<string>();
        npcDialogueStates = data.npcDialogueStates ?? new List<NPCDialogueData>();
    }
}