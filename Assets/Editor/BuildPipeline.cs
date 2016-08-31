using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildPipelineScript :EditorWindow
{
	[MenuItem("File/Build For All Platforms")]
	static void DoIt()
	{

		string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
		string name = PlayerSettings.productName + " LD36 Aggrathon";
		name = name.Replace(" ", "_");
		BuildPlatform(path, name, BuildTarget.StandaloneWindows64);
		BuildPlatform(path, name, BuildTarget.StandaloneLinuxUniversal);
		BuildPlatform(path, name, BuildTarget.StandaloneOSXUniversal);
		BuildPlatform(path, name, BuildTarget.WebGL);
		Process.Start(path);
	}

	static void BuildPlatform(string path, string name, BuildTarget platform)
	{
		switch (platform)
		{
			case BuildTarget.StandaloneOSXUniversal:
			case BuildTarget.StandaloneOSXIntel:
			case BuildTarget.StandaloneOSXIntel64:
				string folder = Path.Combine(path, "mac");
				BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, Path.Combine(folder, name), platform, BuildOptions.None);
				Compress(folder, name);
				break;
			case BuildTarget.StandaloneWindows:
			case BuildTarget.StandaloneWindows64:
				folder = Path.Combine(path, "win");
				BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, Path.Combine(folder, name+".exe"), platform, BuildOptions.None);
				Compress(folder, name);
				break;
			case BuildTarget.WebGL:
				folder = Path.Combine(path, "web");
				BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, folder, platform, BuildOptions.None);
				break;
			case BuildTarget.StandaloneLinux:
			case BuildTarget.StandaloneLinux64:
			case BuildTarget.StandaloneLinuxUniversal:
				folder = Path.Combine(path, "lin");
				BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, Path.Combine(folder, name), platform, BuildOptions.None);
				Compress(folder, name);
				break;
		}
	}

	static void Compress(string folder, string name)
	{
		ProcessStartInfo inf = new ProcessStartInfo("C:\\Program Files\\7-Zip\\7z.exe", "a \"" + Path.Combine(folder, name + ".zip") + "\" \"" + Path.Combine(folder, "*")+"\"");
		inf.CreateNoWindow = true;
		//inf.RedirectStandardOutput = true;
		//inf.UseShellExecute = false;
		var p = Process.Start(inf);
		//p.OutputDataReceived += (sender, data) =>
		//{
		//	UnityEngine.Debug.Log(data.Data);
		//};
		//p.BeginOutputReadLine();
		p.WaitForExit();
		
	}
}