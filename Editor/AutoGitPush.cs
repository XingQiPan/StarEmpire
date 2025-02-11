using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Diagnostics;
using System;
using Debug = System.Diagnostics.Debug;

public class AutoGitPush : MonoBehaviour
{
    [PostProcessBuild(1)]
    public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
    {
        UnityEngine.Debug.Log("尝试请求");
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
        if (ExecuteCommand(gitPushCommand))  // Check if push was successful
        {
            UnityEngine.Debug.Log("Git push successful!"); // Output success message
        }
        

    }

    private static bool ExecuteCommand(string command) // Return bool for success
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
            process.Close();
            return true; // Command executed successfully
        }
        else
        {
            UnityEngine.Debug.LogError("Command Error: " + error);
            process.Close();
            return false; // Command failed
        }
    }
}