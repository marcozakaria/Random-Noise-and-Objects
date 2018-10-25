using System.IO;
using UnityEngine;

public class PersistentStorage : MonoBehaviour
{
    string savePath;

    void Awake()
    {
        // make the savePath and file name
        savePath = Path.Combine("C:/Users/marco/Documents/Unity Projects/Random-Noise-and-Objects/Save Files", "saveFile");
        Debug.Log(Application.persistentDataPath);
    }

    public void Save(PersistableObject o)
    {
        using (
            var writer = new BinaryWriter(File.Open(savePath, FileMode.Create))
        )
        {
            o.Save(new GameDataWritter(writer));
            Debug.Log("Saved");
        }
    }

    public void Load(PersistableObject o)
    {
        using (
            var reader = new BinaryReader(File.Open(savePath, FileMode.Open))
        )
        {
            o.Load(new GameDataReader(reader));
        }
    }
}