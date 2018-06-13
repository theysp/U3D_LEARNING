using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CreateVolData : MonoBehaviour {
    Texture3D texture;

    // Use this for initialization
    void Start () {
        texture = GenerateTex3d(256);
        GetComponent<MeshRenderer>().material.SetTexture();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    Texture3D GenerateTex3d(int size){
        if (size <= 1)
            size = 10;
		Color []colorArray = new Color[size*size*size];
        Texture3D texture = new Texture3D(size, size, size, TextureFormat.ARGB32, true);
        float r = 1.0f / (size - 1);
        for(int i=0,idx=0;i<size;++i, ++idx)
            for(int j=0;j<size;++j, ++idx)
                for(int k = 0; k < size; ++k,++idx)
                {
                    colorArray[idx] = new Color(r*i,r*j,r*k,1.0f);
                }
        texture.SetPixels(colorArray);
        texture.Apply();
        return texture;
    }
}
