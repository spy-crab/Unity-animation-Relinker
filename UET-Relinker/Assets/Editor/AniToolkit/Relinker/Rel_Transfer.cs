using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;
using UnityEditor.Animations;

/*
 * Transfers animation files.. havent touched this in a bit, so need to reconfigure plan
 */

public static class AniTransfer
{



    /*
     * This method grabs the controller and copies it, and it's associated clips to the path given.
     * Helper method to rename the file names perhaps?
     * 
     */

    //to store the clips.
    

    public class sourceAnimationData
    {
        public HashSet<AnimationClip> animationClips = new HashSet<AnimationClip>();
        public string oldPath;
        public string newPath;
        public bool transferClip;

        //GUI variables
        public bool foldout;
    }

    public static HashSet<sourceAnimationData> sourceData = new HashSet<sourceAnimationData>();


    /*
     * Populate source data wiht information we need to transfer
     */
    public static void populateSourceData(AnimatorController controller) 
    {
        /*TODO, research https://docs.unity3d.com/ScriptReference/Animations.AnimatorController.html
         * Animation controller data is stored as such:
         * layers[index]. stateMachine.states[index]
         * 
         * clips will be stored as a string reference in layers[i].stateMachine.states[b].state.name ?
         */

        clearSourceData();
        var sourceProperties = new sourceAnimationData();

        foreach (AnimationClip clip in controller.animationClips)
        {
            
            sourceProperties.animationClips.Add(clip);
            sourceProperties.oldPath = clip.name;
            //newpath
            //foldout
            //transferclilp

            sourceData.Add(sourceProperties);


        }


    }

    public static void processAnimationController()// it might be easier to copy the file and then edit the contents??? //https://docs.unity3d.com/6000.0/Documentation/ScriptReference/AssetDatabase.CopyAsset.html
    {
        /*The thought is. grabbing the blank anim controller, we grab the source data we copied. into the new controller
         * with new names, and everything..
         * will get there eventually
         */
    }


    /*
     * clears up source data to prevent issues with multiple transfers.
     */
    public static void clearSourceData()
    {
        sourceData.Clear();

    }
    
    public static void prefixString(string prefix)
    {
        /*TODO::
         * if this is called,
         * for every animation clip, create a new clip with the new name,
         * and then transfer over data.. i assume i cannot just rename it
         * then finally, create the files in the folders. or maybe transfer over data afterwards? whatever is easier..
         */
    }

    public static void suffixString (string suffix)
    {
        /*TODO::
         * if this is called,
         * for every animation clip, create a new clip with the new name,
         * and then transfer over data.. i assume i cannot just rename it
         * then finally, create the files in the folders. or maybe transfer over data afterwards? whatever is easier..
         */
    }

    public static void transferController(AnimatorController controller, string folderPath, string newName)
    {

        /*Get users input on what the output name will be.
         * Append name y/n
         * add suffix y/x
         * final anim files add those.
         * anim ctrl set name.
         * copy anim data.
         * assign anim files
         */

        /*
         * TODO: 
         * populate sourceData
         * path = path to put the animation files
         * rename files, and then add them to the designated path.
         * controller, reassign clips to new ones.
         * 
         * root object -- controller component added, assigned
         * 
         */

        //Debug.Log(folderPath);
        AnimatorController newController = new AnimatorController();

        //TODO populate with data

        AssetDatabase.CreateAsset(newController, folderPath+"/"+newName+".controller");
        Debug.Log(AssetDatabase.GetAssetPath(newController)); 

    }


}
