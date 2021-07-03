#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ChangeScene : Editor
{

    // [MenuItem("Scene/TestScene")]
    // public static void OpenLoading()
    // {
    //     OpenScene("TestScene");
    // }

    [MenuItem("Scene/PlayScene")]
    public static void OpenPlayerScene()
    {
        OpenScene("PlayScene");
    }

    // [MenuItem("Scene/Popup Prefab")]
    // public static void OpenPopupFolder()
    // {
    //     string path = "Assets/Game/Resources/UI/Popups/PopupOutfit.prefab";

    //     OpenFolder(path);
    // }

    // [MenuItem("Scene/UIPanels Prefab")]
    // public static void OpenUIPanelsFolder()
    // {
    //     string path = "Assets/Game/Resources/UI/UIPanels/UICharacterCard.prefab";

    //     OpenFolder(path);
    // }

    // [MenuItem("Scene/Char Cards")]
    // public static void OpenCharCardsFolder()
    // {
    //     string path = "Assets/Game/Arts/CharCards/astronaus.png";

    //     OpenFolder(path);
    // }

    // [MenuItem("Scene/Manager Scripts")]
    // public static void OpenManagerScriptsFolder()
    // {
    //     string path = "Assets/Game/Scripts/Managers/GameManager.cs";

    //     OpenFolder(path);
    // }

    // [MenuItem("Scene/UI Scripts")]
    // public static void OpenUIScriptsFolder()
    // {
    //     string path = "Assets/Game/Scripts/UI/UICanvas.cs";

    //     OpenFolder(path);
    // }

    // [MenuItem("Scene/Prefabs")]
    // public static void OpenPrefabsFolder()
    // {
    //     string path = "Assets/Game/Prefabs";

    //     OpenFolder(path);
    // }

    // private static void OpenFolder(string _path)
    // {
    //     Object obj = AssetDatabase.LoadAssetAtPath(_path, typeof(Object));

    //     EditorUtility.FocusProjectWindow();
    //     Selection.activeObject = obj;
    //     EditorGUIUtility.PingObject(Selection.activeObject);
    // }

    private static void OpenScene(string sceneName)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene("Assets/Game/Scenes/" + sceneName + ".unity");
        }
    }
}
#endif