using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraObjects : MonoBehaviour
{
    public Camera mainCamera;

    private void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
    }


    private void Update()
    {
        List<Element> elementsInView = new List<Element>();

        // Get all the UI elements with Graphic component
        Graphic[] graphics = FindObjectsOfType<Graphic>();

        foreach (Graphic graphic in graphics)
        {
            if (IsElementInViewport(graphic.transform.position))
            {
                elementsInView.Add(new Element(graphic.gameObject, graphic.depth));
            }
        }

        // Get all the GameObjects with Renderer component
        Renderer[] renderers = FindObjectsOfType<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            if (IsElementInViewport(renderer.transform.position))
            {
                elementsInView.Add(new Element(renderer.gameObject, renderer.sortingOrder));
            }
        }

        // Sort the elements by depth before processing
        elementsInView.Sort((a, b) => a.depth.CompareTo(b.depth));

        // Now you have a list of elements in the camera's viewport
        // Do something with the list, such as printing their names
        foreach (Element element in elementsInView)
        {
            Debug.Log("Element in view: " + element.gameObject.name);
        }
    }

    private bool IsElementInViewport(Vector3 position)
    {
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(position);
        return viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1;
    }

    private class Element
    {
        public GameObject gameObject;
        public int depth;

        public Element(GameObject gameObject, int depth)
        {
            this.gameObject = gameObject;
            this.depth = depth;
        }
    }
}
