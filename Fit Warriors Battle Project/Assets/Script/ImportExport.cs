using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public class ImportExport : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        using (StreamReader sr = new StreamReader("database.dat"))
        {
            string line;
            while((line = sr.ReadLine())!=null)
            {
                Console.WriteLine(line);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
