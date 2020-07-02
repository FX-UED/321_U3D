using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TweenControl : Singleton<TweenControl> {

    public void pauseTween(GameObject go)
    {
        DOTween.Pause(go);
    }

    public void StopTween()
    {
        DOTween.PauseAll();
        
    }
    public void WorldMoveTo(Transform obj, Vector3 toPos, float time, float delay, int looptime, LoopType loopType, Ease ease)
    {
        Tweener paneltweener = obj.DOMove(toPos, time); //这个是修改UGUI的局部坐标,相对与父类的局部坐标
        paneltweener.SetUpdate(true);
        paneltweener.SetDelay(delay);                //设置动画延迟播放
        paneltweener.SetEase(ease);   //设置动画运动的模式
        paneltweener.SetLoops(looptime, loopType);          //设置循环播放并且设置动画循环的模式
    }
    public void MoveTo(Transform obj,Vector3 toPos,float time,float delay,int looptime, LoopType loopType, Ease ease)
    {
        Tweener paneltweener = obj.DOLocalMove(toPos, time); //这个是修改UGUI的局部坐标,相对与父类的局部坐标
        paneltweener.SetUpdate(true);
        paneltweener.SetDelay(delay);                //设置动画延迟播放
        paneltweener.SetEase(ease);   //设置动画运动的模式
        paneltweener.SetLoops(looptime, loopType);          //设置循环播放并且设置动画循环的模式
    }

    public void MovePath(Transform obj, Vector3[] path,  PathType pathType, PathMode pathMode, float time, float delay, int looptime, LoopType loopType, Ease ease,float lookAt)
    {
        
        obj.DOPath(path, time, pathType, pathMode,10).SetUpdate(true).SetDelay(delay).SetEase(ease).SetLoops(looptime, loopType).SetLookAt(lookAt);


    }




    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="toPos">               只在有运动方向的轴上有数值，不运动的地方为0    ！！！ </param>
    /// <param name="time"></param>
    /// <param name="delay"></param>
    /// <param name="looptime"></param>
    /// <param name="loopType"></param>
    /// <param name="ease"></param>
    /// <param name="callback"></param>
    public void MoveBy(Transform obj, Vector3 toPos, float time, float delay, int looptime, LoopType loopType, Ease ease,Action callback)
    {
        Tweener paneltweener = obj.DOBlendableLocalMoveBy(toPos, time); //这个是修改UGUI的局部坐标,相对与父类的局部坐标
        paneltweener.SetUpdate(true);
        paneltweener.SetDelay(delay);                //设置动画延迟播放
        paneltweener.SetEase(ease);   //设置动画运动的模式
        paneltweener.SetLoops(looptime, loopType);          //设置循环播放并且设置动画循环的模式

    }

    public void ColorTo(Material obj, Color toColre, float time, float delay, int looptime, LoopType loopType, Ease ease)
    {
        Tweener paneltweener = obj.DOColor(toColre, time); //这个是修改UGUI的局部坐标,相对与父类的局部坐标
        paneltweener.SetUpdate(true);
        paneltweener.SetDelay(delay);                //设置动画延迟播放
        paneltweener.SetEase(ease);   //设置动画运动的模式
        paneltweener.SetLoops(looptime, loopType);          //设置循环播放并且设置动画循环的模式
    }

    public void FadeTo(Material obj, float toFadeNum, float time, float delay, int looptime, LoopType loopType, Ease ease)
    {
        Tweener paneltweener = obj.DOFade(toFadeNum, time); //这个是修改UGUI的局部坐标,相对与父类的局部坐标
        paneltweener.SetUpdate(true);
        paneltweener.SetDelay(delay);                //设置动画延迟播放
        paneltweener.SetEase(ease);   //设置动画运动的模式
        paneltweener.SetLoops(looptime, loopType);          //设置循环播放并且设置动画循环的模式
    }

    public void ScaleTo(Transform obj, Vector3 toScale, float time, float delay, int looptime, LoopType loopType, Ease ease, Action callback)
    {
        Tweener paneltweener = obj.DOScale(toScale, time); //这个是修改UGUI的局部坐标,相对与父类的局部坐标
        paneltweener.SetUpdate(true);
        paneltweener.SetDelay(delay);                //设置动画延迟播放
        paneltweener.SetEase(ease);   //设置动画运动的模式
        paneltweener.SetLoops(looptime, loopType);          //设置循环播放并且设置动画循环的模式
    }

    public void ScaleFrom(Transform obj, Vector3 toScale, float time, float delay, int looptime, LoopType loopType, Ease ease, Action callback)
    {
        Tweener paneltweener = obj.DOScale(toScale, time).From(); //这个是修改UGUI的局部坐标,相对与父类的局部坐标
        paneltweener.SetUpdate(true);
        paneltweener.SetDelay(delay);                //设置动画延迟播放
        paneltweener.SetEase(ease);   //设置动画运动的模式
        paneltweener.SetLoops(looptime, loopType);          //设置循环播放并且设置动画循环的模式
    }
    public void RotateTo(Transform obj, Vector3 rotate, float time, float delay, int looptime, LoopType loopType, Ease ease)
    {
        Tweener paneltweener = obj.DOLocalRotate(rotate, time); //这个是修改UGUI的局部坐标,相对与父类的局部坐标
        paneltweener.SetUpdate(true);
        paneltweener.SetDelay(delay);                //设置动画延迟播放
        paneltweener.SetEase(ease);   //设置动画运动的模式
        paneltweener.SetLoops(looptime, loopType);          //设置循环播放并且设置动画循环的模式


    }

}

/*
 *        //panelTranform.DOMove(Vector3.zero, 1);     //这个是修改UGUI的世界坐标
        //创建一个简单位移动画,并将动画赋值给一个Tweener进行管理
        Tweener paneltweener = panelTranform.DOLocalMove(Vector3.zero, 2f); //这个是修改UGUI的局部坐标,相对与父类的局部坐标
        //多次调用DOTween动画会影响性能
        //DOTween默认动画播放完毕时会自动销毁动画
        //Tweener对象保存这个动画
        
        paneltweener.SetAutoKill(false);              //禁止销毁动画
        paneltweener.Pause();                         //停止播放动画
        //paneltweener.SetDelay(0.5f);                //设置动画延迟播放
        //paneltweener.SetEase(Ease.INTERNAL_Zero);   //设置动画运动的模式
        //paneltweener.SetLoops(-1);                  //设置循环播放 （当参数为-1的时候循环播放；当参数>=0的时候则表示循环的次数；0的时候表示循环一次）
        //paneltweener.SetLoops(-1,loopType)          //设置循环播放并且设置动画循环的模式
        //paneltweener.PlayForward();                 //播放
        //paneltweener.PlayBackwards();               //倒放
        //paneltweener.OnComplete(OnComplete);        //动画播放完毕后执行，动画倒放时候不执行
        //paneltweener.OnStart(OnStart);              //动画第一次播放时执行
        //paneltweener.OnUpdate(OnUpdate);            //动画播放时不断执行
        //paneltweener.OnPlay(OnPlay);                //动画开始播放时执行
        //paneltweener.OnStepComplete(OnStepComplete);//动画播放且倒放都会执行一次，且执行顺序先于OnComplete
        //paneltweener.OnKill(OnKill);                //动画删除的时候执行事件
        //paneltweener.Kill(true);                    //删除动画且组件会直接到达指定位置
 */
