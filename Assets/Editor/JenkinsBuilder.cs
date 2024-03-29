﻿/* 
JenkinsBuilder.cs
Automatically changes the target platform and creates a build.
 
Installation
Place in an Editor folder.
 
Usage
Go to File > AutoBuilder and select a platform. These methods can also be run from the Unity command line using -executeMethod AutoBuilder.MethodName.
 
License
Copyright (C) 2011 by Thinksquirrel Software, LLC
 
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
 
The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.
 
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

Obtained from: http://wiki.unity3d.com/index.php/AutoBuilder

 */
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public static class JenkinsBuilder
{

    static string GetProjectName()
    {
        string[] s = Application.dataPath.Split('/');
        return s[s.Length - 2];
    }

    static string[] GetScenePaths()
    {
        string[] scenes = new string[EditorBuildSettings.scenes.Length];

        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i] = EditorBuildSettings.scenes[i].path;
        }

        return scenes;
    }

    [MenuItem("File/AutoBuilder/All")]
    static void PerformAllBuild()
    {
        PerformWin64Build();
        PerformOSXUniversalBuild();
        PerformLinuxUniversalBuild();
        //PerformWebBuild();
    }

    [MenuItem("File/AutoBuilder/Windows/32-bit")]
    static void PerformWinBuild()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);
        BuildPipeline.BuildPlayer(GetScenePaths(), "Builds/Win/" + GetProjectName() + ".exe", BuildTarget.StandaloneWindows, BuildOptions.None);
    }

    [MenuItem("File/AutoBuilder/Windows/64-bit")]
    static void PerformWin64Build()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);
        BuildPipeline.BuildPlayer(GetScenePaths(), "Builds/Win64/" + GetProjectName() + ".exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
    }

    [MenuItem("File/AutoBuilder/Mac OSX/Universal")]
    static void PerformOSXUniversalBuild()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneOSX);
        BuildPipeline.BuildPlayer(GetScenePaths(), "Builds/OSX-Universal/" + GetProjectName() + ".app", BuildTarget.StandaloneOSX, BuildOptions.None);
    }
    [MenuItem("File/AutoBuilder/Linux/Universal")]
    static void PerformLinuxUniversalBuild()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneLinuxUniversal);
        BuildPipeline.BuildPlayer(GetScenePaths(), "Builds/Linux/" + GetProjectName() + ".exe", BuildTarget.StandaloneLinuxUniversal, BuildOptions.None);
    }
    [MenuItem("File/AutoBuilder/Web/")]
    static void PerformWebBuild()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);
        BuildPipeline.BuildPlayer(GetScenePaths(), "Builds/Web", BuildTarget.WebGL, BuildOptions.None);
    }
}


