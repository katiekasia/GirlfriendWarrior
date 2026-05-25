
### What Was Fun: Designing the Tilemaps and Sprites

I had a lot of fun creating the tilemap set and learning how to use the sprites and tilemap tools in Unity. I absolutely love doing design work in whatever field I am in, so I ended up spending the most amount of my work time right here. Selecting the assets, painting the grass, building the fences, and structuring the entire map layout felt more like art than programming, and it was by far the most enjoyable part of the milestone.

I also really like how the map transitions turned out. You can skip from one map section to another—meaning you can walk right out of the main "Garden" area and enter the upper "Shop" area, and the gameplay flows without any awkward cuts. 
<p align="center">
  <img width="540" height="324" alt="image" src="https://github.com/user-attachments/assets/38b64116-3706-4994-add3-e591669f3fae" />

</p>
---

### Figuring Out the Camera Setup

The camera system definitely wasn't that easy to set up at first. It took some trial and error to stop it from drifting off the map edges into the empty black background void. To fix it, I followed a really helpful tutorial which explained a lot to me about how to correctly track the user across the screen. 

The tutorial also showed me how to change the lens settings to make the camera viewport more "focused" on the main character. By adjusting the camera zoom properly, I managed to get a great balance where the player is clearly visible, but you can still see the environment around you. Now, when I walk through the gates from the Garden to the Shop, the camera dynamically shifts and follows me perfectly!
<p align="center">
  <img width="451" height="177" alt="image" src="https://github.com/user-attachments/assets/3a6f2bf8-6564-4e1a-983d-228ce8a4b5e8" />
</p>
---

### What Was Hard: The Layer and Collision Trap

While painting the world was incredibly fun, it led to my biggest technical headache during this milestone. Because a 2D map uses multiple layers stacked on top of each other (like background grass, walkable paths, and solid buildings), it is very easy to get confused.

I ran into several annoying problems where I accidentally put tiles on the wrong layers. For example, I would paint a piece of a roof on the ground layer, which suddenly resulted in my character walking *under* the house like a ghost instead of bumping into the wall. Other times, I accidentally attached solid collision settings directly to the ground layer, which meant my character couldn't move at all because the grass itself was blocking her. 

I quickly learned that I needed to be extremely detail-oriented here. I had to go back tile-by-tile, clean up my sorting layers, and carefully separate my walkable paths from my solid obstacles. 
<p align="center">
 <img width="541" height="293" alt="image" src="https://github.com/user-attachments/assets/835e476b-9342-46e6-8833-9b7d058e06db" />
</p>

---

### Summary
Even though managing the layers required a lot of patience, seeing my character walk around a beautifully designed world with a smart tracking camera makes all the hard work worth it! Now that the map framework is solid, I am ready to start building the trading options for Milestone 2.
