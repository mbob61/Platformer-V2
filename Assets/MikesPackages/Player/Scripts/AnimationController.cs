using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public SpriteList animationList = new SpriteList();
    public SpriteRenderer spriteRenderer;
    public float totalAnimationLength;
    public bool playForward = true;
    public bool loop;

    private float lengthOfFrame;
    private int animationIndex;
    private List<Sprite> sprites;
    private int frameIndex;
    private bool canPlayAnimation;

    private float currentAnimationPlayingIndex;

    private void Awake()
    {
        resetAnimationState();
    }

    public void resetAnimationState()
    {
        playForward = true;
        frameIndex = 0;
        reset(0, false);
    }

    public void switchToAnimationWithIndex(int index)
    {
        reset(index, false);
    }

    public void switchToAnimationWithIndex(int index, bool resetFrame)
    {
        reset(index, resetFrame);
    }

    private void reset(int index, bool resetFrame)
    {
        if (resetFrame)
        {
            frameIndex = 0;
        }
        canPlayAnimation = true;
        animationIndex = index;
        sprites = animationList.listOfAnimations[animationIndex].listOfAnimationFrames;

        if (sprites.Count > 0)
        {
            spriteRenderer.sprite = sprites[frameIndex];
        }

        lengthOfFrame = totalAnimationLength / animationList.listOfAnimations[animationIndex].listOfAnimationFrames.Count;
        currentAnimationPlayingIndex = lengthOfFrame;
    }


    // Update is called once per frame
    void Update()
    {
        if (canPlayAnimation)
        {
            playAnimation();
        }
    }


    private void playAnimation()
    {
        if (canPlayAnimation)
        {
            if (currentAnimationPlayingIndex > 0)
            {
                if (currentAnimationPlayingIndex - Time.deltaTime >= 0)
                {
                    currentAnimationPlayingIndex -= Time.deltaTime;
                }
                else
                {
                    play();
                }
            }
            else
            {
                play();
            }
        }
    }

    private void play()
    {
        // Increment frames if we are playing forward, decrement if we are playing backwards.
        if (playForward)
        {
            frameIndex += 1;
        }
        else
        {
            frameIndex -= 1;
        }

        // If we get to the final frame (and we are playing forward)
        if (frameIndex > sprites.Count - 1)
        {
            frameIndex = 0;
            // reset the animation back to beginning if we are looping
            if (!loop)
            {
                canPlayAnimation = false;
            }

        }
        // If we get to the first frame (and we are playing backwards)
        else if (frameIndex < 0)
        {
            frameIndex = sprites.Count - 1;
            // Reset the animation back to the final frame if we are looping
            if (!loop)
            {
                canPlayAnimation = false;
            }
        }


        // Set the sprite to be the correct one from the list]
        if (canPlayAnimation)
        {
            //print("Updating to frame " + (frameIndex + 1));
            spriteRenderer.sprite = sprites[frameIndex];
        }
        currentAnimationPlayingIndex = lengthOfFrame;


    }

    #region getters and setters

    public void setTotalAnimationLength(float l)
    {
        totalAnimationLength = l;
    }

    public void setDirectionForwards(bool b)
    {
        playForward = b;
    }

    public void setLoop(bool l)
    {
        loop = l;
    }

    public float getMaxLifetime()
    {
        return totalAnimationLength;
    }

    public void canPlay(bool c)
    {
        //stopCoRoutineReset = c;
    }

    public void setCanPlayAnimation(bool b)
    {
        canPlayAnimation = b;
    }

    public int getFrameCount()
    {
        return animationList.listOfAnimations[animationIndex].listOfAnimationFrames.Count;
    }

    public int getFrameIndex()
    {
        return frameIndex;
    }

    public float getFrameLength()
    {
        return lengthOfFrame;
    }

    #endregion
}

[System.Serializable]
public class SpriteContainer
{
    public List<Sprite> listOfAnimationFrames;
}

[System.Serializable]
public class SpriteList
{
    public List<SpriteContainer> listOfAnimations;
}
