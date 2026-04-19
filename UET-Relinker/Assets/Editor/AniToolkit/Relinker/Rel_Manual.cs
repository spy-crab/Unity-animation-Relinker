using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/*
 * This grabs invalid animation paths and lets the user rename them.
 */
public static class Rel_Manual
{
    public class InvalidSharedProperty
    {
        //Where the invalid animation bindings are stored
        public HashSet<AnimationClip> foldoutClips = new HashSet<AnimationClip>();
        public bool foldout = true; //display info or fold away
        public string oldPath; //before rename
        public string newPath; //after rename
    }
    public static HashSet<InvalidSharedProperty> invalidProperties = new();

    /*
     * Calls CheckBinding for both float curves, and object curves.
     * populates  hashset with invalid clips. if an invalid root object is given, it provides ALL animation clips.
     * AnimatorController controller: the animation controller to check invalid paths from
     * GameObject root: the game object at the top of the heirarchy holds it all
     * */
    public static void ScanInvalidPaths(AnimatorController controller, GameObject root)
    {
        invalidProperties.Clear();

        foreach (AnimationClip clip in controller.animationClips)
        {
            if (clip == null) continue;

            // Float/transform curves
            var floatCurves = AnimationUtility.GetCurveBindings(clip);
            foreach (var binding in floatCurves)
                CheckBinding(binding, clip, root);

            // Object reference curves
            var objectCurves = AnimationUtility.GetObjectReferenceCurveBindings(clip);
            foreach (var binding in objectCurves)
                CheckBinding(binding, clip, root);
        }
    }


    /*
     * CheckBinding is used to add bindings to pathToSharedProperty. which is used to create constraints, etc.
     *EditorCurveBinding binding: the binding from the animation clip.
     *ANimationClip clip: the animation clip that the bindings originate from..
     *GameObject root: the game object that contains the animated objects. at the top of the heirarchy
     */
    private static void CheckBinding(EditorCurveBinding binding, AnimationClip clip, GameObject root)
    {
        // if the root is null, assign null to animated object. if root exists, get animated object instead.
        object animatedObject = root == null ? null : AnimationUtility.GetAnimatedObject(root, binding);

        //if it isnt null, it exists, and is a valid binding, thus we do NOT want to rename this.
        if (animatedObject != null)
        {
            return;
        }
        //check the HashSet for the first entry where property.oldpath = binding.path and return it in that var.
        var sharedProperty = invalidProperties.FirstOrDefault(property => property.oldPath == binding.path);
        if (sharedProperty == null) //dont have a list of all the bindings + info? make it.
        {
            sharedProperty = new InvalidSharedProperty();
            sharedProperty.oldPath = binding.path;
            sharedProperty.newPath = binding.path; //default is the same, user will change this.
            invalidProperties.Add(sharedProperty); 
        }

        if (!sharedProperty.foldoutClips.Contains(clip))
            sharedProperty.foldoutClips.Add(clip);
    }

    /*
      * replace path names.
      * calls Replace Binding Path to actually do it.
      * 
      * string oldPath: the path before editing
      * string newPath: the new path the user has given
      * AnimatorController controller: the controller we have grabbed the binding names from
      * GameObject root: the root obj at the rop of the heirarchy
      * bool replaceAll: whether the entire string needs to be replaced, or if it is a find + replace.
      * */
    public static void ReplacePathInClips(string oldPath, string newPath, AnimatorController controller, GameObject root, bool replaceAll)
    {
        if (controller == null)
        {
            Debug.LogError("No Animator Controller provided.");
            return;
        }
        //stops the annoying collectino modified errors
        var propertiesToProcess = invalidProperties.ToList();

        try //attempt to replace it otherwise give out a warning.
        {
            AssetDatabase.StartAssetEditing();

            foreach (var sharedProperty in propertiesToProcess)
            {
                //for every clip in the aniomation, and every animation in the controller
                foreach (AnimationClip clip in sharedProperty.foldoutClips)
                {
                    if (clip == null)
                    {
                        continue; //skips past. 
                    }     
                    // Float/transform curves
                    var floatCurves = AnimationUtility.GetCurveBindings(clip);
                    foreach (var binding in floatCurves)
                    {
                        ReplaceBindingPath(binding, clip, oldPath, newPath, replaceAll, root);
                        EditorUtility.SetDirty(clip); //prompt user to save

                    }
                    // Object reference curves
                    var objectCurves = AnimationUtility.GetObjectReferenceCurveBindings(clip);
                    foreach (var binding in objectCurves)
                    {
                        ReplaceBindingPath(binding, clip, oldPath, newPath, replaceAll, root);
                        EditorUtility.SetDirty(clip); //prompt user to save
                    }

                }
            }
        }
        finally
        {
            AssetDatabase.StopAssetEditing();
            GUI.FocusControl(""); //prevents textfield from showing old data
        }
        //only scan once we do everything.
        if (controller != null && root != null)
        {
            ScanInvalidPaths(controller, root);
        }

    }


       /*
       * Replaces the entire binding clip path.
       * 
       * EditorCurveBinding binding: the binding we are going to copy
       * AnimationClip clip: the animation clip we are editing
       * string oldPath: the path before changes
       * string newPath: the path inputted by the user. 
       * bool replaceAll: whether we are doing a partial or full replace.
       * 
       */
    private static void ReplaceBindingPath(EditorCurveBinding binding, AnimationClip clip, string oldPath, string newPath, bool replaceAll, GameObject root)
    {
        //animated object: if the root is == to null, then set it to null, otherwise get the animated object and set animatedObject to that.
        object animatedObject = (root == null) ? null : AnimationUtility.GetAnimatedObject(root, binding);

        if (animatedObject != null)
        {
            return; //skip
        }

        AnimationCurve floatCurve = AnimationUtility.GetEditorCurve(clip, binding);
        ObjectReferenceKeyframe[] objectCurve = AnimationUtility.GetObjectReferenceCurve(clip, binding);

        if (!binding.path.Contains(oldPath)) //if the binding we are given's path does not contain the path we gave: exit.
        {
            return;
        }

        if (binding.type == typeof(Object))
        {
            //only for the functions that display the entire string where the user edits them manually. replaces the entire string.
            if (replaceAll == true)
            {
                Undo.RecordObject(clip, "Rename Binding in " + clip.name);
                if (binding.path == oldPath)//otherwise children will be annhiliated.
                {
                    var keyframes = AnimationUtility.GetObjectReferenceCurve(clip, binding);
                    AnimationUtility.SetObjectReferenceCurve(clip, binding, null); // clear old curve

                    binding.path = newPath; // set new path
                    AnimationUtility.SetObjectReferenceCurve(clip, binding, keyframes);
                }
                else
                {
                    return; //EXIT!!
                }
            }
            else //partial replace!
            {
                if (binding.path.Contains(oldPath))
                    {
                    
                        Undo.RecordObject(clip, "Rename Binding in " + clip.name);
                        AnimationUtility.SetObjectReferenceCurve(clip, binding, null);
                        binding.path = binding.path.Replace(oldPath, newPath);
                        AnimationUtility.SetObjectReferenceCurve(clip, binding, objectCurve);
                    
                 }

            }

        }
        else //if it isnt an object curve
        {
            // Float/transform curve
            if (replaceAll == true)
            {
                if (binding.path == oldPath)
                {
                    Undo.RecordObject(clip, "Rename Binding in " + clip.name);
                    //Debug.Log("here instead");
                    var curve = AnimationUtility.GetEditorCurve(clip, binding);
                    AnimationUtility.SetEditorCurve(clip, binding, null); //clear old curve

                    binding.path = newPath; //set new path
                    AnimationUtility.SetEditorCurve(clip, binding, curve);
                }

                else
                {
                    return;
                }
            }
            else //find and replace PARTIAL
            { 
                Undo.RecordObject(clip, "Find:" + oldPath+ " + Replace:" + newPath);
                string finalPath = binding.path.Replace(oldPath, newPath);

                // verify that the path actually changed before changing it!! 
                if (finalPath == binding.path)
                {
                    return;
                }
                EditorCurveBinding newBinding = binding;
                newBinding.path = finalPath;
                AnimationUtility.SetEditorCurve(clip, binding, null); //clear old
                AnimationUtility.SetEditorCurve(clip, newBinding, floatCurve); //set new
            }

        }
        GUI.FocusControl("");  //prevents textfield from showing old data

    }


}