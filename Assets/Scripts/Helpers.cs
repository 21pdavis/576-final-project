using System;
using System.Collections;
using UnityEngine;

public static class Helpers
{
    public static IEnumerator ExecuteWithDelay(float delay, Action afterDelay)
    {
        yield return new WaitForSeconds(delay);
        afterDelay();
    }
}
