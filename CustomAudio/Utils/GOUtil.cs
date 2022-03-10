using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomAudio.Utils
{
    public static class GOUtil
    {
        public static void GetAllChildren(this GameObject parent,List<GameObject>childlist)
        {
            
            for(int i=0;i<parent.transform.childCount;i++)
            {
                childlist.Add(parent.transform.GetChild(i).gameObject);
            }
            for(int j=0;j<parent.transform.childCount;j++)
            {
                parent.transform.GetChild(j).gameObject.GetAllChildren(childlist);
            }
        }
        public static GameObject FindGameObjectInChildren(this GameObject parent,string childname)
        {
            GameObject Child = null;
            foreach (Transform t in parent.GetComponentsInChildren<Transform>(true))
            {
                if(t.gameObject.name==childname)
                {
                    Child = t.gameObject;
                    break;
                }
            }
            return Child;
        }
        public static List<GameObject> GetAllRootGOInScene(this Scene scene)
        {
            List<GameObject> RootGO = new();
            foreach(var go in scene.GetRootGameObjects())
            {
                RootGO.Add(go);
            }
            return RootGO;

        }
        public static GameObject FindGameObjectNowScene(string goname)
        {
            GameObject result = null;
            foreach(var scene in SceneUtil.GetallScene(true))
            {
                foreach(GameObject go in scene.GetAllRootGOInScene())
                {
                    if (go.name == goname)
                        return go;
                }
            }
            return result;
        }
        public static GameObject[] FindGameObjectsNowScenestartwith(string gostratname)
        {
            List <GameObject> result = new List<GameObject>();
            foreach (var scene in SceneUtil.GetallScene(true))
            {
                foreach (GameObject go in scene.GetAllRootGOInScene())
                {
                    if (go.name.StartsWith(gostratname))
                     {
                        result.Add(go);
                    }
                }
            }
            return (result.ToArray());
        }
        public static void GetALLGoInscene(List<GameObject> allgameobject)
        {
            foreach(var scene in SceneUtil.GetallScene(false))
            {
                foreach(GameObject go in scene.GetAllRootGOInScene())
                {
                    go.GetAllChildren(allgameobject);
                    allgameobject.Add(go);
                }
            }
        }

    }
}
