using UnityEngine;

namespace Utils
{
    public static class RectTransformExtension
    {
        /// <summary>
        /// Counts the bounding box corners of the given RectTransform that are visible in screen space.
        /// </summary>
        /// <returns>The amount of bounding box corners that are visible.</returns>
        /// <param name="rectTransform">Rect transform.</param>
        /// <param name="camera">Camera. Leave it null for Overlay Canvasses.</param>
        private static int CountCornersVisibleFrom(this RectTransform rectTransform, RectTransform viewport,
            Camera camera = null)
        {
            Vector3[] objectCorners = new Vector3[4];
            rectTransform.GetWorldCorners(objectCorners);

            int visibleCorners = 0;
            for (var i = 0; i < objectCorners.Length; i++) // For each corner in rectTransform
            {
                Vector3 tempScreenSpaceCorner; // Cached
                if (camera != null)
                    tempScreenSpaceCorner =
                        camera.WorldToScreenPoint(
                            objectCorners[i]); // Transform world space position of corner to screen space
                else
                {
                    tempScreenSpaceCorner =
                        objectCorners
                            [i]; // If no camera is provided we assume the canvas is Overlay and world space == screen space
                }

                if (RectTransformUtility.RectangleContainsScreenPoint(viewport,
                        tempScreenSpaceCorner, camera)) // If the corner is inside the screen
                {
                    visibleCorners++;
                }
            }

            return visibleCorners;
        }

        /// <summary>
        /// Determines if this RectTransform is fully visible.
        /// Works by checking if each bounding box corner of this RectTransform is inside the screen space view frustrum.
        /// </summary>
        /// <returns><c>true</c> if is fully visible; otherwise, <c>false</c>.</returns>
        /// <param name="rectTransform">Rect transform.</param>
        /// <param name="camera">Camera. Leave it null for Overlay Canvasses.</param>
        public static bool IsFullyVisibleFrom(this RectTransform rectTransform, RectTransform viewport,
            Camera camera = null)
        {
            if (!rectTransform.gameObject.activeInHierarchy)
                return false;

            return CountCornersVisibleFrom(rectTransform, viewport, camera) == 4; // True if all 4 corners are visible
        }

        /// <summary>
        /// Determines if this RectTransform is at least partially visible.
        /// Works by checking if any bounding box corner of this RectTransform is inside the screen space view frustrum.
        /// </summary>
        /// <returns><c>true</c> if is at least partially visible; otherwise, <c>false</c>.</returns>
        /// <param name="rectTransform">Rect transform.</param>
        /// <param name="camera">Camera. Leave it null for Overlay Canvasses.</param>
        public static bool IsVisibleFrom(this RectTransform rectTransform, RectTransform viewport, Camera camera = null)
        {
            if (!rectTransform.gameObject.activeInHierarchy)
                return false;

            return CountCornersVisibleFrom(rectTransform, viewport, camera) > 0; // True if any corners are visible
        }

        public static bool IsBelowScroll(this RectTransform rectTransform, RectTransform viewport) =>
            rectTransform.position.y < viewport.position.y;
    }
}