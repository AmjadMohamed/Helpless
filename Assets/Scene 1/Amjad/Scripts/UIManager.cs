using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager UIMgr;

    private void Start()
    {
        if(!UIMgr)
        {
            UIMgr = this;
        }
    }

    public GameObject PauseMenu;

}
