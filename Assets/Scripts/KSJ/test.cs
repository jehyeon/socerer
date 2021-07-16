using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class test : MonoBehaviour
{
    Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(dqdq);
        button.onClick.AddListener(test2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void dqdq()
    {
        Debug.Log("¶÷´Ù ½È´Ù");
    }

    public void test2()
    {
        Debug.Log("¶÷´Ù ½È´Ù2222");
    }
}
