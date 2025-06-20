using UnityEngine;

namespace Tweener
{
    public interface IExpansionBezier : IExpansionTween<IExpansionBezier>
    {
        Vector3 CurrentPosition { get; }
        Vector3 CurrentRotation { get; }

        IExpansionBezier ChangeSpeed(float speed);

        IExpansionBezier ChangeTime(float time);
    }
}
