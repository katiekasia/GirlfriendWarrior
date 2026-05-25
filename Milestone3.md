
###  Combat Logic, Game Balancing, and Character Design


### Choosing My Monsters and Helpers!

Honestly, choosing the characters to have as helpers and monsters was super fun hehe! Designing the silly, high-stakes story where the Octopus King tries to kidnap your boyfriend gave me so much creative freedom. Turning those ideas into actual visual game elements like the cute but brave Girlfriend Warrior, the automated hired defenders, and the marching waves of octopuses was easily my favourite part of this section. Seeing the pixel art characters occupy the map and interact during the combat phase brought the whole arcade concept to life.

<p align="center">
  <img width="116" height="99" alt="image" src="https://github.com/user-attachments/assets/5a589e75-6ab3-49b4-b111-e5881adb8a39" /><br>
  <img width="101" height="86" alt="image" src="https://github.com/user-attachments/assets/b7cc3e3a-7a59-406c-beca-b6475f3be345" /><br>
</p>

---

### The Combat Logic and Balancing Dilemma

What was hard in this section of my project was for sure trying to think how the whole combat logic should go. It is one thing to make characters move on a map, but it's a completely different challenge to figure out the exact math behind the battles. I spent a lot of time stuck on questions like: *How much attack force should a monster or my defender have? How do I cleanly coordinate their attacks? What should the exact radius of their damage be?*

Beyond the basic code, the actual game balancing was incredibly tricky. I had to run through the game and check it a couple of times to find the absolute best ratio for the match mechanics. I had to balance how many flowers per second were spawning on the grass against the sheer volume of monsters being released by the spawner. I had to carefully test scenarios to see if a player was still able to win the game if they only managed to get a tight setup—like having only 3 helper defenders on the field—ensuring the enemy attack force wasn't so overwhelmingly strong that it felt completely impossible to protect the wall. 
<p align="center">
<img width="554" height="299" alt="image" src="https://github.com/user-attachments/assets/78589948-a093-415a-b64a-c7cccaea3982" />
</p>

---

### How I Fixed It

I solved this by doing a lot of manual playtesting and tweaking design variables step by step directly in the Unity Inspector. Instead of guessing the numbers, I ran the simulation repeatedly to watch how the economy matched the combat flow. 

* **Attack Adjustments:** I fine-tuned the damage radius and attack force values so that defenders feel impactful and responsive, while ensuring the octopus monsters hit with a balanced amount of force.
* **Fair Scaling:** I adjusted the flower spawn rates so that an average player can comfortably afford a baseline defense before the timer hits zero. Testing confirmed that even if you only have 3 defenders on the field, winning is still totally achievable if you stay alert and utilize mid-combat repairs!
