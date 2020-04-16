using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using System.Collections.Generic;

[InitializeOnLoad]
public class FileExtensionGUI
{
    private static Vector2 offset = new Vector2(-15, 0);
    private static GUIStyle style;
    private static StringBuilder sb = new StringBuilder();
    private static string selectedGuid;
    private static HashSet<string> showExt = new HashSet<string>()
    {
        ".tga",
        ".psd",
        ".png",
        ".jpg",
        ".raw",
        ".fbx",
        ".obj",
        ".blend",
        ".ogg",
        ".mp3",
        ".wav",
        ".cs",
        ".mat",
        ".prefab",
        ".shader",
        ".pdf"
    };

    static FileExtensionGUI()
    {
        EditorApplication.projectWindowItemOnGUI += HandleOnGUI;
        Selection.selectionChanged += () =>
        {
            if (Selection.activeObject != null)
                AssetDatabase.TryGetGUIDAndLocalFileIdentifier(Selection.activeObject, out selectedGuid, out long id);
        };

    }

    private static bool ValidString(string str)
    {
        return !string.IsNullOrEmpty(str) && str.Length > 7;
    }

    private static void HandleOnGUI(string guid, Rect selectionRect)
    {
        string path = AssetDatabase.GUIDToAssetPath(guid);
        string extRaw = Path.GetExtension(path);
        if (!showExt.Contains(extRaw.ToLower()))
            return;

        bool selected = false;
        if (ValidString(guid) && ValidString(selectedGuid))
            selected = String.Compare(guid, 0, selectedGuid, 0, 6) == 0;

        DateTime lastModification = File.GetLastWriteTime(path);

        TimeSpan span = DateTime.Now.Subtract(lastModification);
        string fileElapsedText = string.Format("{0}{1}{2}{3}",
            span.Duration().Days > 0 ? string.Format("{0:0} day{1}, ", span.Days, span.Days == 1 ? string.Empty : "s") : string.Empty,
            span.Duration().Hours > 0 ? string.Format("{0:0} hour{1}, ", span.Hours, span.Hours == 1 ? string.Empty : "s") : string.Empty,
            span.Duration().Minutes > 0 ? string.Format("{0:0} min{1}, ", span.Minutes, span.Minutes == 1 ? string.Empty : "s") : string.Empty,
            span.Duration().Seconds > 0 ? string.Format("{0:0} sec{1}", span.Seconds, span.Seconds == 1 ? string.Empty : "s") : string.Empty);

        if (fileElapsedText.EndsWith(", ")) fileElapsedText = fileElapsedText.Substring(0, fileElapsedText.Length - 2);

        if (string.IsNullOrEmpty(fileElapsedText)) fileElapsedText = "0 sec";

        sb.Clear().Append(extRaw);
        if (sb.Length > 0)
        {
            sb.Remove(0, 1);
        }

        sb.Append(" | " + fileElapsedText);

        string ext = sb.ToString();

        if (style == null)
        {
            style = new GUIStyle(EditorStyles.label);
        }

        style.normal.textColor = selected ? new Color32(255, 255, 255, 255) : new Color32(127, 127, 127, 160);
        var size = style.CalcSize(new GUIContent(ext));
        //EditorGUI.DrawRect(selectionRect, new Color(.76f, .76f, .76f));
        //selectionRect.x -= size.x + 12;
        //selectionRect.x += size.x + 10;

        selectionRect.x += Path.GetFileName(path).Length * 8;


        Rect offsetRect = new Rect(selectionRect.position, selectionRect.size);
        EditorGUI.LabelField(offsetRect, ext, style);
        

    }

}