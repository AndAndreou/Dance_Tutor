using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WordsManagerWithSync))]
public class WordManagerEditor : Editor
{
    private string buttonName = "";
    private string modeLayerString = "";

    private bool useSameSizeForWords = true;

    GUIStyle infoLabel = new GUIStyle();

    WordsManagerWithSync myWordManager ;

    MyStreamingGraph streamGraph ;

    private void Awake()
    {
        myWordManager = (WordsManagerWithSync)target;
        streamGraph = FindObjectOfType<MyStreamingGraph>();

        if (myWordManager.syncAnimationEnable)
        {
            buttonName = "Go to async animations";
            modeLayerString = " You are in sync animations";
            infoLabel.normal.textColor = Color.magenta;
        }
        else
        {
            buttonName = "Go to sync animations";
            modeLayerString = " You are in async animations";
            infoLabel.normal.textColor = Color.red;
        }
    }

    public override void OnInspectorGUI()
    {
        myWordManager = (WordsManagerWithSync)target;
        if (GUILayout.Button(buttonName))
        {
            myWordManager.syncAnimationEnable = !myWordManager.syncAnimationEnable;

            if (myWordManager.syncAnimationEnable)
            {
                buttonName = "Go to async animations";
                modeLayerString = " You are in sync animations";
                infoLabel.normal.textColor = Color.magenta;
            }
            else
            {
                buttonName = "Go to sync animations";
                modeLayerString = " You are in async animations";
                infoLabel.normal.textColor = Color.red;
            }
        }

        EditorGUILayout.LabelField(modeLayerString, infoLabel);

        EditorGUILayout.Space();

        if (myWordManager.syncAnimationEnable)
        {
            myWordManager.syncWindowSize = EditorGUILayout.IntField("Sync Window Size", myWordManager.syncWindowSize);
            myWordManager.syncWindowStep = EditorGUILayout.IntField("Sync Window Step", myWordManager.syncWindowStep);

            myWordManager.syncWindowsMoveParallel = EditorGUILayout.ToggleLeft("Use parallel sync windows", myWordManager.syncWindowsMoveParallel);
            myWordManager.positionOfSyncFrame = (WordsManagerWithSync.PositionOfSyncFrameEnum) EditorGUILayout.EnumPopup("Postion of sync frame in words", myWordManager.positionOfSyncFrame);
        }
        else
        {
            myWordManager.motionWordStep = EditorGUILayout.IntField("Motion Word Step", myWordManager.motionWordStep);
            myWordManager.styleWordStep = EditorGUILayout.IntField("Style Word Step", myWordManager.styleWordStep);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Word Sizes", EditorStyles.boldLabel);
        useSameSizeForWords = EditorGUILayout.ToggleLeft("Use same word sizes", useSameSizeForWords);
        if (useSameSizeForWords)
        {
            int size = EditorGUILayout.IntField("Words Size", myWordManager.motionWordWindowSize);
            EditorGUILayout.LabelField("Motion Word Size", size.ToString());
            EditorGUILayout.LabelField("Style Word Size", size.ToString());
            myWordManager.motionWordWindowSize = size;
            myWordManager.styleWordWindowSize = size;
        }
        else
        {
            myWordManager.motionWordWindowSize = EditorGUILayout.IntField("Motion Word Size", myWordManager.motionWordWindowSize);
            myWordManager.styleWordWindowSize = EditorGUILayout.IntField("Style Word Size", myWordManager.styleWordWindowSize);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Word Thresholds", EditorStyles.boldLabel);
        myWordManager.motionWordThreshold = EditorGUILayout.FloatField("Motion Word Threshold", myWordManager.motionWordThreshold);
        myWordManager.styleWordThreshold = EditorGUILayout.FloatField("Motion Word Threshold", myWordManager.styleWordThreshold);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Controllers", EditorStyles.boldLabel);
        
        streamGraph = EditorGUILayout.ObjectField(streamGraph, typeof(MyStreamingGraph), true) as MyStreamingGraph;
        myWordManager.myStreamingGraphController = streamGraph;

        if (GUILayout.Button("Set Defult"))
        {
            SetDefultValues();
        }
    }

    private void SetDefultValues()
    {
        buttonName = "Go to async animations";
        modeLayerString = " You are in sync animations";
        infoLabel.normal.textColor = Color.magenta;
        myWordManager.syncAnimationEnable = true;

        myWordManager.syncWindowSize = 5;
        myWordManager.syncWindowStep = 3;
        myWordManager.syncWindowsMoveParallel = true;
        myWordManager.positionOfSyncFrame = WordsManagerWithSync.PositionOfSyncFrameEnum.end;

        myWordManager.motionWordStep = 5;
        myWordManager.styleWordStep = 5;

        useSameSizeForWords = true;
        myWordManager.motionWordWindowSize = 35;
        myWordManager.styleWordWindowSize = 35;

        myWordManager.motionWordThreshold = 65;
        myWordManager.styleWordThreshold = 5;

    }
}
