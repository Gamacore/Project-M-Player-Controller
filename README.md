# 🎮 Project-M Player Controller

### 🔄 Overview
This custom player controller is a fully customizable and modular 2D player movement system for Unity. It features tuned physics-based movement mechanics designed for platformers, metroidvania, and other side-scrolling games. Built with modularity and extensibility in mind, it leverages Unity's new Input System for better input handling and includes well-structured code for easy modification and integration into any Unity project.

---

### 🎮 Features

#### **🏆 Movement Mechanics**
- **✨ Acceleration Curves:** Smooth transitions between starting, stopping, and maintaining movement, customizable via Unity's AnimationCurves.
- **🚀 Ground and Air Physics:** Separate acceleration and deceleration values for grounded and airborne states to give distinct movement behavior.
- **🔒 Wall Movement:** Includes logic for detecting wall collisions and implementing wall cling mechanics.
- **⏳ Time-Based Acceleration:** Ensures fluid transitions between speed states based on the time the player is moving in a single direction.

#### **🏋️ Jump Mechanics**
- **💪 Standard and Wall Jumping:** Supports both grounded and wall jumping, with customizable parameters for jump speed, direction, and cooldown.
- **🏃 Apex Boost:** Applies a horizontal boost at the apex of a jump to provide better air control and reward precision jumping.
- **⚔️ Zero Gravity Float:** Temporary gravity adjustment during the apex boost for smoother air-time physics.

#### **⏯️ Input System Integration**
- Fully integrated with Unity's **Input System** for handling movement and jump inputs.
- Modular design allows easy rebinding of inputs and compatibility with keyboard, gamepad, and other input devices.

#### **🌐 Modularity and Customization**
- All key parameters, such as speed, acceleration, gravity, and jump settings, are exposed via serialized fields for easy tweaking in the Unity Inspector.
- **🔧 Header Grouping:** Organized parameters in the Inspector for better readability.
- Supports layered animation logic for feet, arms, and environmental interactions.

---

### 🔧 Code Structure

#### 1. **🚗 PlayerMovement**
Handles all horizontal movement logic:
- Processes user input.
- Applies acceleration curves and physics-based movement.
- Manages grounded and airborne states.
- Handles wall clinging and gravity adjustments dynamically.

#### 2. **🏃 PlayerJump**
Handles vertical movement and jumping logic:
- Implements both standard jumping and wall jumping.
- Adds apex boost functionality for advanced air control.
- Integrates a coroutine to handle temporary zero-gravity effects at jump apexes.

#### 3. **🛠️ PlayerInput**
Acts as a bridge between Unity’s Input System and movement/jump mechanics:
- Captures input events and passes data to relevant components.
- Centralizes input logic to allow easy future expansion.

---

### 🌟 Usage
1. **⚖️ Setup:**
   - Add the `PlayerMovement`, `PlayerJump`, and `PlayerInput` scripts to your player GameObject.
   - Assign the required references (e.g., `Rigidbody2D`, `BoxCollider2D`) via the Inspector or code.
   - Configure serialized fields in the Inspector to fine-tune the player's movement and jump behavior.

2. **🔄 Customization:**
   - Modify acceleration curves, gravity scale, jump settings, and other parameters to fit your game’s design.

3. **🎫 Extend:**
   - Add new abilities or behaviors by extending the modular scripts or creating additional components.

---

### 🎯 Future Plans
- Add double-jump, coyote jump, air-dash, and clipping for expanded player control.
- Adjust acceleration curves to make the movement feel satisfying
