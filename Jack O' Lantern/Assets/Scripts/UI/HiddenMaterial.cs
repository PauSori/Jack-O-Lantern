using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenMaterial : MonoBehaviour
{
    public Material materialToHide;

    private void Start()
    {
        // Verificar si se ha asignado un material para ocultar
        if (materialToHide != null)
        {
            // Configurar el material para que no se muestre en la cámara principal
            materialToHide.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Front);
            materialToHide.SetInt("_ZWrite", 0);
            materialToHide.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
            materialToHide.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            materialToHide.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            materialToHide.SetInt("_ShadowCaster", 1);
        }
        else
        {
            Debug.LogError("MaterialToHide no asignado en el script HideMaterial en la cámara.");
        }
    }
}