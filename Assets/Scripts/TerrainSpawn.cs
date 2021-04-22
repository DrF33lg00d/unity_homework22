using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class TerrainSpawn : MonoBehaviour
{
    public string filePath;
    private string _filePathLevels;
    
    public Terrain terrain;
    
    private List<float> terrainLevels;

    private Dictionary<string, float> Metadata;

    private bool haveAllData = false;
    
    
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
            print("YES!");
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
}
