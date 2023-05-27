using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Character;
using RPG.Ultilities;

namespace RPG.Terrain
{
    public class TreeVisual : MonoBehaviour
    {
        private Renderer modelRenderer;
        private Material[] materialArr;
        private Material[] modifiedMaterialArr;
        private int leafMaterialIndex;

        private void Awake()
        {
            modelRenderer = GetComponent<Renderer>();
        }
        private void OnEnable()
        {
            SetMaterialArr();
            FindLeafMaterialIndex();
        }
        private void Start()
        {
            GlobalEventManager.OnTreeAppearedRaised(this);
        }

        public void SetLeafMaterial(Material material)
        {

            materialArr[leafMaterialIndex] = material;
            modelRenderer.materials = materialArr;
        }

        // private void Update()
        // {
        //     if (Input.GetKeyDown(KeyCode.U))
        //     {
        //         var randomMaterial = GetRandomMaterial();
        //         SetLeafMaterial(randomMaterial);
        //         // AssignMaterials();
        //     }
        // }
        private void SetMaterialArr()
        {
            materialArr = modelRenderer.materials;
        }
        private void FindLeafMaterialIndex()
        {
            for (int i = 0; i < materialArr.Length; i++)
            {
                string materialName = materialArr[i].name;
                if (materialName.Contains("leaf"))
                {
                    leafMaterialIndex = i;
                    break;
                }
            }
        }
    }

}



