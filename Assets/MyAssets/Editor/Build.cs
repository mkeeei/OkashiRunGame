using UnityEditor;

public static class BuildApp 
{
    [MenuItem("Build/BuildApp")]
    public static void Build()
    {
        //windows64のプラットフォームでアプリをビルドする
        BuildPipeline.BuildPlayer(
            new string[] { 
                 "Assets/Scenes/TitleScene.unity",
                "Assets/Scenes/AthleticScene01.unity" ,
                 "Assets/Scenes/MiniGameScene01.unity",
                "Assets/Scenes/AthleticScene02.unity" ,
                 "Assets/Scenes/MiniGameScene02.unity" ,
                "Assets/Scenes/GameOverScene.unity"

            },
            "Builds/App/OkashiRunGame.exe",
            BuildTarget.StandaloneWindows64,
            BuildOptions.None
        );
    }
}
