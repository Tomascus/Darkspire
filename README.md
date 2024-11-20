# Darkspire 
This is our 3D-Game Engine project for our Group.** Squirrel Antics**. <br />The roles within the project go as following <br /><br />
**Tomascus:** Leader, Combat Programmer, User Testing<br />
**Smartisek:** UI Programmer, Git Master<br />
**xMaFey:** 3D Modeller, Level Design<br />
**DovydasJakuciunas:** UX Hightligher, Sound Design<br />

## About
Test your skills with engaging and realistic combat, navigate dangerous corridors and rooms of the eerie dungeon and prepare to fight unimaginable enemies that lurk inside the Darkspire. 

Darkspire is a 3D Top down ARPG situated in a desolate world. The main theme of the game is dark fantasy, with creepy atmosphere and melancholic sounds. Game uses low-poly stylized textures and assets to help bring out immersive NPC designs and environmental structures. The player is tasked with venturing inside of a dungeon called "Darkspire" and seek a mysterious artifact. During the playthrough, player is exposed to enemies and puzzles inside of the dungeon, where the only way to overcome them is determination and patience.  

![image alt](https://github.com/Tomascus/Darkspire/blob/20c20a19ad9c19a1e033e0815369e760cb7f296d/DarkSpireMainMenu.jpg)

## Game Mechanics & Systems
<h3>Combat</h3> </br>
To make the player feel engaged and satisfied with the combat, we tried to implement the combat by balancing risk/reward. The player has to stop moving once he starts attacking, giving the enemy a chance to come in closer within its attack distance. This way the player has to strategize on when to attack based on the environment around him, whether those are obstacles or enemies. Attacking consumes stamina of the player so it is essential to regenerate stamina before fights and manage attack with care to avoid stamina burndown.

Furthermore, the player has what could be best described as a combo system, where different animations trigger for attacks to improve variety and fulfilment of weapon slashes. This alongside accurate weapon hitboxes ensure the player has complete precision over the attacks as well as hit registration on the enemies. While attacking the player can smoothly change the direction of the attack by moving the mouse, enhancing the still attacks to be more impactful.

Enemies use variety of attacks and have different attack ranges depending on the enemy attributes. When in attack range, the enemy stops and starts to attack the player. The player is stunned for a short while when attacked, making it punishing to get hit when surrounded by many enemies.

<h3>Movement</h3> </br>
We wanted to make the players movement feel as fluid as possible, while making it accurate. Running interacts with stamina, draining it, which makes it impossible to run from enemies for a long period of time. Dodging in our game is a means of implementing a quick reaction response to enemy attack, making it possible to avoid them by rolling in a certain direction. During the dodge the player is invulnerable, unable to be hit.

Movement works interchangeably with controller and mouse & keyboard. We wanted to make player moving around and rotating smooth so we used dampening and various math calculations to make it more precise. Hitboxes for enemies and players are accurate, meaning every telegraphed attack that collides with the enemy/player should hit.

<h3>Player Stats</h3> </br>
The life of the player depends on the health bar on top right of the screen. This health is static, meaning there is no way to regenerate it naturally, only with a means of item use. Additionally the player has a stamina bar below the health bar, showcasing how many heavy actions are possible to perform. When out of stamina, the player is unable to run, attack and dodge, so manage it carefully! 

<h3>Enemy AI</h3> </br>
Each enemy type has unique attributes. Some more heavy built enemies have bigger health pool, but are slower, while smaller are faster, but easier to kill. Other enemy attributes include detection distance, making some enemies more aware than others and attack range, which depends on the weapon they are holding.

The enemy attacks only while in range. Each enemy hit on the player damages by the value associated with them. Some heavy weapons like hammers would give substantially more damage to the player than a knife would.

Most prominent feature of the AI in our game is the navigation. The enemies patrol areas they are assigned to randomly, making their movement unpredictable. This works against the player, making it harder to sneak up against enemies and kill them before they are noticed. When the enemy leaves the patrol area to chase the player and loses a sight of them, it returns back to the area it patrols. 

<h3>NPC's & Dialogues</h3> </br>
To make our game more story rich and personalized, we introduced a dialogue system, along with NPC's you can talk to. The dialogue is modular, meaning the player has multiple choices on how to answer, giving more freedom on what to say. This helps multiple playthroughs, allowing players to re-experience the story again with different choices, learning what would happen otherwise.

<h3>Inventory & Items</h3> </br>
Players have inventory where they store items, the items are acquired from certain enemies or chests hidden around the dungeon. The items are scriptable objects, meaning the items can be interchanged easily and have different effects. 

The items include health potions, which are the only way to recover HP. Lore books and scrolls are another item type that hold narrative elements inside of them, allowing to learn more about the world and the dungeon.

<h3>Leveling system</h3> </br>
During the playthrough of the level, the player will gain experience (XP) by killing enemies. When players accumulate enough experience (XP bar fills up) they level up and have the ability to increase player stats, such as damage, HP and stamina. This system makes it easier to fight as the game goes on, helping the player prepare for the boss fight that could be very dangerous when started early on. This incentivizes players to explore the dungeon and get stronger, or on other hand challenge players. Hardcore players could focus on going to the boss early on, not being leveled up, testing their skills.

<h3>Dungeon</h3> </br>
The dungeon provides many secrets, whether it is by mysterious structures found inside that have lore descriptions or other scattered objects. The environmental storytelling is an important aspect to our game, where the positions of items and other assets tell a story about what had happened. The player encounters rooms with puzzles, some with many enemies and other hidden secret rooms.

## Lore

