using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Resolution : MonoBehaviour
{

    public TMP_Dropdown resolutionDropdown;
    public Toggle fullScreenToggle;

    UnityEngine.Resolution[] resolutions;
    List<UnityEngine.Resolution> filteredResolutions;
    bool isFullScreen;
    int SelectedResolution;

    PropertyInfo refreshRateRatioProp;
    PropertyInfo numeratorProp;
    PropertyInfo denominatorProp;

    readonly (int w, int h)[] preferredSizes = new (int, int)[]
    {
        (3840,2160),
        (2560,1440),
        (1920,1080),
        (1600,900),
        (1366,768),
        (1280,720)
    };

    // Start is called before the first frame update
    void Start()
    {
        isFullScreen = Screen.fullScreen;
        resolutions = Screen.resolutions;

        refreshRateRatioProp = typeof(UnityEngine.Resolution).GetProperty("refreshRateRatio", BindingFlags.Public | BindingFlags.Instance);
        if (refreshRateRatioProp != null)
        {
            var rrType = refreshRateRatioProp.PropertyType;
            numeratorProp = rrType.GetProperty("numerator", BindingFlags.Public | BindingFlags.Instance) ?? rrType.GetProperty("Numerator", BindingFlags.Public | BindingFlags.Instance);
            denominatorProp = rrType.GetProperty("denominator", BindingFlags.Public | BindingFlags.Instance) ?? rrType.GetProperty("Denominator", BindingFlags.Public | BindingFlags.Instance);
        }

            BuildFilteredResolutions();

        var options = new List<string>();
        int currentIndex = 0;
        var current = Screen.currentResolution;

        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            var r = filteredResolutions[i];
            string refreshStr = GetRefreshRateString(r);
            options.Add($"{r.width} x {r.height} @{refreshStr}Hz");

            if (r.width == current.width && r.height == current.height && Mathf.Approximately(GetRefreshRateValue(r), GetRefreshRateValue(current)))
            {
                currentIndex = i;
            }
        }

        if (resolutionDropdown != null)
        {
            resolutionDropdown.ClearOptions();
            resolutionDropdown.AddOptions(options);
            SelectedResolution = currentIndex;
            resolutionDropdown.value = SelectedResolution;
            resolutionDropdown.RefreshShownValue();
            resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        }

        if (fullScreenToggle != null)
        {
            fullScreenToggle.isOn = isFullScreen;
            fullScreenToggle.onValueChanged.AddListener(OnFullScreenToggle);
        }
    }

    void BuildFilteredResolutions()
    {
        filteredResolutions = new List<UnityEngine.Resolution>();
        var seen = new HashSet<string>();


        foreach (var pref in preferredSizes)
        {
            var matches = resolutions.Where(r => r.width == pref.w && r.height == pref.h);
            if (matches.Any())
            {
                var best = matches.OrderByDescending(r => GetRefreshRateValue(r)).First();
                string key = $"{best.width}x{best.height}";
                if (!seen.Contains(key))
                {
                    filteredResolutions.Add(best);
                    seen.Add(key);
                }
            }

            if (filteredResolutions.Count >= 6) break;
        }

        if (filteredResolutions.Count < 6)
        {
            var others = resolutions
                .OrderByDescending(r => (long)r.width * r.height)
                .Where(r => !seen.Contains($"{r.width}x{r.height}"));

            foreach (var r in others)
            {
                filteredResolutions.Add(r);
                seen.Add($"{r.width}x{r.height}");
                if (filteredResolutions.Count >= 6) break;
            }
        }
    }

    string GetRefreshRateString(UnityEngine.Resolution res)
    {
        if (refreshRateRatioProp != null)
        {
            try
            {
                var rr = refreshRateRatioProp.GetValue(res);
                if (rr != null && numeratorProp != null && denominatorProp != null)
                {
                    var numObj = numeratorProp.GetValue(rr);
                    var denObj = denominatorProp.GetValue(rr);
                    if (numObj != null && denObj != null)
                    {
                        float num = System.Convert.ToSingle(numObj);
                        float den = System.Convert.ToSingle(denObj);
                        if (den != 0f)
                        {
                            float value = num / den;
                            return value.ToString("0.##");
                        }
                    }
                }
            }
            catch { }
        }

        return res.refreshRate.ToString();
    }

    float GetRefreshRateValue(UnityEngine.Resolution res)
    {
        if (refreshRateRatioProp != null)
        {
            try
            {
                var rr = refreshRateRatioProp.GetValue(res);
                if (rr != null && numeratorProp != null && denominatorProp != null)
                {
                    var numObj = numeratorProp.GetValue(rr);
                    var denObj = denominatorProp.GetValue(rr);
                    if (numObj != null && denObj != null)
                    {
                        float num = System.Convert.ToSingle(numObj);
                        float den = System.Convert.ToSingle(denObj);
                        if (den != 0f)
                        {
                            return num / den;
                        }
                    }
                }
            }
            catch { }
        }

        return (float)res.refreshRate;
    }

    public void OnResolutionChanged(int index)
    {
        if (index < 0 || index >= filteredResolutions.Count) return;

        SelectedResolution = index;
        var res = filteredResolutions[index];
        int preferredRefresh = Mathf.RoundToInt(GetRefreshRateValue(res));
        Screen.SetResolution(res.width, res.height, isFullScreen, preferredRefresh);
    }

    public void OnFullScreenToggle(bool fullscreen)
    {
        isFullScreen = fullscreen;
        Screen.fullScreen = isFullScreen;

        if (SelectedResolution >= 0 && SelectedResolution < filteredResolutions.Count)
        {
            var res = filteredResolutions[SelectedResolution];
            int preferredRefresh = Mathf.RoundToInt(GetRefreshRateValue(res));
            Screen.SetResolution(res.width, res.height, isFullScreen, preferredRefresh);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
