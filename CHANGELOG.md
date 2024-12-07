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

- **Frontline Haniwa Create**: Increased cost from 0 -> W
- **Haniwa Blitz**: Now also discards selected cards
- **Exile Summon**: Upgrade now keeps exile. Instead it allows choosing 2 Frontline cards
- **Ashes to Clay**: Increased cost from 0 -> 1W, due to Frontlines now easily exiled, ressurection is now more powerful
- **Exile Upgrade**: Reworked into **Discard Upgrade**. Now discards the selected cards instead of exiling them
  - No longer upgrades the selected cards also
- **Upgraded Summon**: Summoned unit's upgrade changed from 3(7) -> 5
  - Can now choose 2 Frontlines when upgraded

- **Mass Ressurection**: Reworked into **Mass Destruction**. Now exiles all Frontline cards in the hand/draw/discard and deals extra damage per card exiled
  - Damage increased from 20 + 3(5) per card -> 25(35) + 4(6) per card
- **Exile Frontline Blitz**: Random targetting is affected by Commander's Mark debuff from Haniwa Commander
- **Ultra Frontline**: Reworked into an X-Cost
  - Now summons 1 Frontline card into the hand. It gains 3(5) upgrades per mana spent

#### Assign cards

- **Assign Haniwa Create**: Increased cost from 0 -> R
  - Card counter gain increased from 1 -> 3
- **Cavalry Scout**: Now gives Scry instead of Graze, 2(3) scry per trigger

- **Cavalry Rush**: Mana gain is now not affect by extra triggers
- **Cavalry Supplies**: Extra triggers now only affect the red mana portion, effectively reducing mana gain to 1 per trigger
  - Card counter increased from 6(3) -> 8(5), and Cavalry requirement increased from 2 -> 3
- **Fencer Prep Counter**: Now gives Reflection instead of Block/Barrier to differenciate from Fencer Build Barricade
- **Choose Short Assign**: Now affected by abilities that require playing an Assign card (it actually worked with Assign Play Gain Block but it was hardcoded, now it's affected by all abilities)
- **Build Watchtower**: Watchtower gain via extra triggers is halved, eg. Gain Watchtower 10 with 2 triggers = 15
  - Can now play the card again to gain extra triggers
- **Garrison Archer**: Archer sacrifice increased from 1 -> 2
  - Watchtower gain increased from 2(3) -> 4(6)

- **Assign Cost Trigger**: Now only increases cost of Assign cards while they are in the hand
  - Now also increases cost by 1 for each level of the buff
  - Upgrades cost reduced from 1RR -> 1R
- **Assign Play Draw**: Now has Ethereal
  - No longer reduces cost on upgrade, instead it loses Ethereal

#### Hybrid cards

- **Create Cavalry**: Now increases scry from 3 -> 5 on upgrade
- **Assign Trigger Upgrade**: Now also upgrades 2 random cards in hand when played

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