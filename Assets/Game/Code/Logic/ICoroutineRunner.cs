using System.Collections;
using UnityEngine;

namespace Game.Code.Logic
{
    public interface ICoroutineRunner {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}