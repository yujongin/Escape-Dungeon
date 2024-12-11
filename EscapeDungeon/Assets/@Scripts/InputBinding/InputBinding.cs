using static Define;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Text;
/// <summary> 유저 키, 마우스 바인딩 옵션 </summary>
[Serializable]
public class InputBinding
{
    // 저장, 불러오기 시 폴더명, 파일명, 확장자, 고유번호
    public string localDirectoryPath = "Settings"; // "Assets/Settings"
    public string fileName = "InputBindingPreset";
    public string extName = "txt";
    public string id = "001";

    public Dictionary<UserAction, KeyCode> Bindings => _bindingDict;
    private Dictionary<UserAction, KeyCode> _bindingDict;

    public bool showDebug = false;

    /***********************************************************************
    *                               Constructors
    ***********************************************************************/
    #region .
    public InputBinding(bool initalize = true)
    {
        _bindingDict = new Dictionary<UserAction, KeyCode>();

        if (initalize)
        {
            ResetAll();
        }
    }

    public InputBinding(SerializableInputBinding sib)
    {
        _bindingDict = new Dictionary<UserAction, KeyCode>();

        foreach (var pair in sib.bindPairs)
        {
            _bindingDict[pair.key] = pair.value;
        }
    }

    #endregion
    /***********************************************************************
    *                               Private Methods
    ***********************************************************************/
    #region .
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private void DebugLog(object msg)
    {
        if (!showDebug) return;
        UnityEngine.Debug.Log(msg);
    }

    #endregion
    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region .
    /// <summary> 새로운 바인딩 설정 적용 </summary>
    public void ApplyNewBindings(InputBinding newBinding)
    {
        _bindingDict = new Dictionary<UserAction, KeyCode>(newBinding._bindingDict);
    }
    /// <summary> 새로운 바인딩 설정 적용 </summary>
    public void ApplyNewBindings(SerializableInputBinding newBinding)
    {
        _bindingDict.Clear();

        foreach (var pair in newBinding.bindPairs)
        {
            _bindingDict[pair.key] = pair.value;
        }
    }

    /// <summary> 초기 상태로 설정 </summary>
    public void ResetAll()
    {
        Bind(UserAction.Attack, KeyCode.K);

        Bind(UserAction.MoveForward, KeyCode.W);
        Bind(UserAction.MoveBackward, KeyCode.S);
        Bind(UserAction.MoveLeft, KeyCode.A);
        Bind(UserAction.MoveRight, KeyCode.D);

        Bind(UserAction.Dash, KeyCode.Space);
        Bind(UserAction.Postion, KeyCode.P);

        Bind(UserAction.UI_Status, KeyCode.Tab);
    }

    /// <summary> 바인딩 등록 또는 변경
    /// <para/> - allowOverlap이 false이면 기존 해당 키에 연결된 동작의 키가 존재할 경우 none으로 설정
    /// <para/> - allowOverlap이 true이면 바인딩이 중복되도록 유지
    /// </summary>
    public void Bind(in UserAction action, in KeyCode code, bool allowOverlap = false)
    {
        if (!allowOverlap && _bindingDict.ContainsValue(code))
        {
            var copy = new Dictionary<UserAction, KeyCode>(_bindingDict);

            foreach (var pair in copy)
            {
                if (pair.Value.Equals(code))
                {
                    _bindingDict[pair.Key] = KeyCode.None;
#if UNITY_EDITOR
                    DebugLog($"기존 바인딩 제거 : [{pair.Key} - {pair.Value}]");
#endif
                }
            }
        }

#if UNITY_EDITOR
        if (_bindingDict.ContainsKey(action))
        {
            KeyCode prevCode = _bindingDict[action];
            DebugLog($"바인딩 변경 : [{action}] {prevCode} -> {code}");
        }
        else
        {
            DebugLog($"바인딩 등록 : [{action} - {code}]");
        }
#endif

        _bindingDict[action] = code;
    }

    public override string ToString()
    {
        StringBuilder Sb = new StringBuilder("");

        Sb.Append("Bindings \n");
        foreach (var pair in _bindingDict)
        {
            Sb.Append($"{pair.Key} : {pair.Value}\n");
        }

        return Sb.ToString();
    }

    #endregion
    /***********************************************************************
    *                               Input Delegate Examples
    ***********************************************************************/
    #region .
    /*
            public Func<KeyCode, bool> GetKey => (code => Input.GetKey(code));
            public Func<KeyCode, bool> KeyDown => (code => Input.GetKeyDown(code));
            public Func<KeyCode, bool> KeyUp => (code => Input.GetKeyUp(code));
    */
    #endregion
    /***********************************************************************
    *                               File IO Methods
    ***********************************************************************/
    #region .
    public bool SaveToFile()
    {
        SerializableInputBinding sib = new SerializableInputBinding(this);
        string jsonStr = JsonUtility.ToJson(sib);

        if (jsonStr.Length < 3)
        {
            Debug.Log("JSON Serialization Error");
            return false;
        }

        DebugLog($"Save : Assets/{localDirectoryPath}/{fileName}_{id}.{extName}\n\n{this}");
        LocalFileIOHandler.Save(jsonStr, localDirectoryPath, $"{fileName}_{id}", extName);

        return true;
    }

    public bool LoadFromFile()
    {
        string jsonStr = LocalFileIOHandler.Load(localDirectoryPath, $"{fileName}_{id}", extName);

        if (jsonStr == null)
        {
            Debug.Log("File Load Error");
            return false;
        }

        var sib = JsonUtility.FromJson<SerializableInputBinding>(jsonStr);

        if (sib == null || sib.bindPairs == null)
        {
            Debug.Log("Empty File Loaded");
            return false;
        }

        ApplyNewBindings(sib);
        DebugLog($"Load : Assets/{localDirectoryPath}/{fileName}_{id}.{extName}\n\n{this}");

        return true;
    }

    #endregion
}

[Serializable]
public class SerializableInputBinding
{
    public BindPair[] bindPairs;

    public SerializableInputBinding(InputBinding binding)
    {
        int len = binding.Bindings.Count;
        int index = 0;

        bindPairs = new BindPair[len];

        foreach (var pair in binding.Bindings)
        {
            bindPairs[index++] =
                new BindPair(pair.Key, pair.Value);
        }
    }
}

[Serializable]
public class BindPair
{
    public UserAction key;
    public KeyCode value;

    public BindPair(UserAction key, KeyCode value)
    {
        this.key = key;
        this.value = value;
    }
}