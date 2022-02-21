using GameBrains.Messages;
using UnityEngine;

namespace GameBrains.GUI
{
    [AddComponentMenu("Scripts/GameBrains/GUI/Message Viewer")]
    public class MessageViewer : WindowManager
    {
        public MessageQueue messageQueue;

        public override void Start()
        {
            base.Start(); // Initializes the window id.
            windowTitle = "Message Viewer";
            if (!messageQueue)
            {
                Log.Debug("MessageViewer has no MessageQueue assigned.");
            }
        }

        protected override void SetMinimumWindowSize()
        {
            // Override if at bottom so window fits, otherwise let it grow as needed.
            if (messageQueue && verticalAlignment == VerticalAlignment.Bottom)
            {
                minimumWindowSize.y =
                    (messageQueue.MaximumMessages *
                     (UnityEngine.GUI.skin.window.lineHeight + 1))
                    + (titleStyle.lineHeight + 2);
            }
        }

        // This creates the GUI inside the window.
        // It requires the id of the window it's currently making GUI for.
        protected override void WindowFunction(int windowID)
        {
            // Purposely not calling base.WindowFunction here.

            // Draw any Controls inside the window here.

            Color savedColor = UnityEngine.GUI.color;
            UnityEngine.GUI.color = defaultContentColor;

            GUILayout.Label(messageQueue.GetMessages());

            UnityEngine.GUI.color = savedColor;

            // Make the windows be draggable.
            UnityEngine.GUI.DragWindow();
        }
    }
}