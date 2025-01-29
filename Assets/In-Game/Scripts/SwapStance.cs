using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwapStance : MonoBehaviour
{
    public int textureIndex = 1;

    public Swappable[] swappableList;

    private void Start()
    {
        InputSystem.EnableDevice(Keyboard.current);
    }
    private void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null)
            return;

        if (keyboard.eKey.wasPressedThisFrame)
        {
            swapTexture();
            Debug.Log("pres e");
            StartCoroutine(waiter());
        }
    }
    IEnumerator waiter()
    {
        InputSystem.DisableDevice(Keyboard.current);
        yield return new WaitForSeconds(2);
        InputSystem.EnableDevice(Keyboard.current);
    }

    [System.Serializable]
    public struct Swappable
    {
        public string name;
        public Material mat;
        public Texture texture1;
        public Texture texture2;
    }

    public void swapTexture()
    {
        if (textureIndex == 1)
        {
            foreach (var item_swap in swappableList)
            {
                item_swap.mat.SetTexture("_MainTex", item_swap.texture2);
                item_swap.mat.SetTexture("_ShadeTexture", item_swap.texture2);
            }

            textureIndex = 2;
        }

        else if (textureIndex == 2)
        {
            foreach (var item_swap in swappableList)
            {
                item_swap.mat.SetTexture("_MainTex", item_swap.texture1);
                item_swap.mat.SetTexture("_ShadeTexture", item_swap.texture1);
            }

            textureIndex = 1;
        }
    }

    private void OnApplicationQuit()
    {
        if (textureIndex == 2)
            swapTexture();
    }
}