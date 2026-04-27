using System;
using System.Collections.Generic;
[Serializable]
public class PlayerSaveData
{
    public int level;
    public float health;
    public float maxHealth;
    public int gold;
    public float playTimeSeconds;
    public List<string> unlockedAbilities;
    public List<string> inventoryItemIds;
    public List<string> activeQuests;
    public List<string> completedQuests;

    // StatsManager
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

    // ExpManager
    public int expLevel;
    public int currentExp;
    public int expToLevel;

    // SkillTreeManager
    public int availableSkillPoints;
    public bool bowUnlocked;

    // Skill slots
    public List<SkillSlotData> skillSlots;

    // Inventory
    public List<InventorySlotData> inventorySlots;

    // Dialogue & Location history
    public List<string> spokenNPCs;
    public List<string> visitedLocations;

    public List<NPCDialogueData> npcDialogueStates;
}

[Serializable]
public class SkillSlotData
{
    public string skillName;
    public int currentLevel;
    public bool isUnlocked;
}

[Serializable]
public class InventorySlotData
{
    public string itemName;
    public int quantity;
}

[Serializable]
public class NPCDialogueData
{
    public string npcID;
    public List<string> removedDialogues;
}