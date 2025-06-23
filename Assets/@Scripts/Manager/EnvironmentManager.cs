using extension;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : Singleton<EnvironmentManager>, IEventSubscriber
{
    public event Action<Define.TimeOfDayType> OnTimeOfDayChanged;

    #region Dictionary
    private Dictionary<string, Material> _skyBoxList;
    private Dictionary<string, Color> _colorList;
    private Dictionary<string, Color> _variationColorList;
    private Dictionary<string, float> _variationLightList;
    private Dictionary<string, float> _variationMountainList;
    private Dictionary<string, float> _targetColorList;
    private Dictionary<string, float> _targetLightList;
    private Dictionary<string, float> _targetMountainList;
    private Dictionary<string, float> _rotateSkyBoxList;
    private Dictionary<string, float> _rotateLightList;

    #endregion
    private Material _currentSkyBox;

    private Material _mountain;

    private Light _currentLight;

    private Define.TimeOfDayType _currentProperty;

    Coroutine _lerpCoroutine;

    //더 좋은 방법 어디 없나?
    Color _betterColor;

    public Define.TimeOfDayType CurrentProperty
    {
        get { return _currentProperty; }
        set { _currentProperty = value; }
    }

    float _currentLightValue = 1;
    float _currentMountainValue = 1;
    float _rotateSpeed = 1;
    float _skyBoxRotateValue;
    float _toKey = 1;
    bool _isSkyBoxColor = false;


    public float ToKey
    {
        set 
        {
            _toKey = value;   
        }
    }

    //임시 방편
    const string Morning = "MorningSkyBox";
    const string Noon = "NoonSkyBox";
    const string Evening = "EveningSkyBox";
    const string Night = "NightSkyBox";
    protected override void Initialize()
    {
        base.Initialize();

        _skyBoxList = new Dictionary<string, Material>();

        _skyBoxList = ObjectManager.Instance.SkyBoxResourceList;

        _currentLight = GameObject.Find("Directional Light").GetComponent<Light>();

        _colorList = new Dictionary<string, Color>()
        {
            { Morning,  new Color(0.5f, 0.5f, 0.5f, 1) },
            { Noon, new Color(0.5f, 0.5f, 0.5f, 1) },
            { Evening, new Color(0, 0, 0, 1) },
            { Night, new Color(0.26f, 0.34f, 0.415f, 1) }
            //{ Night, new Color(0.426f, 0.611f, 0.896f, 1) }
        };

        _variationColorList = new Dictionary<string, Color>()
        {
            { Morning, new Color(-0.001f, -0.0005f, 0, 0) },
            { Noon, new Color(-0.0003f, -0.0004f, -0.0004f, 0) },
            { Evening, new Color(-0.0004f, -0.0003f, -0.0003f, 0) },
            { Night, new Color(0.0005f, 0.0004f, 0.0004f, 0) }
        };

        _variationLightList = new Dictionary<string, float>()
        {
            { Morning, 0.002f },
            { Noon, -0.001f },
            { Evening, -0.0015f },
            { Night, 0.001f }
        };

        _variationMountainList = new Dictionary<string, float>()
        {
            { Morning,  0.002f},
            { Noon, -0.001f },
            { Evening, -0.0005f },
            { Night, 0.001f}
        };

        _targetColorList = new Dictionary<string, float>()
        {
            { Morning, 0.35f },
            { Noon, 0.29f },
            //{ Evening, 0.1f },
            { Evening, 0.08f },
            { Night, 0.6f }
        };

        _targetLightList = new Dictionary<string, float>()
        {
            { Morning, 1f },
            { Noon, 0.5f },
            { Evening, 0.1f },
            { Night, 0.5f }
        };

        _targetMountainList = new Dictionary<string, float>()
        {
            { Morning, 0.9f},
            { Noon, 0.5f},
            { Evening, 0.33f},
            { Night, 0.66f}
        };

        _rotateSkyBoxList = new Dictionary<string, float>()
        {
            { Morning, 270 },
            { Noon, 0 },
            { Evening, 50 },
            { Night, 312 }
        };

        _rotateLightList = new Dictionary<string, float>()
        {
            { Morning, 20 },
            { Noon, 40 },
            { Evening, 20 },
            { Night, 40 }
        };

        SetEnvironmentObject();
        SetInitLightRotateSpeed();
        SetInitAllSkyBoxRotate();
        ChangeSkyBox(Define.TimeOfDayType.Noon);
    }

    #region Init Environment
    void SetInitAllSkyBoxRotate()
    {
        foreach(var skyBox in _skyBoxList)
        {
            skyBox.Value.SetFloat("_Rotation", _rotateSkyBoxList[skyBox.Key]);
        }
    }

    void SetEnvironmentObject()
    {
        //임시 -> 추후 수정 필요
        GameObject environment = GameObject.Find("Environment");
        GameObject background = Instantiate(ObjectManager.Instance.BackgroundResource);
        background.transform.SetParent(environment.transform);

        MeshRenderer meshRenderer = background.GetComponentInChildren<MeshRenderer>();
        _mountain = meshRenderer.sharedMaterial;
        _mountain.SetColor("_Color", new Color(1, 1, 1, 1));
        _currentLight.transform.eulerAngles = new Vector3(100, 6, 6);
    }

    void SetInitLightRotateSpeed()
    {
        _skyBoxRotateValue = _rotateSkyBoxList[Noon];
        _rotateSpeed = _rotateLightList[Noon] / TimeManager.Instance.Duration;
    }
    #endregion

    #region IEventSubscriber
    public void Subscribe()
    {
        TimeManager.Instance.OnColorChanged += UpdateSkyColor;
        TimeManager.Instance.OnSkyBoxChanged += UpdateDuration;
        TimeManager.Instance.OnTimeSpeedChanged += UpdateDuration;
    }
    #endregion
    void OnDisable()
    {
        TimeManager.Instance.OnColorChanged -= UpdateSkyColor;
        TimeManager.Instance.OnSkyBoxChanged -= UpdateDuration;
        TimeManager.Instance.OnTimeSpeedChanged -= UpdateDuration;
    }

    void Update()
    {
        RotateSkyBox();
        RotateLight();
    }

    #region Rotate
    void RotateSkyBox()
    {
        _skyBoxRotateValue += _rotateSpeed * Time.deltaTime;
        if (_skyBoxRotateValue >= 360)
        {
            _skyBoxRotateValue = 0;
        }
        _currentSkyBox.SetFloat("_Rotation", _skyBoxRotateValue);
    }

    void RotateLight()
    {
        switch (_currentProperty)
        {
            case Define.TimeOfDayType.Noon:
            case Define.TimeOfDayType.Morning:
            case Define.TimeOfDayType.Evening:
                _currentLight.transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime, Space.World);
                break;
            case Define.TimeOfDayType.Night:
                //밤이 되면 빛의 회전을 미리 아침 때의 위치로 변경
                _currentLight.transform.localEulerAngles = new Vector3(140, 78, 86);   
                break;
        }
    }
    #endregion

    #region TimeManager Event Perform
    //TimeManager의 현재 스카이박스의 색을 변경할 때 호출되는 함수
    void UpdateSkyColor(Define.TimeOfDayType type)
    {
        StartCoroutine(LerpSkyBox(type));
    }

    //속도를 변경할 때 호출되는 함수
    void UpdateDuration(Define.TimeOfDayType type)
    {
        SetRotateSpeed(type);
    }
    #endregion

    #region Update Color
    float a=0, b= 1;
    //현재의 스카이박스 색을 변경하는 함수
    IEnumerator LerpSkyBox(Define.TimeOfDayType type)
    {
        Debug.Log("스카이박스 색 변경" + a++);
        string current = GetSkyBoxKey(type);
        Color changeColor = _colorList[current]; //현재 스카이박스의 초기값을 지정
        while (Extension.CheckTwoValues(changeColor.r, _targetColorList[current]))
        {
            _currentSkyBox.SetColor("_Tint", changeColor);
            changeColor += _variationColorList[current] / _toKey;

            LerpLight(current);
            LerpMountain(current);
            yield return null;
        }
        Debug.Log("색 변경 완료" + b++);
        if(current == Noon) _betterColor = changeColor;
        ChangeSkyBox(GetNextType(type));
    }

    void LerpLight(string from)
    {
        float change = _currentLightValue + _variationLightList[from];
        if (Extension.CheckTwoValues(change, _targetLightList[from]))
        {
            _currentLight.color = new Color(_currentLightValue, _currentLightValue, _currentLightValue, 1);
            _currentLightValue += _variationLightList[from] / _toKey;
        }
    }

    void LerpMountain(string from)
    {
        float change = _currentMountainValue + _variationMountainList[from];
        if (Extension.CheckTwoValues(change, _targetMountainList[from]))
        {
            _mountain.SetColor("_Color", new Color(_currentMountainValue, _currentMountainValue, _currentMountainValue, 1));
            _currentMountainValue += _variationMountainList[from] / _toKey; 
        }
    }
    #endregion

    //스카이박스를 변경하는 함수
    void ChangeSkyBox(Define.TimeOfDayType skyBox)
    {
        Debug.Log("스카이박스 변경");
        string skyBoxName = GetSkyBoxKey(skyBox);
        _currentSkyBox = _skyBoxList[skyBoxName];
        _currentProperty =  SetCurrentProperty(skyBoxName);


        //바꿀 스카이박스가 저녁이면
        if(skyBoxName == Evening)
        {
            //초기 저녁 색을 낮의 마지막 색으로 변경
            _colorList[Evening] = _betterColor;

            //초기 저녁 회전값을 낮의 마지막 회전값으로 변경
            _currentSkyBox.SetFloat("_Rotation", _skyBoxRotateValue + Time.deltaTime);
            _currentSkyBox.SetColor("_Tint", _colorList[skyBoxName]);
        }
        else if(skyBoxName == Morning)
        {
            StartCoroutine(ChangeMorningColor());
            _skyBoxRotateValue = _rotateSkyBoxList[skyBoxName];
        }
        else
        {
            //바꿀 스카이박스의 시작 회전값으로 초기화
            _skyBoxRotateValue = _rotateSkyBoxList[skyBoxName];
            _currentSkyBox.SetColor("_Tint", _colorList[skyBoxName]);
        }

        //실제로 스카이박스 변경
        ChangeRenderSettings();
    }

    void SetRotateSpeed(Define.TimeOfDayType type)
    {
        string skyBox = GetSkyBoxKey(type);
        //현재 스카이박스의 회전할 총량을 스카이박스가 유지될 시간으로 나눠 속력을 구함
        _rotateSpeed = _rotateLightList[skyBox] / TimeManager.Instance.Duration;
        Debug.Log(_rotateSpeed);
    }

    IEnumerator ChangeMorningColor()
    {
        Color morningColor = new Color(0.1254f, 0.2081f, 0.2745f, 1);
        while (Extension.CheckTwoValues(morningColor.r, 0.5f))
        {
            _currentSkyBox.SetColor("_Tint", morningColor);
            morningColor += new Color(0.0005f, 0.0004f, 0.0003f, 0) / _toKey;
            if (_currentLightValue < 0.5f)
                LerpLight(Night);
            yield return null;
        }
    }


    Define.TimeOfDayType SetCurrentProperty(string skyBox)
    {
        return skyBox switch
        {
            Morning => Define.TimeOfDayType.Morning,
            Noon => Define.TimeOfDayType.Noon,
            Evening => Define.TimeOfDayType.Evening,
            Night => Define.TimeOfDayType.Night,
            _ => Define.TimeOfDayType.Noon
        };
    }

    void ChangeRenderSettings()
    {
        RenderSettings.skybox = _currentSkyBox;
    }

    string GetSkyBoxKey(Define.TimeOfDayType type)
    {
        return type switch
        {
            Define.TimeOfDayType.Morning => Morning,
            Define.TimeOfDayType.Noon => Noon,
            Define.TimeOfDayType.Evening => Evening,
            Define.TimeOfDayType.Night => Night,
            _ => Noon,
        };
    }

    Define.TimeOfDayType GetNextType(Define.TimeOfDayType type)
    {
        return type switch
        {
            Define.TimeOfDayType.Noon => Define.TimeOfDayType.Evening,
            Define.TimeOfDayType.Evening => Define.TimeOfDayType.Night,
            Define.TimeOfDayType.Night => Define.TimeOfDayType.Morning,
            Define.TimeOfDayType.Morning => Define.TimeOfDayType.Noon,
            _ => 0
        };
    }
}
