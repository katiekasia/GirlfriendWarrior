### Building Trading Booths, Resource Loops, and the Master Clock

Welcome back to my development blog! For Milestone 2, my primary focus was on implementing the trading booths, countdown till my monster wave and Resource collection.

---

### What Was Fun: Resource Collection and Phase Changes

Programming the resource system was an absolute blast because it made the game world immediately feel interactive. I set up a system that dynamically spawns flowers across the grass during the opening minutes of the match. Walking over a flower automatically adds it to your total currency and triggers a light, responsive 2D grass-rustling sound effect. 

I also spent a lot of time testing the master game clock. The match starts with a countdown ticking down from 04:00. The very frame that the timer hits 00:00, the ambient background track cuts out completely, an intense combat track hits, and the octopus spawner kicks into overdrive. Watching the entire mood of the map shift automatically based on the music at the moment was very satisfying.

<p align="center">
  <img width="168" height="55" alt="image" src="https://github.com/user-attachments/assets/7688b64c-ee54-4d0c-b394-3f12ec08a272" />
</p>

---
### What Was Hard: Connecting Walk-In Triggers, Math, and Animations

The most frustrating challenge I encountered during this milestone was trying to get the physical walk-in trading booths to process multiple systems all at once. Specifically, I struggled with setting up a physical map space that triggers an event purely by walking into it, instantly running the resource exchange math (subtracting 8 flowers and adding 1 meat), and triggering a visual animation at the exact same time.

Because this game is optimized for an arcade cabinet layout, I didn't want any traditional mouse menus. Everything had to happen automatically behind the scenes using collision triggers. Early on, this was just a really hard concept for me to fully grasp. Getting Unity's 2D physics system to talk nicely with my backend UI scripts, while passing states over to the Animator controller to play a visual trading effect, felt like trying to spin too many plates at once. Every time I stepped into the booth with my joystick, either the math would break or the animation wouldn't trigger.

<p align="center">
 <img width="92" height="113" alt="image" src="https://github.com/user-attachments/assets/08601fe0-4270-46f5-aa85-4928124d0979" />
  <img width="101" height="115" alt="image" src="https://github.com/user-attachments/assets/ddd19bfa-da74-4b1a-bd27-fac440d14c10" />

</p>

---

### How I Fixed It

I managed to fix this problem by refusing to get overwhelmed and instead breaking the entire implementation down step by step. 

First, I isolated the physics logic to make sure the game accurately recognized when the character entered the trading zone box. Once that detection was stable, I layered on the economic math to guarantee flowers were being deducted and meat was being added properly. 

To bridge the final gap with the visuals, I ended up asking Chat how to correctly pass parameters from a trigger script over to an Animator controller component. By mapping out the flow logically and using AI as a coding assistant to clarify the animation hooks, everything finally clicked into place perfectly. Now, you can run right into the booth, watch the shop animation bounce cleanly, and see your inventory numbers update dynamically on the fly!
<p align="center">
<img width="105" height="121" alt="image" src="https://github.com/user-attachments/assets/00bda09b-3f4c-497c-85b0-60b9b76ae7f9" />
</p>
