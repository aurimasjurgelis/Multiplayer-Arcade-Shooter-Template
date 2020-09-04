using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebTest : MonoBehaviour
{
    
    IEnumerator Start()
    {
        Debug.Log(DatabaseManager.SecurityCode());
        Debug.Log(DatabaseManager.SecurityCode());
        Debug.Log(DatabaseManager.SecurityCode());
        Debug.Log(DatabaseManager.SecurityCode());
        WWW request = new WWW("http://localhost/sqlconnect/webtest.php");
        yield return request;
        Debug.Log(request.text);
    }

}
