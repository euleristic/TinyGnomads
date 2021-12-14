using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TextureSwap : MonoBehaviour
{
    public Material[] texture;
    int textureIndex;
    public int amountOfTextures;
    Renderer m_Renderer;


    void Start()
    {
        textureIndex = 0;
    }

    void Update()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            CycleNextTexure();
        }
    }


    public  void CycleNextTexure()
    {
        textureIndex++;
        if(textureIndex >= amountOfTextures)
        {
            textureIndex = 0;
        }

        switch (textureIndex)
        {
            case 0:
                if(texture[textureIndex] != null)
                gameObject.GetComponent<SkinnedMeshRenderer>().material = texture[textureIndex];
                break;
            case 1:
                if (texture[textureIndex] != null)
                    gameObject.GetComponent<SkinnedMeshRenderer>().material = texture[textureIndex];
                break;
            case 2:
                if (texture[textureIndex] != null)
                    gameObject.GetComponent<SkinnedMeshRenderer>().material = texture[textureIndex];
                break;
            case 3:
                if (texture[textureIndex] != null)
                    gameObject.GetComponent<SkinnedMeshRenderer>().material = texture[textureIndex];
                break;
            case 4:
                if (texture[textureIndex] != null)
                    gameObject.GetComponent<SkinnedMeshRenderer>().material = texture[textureIndex];
                break;
            case 5:
                if (texture[textureIndex] != null)
                    gameObject.GetComponent<SkinnedMeshRenderer>().material = texture[textureIndex];
                break;
        }
    }


}
