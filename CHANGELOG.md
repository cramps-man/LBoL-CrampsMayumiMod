## 0.4.0

### Overview of changes

Slight rework of Assign buffs to enable the buildup/stacking playstyle. Those stacking effects felt mostly as an extra to the quicker playstyle of setting off many Assign buffs. Now they should allow both playstyles.  
Completed the offcolor card pool.  
Some general card/balance changes as well.  

### General changes

- Frontline and Assign has been swapped on the character select screen (keeping the current colours), Frontline is now A and Assign is now B
- Blank card now allows selecting offcolor options when summoning or using **Choose Short Assign**

#### Assign

- Triggers have been reworked into Task Level, now no longer increases number of times an Assign buff activates
- Instead Task Level is used to determine the strength of Assign buffs, differing for each type of buff
- All Assign buffs have their own starting Task Levels
- When stacking the same type of Assign buff, add the starting Task Level onto the current amount, and increase the counter by 3 instead of choosing the lowest count
- Also accumulates the number of Assigned Haniwa onto the buff, only needing 1 Haniwa to stack the same type of buff. Thus all Haniwa are returned to your pool even when stacked
- Task Level increases per card play, based on half the total number of Assigned Haniwa rounded up
- Accumilating Haniwa onto an Assign buff will now have big benefits if you can keep stacking them, before finally letting them have an explosive finish

#### Frontline

- Loyalty gain when gaining Haniwa has been reduced to half the total Haniwa gained rounded up, instead of being 1 to 1
- All passive effects that deal damage are now Reaction type, thus no longer scale from firepower but bypass graze
- Commanders can now be selected for blitz type actions that execute the On Play action (I originally didn't allow this due to a bug, but now it's fixed I might as well try it out)

### Quality of life

- Update texts that mention a color mana to instead use the mana symbol
- Scry interactions with **Haniwa Spy** and **Cavalry Scout** now show in the title it is Scry and how much draw
- Assign cards now update their display for Haniwa requirements when a buff of the same type is already active (only requires 1 to stack)
- Frontline selection cards now have colour and proper rarity, and hidden types now show at the start of the list so it is more visible when they are available
- Haniwa Exploiter On Play damage now updates when hovering over an enemy

### Card changes

#### Frontline cards

##### Frontlines

- **Haniwa Bodyguard**: Now has scaling on start of turn Loyalty replenish, +1 per 4 upgrades
- **Haniwa Upgrader**: Removed scaling on additional upgraded cards block, now always grants 1 block per upgraded card
- **Haniwa Commander**: Renamed to **Frontline Commander**
  - No longer has Replenish
  - Passive Loyalty cost increased from 3 -> 5
  - Now gains 2 Loyalty at end of turn

##### Uncommon

- **Loyalty Strike**: Lower base damage from 20(26) -> 18
  - The lowered mana cost from last update already makes the upgrade worth it
- **Frontline Copy**: Now copies over the current Loyalty instead of letting it start at default

##### Rare

- **Mass Summon**: Now sacrifices all Haniwa instead of 10(15)
  - Now also takes into account summon cost when randomly selecting Frontlines
  - eg. sacrificing 5 Archer could spawn 5 Sharpshooter or 1 Sharpshooter + 2 Exploiter etc.
- **Summon Commander**: Now allows choosing between **Frontline Commander** and **Assign Commander** (details on new cards below)
- **Ultra Frontline**: Add a base value for upgrades 9(18)
  - Scaling per mana reduced from 3(5) -> 3

#### Assign cards

##### Common

- **Archer Prep Volley**: Now deals 5 damage X times to random enemies, where X is Task Level / 5
  - Starts with 10(17) Task Level
- **Fencer Build Barricade**: Now gains 1 barrier per 2 Task Level
  - Starts with 13(22) Task Level
- **Cavalry Scout**: Now Scries 1 per 4 Task Level and draws 1 card per 7 Task Level (minimum of 1 card)
  - Starts with 6(13) Task Level
- **Assign Fast Trigger**: Now also converts remaining count into Task Level before triggering
- **Assign Extra Trigger**: Now **Assign Extra Time**, increases an Assigns counter by 3(5) instead of granting extra triggers
- **Zero Cost Move**: Has been moved to Uncommon

##### Uncommon

- **Cavalry Rush**: Now deals damage equal to Task Level, and gains 1 red mana per 15 Task Level
  - Starts with 9(16) Task Level
- **Archer Prep Debuff**: Now deals damage equal to Task Level, and applies 1 vulnerable per 12(10) Task Level
  - Starts with 6(12) Task Level
- **Fencer Prep Counter**: Now deals damage equal to Task Level. Gains 4(5) Task Level per hit
  - Starts with 10(16) Task Level
  - Now attacks a random enemy instead of not attacking, if an enemy hasn't attacked
- **Build Watchtower**: Now gains 1 Watchtower per 5 Task Level
  - Starts with 15(40) Task Level
- **Garrison Archer**: Now grants different amounts of Watchtower depending on Archer level, and sacrifices the higher amount
  - 0 Archers 1 Watchtower, 1 Archer 2(3) Watchtower, 2 Archers 4(6) Watchtower
  - Thus this card no longer bricks if you don't have any Archers
- **Charge Attack**: Now deals 15(20) damage per 15 Task Level. Gains 15 Task Level per Assign buff trigger
  - No longer gains damage when an Assign buff triggers
  - Starts with 15(25) Task Level
- **Cavalry Supplies**: Reworked effect, no longer grants RW(RP) mana
  - Now allows choosing X out of X+2(4) cards to add to the draw pile, where X is Task Level / 25
  - Each choice is randomised between Flame, Radiance or random Frontline
- **Choose Short Assign**: Now grants 2(5) extra Task Level to the chosen buff

##### Rare

- **Assign Cost Trigger**: Has been moved to a green off colour
  - Increased cost stays at 1 no matter the level of the buff
  - Now grants 3 bonus Task Level per card play instead of extra triggers
  - Also refunds mana spent on Assign cards at the start of next turn
- **Assign Reverse Tickdown**: Cost updated from RRR(R) -> RR1(R)
- **Mass Extra Trigger**: Now **Mass Task Level**, grants 30 Task Level instead of extra triggers
- **Gain Buff Tickdown**: Now grants temporary firepower at start of next turn for every 3 Assign buffs triggered

#### Hybrid cards

##### Uncommon

- **Exile Frontline Extra Trigger**: Reworked into an ability **Frontline Loyalty Task Level**
  - Whenever a Frontline's Loyalty is consumed, all Assign buffs gain Task Level equal to consumed Loyalty

#### Off colors

- **Archer Prep Frost Arrow**: Now deals X hits per 9 Task Level
  - Starts with 9(18) Task Level
- **Permanent Assign**: Permanent Assign buffs now stack normally with the new changes
  - After triggering, they reset their counter to 5, and Task Level/Assigned Haniwa to their starting values
- **Assign Duplicate Trigger**: Has been moved to Assign rare colour, no longer triggers an existing Assign buff without removing it
  - Now grants 1(2) levels of Assign Double Trigger, which activates the next Assign buff twice

### Removed cards

- **Exile Summon**: A new summon card was added to commons which was kinda similar. The free haniwa cost was also probably too good for 2 free summons
- **Assign Summon Frontline**: The effect is kinda redundant with cards like **Assign Trigger Summon** and the new **Cavalry Supplies** rework. 

### New cards

- **Fast Summon**: Yellow Common, Choose 1 Frontline card to summon while sacrificing Haniwa, has inate and upgrade gives replenish and 0 cost
- **Blitz Summon**: Yellow Uncommon, Choose 1 Frontline card to summon while sacrificing Haniwa and give it extra Loyalty. Execute its On Play action and consume Loyalty
- **Loyalty Protection**: Yellow Uncommon, Gain levels of Loyalty Protection which prevent Loyalty consumption, and grant block equal to reduced consumption
- **Haniwa Charger**: Yellow Uncommon Frontline, A Frontline that works well with more of the same type in hand, gaining more Loyalty and dealing more damage based on Loyalty
- **Haniwa Sentinel**: Yellow Uncommon Frontline, A supportive type that applies weak to all attacking enemies at end of turn, and can lower duration/level of incoming debuffs
- **Assign Commander**: Yellow Rare Frontline, A commander type for Assigns, able to redirect Assign buff triggers into new tasks automatically and duplicate Assign buffs
- **Haniwa Assassin**: Offcolor Purple Frontline, A follow up type of attacker which can then finish of a target who is below a certain life threshold
- **Mud Extraction**: Offcolor Blue Uncommon, Exile or consume Loyalty of a Frontline, and add a Splash to the hand
- **Frontline Loyalty Gain**: Offcolor Green Uncommon, All Frontlines in hand/draw/discard piles gain Loyalty
- **Chain of Command**: Offcolor Green Uncommon, Allows commander frontline types to be selectable for summons and spawn from random spawns
- **Assign Separation**: Offcolor Purple Uncommon, Assign buffs will no longer stack and create a separate buff instead
- **Darkness Summon**: Offcolor Purple Rare, Summons 1(2) Frontline cards which have infinite Loyalty and don't leave the hand when played. They cost Power to use

### Fixes

- Fix **Haniwa Horse Archer** not being uncommon
- Add missing cost change to 0.3.0 changelog for **Loyalty Strike**
- Fix **Discard Upgrade** having the delay of upgrading even when no cards were selected
- Fix effects that spawn random frontlines to have the chance to spawn offcolour ones

## 0.3.0

### Overview of changes

Updating mod to the 1.6 beta version. Will not be compatible with main branch 1.5.1 anymore.  
Also comes with balance changes to Frontline as they felt less fun to play in last update. This new version should be closer to the initial release for Frontlines, but more balanced.  
General card adjustments as well. 

### General changes

#### Frontline

- Frontline cards no longer have Exile, and no longer gains 1 cost at 0 unity
- The unity counter on the left of the card has been given a keyword "Loyalty"
- Loyalty is used for both passive and On Play effects, with varying costs
- If a Frontline card goes below 0 Loyalty, it is Exiled
- Loyalty is still increased via upgrading, but now also increased if Haniwa of the same type was gained, and the card is in the hand

#### Haniwa

- Haniwa buffs no longer get removed when they reach 0
- Mostly to prevent too good synergy with Reimu's buff loss cards and for better visual clarity

### Card changes

#### Frontline cards

##### Frontlines

- **Haniwa Attacker**: Loyalty costs - Passive 2, On Play 3, Starts with 5
  - On Play damage increased from 9 -> 10
- **Haniwa Bodyguard**: Loyalty costs - Passive = damage reduced, On Play 5, Starts with 5
  - If Loyalty is below 4 and this card is in the hand at start of turn, Loyalty becomes 4
- **Haniwa Sharpshooter**: Loyalty costs - Passive 3, On Play 1, Starts with 5
- **Haniwa Support**: Loyalty costs - Passive 3, On Play 5, Starts with 4
- **Haniwa Upgrader**: Loyalty costs - Passive 2, On Play 5, Starts with 4
  - Passive no longer has restriction on upgrading other upgraders or itself
  - Passive reworked to now upgrade 2 random cards at end of turn, prioritising other cards first
  - On Play added new effect of upgrading all cards in the hand alongside existing block gain
- **Haniwa Exploiter**: Loyalty costs - Passive 2, On Play 3, Starts with 4
  - Passive reworked to now deal 10 damage to all enemies with debuffs
  - On Play damage reduced from 15 -> 12 and upgrade scaling reduced from 2 -> 1
  - Debuff damage reduced from 5 -> 3 but now scales with upgrades at +1 per 5 upgrades
- **Haniwa Spy**: Loyalty costs - Passive 2, On Play 5, Starts with 4
  - Draw/Discard pile effect now also requires Loyalty
  - On Play Scry increased from 1 -> 3 but now scales +1 per 3 upgrades instead of 1 to 1
- **Haniwa Monk**: Loyalty costs - Passive 1, On Play 4, Starts with 6
- **Haniwa Commander**: Loyalty costs - Passive 3, On Play 0, Starts with 3
  - Does not gain Loyalty via Haniwa gain
- **Frozen Haniwa**: Loyalty costs - Passive 3, On Play 2, Starts with 3
  - Now shows name of original card in description

##### Common

- **Frontline Haniwa Create**: Cost updated to be W(1)
  - Uncommon and Rare haniwa gains reduced by 1. The gain was a bit much especially since it gains all 3 types
- **Summon Haniwa**: Now requires selecting 2 different Frontline cards instead of creating 2 copies of 1
  - This effectively increases the Haniwa cost. It's quite effecient costwise but didn't want to nerf the mana
- **Haniwa Blitz**: No longer discards selected cards
  - Selected cards consume Loyalty instead
- **Frontline Upgrade**: Now always allows selecting 2 cards
  - Upgrade now increases additional upgrades 1 -> 2

##### Uncommon

- **Discard Upgrade**: Upgrade effect reworked, no longer upgrades cards in the hand
  - Now upgrades the selected cards 5 times
- **Loyalty Strike**: Reworked bonus damage, no longer increased for each Frontline card in hand
  - Now increases by 1 for total Loyalty of all Frontline cards in hand
  - Damage reduced from 22(28) -> 20(26)
  - Cost reduced from WW1(W2) -> WW1(W1)
- **Ashes To Clay**: Now moves from Exile to draw pile randomly, instead of the top
- **Heal Sacrifice**: Reduced healing from 5(7) -> 4(6)

##### Rare

- **Mass Summon**: Reworked effect, no longer adds Frontlines to hand until full nor to draw/discard pile
  - Now consumes up to 10(15) Haniwa
  - For each Haniwa consumed, add 1 Frontline card to the draw pile
  - The Frontline card created corresponds to one of the Frontlines of the same type eg. If Archer was consumed, only Sharpshooter or Exploiter will be created

#### Assign cards

##### Common

- **Assign Haniwa Create**: Cost updated to be R(1)
  - Bonus Haniwa gain increased from 2(3) -> 2(4)
- **Fencer Build Barricade**: Fencer requirement reduced from 2 -> 1
  - Barrier gain reduced from 10(15) -> 8(12)

##### Uncommon

- **Assign Tickdown Return**: Upgrade changed, no longer increases tickdown from 2 -> 3
  - Instead gains draw 1 effect
- **Fencer Prep Counter**: Effect reworked, no longer gives reflect based on current block/barrier
  - Now after 9 cards are played, deals 5(7) damage to all enemies that have attacked the player
  - Damage is multiplied by number of triggers
  - Number of triggers increases by 1 for each hit from an enemy
  - At end of turn, gain 2(3) block for each incoming enemy hit
- **Archer Prep Debuff**: Damage reduced from 13(18) -> 12(15)
- **Cavalry Rush**: Damage reduced from 15(19) -> 13(17)

#### Hybrid cards

##### Uncommon

- **Create Fencer**: Has been reworked, no longer provides 3 block per Fencer level
  - Now has different effects based on Fencer level like "Create Archer" and "Create Cavalry"
  - 3: Gain 8(10) block
  - 5: Upgrade 1(2) random cards in the hand
  - 7: Gain 8(10) barrier
  - 10: Gain 4(6) static charge
- **Create Cavalry**: Level 10 effect no longer grants RW(PP) mana
  - Instead it now Assigns 1 instance of **Cavalry Rush** or **Cavalry Rush+** on upgrade
- **Assign Trigger Upgrade**: On Play now upgrades all cards in the hand instead of 1(2)
- **Exile Frontline Extra Trigger**: Reduced cost from hybrid + 1 -> hybrid
  - Now has Retain

#### Off colors

- **Assign Duplicate Trigger**: Effect slightly reworked, no longer triggers an Assign buff and all its triggers
  - Now only triggers 1(2) triggers for the selected buff
  - No longer Exiles
  - Cost updated from RRG(hybrid) -> RG(1 + hybrid)
- **Archer Prep Frost Arrow**: Damage and block increased from 6(9) -> 9
  - Increased starting card counter from 6(4) -> 9
  - Upgrade now starts with 2 triggers
- **Permanent Assign**: Life loss increased from 1 -> number of required Haniwa for the Assign buff

### Removed cards

- **Strength In Numbers**: This card was just hard to balance without changing its effect entirely
- **Sturdy Frontline**: This card is basically redundant with the new Loyalty changes
  - I could have made it not Exile if you overspend Loyalty, but that would be hard to portray perhaps. Not worth a card for that
- **Frontline Discard Draw**: An off-color that works similarly to **Discard Upgrade**, except with some extra draw. Didn't think it was worth having a card for

### New cards

- **Max Haniwa Up**: Hybrid Rare, Increases max Haniwa level by 3(5) and immidiately gain 3(5) of all Haniwa
- **Haniwa Horse Archer**: Off-color Green Frontline, requires green in the mana base to be selectable for summons
  - Passive deals follow up damage when player grazes, On Play deals accurate damage and gains 1 graze

### Fixes

- Add missing 0.2.0 change for Cavalry Supplies increasing starting card counter and cavalry requirement
- Fixed broken spell card animations for cards like Reimu's extra turn card
- Fixed Commander's Mark debuff getting removed if reapplying to the same target
- Fixed **Auto Create Reserves** not adding a **Create Haniwa** on play

## 0.2.1

Fix readme stuff not getting updated

## 0.2.0

### Overview of changes

This version is mostly about some rebalancing to make both playstyles not as "easy" or "braindead". 
Frontline has a few tweaks on how they work overall so they aren't too free to play. 
Assign mostly was card adjustments and a slight rework of the exhibit to make card draw less free. 
Some additional cards added to the pool as well. 

### General changes

#### Frontline

- Frontline cards by default now have Exile
- All Frontlines now have a limit on their in-hand passives, and don't refresh at start of turn
- To refresh them, the card needs to be drawn or upgraded
- If the card's limit is at 0, the card gains 1 cost until it is refreshed
- The unity widget is now used to show remaining activations

#### Assign

- The exhibit now doesn't directly give any draw, it now gives a buff "Assignment Bonus"
- Every 5 buffs gained, the exhibit gives 1 Assignment Bonus
- Assignment Bonus is the same draw effect as the old exhibit (Assign triggers during the turn, draw 1 card) but now loses 1 level when activated and removes itself when it reaches 0
- This should make card draw less frequent if you're setting off Assigns often, though this does allow premptive buff gaining to gain Assignment Bonus ahead of time as well
- Assign cards now have the "Assign" keyword, so it's clear which cards are affected by Assign Play Draw for example

#### Card colours swapped

- Frontline and Assign cards now have their colours swapped
- Frontline is now Yellow and Assigns are Red
- Just felt it was more fitting this way due to the mechanics and theming (Assign more aggressive = red, Frontlines have lots of exile and upgrading = yellow etc.)
- The only card that hasn't changed colour is "Strength in Numbers" which remains a red, and the hybrid cards

### Card changes

#### Frontline cards

##### Frontlines

- **Haniwa Attacker**: Added in-hand activation limit of 2(3)
  - On Play damage scaling reduced from +2 -> +1 per upgrade
  - On Play base damage increased from 8 -> 9
- **Haniwa Bodyguard**: Now has 4 -> 5 starting passive damage reduction
  - On Play block scaling from +2 -> +1 per upgrade
  - Now doesn't reduce damage if the player has enough block, to prevent wasted reduction
- **Haniwa Support**: In-hand activation limit doesn't scale, set to 1(2)
  - Mana gain scaling reduced from 1 mana per 5 upgrades -> 1 mana per 8 upgrades
  - Now displays extra mana gain on the card
- **Haniwa Upgrader**: Now doesn't target any other upgraders
  - In-hand activation limit now doesn't scale, set to 2(3)
  - On Play block scaling from +2(3) per remaining activation -> +1 per upgrade
  - Now gains additional block On Play for each upgraded card in the hand starting at +1, scales by +1 per 5 upgrades
- **Haniwa Exploiter**: On Play action now doesn't gain extra hits for each enemy debuff, it instead gains +5 damage per debuff
  - On Play base damage increased from 9 -> 15
  - On Play damage scaling per upgrade increased from +1 -> +2 per upgrade
- **Haniwa Spy**: Now has an in-hand effect, any cards move from draw to discard temporarily cost 1 less, up to 2(3) times
  - On Play scry no longer temporarily reduces cost of scried cards
- **Haniwa Commander**: In-hand and On Play reworked
  - In-hand: At start of turn, all Frontline cards activate their On Play effects, up to 1(2) times. Effects that target an enemy target a random enemy
  - On Play: Deal 25 damage and apply Commander's Mark debuff on the target
  - Commander's Mark directs all random Frontline effects onto the enemy with this debuff. Only 1 mark can be placed at a time
  - On Play damage scales by +2 per upgrade
- **Frozen Haniwa**: Now doesn't scale number of cold activations with upgrades, stays at 1(2)

##### Common

- **Frontline Haniwa Create**: Increased cost from 0 -> W
- **Haniwa Blitz**: Now also discards selected cards
- **Exile Summon**: Upgrade now keeps exile. Instead it allows choosing 2 Frontline cards

##### Uncommon

- **Ashes to Clay**: Increased cost from 0 -> 1W, due to Frontlines now easily exiled, ressurection is now more powerful
- **Exile Upgrade**: Reworked into **Discard Upgrade**. Now discards the selected cards instead of exiling them
  - No longer upgrades the selected cards also
- **Upgraded Summon**: Summoned unit's upgrade changed from 3(7) -> 5
  - Can now choose 2 Frontlines when upgraded

##### Rare

- **Mass Ressurection**: Reworked into **Mass Destruction**. Now exiles all Frontline cards in the hand/draw/discard and deals extra damage per card exiled
  - Damage increased from 20 + 3(5) per card -> 25(35) + 4(6) per card
- **Exile Frontline Blitz**: Random targetting is affected by Commander's Mark debuff from Haniwa Commander
- **Ultra Frontline**: Reworked into an X-Cost
  - Now summons 1 Frontline card into the hand. It gains 3(5) upgrades per mana spent

#### Assign cards

##### Common

- **Assign Haniwa Create**: Increased cost from 0 -> R
  - Card counter gain increased from 1 -> 3
- **Cavalry Scout**: Now gives Scry instead of Graze, 2(3) scry per trigger

##### Uncommon

- **Cavalry Rush**: Mana gain is now not affect by extra triggers
- **Cavalry Supplies**: Extra triggers now only affect the red mana portion, effectively reducing mana gain to 1 per trigger
  - Card counter increased from 6(3) -> 8(5), and Cavalry requirement increased from 2 -> 3
- **Fencer Prep Counter**: Now gives Reflection instead of Block/Barrier to differenciate from Fencer Build Barricade
- **Choose Short Assign**: Now affected by abilities that require playing an Assign card (it actually worked with Assign Play Gain Block but it was hardcoded, now it's affected by all abilities)
- **Build Watchtower**: Watchtower gain via extra triggers is halved, eg. Gain Watchtower 10 with 2 triggers = 15
  - Can now play the card again to gain extra triggers
- **Garrison Archer**: Archer sacrifice increased from 1 -> 2
  - Watchtower gain increased from 2(3) -> 4(6)

##### Rare

- **Assign Cost Trigger**: Now only increases cost of Assign cards while they are in the hand
  - Now also increases cost by 1 for each level of the buff
  - Upgrades cost reduced from 1RR -> 1R
- **Assign Play Draw**: Now has Ethereal
  - No longer reduces cost on upgrade, instead it loses Ethereal

#### Hybrid cards

##### Uncommon

- **Create Cavalry**: Now increases scry from 3 -> 5 on upgrade
- **Assign Trigger Upgrade**: Now also upgrades 2 random cards in hand when played

##### Rare

- **Full Frontal Assault**: Now sacrifices all Haniwa instead of 3 of each type
- **Zero Cost Reduction**: Now has a per turn limit for cost reductions, 3(5)
  - Cost on upgrade increased from 1WR to 2(2hybridWR)

#### Off colours

- **Archer Prep Frost Arrow**: Damage reduced from 15(20) -> 6(9)
  - Block reduced from 8(12) -> 6(9)
- **Keiki teammate**: Ultimate skill now moves exiled Frontline cards into the discard pile instead of exiling. 
  - Damage adjusted from 10(16) -> 8(12) per card
  - Ultimate skill loyalty cost from 9 -> 8

### New cards

- **Haniwa Monk**: Frontline, a Frontline card that increases damage when the player gains buffs
- **Frontline Copy**: Frontline, creates copies of Frontline cards that lose Retain
- **Sturdy Frontline**: Frontline, removes Exile and Replenish from chosen Frontline cards
- **Assign Reverse Tickdown**: Assign, card counters for Assign buffs increase instead of decreasing until end of turn. At end of turn gain block equal to total card counters of all Assign buffs
- **Auto Create Reserves**: Hybrid, If there isn't a **Create Haniwa** in hand at start of turn, add one
- **Handsize Block**: Hybrid, for each card in hand block, gain is increased. Cost also increases
- **Assign Duplicate Trigger**: Off-colour green, triggers an Assign buff's effects without removing it

### Fixes

- Fixed Assign buffs not ticking down in certain circumstances (if the same card that created the buff was played with the buff still active, or when an assign buff was upgraded via playing an upgraded version of the Assign card)
  - Choose Short Assign now doesn't tickdown the generated buff, text updated to the actual values 4(1) -> 3(0)
- Fixed weird issue with gaining extra Haniwa buffs at start of battle (though I haven't received any reports of this and it seemed to only occur in dev testing for me)

### What's next

On colours are basically done, just a few off-colours to fill in. 
Will most likely start working on giving cards proper naming/themes, and status effect art. 
Not sure when card art or attack animations will get done. 

## 0.1.1

### Fixes

- If you swap your exhibit as Mayumi, you will still get your starting Haniwa per battle
- Fix starting power being 30 instead of 0

### Text adjustments

- Adjust text for Permanent Assign to say "up to 2" when upgraded
- Adjusted cases where the word "hp" was used instead of "life" (Cavalry Rush and Charge Attack)
- Adjusted Cavalry Rush to be more clear for when mana can be gained

### Other

- Adjusted complexity rating for both decks to be 3

## 0.1.0

Initial version