using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MenuScript
{
    [MenuItem("Outils/Creer des Tiles")]
    public static void CreerDesTiles()
    {
        GameObject Tile = Resources.Load<GameObject>("UVCube");
        int i = 0;
        while (i < 3)
        {
            GameObject z = Tile;
            z.transform.position = new Vector3(i, 0f, 0f);
            PrefabUtility.InstantiatePrefab(z);
            int y = 1;
            while (y < 3)
            {
                z.transform.position = new Vector3(i, 0f, y);
                PrefabUtility.InstantiatePrefab(z);
                y++;
            }
            i++;
        }
    }
    [MenuItem("Outils/Detruire des Tiles")]
    public static void DetruireDesTiles()
    {
        GameObject[] Tiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject t in Tiles)
        {
            GameObject.DestroyImmediate(t);
        }
    }
    [MenuItem("Outils/Créer une tour")]
    public static void ConstruireUneTour()
    {
        GameObject Tile = Resources.Load<GameObject>("Brique");
        int i = 0;
        while (i < 3)
        {
            GameObject z = Tile;
            z.transform.position = new Vector3(i, 0f, 0f);
            PrefabUtility.InstantiatePrefab(z);
            int y = 1;
            while (y < 3)
            {
                z.transform.position = new Vector3(i, 0f, y);
                PrefabUtility.InstantiatePrefab(z);
                int a = 0;
                while (a < 6)
                {
                    z.transform.position = new Vector3(i, a, y);
                    PrefabUtility.InstantiatePrefab(z);
                    a++;
                }
                y++;
            }
            i++;
        }
    }
}



