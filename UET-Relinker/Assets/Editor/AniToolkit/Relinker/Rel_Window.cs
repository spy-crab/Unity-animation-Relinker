using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

/*
 * This is the window for the animation relinker
 * 
 * The relinker lets you: 
 * rename individual animation bindings
 * mass rename bindings using find and replace
 * 
 * Requires: Rel_Manual
 * Future: Rel_Transfer, Rel_Auto
 */
public class Rel_Window : EditorWindow
{

    [MenuItem("Animation Editor Toolkit/Animation Relinker")]
    static void Init()
    {
        Rel_Window window = (Rel_Window)EditorWindow.GetWindow(typeof(Rel_Window), false, "Animation Relinker");
        window.Show();

    }

    //MANUAL
    private AnimatorController selectedController; //the controller the user has inputted to check for invalid bindings
    private GameObject rootObject; //Scene object containing the Animator
    //FIND AND REPLACE VARS
    private string findPath = ""; //the string to look for
    private string replacePath = ""; //the string to replace the 'found'

    //AUTOMATIC - TODO
    private AnimatorController targetAnim;
    private AnimatorController sourceAnim; 

    //TRANSFER - TODO
    private AnimatorController controllerToTransfer;
    private GameObject rootToInheritInfo;
    private string pathToTarget;



    //EDITOR WINDOW VARIABLES -- not used in any scripts
    private Vector2 scrollPos; //for scrolling the binding list
    private bool collapseAll = false; //why the error
    private bool openAll = false; //there is probably a better way but i dont know currently.
    private bool hasBeenEdited = false; //essentually used to check if collapseAll has been enabled, and if so, go through all of them, then mark this as edited. when edited, collapseall is false again.
    private int toolbarInt = 0; //for future 
    private string[] toolbarStrings = { "Manual" }; //for future

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal(); //toolbar, help
        toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarStrings);
        GUILayout.Space(200);
        GUIContent contentHelp = new GUIContent("Help", "https://github.com/spy-crab/Unity-animation-Relinker"); //TODO change lol
        if (GUILayout.Button(contentHelp, GUILayout.Width(50)))
        {
            Application.OpenURL("https://github.com/spy-crab/Unity-animation-Relinker"); //temporary
        }
        EditorGUILayout.EndHorizontal(); //toolbar, help

        switch (toolbarInt)
        {
            //Manual lets the user change file paths per binding, and for all files using find and replace
            case 0: //Manual

                EditorGUILayout.LabelField("Manual", EditorStyles.boldLabel);

                GUIContent contentController = new GUIContent("Animation Controller", "The controller to check for invalid bindings from.");
                selectedController = (AnimatorController)EditorGUILayout.ObjectField(contentController, selectedController, typeof(AnimatorController), false);//does not come from the scene, searches project files

                //allows us to check for invalid objects
                GUIContent contentRootObj = new GUIContent("Root Scene Object", "The game object at the top of the heirarchy that holds the skeleton.");
                rootObject = (GameObject)EditorGUILayout.ObjectField(contentRootObj, rootObject, typeof(GameObject), true);//searches the scene

                EditorGUILayout.Space(2);

                EditorGUI.BeginDisabledGroup(selectedController == null || rootObject == null || rootObject == null && selectedController == null); //no controller / root

                GUIContent contentRefresh = new GUIContent("Refresh", "Fetch/refresh invalid binding names");
                if (GUILayout.Button(contentRefresh))
                {
                    //Populates with invalid object paths
                    Rel_Manual.ScanInvalidPaths(selectedController, rootObject);
                }

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal(); //invalid paths, collapse, open
                GUIContent contentInvalid = new GUIContent("Invalid Animation Paths", "Shows paths that have invalid names. If the incorrect root scene object is selected, it will display all paths as 'invalid' ");
                EditorGUILayout.LabelField(contentInvalid, EditorStyles.boldLabel);

                //The way the collapsing and opening is.. not ideal but we enable either collapseAll or openAll, and then within the loop, close all of the foldouts, and afterwards, reset the boolean states.
                GUIContent contentCollapseAll = new GUIContent("Collapse all", "Collapses all of the foldowns");
                if (GUILayout.Button(contentCollapseAll))
                {
                    collapseAll = true;
                    hasBeenEdited = false;

                }
                GUIContent contentOpenAll = new GUIContent("Open all", "Opens all of the foldowns");
                if (GUILayout.Button(contentOpenAll))
                {
                    openAll = true;
                    hasBeenEdited = false;

                }

                EditorGUILayout.EndHorizontal(); //invalid paths, collapse, open
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos); //scrollPos

                // go through all the inalid properties found, sort it.
                var sortedList = Rel_Manual.invalidProperties.OrderBy(property => property.oldPath).ToList();
                foreach (var sharedProperty in sortedList)
                {
                    sharedProperty.foldout = EditorGUILayout.Foldout(sharedProperty.foldout, sharedProperty.oldPath); // collapseAll

                    if (hasBeenEdited == false)
                    {
                        if (openAll)
                        {
                            sharedProperty.foldout = true;
                        }
                        else //this means that collapseAll is true!
                        {
                            if (collapseAll)//silence
                            sharedProperty.foldout = false; //collapse it!
                        }

                    }
                    //if content is to be shown
                    if (sharedProperty.foldout)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.BeginHorizontal(BackgroundStyle.Get(new Color(0.15f, 0.15f, 0.15f, 0.7f))); //box
                        sharedProperty.newPath = EditorGUILayout.TextField("New Path", sharedProperty.newPath);
                        if (GUILayout.Button("Replace"))
                        {
                            //replace
                            Rel_Manual.ReplacePathInClips(sharedProperty.oldPath, sharedProperty.newPath, selectedController, rootObject, true); //TODO: overload so you dont need the root object pls

                            //removes fixed path from ui
                            Rel_Manual.ScanInvalidPaths(selectedController, rootObject);
                            GUIUtility.ExitGUI(); // Prevent layout errors after modifying the list we are looping through
                        }
                        EditorGUILayout.EndHorizontal(); //box

                        EditorGUI.indentLevel--;
                    }
                }
                //resets collapseAll and openAll
                if (hasBeenEdited == false)
                {
                    hasBeenEdited = true;
                    collapseAll = false;
                    openAll = false;
                }
                EditorGUILayout.EndScrollView(); //scrollPos

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Find and Replace Paths", EditorStyles.boldLabel);

                findPath = EditorGUILayout.TextField("Find Path", findPath);
                replacePath = EditorGUILayout.TextField("Replace Path", replacePath);

                if (GUILayout.Button("Replace"))
                {
                    if (string.IsNullOrEmpty(findPath))
                    {   //you cant find nothin!
                        EditorUtility.DisplayDialog("Error", "Please enter a path to find.", "OK");
                    }
                    else if (string.IsNullOrEmpty(replacePath))
                    {   //you cant replace nothin! that would break things
                        EditorUtility.DisplayDialog("Error", "Please enter a replacement path.", "OK");
                    }
                    else if (rootObject == null)
                    {   //... shouldnt be possible but this is old code
                        EditorUtility.DisplayDialog("Error", "Please assign a root scene object.", "OK");
                    }
                    else
                    {   //if all of the bove are true, then you may continue and replace paths.
                        Rel_Manual.ReplacePathInClips(findPath, replacePath, selectedController, rootObject, false);

                        //refresh data after mass replace
                        Rel_Manual.ScanInvalidPaths(selectedController, rootObject);
                    }
                }

                EditorGUILayout.Space();

                EditorGUI.EndDisabledGroup(); //no controller / root

                break;
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }


        //should this be dismissable?
        EditorGUILayout.HelpBox("Version 0.2 - Early Access | Send feedback on GitHub ", MessageType.Info);

    }


    public static class BackgroundStyle     //used to create the black boxes -- https://discussions.unity.com/t/changing-the-background-color-for-beginhorizontal/427449/15
    {
        private static GUIStyle style = new GUIStyle();
        private static Texture2D texture = new Texture2D(1, 1);


        public static GUIStyle Get(Color color)
        {
            texture.SetPixel(0, 0, color);
            texture.Apply();
            style.normal.background = texture;
            return style;
        }
    }


}