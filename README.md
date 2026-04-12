# Relinker [Ver. 0.1]
The Relinker is a tool to fix already broken animation links. Broken animation links in Unity are defined by their characteristic yellow tint, but complete animation data. This is caused by a change in the hierarchy, which breaks the file path connections that the animations were built off of. 

To fix this you can manually rename every binding, but this quickly adds up across dozens of characters, complex rigs, and complex animations. The Relinker looks for invalid paths within a given animation controller, and gives the user the option of manually renaming individual bindings, or mass renaming using 'find' and 'replace'.  You can use the tool without opening your prefabs, letting you quickly check the validity of your animation clips, and fix issues. 

   I started this tool to dip my toes into Unity Editor Window scripting, before attempting my [Constraints tool.](https://github.com/spy-crab/Unity-animation-Constraints)

**Complete feature list:**
   - Individual renaming
   - Find and replace.

### Let me know if you have any feedback, or run into issues. If you found the tool useful please give the repository a star!

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
   <img width="474" height="60" alt="image" src="https://github.com/user-attachments/assets/c370d007-2f36-4e5c-b0f7-89635534443c" />
   
</p>

| Label  | Description |
| ------------- | ------------- |
| Animation Controller  | Refers to the controller that you wish to fix broken keys from.  |
| Root Scene Object | This is the object at the top of the hierarchy that holds the animation data. **It is important that you put the correct object in here**, otherwise the script will grab every binding available, and find and replace may break otherwise non-broken keys. If you wish to edit all keys, even valid ones, then putting any object other than the root object will give you that ability, however it may not work as intended-- use with caution.  |
| Refresh | This refreshes the list of bindings on the list. The script does not automatically refresh. It refreshes under two cases: renaming, and the click of this button. |

<p align="center">
   <img width="474" height="52" alt="image" src="https://github.com/user-attachments/assets/494b1a73-660c-4ee5-9a1a-b9a8af044a36" />
</p>

| Label  | Description |
| ------------- | ------------- |
| New Path  | You can edit paths individually here. The script will use the entire string as an input for the path.  |
| Replace  | Using the path given, it will replace the binding with a new binding, with the same values, with the new filepath. |

<p align="center">
   <img width="474" height="61" alt="image" src="https://github.com/user-attachments/assets/eee2ed00-385d-4a95-b13b-893ae9cee1d0" />
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

