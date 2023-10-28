using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Google.XR.ARCoreExtensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class StreetscapeMenuOptions
{
    public bool BuildingsOn { get; set; }

    public bool TerrainsOn { get; set; }

    public bool AnchorsOn { get; set; }
}

public class GeospatialStreetscapeManager : MonoBehaviour
{
    [SerializeField]
    private ARStreetscapeGeometryManager streetscapeGeometryManager;

    [SerializeField]
    private Material buildingMaterial;

    [SerializeField]
    private Material terrainMaterial;

   // [SerializeField]
   // private ARRaycastManager raycastManager;

 

    private Dictionary<TrackableId, GameObject> streetscapeGeometryCached =
            new Dictionary<TrackableId, GameObject>();

    

    private bool allowPlacement = true;

    private StreetscapeMenuOptions options = new StreetscapeMenuOptions();

    [SerializeField]
    private Toggle buildingsToggle;

    /*  [SerializeField]
      private Toggle terrainsToggle;

      [SerializeField]
      private Toggle anchorsToggle;

      [SerializeField]
      private Slider projectileSlider;*/

    private void OnEnable()
    {
        streetscapeGeometryManager.StreetscapeGeometriesChanged += StreetscapeGeometriesChanged;

        options.BuildingsOn = buildingsToggle.isOn;

        buildingsToggle.onValueChanged.AddListener((_) =>
        {
            options.BuildingsOn = !options.BuildingsOn;
            if (!options.BuildingsOn)
            {
                DestroyAllRenderGeometry();
            }
        });

        /*
                options.AnchorsOn = anchorsToggle.isOn;

                options.TerrainsOn = terrainsToggle.isOn;

                projectileSlider.gameObject.SetActive(!anchorsToggle.isOn);

              

                terrainsToggle.onValueChanged.AddListener((_) =>
                {
                    options.TerrainsOn = !options.TerrainsOn;
                    if (!options.TerrainsOn)
                    {
                        DestroyAllRenderGeometry();
                    }
                });

                anchorsToggle.onValueChanged.AddListener((_) =>
                {
                    options.AnchorsOn = !options.AnchorsOn;
                    projectileSlider.gameObject.SetActive(!options.AnchorsOn);
                });*/


    }

    private void OnDisable()
    {
        streetscapeGeometryManager.StreetscapeGeometriesChanged -= StreetscapeGeometriesChanged;
      //  anchorsToggle.onValueChanged.RemoveAllListeners();
    }


    private void StreetscapeGeometriesChanged(ARStreetscapeGeometriesChangedEventArgs geometries)
    {
        geometries.Added.ForEach(g => AddRenderGeometry(g));
        geometries.Updated.ForEach(g => UpdateRenderGeometry(g));
        geometries.Removed.ForEach(g => DestroyRenderGeometry(g));
    }

    private void AddRenderGeometry(ARStreetscapeGeometry geometry)
    {
        if (!streetscapeGeometryCached.ContainsKey(geometry.trackableId))
        {
            if ((geometry.streetscapeGeometryType == StreetscapeGeometryType.Building && options.BuildingsOn)
                ||
               (geometry.streetscapeGeometryType == StreetscapeGeometryType.Terrain && options.TerrainsOn))
            {

                GameObject renderGeometryObject = new GameObject(
                    "StreetscapeGeometryMesh", typeof(MeshFilter), typeof(MeshRenderer));

                renderGeometryObject.GetComponent<MeshFilter>().mesh = geometry.mesh;

                renderGeometryObject.GetComponent<MeshRenderer>().material =
                       geometry.streetscapeGeometryType == StreetscapeGeometryType.Building ? buildingMaterial : terrainMaterial;

                renderGeometryObject.AddComponent<MeshCollider>();

                renderGeometryObject.transform.position = geometry.pose.position;
                renderGeometryObject.transform.rotation = geometry.pose.rotation;

                streetscapeGeometryCached.Add(geometry.trackableId, renderGeometryObject);
            }
        }
    }

    private void UpdateRenderGeometry(ARStreetscapeGeometry geometry)
    {
        if (streetscapeGeometryCached.ContainsKey(geometry.trackableId))
        {
            GameObject renderGeometryObject = streetscapeGeometryCached[geometry.trackableId];
            renderGeometryObject.transform.position = geometry.pose.position;
            renderGeometryObject.transform.rotation = geometry.pose.rotation;
        }
        else
        {
            // in case we toggled it off and on
            AddRenderGeometry(geometry);
        }
    }

    private void DestroyRenderGeometry(ARStreetscapeGeometry geometry)
    {
        if (streetscapeGeometryCached.ContainsKey(geometry.trackableId))
        {
            var renderGeometryObject = streetscapeGeometryCached[geometry.trackableId];
            streetscapeGeometryCached.Remove(geometry.trackableId);
            Destroy(renderGeometryObject);
        }
    }

    private void DestroyAllRenderGeometry()
    {
        var keys = streetscapeGeometryCached.Keys;
        foreach (var key in keys)
        {
            var renderObject = streetscapeGeometryCached[key];
            Destroy(renderObject);
        }
        streetscapeGeometryCached.Clear();
    }

    private void Update()
    {
        
    }
}
