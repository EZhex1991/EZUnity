/*
 * Author:      #AUTHORNAME#
 * CreateTime:  #CREATETIME#
 * Description:
 * 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZhex1991.EZUnity.Demo
{
    public class AStar : MonoBehaviour
    {
        [System.Serializable]
        public class Point
        {
            public static bool allowObliqueMove; // 是否允许斜对角运动

            public int x;
            public int y;
            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public IEnumerable<Point> GetChildren() // 获取附近的点
            {
                yield return new Point(x - 1, y);
                yield return new Point(x + 1, y);
                yield return new Point(x, y + 1);
                yield return new Point(x, y - 1);
                if (allowObliqueMove)
                {
                    yield return new Point(x - 1, y - 1);
                    yield return new Point(x - 1, y + 1);
                    yield return new Point(x + 1, y - 1);
                    yield return new Point(x + 1, y + 1);
                }
            }

            public static float operator -(Point p1, Point p2)
            {
                return allowObliqueMove
                    ? (new Vector2(p1.x, p1.y) - new Vector2(p2.x, p2.y)).magnitude
                    : Mathf.Abs(p1.x - p2.x) + Mathf.Abs(p1.y - p2.y);
            }
            public static bool operator ==(Point p1, Point p2)
            {
                if (System.Object.ReferenceEquals(p1, p2))
                {
                    return true;
                }
                else if ((object)p1 == null || (object)p2 == null)
                {
                    return false;
                }
                return p1.x == p2.x && p1.y == p2.y;
            }
            public static bool operator !=(Point p1, Point p2)
            {
                return !(p1 == p2);
            }
            public override bool Equals(object obj)
            {
                if (obj is Point)
                {
                    Point p = obj as Point;
                    return this == p;
                }
                return false;
            }
            public override int GetHashCode()
            {
                return x * 1000 + y;
            }
        }
        public class Status
        {
            public Point parent;
            public float distance;
            public Status(Point parent, float distance)
            {
                this.parent = parent;
                this.distance = distance;
            }
        }

        public Point size = new Point(50, 50);
        public Point startPoint = new Point(5, 5);
        public Point endPoint = new Point(45, 45);
        public bool allowObliqueMove;

        public GameObject prefab;
        private Toggle[,] toggles;
        private Image[,] images;

        List<Point> openList = new List<Point>();
        bool[,] closedList;
        Dictionary<Point, Status> record = new Dictionary<Point, Status>();

        void Start()
        {
            StartCoroutine(InitMap());
        }

        IEnumerator InitMap()
        {
            // 设置网格和Map大小
            float cellSizeX = 1000f / size.x - 1;
            float cellSizeY = 1000f / size.y - 1;
            GetComponent<GridLayoutGroup>().cellSize = new Vector2(cellSizeX, cellSizeY);
            toggles = new Toggle[size.x, size.y];
            images = new Image[size.x, size.y];
            closedList = new bool[size.x, size.y];
            Point.allowObliqueMove = allowObliqueMove;
            // 初始化Map
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    GameObject element = Instantiate(prefab, transform, false);
                    toggles[x, y] = element.GetComponent<Toggle>();
                    toggles[x, y].isOn = Random.Range(0, 3) == 0;
                    images[x, y] = element.GetComponent<Image>();
                }
                if (x % 10 == 0) yield return null; // 如果地图过大初始化可能很慢，每10行等待一次界面刷新
            }
            // 起始和结束点必须为可达点
            toggles[startPoint.x, startPoint.y].isOn = false;
            toggles[endPoint.x, endPoint.y].isOn = false;
            // 每次点击Toggle都会重新寻路
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    toggles[x, y].onValueChanged.AddListener(delegate { Search(); });
                }
            }
            // 初始化完后进行一次寻路
            Search();
        }

        float GetDistance(Point point)
        {
            return point - endPoint;
        }
        void Insert(float distance, Point p)
        {
            int index = 0;
            while (index < openList.Count && record[openList[index]].distance < distance)
            {
                index++;
            }
            openList.Insert(index, p);
        }
        int Sort(Point p1, Point p2)
        {
            if (GetDistance(p1) > GetDistance(p2))
            {
                return 1;
            }
            else if (GetDistance(p1) < GetDistance(p2))
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        bool IsValid(Point point)
        {
            if (point.x >= size.x || point.y >= size.y || point.x < 0 || point.y < 0)
                return false;
            return !toggles[point.x, point.y].isOn;
        }

        void Reset()
        {
            openList.Clear();
            closedList = new bool[size.x, size.y];
            record.Clear();
            openList.Add(startPoint);
            record.Add(startPoint, new Status(null, 0));
        }
        void Search()
        {
            Point.allowObliqueMove = allowObliqueMove;
            Reset();
            float startTime = Time.realtimeSinceStartup;
            while (openList.Count > 0)
            {
                Point parent = openList[0];
                if (parent == endPoint)
                {
                    break;
                }
                openList.Remove(parent);
                closedList[parent.x, parent.y] = true;
                foreach (Point child in parent.GetChildren())
                {
                    if (!IsValid(child))
                        continue;
                    float distance = record[parent].distance + (child - parent);
                    if (record.ContainsKey(child))
                    {
                        if (distance < record[child].distance)
                        {
                            record[child] = new Status(parent, distance);
                        }
                    }
                    else
                    {
                        openList.Add(child);
                        record[child] = new Status(parent, distance);
                    }
                }
                openList.Sort(Sort);
            }
            float endTime = Time.realtimeSinceStartup;
            print("time cost: " + (endTime - startTime));
            Refresh();
        }

        void Refresh()
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Point point = new Point(x, y);
                    if (closedList[point.x, point.y])
                    {
                        images[x, y].color = Color.red;
                    }
                    else if (openList.Contains(point))
                    {
                        images[x, y].color = Color.yellow;
                    }
                    else
                    {
                        images[x, y].color = Color.white;
                    }
                }
            }
            Point path = endPoint;
            Status status;
            while (path != null && record.TryGetValue(path, out status))
            {
                images[path.x, path.y].color = Color.green;
                path = status.parent;
            }
            images[startPoint.x, startPoint.y].color = Color.blue;
            images[endPoint.x, endPoint.y].color = Color.blue;
        }
    }
}