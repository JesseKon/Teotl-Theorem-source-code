using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public struct AffectedMaterial
{
    public uint MaterialIndex;
    public Vector2 Offset;
    public Vector2 Speed;
    public int Steps;
}


public class TextureMover : MonoBehaviour
{
    public AffectedMaterial[] affectedMaterials;
    private Material[] m_Materials;


    private void Start() {
        m_Materials = GetComponent<Renderer>().materials;
    }


    private void Update() {
        for (int i = 0; i < affectedMaterials.Length; ++i) {
            float realStep = 1.0f / affectedMaterials[i].Steps;

            affectedMaterials[i].Offset += affectedMaterials[i].Speed * Time.deltaTime * affectedMaterials[i].Steps;
            Vector2Int offsetToInt = new Vector2Int((int)affectedMaterials[i].Offset.x, (int)affectedMaterials[i].Offset.y);

            m_Materials[affectedMaterials[i].MaterialIndex].SetTextureOffset("_MainTex", new Vector2(realStep * offsetToInt.x, realStep * offsetToInt.y));
        }
    }
}
