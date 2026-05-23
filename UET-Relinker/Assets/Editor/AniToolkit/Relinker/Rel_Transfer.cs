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

            //sourceData.Add(clip);
            //(EditorCurveBinding binding in clip.bindin)
            
            //Debug.Log(clip.name); // verified

        }


    }


    /*
     * clears up source data to prevent issues with multiple transfers.
     */
    public static void clearSourceData()
    {
        sourceData.Clear();

    }


    public static void transferController(AnimatorController controller, string folderPath, GameObject root, string newName)
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

        //how to create an object? / copy file? //https://docs.unity3d.com/6000.0/Documentation/ScriptReference/AssetDatabase.CreateAsset.html
        //new AnimatorController test = controller;

        //string controllerPath = AssetDatabase.GetAssetPath(controller);
        //Debug.Log(controllerPath);

        ////separate this out into its own method. Also i dont think this is necessary anymore, on second thought people will likely already have a folder made, they arent expecting a folder rename
        //string newController = "";
        //for (int i = controllerPath.Length-1; i > 0; i--) // surely there is a better way to do this??
        //{
        //    if (controllerPath[i] == '/') // we have reached the folder name -- ABORT!
        //    {

        //        break;
        //    }
        //    newController = controllerPath[i] + newController;

        //}
        ////Debug.Log(oldRootName);

        //string newControllerPath = controllerPath.Replace(newController, newName);
        //Debug.Log(newControllerPath);
        //Debug.Log("inside");
        AnimatorController newController = new AnimatorController();
        //Debug.Log("new");
        AssetDatabase.CreateAsset(newController, folderPath+".controller");
        //Debug.Log("created");
        Debug.Log(AssetDatabase.GetAssetPath(newController)); //WRONG PATH!!!

    }


}
