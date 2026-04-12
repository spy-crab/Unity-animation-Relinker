# Relinker [Ver. 0.1]
The Relinker is a tool to fix already broken animation links. Broken animation links in Unity are defined by their characteristic yellow tint, but complete animation data. This is caused by a change in the hierarchy, which breaks the file path connections that the animations were built off of. 

To fix this you can manually rename every binding, but this quickly adds up across dozens of characters, complex rigs, and complex animations. The Relinker looks for invalid paths within a given animation controller, and gives the user the option of manually renaming individual bindings, or mass renaming using 'find' and 'replace'.  You can use the tool without opening your prefabs, letting you quickly check the validity of your animation clips, and fix issues. 

   I started this tool to dip my toes into Unity Editor Window scripting, before attempting my [Constraints tool.](https://github.com/spy-crab/Unity-animation-Constraints)

**Complete feature list:**
   - Individual renaming
   - Find and replace.


## Installation
Installation for this should be similar to any other Unity editor tools you have installed. 
**Unity Import Package**
   - In Unity Editor go to Assets -> Import Package -> Custom Package...
   - Select the unity package file you have downloaded.
> [!NOTE]
> I have only tested this project on Windows, whether it works on Linux or Mac is **untested**.

## Inputs
Hovering over labels in the Editor Window should give you a description on what it is, otherwise the following may give you a clearer idea on what is going on.

<p align="center">
<img width="474" height="60" alt="image" src="https://github.com/user-attachments/assets/56414b95-40bb-4eb8-b8c3-9f553957e996" />
</p>

| Label  | Description |
| ------------- | ------------- |
| Animation Controller  | Refers to the controller that you wish to fix broken keys from.  |
| Root Scene Object | This is the object at the top of the hierarchy that holds the animation data. **It is important that you put the correct object in here**, otherwise the script will grab every binding available, and find and replace may break otherwise non-broken keys. If you wish to edit all keys, even valid ones, then putting any object other than the root object will give you that ability, however it may not work as intended-- use with caution.  |
| Refresh | This refreshes the list of bindings on the list. The script does not automatically refresh. It refreshes under two cases: renaming, and the click of this button. |

<p align="center">
<img width="474" height="52" alt="image" src="https://github.com/user-attachments/assets/cf53db63-ae82-4385-b271-783e61513a09" />
</p>

| Label  | Description |
| ------------- | ------------- |
| New Path  | You can edit paths individually here. The script will use the entire string as an input for the path.  |
| Replace  | Using the path given, it will replace the binding with a new binding, with the same values, with the new filepath. |

<p align="center">
<img width="474" height="71" alt="image" src="https://github.com/user-attachments/assets/8bf8cf54-1938-4134-bf12-339590630cd3" />
</p>

| Label  | Description |
| ------------- | ------------- |
| Find Path  | The string to be found across all the bindings. |
| Replace Path  | The string to replace the found string across all the bindings.   |
| Replace | Activates the find and replace script. This will edit all the bindings listed when you click ‘Refresh’ if there are no invalid bindings, nothing will be renamed. Example: Binding1 = Hamburger, Find = Ham, Replace = Steamed Ham -> Result: Steamed Hamburger|



-- 

## Special thanks
**Alain Schneuwly**, for feedback, supervision, and tremendous support.

**Quimby, the cat**, for supervision. Bug inspector.

[**Caiden Muro**](https://www.caidendmuro.design/), for UI, UX feedback. This tool used to be a lot uglier, and clunkier to use without their feedback!

[**Corbyn Lamar**](https://corbyn-lamar.com/), for your knowledge and helpful tips. They’re amazing!

[**hfcRed**](https://github.com/hfcRed). When doing early research I had found their [tool](https://github.com/hfcRed/Animation-Repathing/tree/main), and it served as a initial reference point for editing animation data. 

**Skyler** @magussparky.bsky.social on [Bluesky](https://bsky.app/profile/magussparky.bsky.social), for introducing me to Unity Editor Window! I wouldn’t have gotten the idea to do this project without her.

