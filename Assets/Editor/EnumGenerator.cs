using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.IO;
using System.Linq;

[InitializeOnLoad]
public static class EnumAutoGenerator
{
    static EnumAutoGenerator()
    {
        EditorSceneManager.sceneOpened += OnSceneOpened;
    }

    private static void OnSceneOpened(UnityEngine.SceneManagement.Scene scene, OpenSceneMode mode)
    {
        GerarEnumFases();
    }

    public static void GerarEnumFases()
    {
        string resourcePath = "Assets/Resources/fases";
        string[] arquivos = Directory.GetFiles(resourcePath, "*.*");

        var nomes = arquivos
            .Select(Path.GetFileNameWithoutExtension)
            .Select(n => n.Replace(" ", "_")) // opcional: limpar espaços
            .Distinct()
            .ToList();

        string enumPath = "Assets/Scripts/framework/Gerados/RelacaoFases.cs";
        using (StreamWriter writer = new StreamWriter(enumPath))
        {
            writer.WriteLine("public enum RelacaoFases");
            writer.WriteLine("{");
            foreach (var nome in nomes)
            {
                writer.WriteLine($"    {nome},");
            }
            writer.WriteLine("}");
        }

        AssetDatabase.Refresh();
        Debug.Log("RelacaoFases gerado com sucesso!");
    }

    private static string SanitizeEnumName(string input)
    {
        // Remove espaços, símbolos, etc.
        string clean = new string(input
            .Where(c => char.IsLetterOrDigit(c) || c == '_')
            .ToArray());

        if (char.IsDigit(clean[0]))
            clean = "_" + clean;

        return clean;
    }
}
#endif


