using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


public class TerrainSpawn : MonoBehaviour
{
    public string filePath;
    private string _filePathLevels;
    
    public Terrain terrain;
    public Material newMaterialRef;
    
    private List<float> terrainLevels;

    private Dictionary<string, float> Metadata;

    private bool haveAllData = false;

    private float x_range;
    private float z_range;
    
    
    void Start()
    {
        Metadata = new Dictionary<string, float>();
        if (LoadMetadata())
        {
            terrainLevels = new List<float>();
            _filePathLevels = filePath.Replace(".hdr", ".txt");
            haveAllData = LoadLevels();
        }

        if (haveAllData)
        {
            x_range = Math.Abs(Metadata["left_map_x"] - Metadata["right_map_x"]);
            z_range = Math.Abs(Metadata["upper_map_y"] - Metadata["lower_map_y"]);
            GenerateTerrain();
        }
        
    }

    private bool LoadMetadata()
    {
        try
        {
            string line;
            StreamReader theReader = new StreamReader(filePath);
            using (theReader)
            {
                do
                {
                    line = theReader.ReadLine();
                    if (line != null)
                    {
                        string[] entries = line.Split('=');
                        if (entries.Length > 0)
                            FillMetadata(entries);
                    }
                }
                while (line != null);
                theReader.Close();
                return true;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("{0}\n", e.Message);
            print(e.Message);
            return false;
        }
    }

    private void FillMetadata(string[] values)
    {
        try
        {
            float value = float.Parse(values[1].Trim().Replace('.', ','));
            Metadata.Add(values[0].Trim(), value);
        }
        catch (FormatException)
        {
            print(values[0].Trim() + " are not a float field :c");
            Console.WriteLine("{0} are not a float field :c", values[0].Trim());
        }
    }

    private bool LoadLevels()
    {
        try
        {
            string line;
            string[] entries;
            StreamReader theReader = new StreamReader(_filePathLevels);
            using (theReader)
            {
                line = theReader.ReadLine();
                if (line != null)
                {
                    entries = line.Trim().Replace("  ", " ").Split();
                    if (entries.Length == Metadata["number_of_rows"] * Metadata["number_of_columns"])
                        FillLevels(entries);
                    else
                    {
                        return false;
                    }
                }
                theReader.Close();
                return true;
            }
            
        }
        catch (Exception e)
        {
            Console.WriteLine("{0}\n", e.Message);
            print(e.Message);
            return false;
        }
    }

    private void FillLevels(string[] values)
    {
        foreach (var value in values)
        {
            terrainLevels.Add(float.Parse(value.Replace('.', ',')));
        }
    }

    private void GenerateTerrain()
    {
        GameObject TerrainObj = this.gameObject;
        TerrainData _TerrainData = new TerrainData();
        
        LoadTerrainData(_TerrainData);
        
        TerrainCollider _TerrainCollider = TerrainObj.AddComponent<TerrainCollider>();
        terrain = TerrainObj.AddComponent<Terrain>();
        // Renderer rend = TerrainObj.AddComponent<Renderer>();
        // rend.material = newMaterialRef;
        
        
        _TerrainCollider.terrainData = _TerrainData;
        terrain.terrainData = _TerrainData;
        terrain.materialTemplate = newMaterialRef;


    }


    private void LoadTerrainData(TerrainData tData)
    {
        float maxHeight = Metadata["elev_m_minimum"];
        float minHeight = Metadata["elev_m_maximum"];
        int _heightmapWidth = (int) Metadata["number_of_columns"];
        int _heightmapHeight = (int) Metadata["number_of_rows"];

        tData.size = new Vector3(30, 10, 30);
        tData.heightmapResolution = _heightmapWidth;
        tData.baseMapResolution = _heightmapWidth;
        tData.SetDetailResolution(_heightmapWidth, 16);
        
        
        
        float[,] normilazeData = new float[_heightmapHeight, _heightmapWidth];
        int count = 0;
        for (int y = 0; y < _heightmapHeight; y++)
        {
            for (int x = 0; x < _heightmapWidth; x++)
            {
                normilazeData[y, x] = 1 - (terrainLevels[count] - minHeight)/(maxHeight - minHeight);
                count++;
            }
        }
        tData.SetHeights(0, 0, normilazeData);
    }
}
