using UnityEngine;

public class AnimatorDebug : MonoBehaviour
{
    private Animator anim;
    private EnemyMovement enemy;

    void Start()
    {
        anim = GetComponent<Animator>();
        enemy = GetComponent<EnemyMovement>();

        Debug.Log($"[AnimatorDebug] START - EnemyState: {enemy.GetCurrentState()}");
        Debug.Log($"[AnimatorDebug] START - isIdle: {anim.GetBool("isIdle")}");
        Debug.Log($"[AnimatorDebug] START - isChasing: {anim.GetBool("isChasing")}");
        Debug.Log($"[AnimatorDebug] START - isAttacking: {anim.GetBool("isAttacking")}");
        Debug.Log($"[AnimatorDebug] START - isReturning: {anim.GetBool("isReturning")}");
        Debug.Log($"[AnimatorDebug] START - AnimState hash: {anim.GetCurrentAnimatorStateInfo(0).fullPathHash}");
    }

    void Update()
    {
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        Debug.Log($"[AnimatorDebug] " +
            $"EnemyState: {enemy.GetCurrentState()} | " +
            $"isIdle: {anim.GetBool("isIdle")} | " +
            $"isChasing: {anim.GetBool("isChasing")} | " +
            $"isAttacking: {anim.GetBool("isAttacking")} | " +
            $"isReturning: {anim.GetBool("isReturning")} | " +
            $"AnimState: {state.fullPathHash} | " +
            $"NormalizedTime: {state.normalizedTime}"
        );
    }
}