using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class IdleBehavior : StateMachineBehaviour
{
     [SerializeField]
     private float timeUntilBored;
 
     [SerializeField]
     private int numberOfBoredAnimations;
 
     private bool _isBored;
     private float _idleTime; 
     private int _boredAnimation;
     
     private static readonly int IdleAnimation = Animator.StringToHash("IdleAnimation");
 
     // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
     public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
     {
         ResetIdle();
     }
 
     // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
     public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
     {
         if (_isBored == false)
         {
             _idleTime += Time.deltaTime;
 
             if (_idleTime > timeUntilBored && stateInfo.normalizedTime % 1 < 0.02f) 
             {
                 _isBored = true;
                 _boredAnimation = Random.Range(1, numberOfBoredAnimations + 1);
                 _boredAnimation = _boredAnimation * 2 - 1;
 
                 animator.SetFloat(IdleAnimation, _boredAnimation - 1);
             }
         }
         else if (stateInfo.normalizedTime % 1 > 0.98)
         {
             ResetIdle();
         }
 
         animator.SetFloat(IdleAnimation, _boredAnimation, 0.2f, Time.deltaTime);
     }
 
     private void ResetIdle()
     {
         if (_isBored)
         {
             _boredAnimation--;
         }
 
         _isBored = false;
         _idleTime = 0;
     }
}

