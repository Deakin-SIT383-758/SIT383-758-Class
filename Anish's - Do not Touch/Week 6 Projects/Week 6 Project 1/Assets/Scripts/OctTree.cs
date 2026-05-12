using UnityEngine;
using System.Collections.Generic;

public class OctTree
{

    protected float OctTreeLeafSize = 0.01f;

    protected int maxPointPerVoxel = 5;

    protected GameObject nodeShape;

    protected class OctTreeElement
    {
        public Vector3 point;

        public Color colour;

        public OctTreeNode containedIn;

        public OctTreeElement(Vector3 p, Color c)
        {
            point = p;
            colour = c;
            containedIn = null;
        }
    }

    protected class OctTreeNode
    {
        public Vector3 minCorner;
        public Vector3 maxCorner;

        public OctTreeNode parent;

        public OctTreeNode[] children = new OctTreeNode[8];

        public List<OctTreeElement> containedEntities;

        public int leafcount;

        public GameObject voxel;

        public static int encode(bool x, bool y, bool z)
        {
            int r = 0;
            if (x) r += 4;
            if (y) r += 2;
            if (z) r += 1;
            return r;
        }

        public OctTreeNode(Vector3 minc, Vector3 maxc, OctTreeNode parnt)
        {
            minCorner = minc;
            maxCorner = maxc;
            parent = parnt;

            for (int i = 0; i < 8; i++)
            {
                children[i] = null;
            }

            leafcount = 0;
            containedEntities = new List<OctTreeElement>();
            voxel = null;
        }
    }

    protected OctTreeNode octtreeroot;

    public OctTree(GameObject n)
    {
        nodeShape = n;
        octtreeroot = null;
    }

    public void addPoint(Vector3 point, Color c)
    {
        placeNodeInTree(new OctTreeElement(point, c));
    }

    private void removeFromContainment(OctTreeElement o)
    {
        if (o.containedIn != null)
        {
            if (!o.containedIn.containedEntities.Remove(o))
            {
                Debug.Log("Oct Tree Element not found in container");
            }
        }
    }

    private void changeContainment(OctTreeElement o, OctTreeNode dest)
    {
        removeFromContainment(o);

        o.containedIn = dest;

        if (dest != null)
        {
            while (o.containedIn.containedEntities.Count > maxPointPerVoxel)
            {
                o.containedIn.containedEntities.RemoveAt(0);
            }

            o.containedIn.containedEntities.Add(o);
        }
    }

    protected void placeNodeInTree(OctTreeElement o)
    {
        if (o.containedIn == null)
        {
            if (octtreeroot == null)
            {
                float size = OctTreeLeafSize;

                octtreeroot = new OctTreeNode(
                    new Vector3(
                        o.point.x - size / 2.0f,
                        o.point.y - size / 2.0f,
                        o.point.z - size / 2.0f
                    ),
                    new Vector3(
                        o.point.x + size / 2.0f,
                        o.point.y + size / 2.0f,
                        o.point.z + size / 2.0f
                    ),
                    null
                );
            }

            changeContainment(o, octtreeroot);
        }

        while (
            (o.point.x <= o.containedIn.minCorner.x) ||
            (o.point.y <= o.containedIn.minCorner.y) ||
            (o.point.z <= o.containedIn.minCorner.z) ||
            (o.point.x > o.containedIn.maxCorner.x) ||
            (o.point.y > o.containedIn.maxCorner.y) ||
            (o.point.z > o.containedIn.maxCorner.z)
        )
        {
            if (o.containedIn.parent == null)
            {
                bool crossx = o.point.x <= o.containedIn.minCorner.x;
                bool crossy = o.point.y <= o.containedIn.minCorner.y;
                bool crossz = o.point.z <= o.containedIn.minCorner.z;

                float size =
                    (o.containedIn.maxCorner.x - o.containedIn.minCorner.x);

                Vector3 pmin = new Vector3(
                    crossx ? o.containedIn.minCorner.x - size : o.containedIn.minCorner.x,
                    crossy ? o.containedIn.minCorner.y - size : o.containedIn.minCorner.y,
                    crossz ? o.containedIn.minCorner.z - size : o.containedIn.minCorner.z
                );

                Vector3 pmax = new Vector3(
                    crossx ? o.containedIn.maxCorner.x : o.containedIn.maxCorner.x + size,
                    crossy ? o.containedIn.maxCorner.y : o.containedIn.maxCorner.y + size,
                    crossz ? o.containedIn.maxCorner.z : o.containedIn.maxCorner.z + size
                );

                OctTreeNode newoct = new OctTreeNode(pmin, pmax, null);

                newoct.children[
                    OctTreeNode.encode(crossx, crossy, crossz)
                ] = octtreeroot;

                newoct.leafcount++;

                octtreeroot.parent = newoct;
                octtreeroot = newoct;
            }

            changeContainment(o, o.containedIn.parent);

            for (int i = 0; i < 8; i++)
            {
                if (
                    o.containedIn.children[i] != null &&
                    o.containedIn.children[i].leafcount == 0 &&
                    o.containedIn.children[i].containedEntities.Count == 0
                )
                {
                    o.containedIn.children[i] = null;
                    o.containedIn.leafcount--;
                }
            }
        }

        while (
            o.containedIn.maxCorner.x - o.containedIn.minCorner.x >
            OctTreeLeafSize
        )
        {
            if (
                (o.containedIn.leafcount == 0) &&
                (o.containedIn.containedEntities.Count <= 1)
            )
            {
                break;
            }

            Vector3 mid =
                0.5f * (o.containedIn.maxCorner + o.containedIn.minCorner);

            bool crossx = o.point.x <= mid.x;
            bool crossy = o.point.y <= mid.y;
            bool crossz = o.point.z <= mid.z;

            int cindex = OctTreeNode.encode(crossx, crossy, crossz);

            Vector3 cmin = new Vector3(
                crossx ? o.containedIn.minCorner.x : mid.x,
                crossy ? o.containedIn.minCorner.y : mid.y,
                crossz ? o.containedIn.minCorner.z : mid.z
            );

            Vector3 cmax = new Vector3(
                crossx ? mid.x : o.containedIn.maxCorner.x,
                crossy ? mid.y : o.containedIn.maxCorner.y,
                crossz ? mid.z : o.containedIn.maxCorner.z
            );

            if (o.containedIn.children[cindex] == null)
            {
                o.containedIn.children[cindex] =
                    new OctTreeNode(cmin, cmax, o.containedIn);

                o.containedIn.leafcount++;
            }

            changeContainment(o, o.containedIn.children[cindex]);
        }

        while (
            octtreeroot != null &&
            octtreeroot.leafcount <= 1 &&
            octtreeroot.containedEntities.Count == 0
        )
        {
            for (int i = 0; i < 8; i++)
            {
                if (octtreeroot.children[i] != null)
                {
                    octtreeroot = octtreeroot.children[i];
                    break;
                }
            }
        }
    }

    public void renderOctTree(GameObject parent)
    {
        renderOctTreeNode(octtreeroot, parent);
    }

    protected void renderOctTreeNode(OctTreeNode root, GameObject parent)
    {
        if (root != null)
        {
            if (root.leafcount == 0)
            {
                if (root.voxel == null)
                {
                    root.voxel = UnityEngine.Object.Instantiate(nodeShape);
                }

                Color c = new Color(0, 0, 0);

                foreach (OctTreeElement e in root.containedEntities)
                {
                    c += e.colour;
                }

                c = (1.0f / root.containedEntities.Count) * c;

                root.voxel.transform.position =
                    0.5f * (root.minCorner + root.maxCorner);

                root.voxel.transform.localScale =
                    root.maxCorner - root.minCorner;

                root.voxel.transform.SetParent(parent.transform);

                root.voxel
                    .GetComponent<MeshRenderer>()
                    .material.color = c;
            }
            else
            {
                if (root.voxel != null)
                {
                    GameObject.Destroy(root.voxel);
                    root.voxel = null;
                }

                for (int i = 0; i < 8; i++)
                {
                    renderOctTreeNode(root.children[i], parent);
                }
            }
        }
    }
}