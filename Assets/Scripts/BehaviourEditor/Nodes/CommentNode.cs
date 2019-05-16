using UnityEngine;

public class CommentNode : BaseNode
{
    public string comment = "This is a comment";
    public override void DrawWindow()
    {
        comment = GUILayout.TextArea(comment, 200);
    }
}
