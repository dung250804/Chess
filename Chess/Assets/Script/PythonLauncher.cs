using System.Diagnostics;
using UnityEngine;

public class PythonLauncher : MonoBehaviour
{
    private Process pythonProcess;

    private void Start()
    {
        StartPythonScript();
    }

    private void StartPythonScript()
    {
        // Đường dẫn đến Python và script bạn muốn chạy
        string pythonPath = @"C:\Users\Tuan Dung\IdeaProjects\CSCHTTT\venv\Scripts\python.exe";  // Path to Python interpreter
        string scriptPath = @"C:\Users\Tuan Dung\IdeaProjects\CSCHTTT\.idea\Chess\Predict.py";  // Path to the Python script

        // Cấu hình tiến trình
        pythonProcess = new Process();
        pythonProcess.StartInfo.FileName = pythonPath;
        pythonProcess.StartInfo.Arguments = $"\"{scriptPath}\"";
        pythonProcess.StartInfo.UseShellExecute = false;
        pythonProcess.StartInfo.CreateNoWindow = true;
        pythonProcess.StartInfo.RedirectStandardOutput = true;
        pythonProcess.StartInfo.RedirectStandardError = true;

        // Bắt đầu tiến trình Python
        pythonProcess.Start();

        // Đọc output từ Python (tuỳ chọn)
        pythonProcess.OutputDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                UnityEngine.Debug.Log("Python Output: " + e.Data);
            }
        };
        pythonProcess.BeginOutputReadLine();

        // Đọc lỗi từ Python (tuỳ chọn)
        pythonProcess.ErrorDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                UnityEngine.Debug.LogError("Python Error: " + e.Data);
            }
        };
        pythonProcess.BeginErrorReadLine();
    }

    private void OnApplicationQuit()
    {
        // Dừng tiến trình Python khi tắt ứng dụng
        if (pythonProcess != null && !pythonProcess.HasExited)
        {
            pythonProcess.Kill();
        }
    }
}