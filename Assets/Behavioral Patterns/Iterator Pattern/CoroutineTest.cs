using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

    class CoroutineTest:MonoBehaviour
    {
    private void Start()
    {
      //  StartCoroutine(UnityCoroutine());

        // start self coroutine

        CoroutineManager.Instance.StartCoroutineSimple(SelfCoroutine());

    }

    IEnumerator UnityCoroutine()
    {

        Debug.Log("Unity coroutine begin at time : " + Time.time);

        yield return new WaitForSeconds(5);

        Debug.Log("Unity coroutine begin at time : " + Time.time);

    }

    IEnumerator SelfCoroutine()
    {

      //  Debug.Log("Self coroutine begin at time : " + Time.time);

      yield return new CoroutineWaitForSeconds(0.1f);
        Debug.Log("yield1");
        yield return new CoroutineWaitForSeconds(3f);
        Debug.Log("yield2");
        //Debug.Log("Self coroutine begin at time : " + Time.time);
    }
}

