using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.Tilemaps;

namespace Assets
{
    public class TilemapController : Singleton<TilemapController>
    {
        public int Buffer { get => 300; }
        public Tilemap tilemap;
        public NoiseWrapper noiseWrapper;
        public TileBase GreenTile1;
        public TileBase GreenTile2;
        public TileBase GreenTile3;
        public TileBase BrownTile1;
        public TileBase BrownTile2;
        public TileBase BrownTile3;
        public TileBase GrayTile1;
        public TileBase GrayTile2;
        public TileBase GrayTile3;
        TileBase[] allTiles;
        Bounds lastBufferedScope;
        Bounds currentBufferedScope;

        // Start is called before the first frame update
        void Start()
        {
            lastBufferedScope = GetScopeWorldBounds();
            noiseWrapper = new NoiseWrapper(1337);
            allTiles = new TileBase[]
            {
                BrownTile1, BrownTile2, BrownTile3,
                GreenTile2, GreenTile1, GreenTile3,
                GrayTile1, GrayTile2, GrayTile3
            };
            UpdateTiles();
        }

        private void LoadTilesToPoints(IEnumerable<Vector3> points)
        {
            foreach (Vector3 currVector in points)
            {
                float noiseVal = noiseWrapper.GetNoise(currVector.x, currVector.y);
                tilemap.SetTile(tilemap.layoutGrid.WorldToCell(currVector), GetTileForNoiseValue(noiseVal));
            }
        }

        private Vector3[] GetAllPointsWithinBounds(Bounds bounds)
        {
            Vector3[] points = new Vector3[(int)((bounds.size.x + 1) * (bounds.size.y + 1))];
            int currPoint = 0;
            for (float y = bounds.min.y; y < bounds.max.y; y += 1)
            {
                for (float x = bounds.min.x; x < bounds.max.x; x += 1)
                {
                    points[currPoint] = new Vector3(x, y, bounds.center.z);
                    currPoint++;
                }
            }
            return points;
        }

        public void UpdateTiles()
        {
            //tilemap.ClearAllTiles();
            currentBufferedScope = GetScopeWorldBounds();
            Vector3 offset = lastBufferedScope.center - currentBufferedScope.center;

            if (lastBufferedScope.Intersects(currentBufferedScope) && (offset.x != 0 || offset.y != 0))
            {
                IEnumerable<Vector3> offsetBounds = GetOffsetPoints(offset);
                LoadTilesToPoints(offsetBounds);
            }
            else
            {
                IEnumerable<Vector3> boundPoints = GetAllPointsWithinBounds(currentBufferedScope);
                LoadTilesToPoints(boundPoints);
            }
            lastBufferedScope = new Bounds(currentBufferedScope.center, currentBufferedScope.size);
        }

        private IEnumerable<Vector3> GetOffsetPoints(Vector3 offset)
        {
            IEnumerable<Vector3> retPoints = new Vector3[0];
            if (offset.x == 0 && offset.y < 0) //up
            {
                Vector3 center = new Vector3(currentBufferedScope.center.x, lastBufferedScope.max.y + Math.Abs(offset.y / 2), currentBufferedScope.center.z);
                Vector3 size = new Vector3(currentBufferedScope.size.x, Math.Abs(offset.y), lastBufferedScope.size.z);
                retPoints = GetAllPointsWithinBounds(new Bounds(center, size));
            }
            else if (offset.x == 0 && offset.y > 0) //down
            {
                Vector3 center = new Vector3(currentBufferedScope.center.x, lastBufferedScope.min.y - Math.Abs(offset.y / 2), currentBufferedScope.center.z);
                Vector3 size = new Vector3(currentBufferedScope.size.x, Math.Abs(offset.y), lastBufferedScope.size.z);
                retPoints = GetAllPointsWithinBounds(new Bounds(center, size));
            }
            else if (offset.x < 0 && offset.y == 0) //right
            {
                Vector3 center = new Vector3(lastBufferedScope.max.x + Math.Abs(offset.x / 2), currentBufferedScope.center.y, currentBufferedScope.center.z);
                Vector3 size = new Vector3(Math.Abs(offset.x), currentBufferedScope.size.y, lastBufferedScope.size.z);
                retPoints = GetAllPointsWithinBounds(new Bounds(center, size));
            }
            else if (offset.x > 0 && offset.y == 0) //left
            {
                Vector3 center = new Vector3(lastBufferedScope.min.x - Math.Abs(offset.x / 2), currentBufferedScope.center.y, currentBufferedScope.center.z);
                Vector3 size = new Vector3(Math.Abs(offset.x), currentBufferedScope.size.y, lastBufferedScope.size.z);
                retPoints = GetAllPointsWithinBounds(new Bounds(center, size));
            }
            else if (offset.x < 0 && offset.y < 0) //upright
            {
                Vector3 centerUp = new Vector3(currentBufferedScope.center.x, lastBufferedScope.max.y + Math.Abs(offset.y / 2), currentBufferedScope.center.z);
                Vector3 sizeUp = new Vector3(currentBufferedScope.size.x, Math.Abs(offset.y), lastBufferedScope.size.z);
                Bounds upperBound = new Bounds(centerUp, sizeUp);
                Vector3[] upperPoints = GetAllPointsWithinBounds(upperBound);

                Vector3 centerRight = new Vector3(lastBufferedScope.max.x + Math.Abs(offset.x / 2), currentBufferedScope.center.y, currentBufferedScope.center.z);
                Vector3 sizeRight = new Vector3(Math.Abs(offset.x), currentBufferedScope.size.y -Math.Abs(offset.y / 2), lastBufferedScope.size.z);
                Bounds righterBounds = new Bounds(centerRight, sizeRight);
                Vector3[] righterPoints = GetAllPointsWithinBounds(righterBounds);

                retPoints = upperPoints.Concat(righterPoints);
            }
            else if (offset.x > 0 && offset.y < 0) //upleft
            {
                Vector3 centerUp = new Vector3(currentBufferedScope.center.x, lastBufferedScope.max.y + Math.Abs(offset.y / 2), currentBufferedScope.center.z);
                Vector3 sizeUp = new Vector3(currentBufferedScope.size.x, Math.Abs(offset.y), lastBufferedScope.size.z);
                Bounds upperBound = new Bounds(centerUp, sizeUp);
                Vector3[] upperPoints = GetAllPointsWithinBounds(upperBound);

                Vector3 centerLeft = new Vector3(lastBufferedScope.min.x - Math.Abs(offset.x / 2), currentBufferedScope.center.y, currentBufferedScope.center.z);
                Vector3 sizeLeft = new Vector3(Math.Abs(offset.x), currentBufferedScope.size.y - Math.Abs(offset.y / 2), lastBufferedScope.size.z);
                Bounds leftBounds = new Bounds(centerLeft, sizeLeft);
                Vector3[] righterPoints = GetAllPointsWithinBounds(leftBounds);

                retPoints = upperPoints.Concat(righterPoints);
            }
            else if (offset.x < 0 && offset.y > 0) //down right
            {
                Vector3 centerDown = new Vector3(currentBufferedScope.center.x, lastBufferedScope.min.y - Math.Abs(offset.y / 2), currentBufferedScope.center.z);
                Vector3 sizeDown = new Vector3(currentBufferedScope.size.x, Math.Abs(offset.y), lastBufferedScope.size.z);
                Bounds downBounds = new Bounds(centerDown, sizeDown);
                Vector3[] downPoints = GetAllPointsWithinBounds(downBounds);

                Vector3 centerRight = new Vector3(lastBufferedScope.max.x + Math.Abs(offset.x / 2), currentBufferedScope.center.y, currentBufferedScope.center.z);
                Vector3 sizeRight = new Vector3(Math.Abs(offset.x), currentBufferedScope.size.y - Math.Abs(offset.y / 2), lastBufferedScope.size.z);
                Bounds righterBounds = new Bounds(centerRight, sizeRight);
                Vector3[] righterPoints = GetAllPointsWithinBounds(righterBounds);

                retPoints = downPoints.Concat(righterPoints);
            }
            else  //down left
            {
                Vector3 centerDown = new Vector3(currentBufferedScope.center.x, lastBufferedScope.min.y - Math.Abs(offset.y / 2), currentBufferedScope.center.z);
                Vector3 sizeDown = new Vector3(currentBufferedScope.size.x, Math.Abs(offset.y), lastBufferedScope.size.z);
                Bounds downBounds = new Bounds(centerDown, sizeDown);
                Vector3[] downPoints = GetAllPointsWithinBounds(downBounds);

                Vector3 centerLeft = new Vector3(lastBufferedScope.min.x - Math.Abs(offset.x / 2), currentBufferedScope.center.y, currentBufferedScope.center.z);
                Vector3 sizeLeft = new Vector3(Math.Abs(offset.x), currentBufferedScope.size.y - Math.Abs(offset.y / 2), lastBufferedScope.size.z);
                Bounds leftBounds = new Bounds(centerLeft, sizeLeft);
                Vector3[] leftPoints = GetAllPointsWithinBounds(leftBounds);

                retPoints = downPoints.Concat(leftPoints);
            }
            return retPoints;
        }

        private Bounds GetScopeWorldBounds()
        {
            Bounds cameraBounds = GetCameraWorldBounds();
            Bounds bufferedScope = new Bounds(cameraBounds.center, new Vector3(cameraBounds.size.x + Buffer, cameraBounds.size.y + Buffer, 1));
            return bufferedScope;
        }

        private Bounds GetCameraWorldBounds()
        {
            Vector3 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
            Vector3 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
            Vector3 size = new Vector3(Math.Abs(max.x - min.x), Math.Abs(max.y - min.y));

            Vector3 center = Camera.main.transform.position;

            Bounds camBounds = new Bounds(center, size);
            return camBounds;
        }

        private TileBase GetTileForNoiseValue(float value)
        {
            value = (value + 1) / 2; //make val between 0 and 1
            int index =
                value < 0.1f ? 0 :
                value < 0.15f ? 1 :
                value < 0.25f ? 2 :
                value < 0.5 ? 3 :
                value < 0.725 ? 4 :
                value < 0.85 ? 5 :
                value < 0.875 ? 6 :
                value < 0.95 ? 7 :
                8;

            return allTiles[index];
        }
    }
}