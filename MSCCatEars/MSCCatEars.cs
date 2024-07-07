using System;
using System.Reflection;
using MSCLoader;
using UnityEngine;

namespace MSCCatEars
{
    public class MSCCatEars : Mod
    {
        public override string ID => "CatEars";
        public override string Version => "1.0";
        public override string Author => "アカツキ2555";

        public override void ModSetup()
        {
            SetupFunction(Setup.PostLoad, Mod_Load); // Affect mods too by using PostLoad
        }

        private void Mod_Load()
        {
            byte[] numArray;
            using (var manifestResourceStream = Assembly.GetExecutingAssembly()
                       .GetManifestResourceStream("MSCCatEars.Resources.catears.unity3d"))
            {
                if (manifestResourceStream == null)
                    throw new Exception("The mod DLL is corrupted, unable to load catears.unity3d. Cannot continue");
                numArray = new byte[manifestResourceStream.Length];
                _ = manifestResourceStream.Read(numArray, 0, numArray.Length);
            }

            var assetBundle = numArray.Length != 0
                ? AssetBundle.CreateFromMemoryImmediate(numArray)
                : throw new Exception("The mod DLL is corrupted, unable to load catears.unity3d. Cannot continue");

            var catEars = assetBundle.LoadAsset<GameObject>("catears");

            assetBundle.Unload(false);
            
            foreach (var gameObject in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                var path = GO_Path(gameObject);
                if (path.Contains("skeleton/pelvis/spine_middle/spine_upper/HeadPivot/head") &&
                    gameObject.transform.parent.name == "HeadPivot")
                {
                    var inst = GameObject.Instantiate(catEars);
                    inst.transform.parent = gameObject.transform;
                    inst.transform.localPosition = new Vector3(-0.2001f, 0.006f, -0.1217f);
                    inst.transform.localEulerAngles = new Vector3(1.22f, -180.407f, -87.094f);
                    inst.transform.localScale = Vector3.one * 0.08102f;
                }

                if (path.Contains("skeleton/pelvis/RotationPivot/spine_middle/spine_upper/head") && gameObject.transform.parent.name == "spine_upper")
                {
                    var inst = GameObject.Instantiate(catEars);
                    inst.transform.parent = gameObject.transform;
                    inst.transform.localPosition = new Vector3(-0.2001f, 0.006f, -0.1217f);
                    inst.transform.localEulerAngles = new Vector3(1.22f, -180.407f, -87.094f);
                    inst.transform.localScale = Vector3.one * 0.08102f;
                }
            }
        }


        private string GO_Path(GameObject go)
        {
            // generate path to gameobject, separated by '/'
            var path = go.name;
            while (go.transform.parent != null)
            {
                go = go.transform.parent.gameObject;
                path = go.name + "/" + path;
            }

            return path;
        }
    }
}