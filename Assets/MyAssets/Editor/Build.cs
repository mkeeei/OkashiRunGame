using UnityEditor;

public static class BuildApp 
{
    [MenuItem("Build/BuildApp")]
    public static void Build()
    {
        //windows64のプラットフォームでアプリをビルドする
        BuildPipeline.BuildPlayer(
            new string[] { 
                 "MyAssets/Scenes/TitleScene.unity",
	            "MyAssets/Scenes/AthleticScene01.unity" , 
	             "MyAssets/Scenes/MiniGameScene01.unity",
	            "MyAssets/Scenes/AthleticScene02.unity" ,
	             "MyAssets/Scenes/MiniGameScene02.unity" ,
	            "MyAssets/Scenes/GameOverScene.unity" 
	             
            },
            "Builds/App/OkashiRunGame.exe",
            BuildTarget.StandaloneWindows64,
            BuildOptions.None
        );
    }
}
