using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Diagnostics;
using System;

public class AutoGitPush : MonoBehaviour
{
    [PostProcessBuild(1)]
    public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
    {
        PushToGit();
    }

    private static void PushToGit()
    {
        // 获取当前时间并格式化为字符串
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string commitMessage = $"Auto commit after build at {timestamp}";

        string gitAddCommand = "git add .";
        string gitCommitCommand = $"git commit -m \"{commitMessage}\"";
        string gitPushCommand = "git push origin main"; // 请根据你的分支名称修改

        ExecuteCommand(gitAddCommand);
        ExecuteCommand(gitCommitCommand);
        ExecuteCommand(gitPushCommand);
    }

    private static void ExecuteCommand(string command)
    {
        ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", "/c " + command)
        {
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            WorkingDirectory = System.IO.Directory.GetCurrentDirectory()
        };

        Process process = Process.Start(processInfo);
        process.WaitForExit();

        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();

        if (string.IsNullOrEmpty(error))
        {
            UnityEngine.Debug.Log("Command Output: " + output);
        }
        else
        {
            UnityEngine.Debug.LogError("Command Error: " + error);
        }

        process.Close();
    }
}