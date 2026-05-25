# Girlfriend Warrior - Game Design Document

**Target Platform:** VIA Arcade Cabinet (Joystick + Face Buttons) & PC Computer (Keyboard Only)  

---

### 1. Summary & Story

* **Core Concept:** *Girlfriend Warrior* is a fast-paced, top-down 2D arcade game. It combines time-management and base-defense rules to make a high-tension match where you try to survive a single wave of enemies.   
* **Target Audience:** Casual arcade players and students who want a quick, exciting game to play on a real arcade machine.  
* **Project Scope:** Made specifically for an arcade cabinet but fully playable on a computer using only a keyboard with zero mouse input required. There are no save files, no setting menus, and no mouse menus. Everything is kept simple so players can start playing immediately.

<p align="center">
  <img width="297" height="52" alt="image" src="https://github.com/user-attachments/assets/8e1ba0ec-6015-44c6-bcce-4428fb3c9715" /><br>
  Figure 1: Playable character - Girlfriend Warrior
</p>

* **The Story:** The story is silly and fun. The Octopus King thinks your boyfriend is super cute and wants him for himself! He has sent his whole sea-monster army to kidnap him.  
* **The Mission:** Your boyfriend is hiding behind an unbuilt fortress wall. Playing as the brave Girlfriend, you must quickly collect resources, build up a safe wall, and hire automated warriors to help protect him before time runs out.  
* **Level Design:** The game features multiple connected map sections, including a central garden field at the bottom and an upper shop layout. The player can walk through gates to cross between these different map zones during gameplay.

---

### 2. Match Structure & Gameplay Flow

The game has a countdown timer that splits the match into two back-to-back phases:

#### **Phase 1: Preparation (04:00 – 00:00)**

* **Safety:** The arena zones are completely safe and there are no enemies.  
* **Objective:** The player runs around the garden area to collect flowers that pop up on the grass.  
* **Action:** The player walks across map zones to use the trading booths and set up their defenses before the timer hits zero.  
* **Audio:** Calm, peaceful background music plays during this time.

#### **Phase 2: The Fight (Timer Hits 00:00)**

* **The Change:** The calm music stops instantly, and an intense battle music track starts playing.  
* **The Wave:** An enemy spawner turns on and releases a massive wave of octopus monsters in the direction of the garden.  
* **Active Trading:** The booths stay open during the attack, so you can still travel through the gates and spend your leftover resources while fighting.

---

### 3. Trading Options 

Trading is built directly into the map layout. There are no shopping menus or inventory screens. Instead, players just walk their character straight into the trading zones, and the trade happens automatically. This makes trading very easy with an arcade joystick or keyboard arrow keys. 

| Resource / Booth | Cost / Transaction | Action & Feedback |
| :--- | :--- | :--- |
| **Flowers** | Gathered from the ground | Walk over them to pick them up. This is your main currency. Picking them up plays a light grass-rustling sound. |
| **Meat** | Bought with Flowers | Secondary currency. Used exclusively to recruit defenders. |
| **Booth 1: Wall Upgrade/Repair** | 5 Flowers | **Healthy Wall:** Upgrades appearance across 5 visual tiers and expands max health. <br>**Damaged Wall:** Acts as a repair tool, instantly fully healing missing hearts. |
| **Booth 2: Meat Exchange** | 8 Flowers - Meat | Standard currency trade. Plays a short transaction sound effect. |
| **Booth 3: Warrior Recruitment** | 4 Meat - Warrior | Instantly summons an automated defender at a fixed spawn point. |

---

### 4. Character AI & Combat Rules

To make the game run smoothly on arcade hardware, characters do not push each other using physics. Instead, friendly defenders and enemies fight based on simple distance math.

* **Friendly Hired Defenders:** The warriors you hire are completely invincible. They do not have health bars and cannot die. They act like moving defense towers. They scan the area for objects labeled "Monster," run toward the closest one, stop exactly when they get close, and attack every 1.2 seconds. When there are no enemies, they walk around in a small circle to keep the map looking active.

<p align="center">
  <img width="101" height="86" alt="image" src="https://github.com/user-attachments/assets/b7cc3e3a-7a59-406c-beca-b6475f3be345" /><br>
  <i>Figure 2: Friendly Hired Defender</i>
</p>

* **Octopus Monsters:** The octopus enemies only care about destroying the wall. They completely ignore your friendly warriors and cannot be distracted. When they spawn, they pick a random spot along the wall and march straight down toward it. Once they touch the wall, they stop, play an attack animation, and deal damage directly to your health hearts. They have a floating health bar above their heads that gets shorter as they take damage.

<p align="center">
  <img width="116" height="99" alt="image" src="https://github.com/user-attachments/assets/5a589e75-6ab3-49b4-b111-e5881adb8a39" /><br>
  <i>Figure 3: Octopus Monster</i>
</p>

---

### 5. Fortress Wall Health

* **The Heart HUD:** Your base health is tracked on the left side of the screen using up to 5 heart symbols. Each heart holds 8 health points (up to 40 HP total capacity). Hearts visually break away one by one as monsters attack the wall.

<p align="center">
  <img width="381" height="51" alt="image" src="https://github.com/user-attachments/assets/a33c8807-6063-4c20-81a3-a0320e285d93" /><br>
  <i>Figure 4: Wall Health that updates due to an upgrade or an attack of octopuses</i>
</p>


---

### 6. Win & Loss Conditions

* **Victory (How to Win):** You win the game when the enemy spawner is empty, the wall isn’t destroyed AND every single octopus monster on the screen has been destroyed by your warriors. Winning triggers a full-screen panel with a big green victory card.  
* **Defeat (How to Lose):** You lose instantly if the wall's health drops to zero, because the monsters break through and kidnap your boyfriend. This happens if you leave the wall unbuilt at Level 0, or if you do not hire enough warriors to stop the monsters from hitting the wall. Defeat freezes the game and shows a big red defeat card.


# 🎯 Project Milestones - Girlfriend Warrior

Here are the three development milestones that will be documented in the upcoming project blog posts. These milestones break the work down into clear, manageable steps, moving from basic layout to final combat mechanics.

---

### 📍 Milestone 1: Map Layout & Camera
*This milestone focuses on setting up the basic world and getting the main character moving cleanly around the screen.*

* **Creating the Map Zones:** Design and paint the 2D tiles for the multiple connected areas, making sure the lower garden zone and the upper shop zone link together correctly.
* **Keyboard:** Program the player character to move in 8 directions using either a PC keyboard or an arcade joystick without ever needing a mouse menu.
* **Camera Boundaries:** Set up the 2D camera system to follow the player, but the background void stays hidden.

---

### 📍 Milestone 2: Trading Booths & Game Time
*This milestone focuses on creating the resource collection system, the automatic trading options, and the countdown clock.*

* **Flower Collection:** Program resource flowers to spawn naturally on the grass and trigger a light rustling sound effect.
* **Walk-In Trading Booths:** Build the three physical trigger zones on the map (Wall Upgrades, Meat Exchange, and Warrior Recruitment) so trades happen automatically when stepping inside.
* **The Phase Timer:** Create the master countdown clock that ticks down from 04:00 to 00:00, switching the game and the background music automatically from preparation mode to fight mode.

---

### 📍 Milestone 3: Monster Wave, Defender AI, & End Screens
*This final milestone adds the actual sea-monster threats, automated defender combat logic, and match results screens.*

* **Octopus Monster AI:** Program the octopus enemies to spawn, ignore friendly units, march straight down to a random spot on the wall, and deal damage to your health hearts.
* **Invincible Warrior AI:** Program hired defenders to scan for nearby monsters, run toward them automatically, stand their ground, and strike while staying safe from damage.
* **Win/Loss System:** Connect the wall's health and enemy count to full-screen green Victory and red Defeat cards that freeze gameplay and let players navigate options with a joystick or keyboard.
