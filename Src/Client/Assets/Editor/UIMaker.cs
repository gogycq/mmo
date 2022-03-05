using UnityEngine;
using UnityEditor;
public class UIMakder : EditorWindow {
    [MenuItem("GameTools/CreateUI")]
    public static void ConfigDialog() {
        EditorWindow.GetWindow(typeof(UIMakder));
    }

    public UnityEngine.Object go = null;

    string name = "";
    string path = "";

    int buttonNum = 1;
    bool isMain = true;
    bool isSecond = false;
    bool isPop = false;
    bool containOut = false;
    bool toggleEnabled;

    void OnGUI() {
        GUILayout.Label("页面选项", EditorStyles.boldLabel);

        buttonNum = int.Parse(EditorGUILayout.TextField("按钮数量", buttonNum + ""));
        containOut = EditorGUILayout.Toggle("containOut", containOut);
        isMain = EditorGUILayout.Toggle("Main", isMain);
        isSecond = EditorGUILayout.Toggle("Second", isSecond);
        isPop = EditorGUILayout.Toggle("Pop", isPop);

        if (isMain) {

            isPop = isSecond = false; 
        } else if (isSecond) {

            isMain = isPop = false;
        } else if (isPop) {

            isMain = isSecond = false;
        }
        
        if (GUILayout.Button("创建按钮")) { 
        }
    }
}