using UnityEditor;

public static class BuildApp 
{
    [MenuItem("Build/BuildApp")]
    public static void Build()
    {
        //windows64のプラットフォームでアプリをビルドする
        BuildPipeline.BuildPlayer(
            new string[] { 
                 "Assets/MyAssets/Scenes/TitleScene.unity",
                "Assets/MyAssets/Scenes/AthleticScene01.unity" ,
                 "Assets/MyAssets/Scenes/MiniGameScene01.unity",
                "Assets/MyAssets/Scenes/AthleticScene02.unity" ,
                 "Assets/MyAssets/Scenes/MiniGameScene02.unity" ,
                "Assets/MyAssets/Scenes/GameOverScene.unity"

            },
            "Builds/App/OkashiRunGame.exe",
            BuildTarget.StandaloneWindows64,
            BuildOptions.None
        );
    }
}
