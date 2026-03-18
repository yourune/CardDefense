# Reward System Setup Guide

## Overview
The reward system allows enemies to drop gold and XP when they die, with visual feedback and a level-up system for the castle. **The system now uses object pooling for optimal performance**, reducing instantiation overhead and memory allocation.

## Components Created

### 1. GameActions
- **GainGoldGA**: Increases player's gold
- **GainXpGA**: Increases player's XP (triggers level up when threshold reached)
- **LevelUpGA**: Handles castle level up (increases max health)
- **DropRewardGA**: Triggers visual reward drops

### 2. Systems
- **RewardSystem**: Manages gold, XP, and leveling progression
- **RewardVisualSystem**: Handles visual feedback for reward drops (with object pooling)

### 3. Utility
- **SimpleObjectPool\<T\>**: Generic object pooling system for efficient object reuse

### 4. UI Components
- **RewardUI**: Displays gold, XP bar, and level information

### 5. Visual Components
- **FloatingRewardText**: Shows floating "+Gold +XP" text at enemy death location
- **GoldCoin**: Visual gold coin that animates to UI (pooled)
- **XpOrb**: Visual XP orb that animates to UI (pooled)

## Setup Instructions

### Step 1: Configure Enemy Data
1. Select your EnemyData ScriptableObjects in the Project window
2. Set the **Gold Drop** value (e.g., 10-50 gold per enemy)
3. Set the **XP Drop** value (e.g., 5-20 XP per enemy)

### Step 2: Create Systems in Scene
1. Create an empty GameObject named "RewardSystem"
2. Add the **RewardSystem** component
   - Set Starting Gold (default: 0)
   - Set Starting Level (default: 1)
   - Configure XP scaling:
     - Base XP Required: 100 (XP needed for level 2)
     - XP Scaling Factor: 1.5 (each level requires 1.5x more XP)
     - Health Per Level: 10 (castle gains 10 HP per level)

3. Create an empty GameObject named "RewardVisualSystem"
4. Add the **RewardVisualSystem** component
   - **Spawn Settings** (NEW):
     - Gold Per Coin: 20 (spawn 1 coin per 20 gold)
     - XP Per Orb: 10 (spawn 1 orb per 10 XP)
     - Max Coins Per Drop: 3 (maximum visual coins)
     - Max Orbs Per Drop: 3 (maximum visual orbs)
   - **Object Pool Settings** (NEW):
     - Initial Pool Size: 20 (pre-instantiate 20 of each type)
   - See Step 3 for prefab setup

### Step 3: Create Visual Prefabs

#### A. Gold Coin Prefab
1. Create a new Sprite GameObject (2D Object > Sprite)
2. Name it "GoldCoin"
3. Set a gold coin sprite (or use a yellow circle placeholder)
4. Add the **GoldCoin** component
5. Set scale to (0.5, 0.5, 1) or desired size
6. Save as prefab in your Prefabs folder

#### B. XP Orb Prefab
1. Create a new Sprite GameObject
2. Name it "XpOrb"
3. Set an orb/particle sprite (or use a blue/purple circle placeholder)
4. Add the **XpOrb** component
5. Set scale to (0.4, 0.4, 1) or desired size
6. Save as prefab

#### C. Floating Text Prefab
1. Create an empty GameObject named "FloatingRewardText"
2. Add the **FloatingRewardText** component
3. Create two child TextMeshPro objects:
   - **GoldText** (color: yellow/gold, text: "+10G")
   - **XpText** (color: cyan/blue, text: "+5XP")
4. Position them side by side or stacked
5. Set font size (e.g., 24-36)
6. Enable "Auto Size" for better scaling
7. Set Alignment to Center
8. Save as prefab

#### D. Configure RewardVisualSystem
1. Select the RewardVisualSystem GameObject
2. Assign the three prefabs:
   - Gold Coin Prefab
   - XP Orb Prefab
   - Floating Text Prefab
3. UI Targets will be set in Step 4

### Step 4: Create UI

#### A. Create Canvas
1. Right-click in Hierarchy > UI > Canvas
2. Set Canvas Scaler to "Scale with Screen Size" (1920x1080)

#### B. Gold Display
1. Create UI > Panel (name: "GoldPanel")
2. Position in top-right corner
3. Add children:
   - **UI > Image** (name: "GoldIcon") - Add a gold coin icon
   - **UI > Text - TextMeshPro** (name: "GoldText")
     - Text: "0"
     - Font Size: 36
     - Alignment: Middle Left

#### C. XP Display
1. Create UI > Panel (name: "XPPanel")
2. Position in top-left or bottom
3. Add children:
   - **UI > Text - TextMeshPro** (name: "LevelText")
     - Text: "Level 1"
     - Font Size: 28
   - **UI > Image** (name: "XPBarBackground") - Dark gray bar
   - **UI > Image** (name: "XPBarFill") - Blue/cyan bar
     - Set Image Type: Filled
     - Fill Method: Horizontal
     - Fill Origin: Left
     - Fill Amount: 0.5
   - **UI > Text - TextMeshPro** (name: "XPText")
     - Text: "50/100 XP"
     - Font Size: 20

#### D. Add RewardUI Component
1. Create empty GameObject "RewardUI" under Canvas
2. Add **RewardUI** component
3. Assign UI elements:
   - Gold Text → GoldText
   - Gold Icon → GoldIcon
   - XP Text → XPText
   - Level Text → LevelText
   - XP Bar Fill → XPBarFill

#### E. Set UI Targets in RewardVisualSystem
1. Select RewardVisualSystem GameObject
2. Assign UI Targets:
   - Gold UI Target → GoldIcon transform
   - XP UI Target → XPBarFill transform

### Step 5: Update Castle Data (Optional)
1. Select your CastleData ScriptableObject
2. Set **Max Health** field (this is used for level progression calculations)

## How It Works

### Reward Drop Flow
```
Enemy Dies (currentHealth <= 0)
    ↓
DamageSystem creates DropRewardGA
    ↓
RewardVisualSystem spawns:
    - Floating text at enemy position
    - Gold coins that drop and fly to UI
    - XP orbs that drop and fly to UI
    ↓
After animations complete:
    - GainGoldGA added (updates gold)
    - GainXpGA added (updates XP)
    ↓
If XP >= required:
    - LevelUpGA triggered
    - Castle gains health
    - XP bar resets
    - Level increases
```

### Level Progression
- **Level 2**: 100 XP
- **Level 3**: 150 XP (100 × 1.5)
- **Level 4**: 225 XP (150 × 1.5)
- Each level: Castle gains +10 HP (configurable)

## Customization Options

### Spawn Behavior (NEW)
In **RewardVisualSystem**:
- `goldPerCoin`: How much gold each visual coin represents (default: 20)
  - Lower = more coins spawn (more visual clutter, worse performance)
  - Higher = fewer coins spawn (cleaner, better performance)
- `xpPerOrb`: How much XP each visual orb represents (default: 10)
- `maxCoinsPerDrop`: Maximum number of coins that can spawn at once (default: 3)
- `maxOrbsPerDrop`: Maximum number of orbs that can spawn at once (default: 3)

**Spawn Formula:**
```
coins = Clamp((goldAmount / goldPerCoin) + 1, 1, maxCoinsPerDrop)
orbs = Clamp((xpAmount / xpPerOrb) + 1, 1, maxOrbsPerDrop)
```
This ensures at least 1 visual object always spawns, regardless of amount.

### Object Pooling (NEW)
In **RewardVisualSystem**:
- `initialPoolSize`: Number of coins and orbs pre-instantiated at start (default: 20)
  - Set higher if you have many enemies dying simultaneously
  - Pool automatically expands if needed, but pre-allocation improves performance

### Visual Feedback
In **RewardVisualSystem**:
- `dropDelay`: Time between coin/orb spawns (default: 0.1s)
- `dropDuration`: How long drop animation takes (default: 0.8s)
- `dropHeight`: How high items jump (default: 2 units)
- `collectDuration`: Speed of UI collection animation (default: 0.5s)
- `collectEase`: Animation curve for collection

### Level Progression
In **RewardSystem**:
- `baseXpRequired`: XP for level 2 (default: 100)
- `xpScalingFactor`: Growth rate (default: 1.5)
- `healthPerLevel`: HP gained per level (default: 10)

### Floating Text
In **FloatingRewardText**:
- `floatDuration`: How long text stays visible (default: 1.5s)
- `floatHeight`: How high text floats (default: 1.5 units)

## Testing
1. Play the game and kill an enemy
2. You should see:
   - Floating "+Gold +XP" text
   - Coins/orbs dropping and flying to UI
   - Gold counter updating
   - XP bar filling
3. Kill enough enemies to reach 100 XP
4. Level up should trigger:
   - Level text updates
   - Castle HP increases
   - XP bar resets

## Future Shop Integration
The `RewardSystem.SpendGold(int amount)` method is ready for shop use:
```csharp
if (RewardSystem.Instance.SpendGold(50))
{
    // Purchase successful
    // Give player the item
}
else
{
    // Not enough gold
}
```

## Troubleshooting

**Rewards not dropping:**
- Check that DamageSystem is in the scene
- Verify EnemyData has GoldDrop/XpDrop values set
- Check Console for errors

**Visual effects not showing:**
- Ensure RewardVisualSystem has prefabs assigned
- Check that UI targets are set correctly
- Verify Camera.main is working

**UI not updating:**
- Check that RewardUI component is active
- Verify all UI elements are assigned
- Check that events are subscribing in OnEnable

**Level up not working:**
- Verify XP is accumulating (check Debug.Log in console)
- Check XpRequiredForNextLevel value
- Ensure CastleSystem.Instance.CastleView is not null
