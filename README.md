# Cozy Ship: The Endless Journey
### A small "cozy" game created in a few days as part of a recruitment process to showcase that my project and code are not a spaghetti.

#### Description:
- The project was created using the MVC design pattern (Model, View, Controller) along with Dependency Injection (Zenject).
- The new Input System was implemented and mapped to ensure the game works seamlessly with both keyboard and gamepad (UI navigation has also been handled).

- The water shader was created based on this [ShaderGraph tutorial](https://youtu.be/78WCzTVmc28).  
  To simulate the ship's swaying on the water, I used DOTween (as well as for other animations).  
  If you want to create ship physics and other objects that react to waves, I recommend this tutorial: https://youtu.be/eL_zHQEju8 + a tutorial on how to create realistic waves using a shader, which can be combined with that physics: https://catlikecoding.com/unity/tutorials/flow/waves

- I implemented my own Object Pooling system to dynamically instantiate objects in the scene, and also used Zenject's built-in MemoryPool (to generate coins when the player hits an obstacle).
- 3 levels were created, and when they are finished, the player's score is saved to the leaderboard (using JSON for save/load).
- Audio settings (volume for music and effects) and font selection are saved and loaded using PlayerPrefs.
- I used my own [MeshCombiner](https://assetstore.unity.com/packages/tools/modeling/mesh-combiner-157192) asset to combine meshes in the gameplay scene, reducing the initial number of draw calls from ~100 to ~50.
- I also added the AllIn1SpriteShader asset to the project, but didn’t have a chance to use it here.
- The project was developed in **Unity 2020.3.38f1** and **Universal Render Pipeline (URP)**.

- Assets used:
  - **"Low Poly Ultimate Pack"** – 3D low poly objects [link](https://assetstore.unity.com/packages/3d/props/low-poly-ultimate-pack-54733)
  - **"GUI Pro-FantasyRPG"** – UI assets [link](https://assetstore.unity.com/packages/2d/gui/gui-pro-fantasy-rpg-170168)
  - **"Free Game Items"** – UI assets [link](https://assetstore.unity.com/packages/2d/environments/free-game-items-131764)
  - **"Coins Sfx"** – coin collection sound effects [link](https://assetstore.unity.com/packages/audio/sound-fx/coins-sfx-39052)
  - **"Absolutely Free Music"** – background music [link](https://assetstore.unity.com/packages/audio/music/absolutely-free-music-4883)
