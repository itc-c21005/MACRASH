using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class woolscript : MonoBehaviour
{
    //public MeshRenderer mesh;

    //int i;

    public BottunCT BCT;

    // Start is called before the first frame update
    void Start()
    {
        //mesh.material.color = mesh.material.color - new Color32(0, 0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //透明になったらオブジェクトを消去する
        /*if(i > 63)
        {
            Destroy(gameObject);
        }*/

    }

    private void OnCollisionEnter(Collision collision)
    {
        //StartCoroutine("Transparent");
        BCT.ReloadMakura();
        Destroy(gameObject);
    }

    /*IEnumerator Transparent()
    {
        for (i = 0; i < 255; i++)
        {
            
            mesh.material.color = mesh.material.color - new Color32(0, 0, 0, 4);
            
            yield return new WaitForSeconds(0.01f);
        }
    }*/

}
